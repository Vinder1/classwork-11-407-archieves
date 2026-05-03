namespace AspNetApp.Dto;

public class MoveSpaceshipRequest
{
    public Guid OwnerId { get; set; }
    public int NewRadius { get; set; }
    public double NewAngle { get; set; }
}