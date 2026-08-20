using LabDiag.Domain.DTO;
using LabDiag.Domain.Entity;

namespace LabDiag.Domain.Interface;

public interface INicService
{
    Task<Nic?> GetNicByUuid(Guid nicId, CancellationToken cancellationToken);

    Task<int> UptadeNicLinkSpeed(Guid nicId, string linkSpeed,
        CancellationToken cancellationToken);
}