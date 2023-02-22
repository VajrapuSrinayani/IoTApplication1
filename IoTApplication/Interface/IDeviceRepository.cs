using IoTApplication.Models;
using Microsoft.Azure.Devices;

namespace IoTApplication.Interface
{
    public interface IDeviceRepository
    {
        Task<string> AddDeviceAsync(Devices IoTDevice);
        Task<Device> GetDeviceAsync(string deviceId);
        Task<string> UpdateDeviceStatusAsync(string deviceId, string status);
        Task<string> DeleteDeviceAsync(string deviceId);
    }
}
