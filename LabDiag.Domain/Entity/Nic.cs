namespace LabDiag.Domain.Entity;

public class Nic
{
    public Guid Id { get; set; }
    public Computer Computer { get; set; } = null!; 
    public Guid ComputerId { get; set; }
    public string? MacAddress { get; set; }
    public string? Name { get; set; }
    public string? LinkSpeed { get; set; }
}
