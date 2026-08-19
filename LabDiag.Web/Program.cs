using LabDiag.Domain.Entity;
using LabDiag.Domain.Interface;
using LabDiag.Web.Api.V1.Service;
using LabDiag.Web.Components;
using LabDiag.Web.Database;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IComputerService, ComputerService>();
builder.Services.AddScoped<INicService, NicService>();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddControllers();

builder.Services.AddOpenApi();



builder.Services.AddDbContextPool<WebContext>(opt => 
    opt.UseNpgsql(builder.Configuration.GetConnectionString("LabDiagConnection")));

var app = builder.Build();

app.MapControllers();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}
else
{
    app.MapOpenApi();
    app.MapScalarApiReference("/docs");
}



app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();