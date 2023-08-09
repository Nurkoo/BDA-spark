namespace BigDataAcademy.Model.Event;

public class IntegrationEvent
{
    public Guid EventId { get; set; }

    public required string Type { get; set; }

    public required string Body { get; set; }

    public required DateTime ProcessAt { get; set; }

    public required bool Processed { get; set; }
}
