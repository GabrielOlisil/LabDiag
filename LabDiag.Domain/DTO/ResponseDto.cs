using LabDiag.Domain.Entity;

namespace LabDiag.Domain.DTO;

public record ComputerResponseDto(Guid Id, string HostName, ICollection<NicResponseDto> Nic, Guid? ActiveNic);


public record LabResponseDto(Guid Id, string Name, string Description, string IpRange, ICollection<ComputerResponseDto> Computers);


public record NicResponseDto(Guid Id, string? MacAddress, string? Name, string? LinkSpeed);