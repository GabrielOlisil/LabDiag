using LabDiag.Domain.DTO;
using LabDiag.Domain.Entity;

namespace LabDiag.Domain.Interface;

public interface IComputerService
{
    Computer? OnComputerInitialize(ComputerRegisterDTO computerRegisterDto);
    Task<Computer?> GetComputerByHostNameIfExists(string hostName, CancellationToken cancellationToken);

    Task<Computer> CreateComputer(string hostName, ComputerRegisterDTO computerRegister,
        CancellationToken cancellationToken);

    Task<List<Computer>> GetAllComputers(CancellationToken cancellationToken = default);
}