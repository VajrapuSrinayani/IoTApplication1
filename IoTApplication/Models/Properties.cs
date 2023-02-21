namespace IoTApplication.Models
{
    public class Properties
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }
    public class DeviceTwinModel
    {
        public string DeviceId { get; set; }
        public Dictionary<string, object> DesiredProperties { get; set; }
        public Dictionary<string, object> ReportedProperties { get; set; }
    }
}
