using IoTApplication.Models;

namespace IoTApplication.Interface
{
    public interface IUpdatePropertiesRepository
    {
        Task<DeviceTwinModel> GetDeviceTwinAsync(string deviceId);
        Task<string> UpdateReportedProperties(string deviceId, Properties updatePropertiesModel);
        Task<string> UpdateDesiredPropertiesAsync(string deviceId, Properties updatePropertiesModel);
    }
}
