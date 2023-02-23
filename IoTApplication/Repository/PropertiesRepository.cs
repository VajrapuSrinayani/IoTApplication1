using IoTApplication.Models;
using IoTApplication.Interface;
using Microsoft.Azure.Devices.Client;
using Microsoft.Azure.Devices.Shared;
using Microsoft.Azure.Devices;

namespace IoTApplication.Repository
{
    public class PropertiesRepository : IUpdatePropertiesRepository
    {
        private static string connectionString = "HostName=srinayaniiothub.azure-devices.net;SharedAccessKeyName=iothubowner;SharedAccessKey=pGoLZGOJIhGR3XI/7C+rYDUp5BSGRHlEt1pITyz6LrQ=";

        public PropertiesRepository()
        {
        }

        public static async Task<bool> IsDeviceAvailable(string deviceId)
        {
            var registrymanager = RegistryManager.CreateFromConnectionString(connectionString);
            Device device = await registrymanager.GetDeviceAsync(deviceId);
            if (device.Status == DeviceStatus.Enabled)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<DeviceTwinModel> GetDeviceTwinAsync(string deviceId)
        {
            var registryManager = RegistryManager.CreateFromConnectionString(connectionString);
            var twin = await registryManager.GetTwinAsync(deviceId);
            var desiredProperties = twin.Properties.Desired;
            var reportedProperties = twin.Properties.Reported;

            var desiredPropertiesDict = new Dictionary<string, object>();
            foreach (var property in desiredProperties.Cast<KeyValuePair<string, object>>())
            {
                desiredPropertiesDict[property.Key] = property.Value;
                Console.WriteLine(property.Value);
            }

            var reportedPropertiesDict = new Dictionary<string, object>();
            foreach (var property in reportedProperties.Cast<KeyValuePair<string, object>>())
            {
                reportedPropertiesDict[property.Key] = property.Value;
            }

            return new DeviceTwinModel
            {
                DeviceId = deviceId,
                DesiredProperties = desiredPropertiesDict,
                ReportedProperties = reportedPropertiesDict,
            };
        }

        public async Task<string> UpdateReportedProperties(string deviceId, Properties properties)
        {
            // Check if the device is available
            if (await IsDeviceAvailable(deviceId))
            {
                try
                {
                    var deviceClient = DeviceClient.CreateFromConnectionString(connectionString, deviceId, Microsoft.Azure.Devices.Client.TransportType.Mqtt);
                    //var twin = await deviceClient.GetTwinAsync();
                    var reportedProperties = new TwinCollection();
                    reportedProperties[properties.Key] = properties.Value;
                    await deviceClient.UpdateReportedPropertiesAsync(reportedProperties);
                    return "Updated the Reported Properties successfully";
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error sending telemetry data: {0}", ex.Message);
                    throw ex;
                }
            }
            return "Device is disabled";
        }

        public async Task<string> UpdateDesiredPropertiesAsync(string deviceId, Properties properties)
        {
            // Check if the device is available
            if (await IsDeviceAvailable(deviceId))
            {
                try
                {
                    var registryManager = RegistryManager.CreateFromConnectionString(connectionString);
                    var desiredProperties = new TwinCollection();
                    desiredProperties[properties.Key] = properties.Value;

                    var twin = await registryManager.GetTwinAsync(deviceId);
                    twin.Properties.Desired = desiredProperties;

                    await registryManager.UpdateTwinAsync(twin.DeviceId, twin, twin.ETag);
                    return "Updated the Desired Properties successfully";
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error sending telemetry data: {0}", ex.Message);
                    throw ex;
                }
            }
            return "Device is disabled";
        }
    }
}
