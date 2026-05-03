namespace AspNetApp.Dto;

/// <summary>
/// DTO для параметров звезды
/// </summary>
public class StarVisitRequest
{
    /// <summary>
    /// Владелец корабля
    /// </summary>
    public Guid OwnerId { get; set;}

    /// <summary>
    /// Радиус звезды
    /// </summary>
    public int Radius { get; set; }

    /// <summary>
    /// Угол звезды в миллирадианах
    /// </summary>
    public int AngleMilliradians { get; set; }
}