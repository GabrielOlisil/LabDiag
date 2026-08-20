using LabDiag.Domain.DTO;
using LabDiag.Domain.Interface;
using LabDiag.Web.Api.V1.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LabDiag.Web.Api.V1.Controller;

[Route("api/v1/[controller]")]
[ApiController]
public partial class ComputerController(
    IComputerService computerService,
    INicService nicService,
    ILogger<ComputerController> logger)
    : ControllerBase
{


    [HttpGet]
    public async Task<IActionResult> GetComputers()
    {

        var computers = await computerService.GetAllComputers();
        
        var computersResponse = computers.Select(computer => new ComputerResponseDto(computer.Id, computer.HostName, [.. computer.Nic.Select(n => new NicResponseDto(n.Id, n.MacAddress, n.Name, n.LinkSpeed))], computer.ActiveNic)).ToList();


        return Ok(computersResponse);
    }
    
    // POST api/v1/<ComputerController>
    [HttpPost("{hostName}")]
    public async Task<IActionResult> Post(string hostName, ComputerRegisterDTO computerRegister,
        CancellationToken cancellationToken)
    {
        var computer = await computerService.GetComputerByHostNameIfExists(hostName, cancellationToken);


        // Cenario 1: Computador não existe ainda

        if (computer == null)
        {
            LogComputadorCriadoEmDatacriacao(DateTime.Now);
            var newComputer = await computerService.CreateComputer(hostName, computerRegister, cancellationToken);

            var computerResponse = new ComputerResponseDto(
                newComputer.Id, newComputer.HostName,
                [..newComputer.Nic.Select(n => new NicResponseDto(n.Id, n.MacAddress, n.Name, n.LinkSpeed))], 
                newComputer.ActiveNic);
            
            return Created("Computador Criado Com Sucesso", new { data = computerResponse });
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

        if (mainNic == null )
        {
            return BadRequest();
        }

        var nicForUpdate = computerRegister.Nics.FirstOrDefault(n => n.MacAddress == mainNic.MacAddress);

        if (nicForUpdate == null)
        {
            return BadRequest();
        }
        
        
        
        
        var numRowsUpdated = await nicService.UptadeNicLinkSpeed(mainNic.Id, nicForUpdate.LinkSpeed, cancellationToken);


        return numRowsUpdated > 0? Ok("NicPrincipal Atualizada com informações novas") : BadRequest();
    }

    [LoggerMessage(LogLevel.Information, "Computador criado em {DataCriacao}")]
    partial void LogComputadorCriadoEmDatacriacao(DateTime dataCriacao);
}