namespace LabDiag.Domain.DTO;

public record ComputerRegisterDTO
{
    public ICollection<NicPropertiesDTO> Nics { get; set; } = new List<NicPropertiesDTO>();
}

public record NicPropertiesDTO
{
    public string Name { get; set; }
    public string LinkSpeed { get; set; }
    public string Description { get; set; }
    public string MacAddress { get; set; }
    public string Status { get; set; }
}