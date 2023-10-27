using System.Xml.Serialization;

namespace AnprEqs.Models;

[XmlRoot("EventNotificationAlert")]
public class EventNotificationAlert
{
    [XmlElement("eventType")]
    public string? EventType { get; set; }

    [XmlElement("dateTime")]
    public string? DateTime { get; set; }

    [XmlElement("licensePlate")]
    public string? LicensePlate { get; set; }
}