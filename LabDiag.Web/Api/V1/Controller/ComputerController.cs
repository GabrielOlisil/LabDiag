using LabDiag.Domain.DTO;
using LabDiag.Domain.Interface;
using LabDiag.Web.Api.V1.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LabDiag.Web.Api.V1.Controller;

[Route("api/v1/[controller]")]
[ApiController]
public class ComputerController(IComputerService computerService, INicService nicService) : ControllerBase
{
  

    // POST api/v1/<ComputerController>
    [HttpPost("{hostName}")]
    public async Task<IActionResult> Post(string hostName, ComputerRegisterDTO computerRegister, CancellationToken cancellationToken)
    {
        var computer = await computerService.GetComputerByHostNameIfExists(hostName, cancellationToken);
        
        
        
        // Cenario 1: Computador não existe ainda

        if (computer == null)
        {
            var newComputer = await computerService.CreateComputer(hostName, computerRegister, cancellationToken);
            return Ok(new{message="Computador Criado Com Sucesso",data=newComputer});
        }


        // Cenário 2: Computador existe, mas não está autorizado
        

        if (!computer.RegistredHost)
        {
            return BadRequest("Permissão dos administradores necessárias para enviar logs");
        }
        
        
        // Cenário 3: Computador existe, está autorizado, mas mac não bate
        if (computer.ActiveNic is not { } activeNicId)
        {
            return BadRequest("Permissão dos administradores necessárias para enviar logs");

        }


        var mainNic = await nicService.GetNicByUuid(activeNicId, cancellationToken);

        if (!computerRegister.Nics.Any(n => n.MacAddress == mainNic?.MacAddress))
        {
            return BadRequest();
        }
        
        
        return Ok("Atualizar Nic principal");
        
        

        
    }

}