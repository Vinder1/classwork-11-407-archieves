namespace AspNetApp.Dto;

public class UpdatePlayerNameRequest
{
    public Guid Id { get; init;} 
    public string Name { get; init; } = null!;
}