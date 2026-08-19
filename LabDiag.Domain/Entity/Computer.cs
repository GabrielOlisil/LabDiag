namespace LabDiag.Domain.Entity;

public class Computer
{
    public Guid Id { get; set; }
    public string? HostName { get; set; }
    public ICollection<Nic> Nic { get; set; } = new List<Nic>();
    public Guid? ActiveNic { get; set; }
    public bool RegistredHost { get; set; } = false;
    public Lab? Lab { get; set; }
    public Guid? LabId { get; set; }
}

