using LabDiag.Domain.DTO;
using LabDiag.Domain.Entity;
using LabDiag.Domain.Interface;
using LabDiag.Web.Database;
using Microsoft.EntityFrameworkCore;

namespace LabDiag.Web.Api.V1.Service;

public class ComputerService(WebContext context): IComputerService
{
    

    public async Task<Computer?> GetComputerByHostNameIfExists(string hostName, CancellationToken cancellationToken = default)
    {
        
        var computer = await context.Computer.AsNoTracking().FirstOrDefaultAsync(x => x.HostName == hostName, cancellationToken);
        
        return computer;
    }

    public async Task<Computer> CreateComputer(string hostName, ComputerRegisterDTO computerRegister, CancellationToken cancellationToken = default)
    {
        var computer = new Computer
        {
            HostName = hostName,
            Id =  Guid.NewGuid(),
        };

        foreach (var computerRegisterNic in computerRegister.Nics)
        {
            computer.Nic.Add(new Nic
            {
                Id =  Guid.NewGuid(),
                MacAddress =  computerRegisterNic.MacAddress,
                LinkSpeed =  computerRegisterNic.LinkSpeed,
                Name =  computerRegisterNic.Name,
            });
        }
        
        context.Computer.Add(computer);
        await context.SaveChangesAsync(cancellationToken);
        return computer;
    }

   
    
    
    
    
    public Computer? OnComputerInitialize(ComputerRegisterDTO computerRegisterDto)
    {
        /*
         * Etapas
         * 1 - Checar se mesmo HostName já existe
         * 2 - Checar se Mac existe
         */
        
        
        
        /*
         {
             "hostName": "",
             "nics": [
               {
                 "name": "",
                 "linkSpeed": "",
                 "description": "",
                 "macAddress": "",
                 "status": ""
               }
             ]
        }
         
         */
        
        throw new NotImplementedException();
    }

    public Task<Computer?> GetIdByHostNameIfExists(string hostName, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}