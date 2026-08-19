namespace LabDiag.Domain.Entity;

public class Lab
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string IpRange { get; set; }
    public ICollection<Computer> Computers { get; set; } =  new List<Computer>();
}