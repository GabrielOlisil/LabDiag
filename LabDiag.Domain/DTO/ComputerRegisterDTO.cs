using System.ComponentModel.DataAnnotations;

namespace LabDiag.Domain.DTO;

public record ComputerRegisterDTO
{
    [Required]
    public required ICollection<NicPropertiesDto> Nics { get; set; } = new List<NicPropertiesDto>();
}

public record NicPropertiesDto
{
    [Required]
    public required string Name { get; set; }
    [Required]
    public required string LinkSpeed { get; set; }
    [Required]
    public required string Description { get; set; }
    public required string MacAddress { get; set; }
    [Required]
    public string? Status { get; set; }
}