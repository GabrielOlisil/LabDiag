using LabDiag.Domain.Entity;
using LabDiag.Domain.Interface;
using LabDiag.Web.Api.V1.Service;
using LabDiag.Web.Components;
using LabDiag.Web.Database;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IComputerService, ComputerService>();
builder.Services.AddScoped<INicService, NicService>();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddControllers();

builder.Services.AddOpenApi();


builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
    })
.AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, oidcOptions =>
{
    
    oidcOptions.PushedAuthorizationBehavior = PushedAuthorizationBehavior.UseIfAvailable;
    oidcOptions.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    
    oidcOptions.ClientSecret = builder.Configuration["Authentication:Schemes:MicrosoftOidc:ClientSecret"]
                               ?? throw new InvalidOperationException("ClientSecret não encontrado nos secrets.");
    
    
    oidcOptions.Events = new OpenIdConnectEvents
    {
        OnRedirectToIdentityProviderForSignOut = async context =>
        {
            var idToken = await context.HttpContext.GetTokenAsync("id_token");
            if (!string.IsNullOrEmpty(idToken))
            {
                context.ProtocolMessage.IdTokenHint = idToken;
            }
        }
    };
    
    oidcOptions.SaveTokens = true;
    oidcOptions.Authority = builder.Configuration["OIDC:Authority"];
    oidcOptions.ClientId = builder.Configuration["OIDC:ClientId"];
    oidcOptions.ResponseType = OpenIdConnectResponseType.Code;
    oidcOptions.RequireHttpsMetadata = false;
    oidcOptions.MapInboundClaims = false;
    oidcOptions.TokenValidationParameters.NameClaimType = "name";
    oidcOptions.TokenValidationParameters.RoleClaimType = "roles";
    oidcOptions.Scope.Add(OpenIdConnectScope.OpenIdProfile);
    
    oidcOptions.Scope.Add("email");
    oidcOptions.Scope.Add(OpenIdConnectScope.OfflineAccess);
    oidcOptions.Scope.Add("roles");
    
    
})
.AddCookie(CookieAuthenticationDefaults.AuthenticationScheme);


builder.Services.AddAuthorization();
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddHttpContextAccessor();



builder.Services.AddDbContextPool<WebContext>(opt => 
    opt.UseNpgsql(builder.Configuration.GetConnectionString("LabDiagConnection")));

var app = builder.Build();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
    app.UseHttpsRedirection();
    
}
else
{
    app.MapOpenApi();
    app.MapScalarApiReference("/docs");
}



app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);

app.UseAntiforgery();

app.MapStaticAssets();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();


app.MapPost("/authentication/logout", async (HttpContext context) =>
{
    await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

    await context.SignOutAsync(OpenIdConnectDefaults.AuthenticationScheme, new AuthenticationProperties
    {
        RedirectUri = "/" 
    });
});

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();