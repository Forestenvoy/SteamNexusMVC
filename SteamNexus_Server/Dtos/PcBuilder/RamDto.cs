namespace SteamNexus_Server.Dtos;

public class RamDto
{
    public int Id { get; set; }
    public string? Classification { get; set; }
    public string? Name { get; set; }
    public string? Specification { get; set; }
    public int Price { get; set; }
    public int Wattage { get; set; }
    public int? Recommend { get; set; }
    public int Size { get; set; }
}
