namespace AH.CancerConnect.API.Features.OrderEventNotification;

public class OrderEventNotificationRequest
{
    public string? EventId { get; set; }

    public DateTime? EventTimeStamp { get; set; }

    public string? EventType { get; set; }

    public string? EventContext { get; set; }

    public string? OrderId { get; set; }

    public string? Deployment { get; set; }

    public string? PartnerId { get; set; }

    public string? ProgramId { get; set; }

    public PatientIdentity? PatientIdentity { get; set; }

    //public List<IdModel>? Ids { get; set; }

    //public List<IdModel>? HistoricalIds { get; set; }
}

public class PatientIdentity
{
    public List<IdModel>? Ids { get; set; }

    public List<IdModel>? HistoricalIds { get; set; }
}

public class IdModel
{
    public string? Id { get; set; }

    public string? Type { get; set; }

    public string? Origin { get; set; }
}