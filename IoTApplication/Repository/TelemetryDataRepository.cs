using IoTApplication.Models;
using Microsoft.Azure.Devices.Client;
using Microsoft.Azure.Devices;
using Newtonsoft.Json;
using System.Text;

namespace IoTApplication.Repository
{
    public class TelemetryDataRepository
    {
        private static string connectionString = "";

        public TelemetryDataRepository()
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

        public async Task<string> SendTelemetryData(string deviceId, TelemetryData telemetryData)
        {
            // Check if the device is available
            if (await IsDeviceAvailable(deviceId))
            {
                try
                {
                    var deviceClient = DeviceClient.CreateFromConnectionString(connectionString, deviceId, Microsoft.Azure.Devices.Client.TransportType.Mqtt);

                    var telemetryDataPoint = new
                    {
                        data = telemetryData.Data,
                    };

                    var messageString = JsonConvert.SerializeObject(telemetryDataPoint);

                    var message = new Microsoft.Azure.Devices.Client.Message(Encoding.ASCII.GetBytes(messageString));

                    message.Properties.Add("deviceId", deviceId);

                    await deviceClient.SendEventAsync(message);

                    return "Telemetry data sent successfully";
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
