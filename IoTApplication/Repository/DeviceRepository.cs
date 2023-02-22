using Microsoft.Azure.Devices;
using IoTApplication.Models;
using IoTApplication.Interface;

namespace IoTApplication.Repository
{
    public class DeviceRepository : IDeviceRepository
    {
        private static string connectionString = "HostName=iothub101.azure-devices.net;SharedAccessKeyName=iothubowner;SharedAccessKey=tCxaeJBez3s8UrZjg28mIfjmXbzpjIuBir0ljBNP3P0=";
        private static RegistryManager registryManager;

        public DeviceRepository()
        {
            registryManager = RegistryManager.CreateFromConnectionString(connectionString);
        }

        // CREATE
        public async Task<string> AddDeviceAsync(Devices IoTDevice)
        {
            var device = new Device(IoTDevice.DeviceId);
            Device createdDevice = await registryManager.AddDeviceAsync(device);
            return $"Primary key: {createdDevice.Authentication.SymmetricKey.PrimaryKey}";
        }

        // READ
        public async Task<Device> GetDeviceAsync(string deviceId)
        {
            Device device = await registryManager.GetDeviceAsync(deviceId);
            return device;
        }

        //UPDATE
        public async Task<string> UpdateDeviceStatusAsync(string deviceId, string status)
        {
            Device device = await registryManager.GetDeviceAsync(deviceId);
            device.Status = (DeviceStatus)Enum.Parse(typeof(DeviceStatus), status, true);
            await registryManager.UpdateDeviceAsync(device);
            return $"Updated the {device.Status} of the device";
        }


        // DELETE
        public async Task<string> DeleteDeviceAsync(string deviceId)
        {
            await registryManager.RemoveDeviceAsync(deviceId);
            return $"Deleted device with id: {deviceId}";
        }
    }
}
