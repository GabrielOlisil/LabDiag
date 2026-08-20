using LabDiag.Domain.Entity;
using LabDiag.Domain.Interface;
using LabDiag.Web.Database;

namespace LabDiag.Web.Api.V1.Service;

public class NicService(WebContext context): INicService
{
    public async Task<Nic?> GetNicByUuid(Guid nicId, CancellationToken cancellationToken = default)
    {
        return await context.Nic.FindAsync([nicId], cancellationToken: cancellationToken);
    }


    public async Task<int> UptadeNicLinkSpeed(Guid nicId, string linkSpeed,
        CancellationToken cancellationToken = default)
    {
        var nic = await context.Nic.FindAsync([nicId], cancellationToken: cancellationToken);
        
        nic?.LinkSpeed = linkSpeed;
        
        return await context.SaveChangesAsync(cancellationToken);
    }
}