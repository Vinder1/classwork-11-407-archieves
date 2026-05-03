namespace TaskTracker.Api.Domain.Entities;

public class Board
{
    public int Id { get; set; }
    
    public string Name { get; set; } = null!;
    
    // пусть будет
    public DateTime CreatedAtUtc { get; set; }

    public List<TaskItem> Tasks { get; set; } = [];

    public int ProjectId { get; set; }
    public Project Project { get; set; } = null!;
}