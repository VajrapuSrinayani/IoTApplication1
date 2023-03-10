using IoTApplication.Models;
using IoTApplication.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Devices;

namespace IoTApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeviceController : ControllerBase
    {
        private static DeviceRepository deviceRepository;

        public DeviceController()
        {
            deviceRepository = new DeviceRepository();
        }

        // CREATE
        [HttpPost]
        [Route("create")]
        public async Task<string> AddDeviceAsync(Devices IoTDevice)
        {
            return await deviceRepository.AddDeviceAsync(IoTDevice);
        }

        // READ
        [HttpGet]
        [Route("retrieve/{deviceId}")]
        public async Task<Device> GetDeviceAsync(string deviceId)
        {
            return await deviceRepository.GetDeviceAsync(deviceId);
        }

        [HttpPut]
        [Route("update/{status}")]
        public async Task<string> UpdateDeviceStatusAsync(string deviceId, string status)
        {
            var result = await deviceRepository.UpdateDeviceStatusAsync(deviceId, status);
            return result;
        }

        // DELETE
        [HttpDelete]
        [Route("delete/{deviceId}")]
        public async Task<string> DeleteDeviceAsync(string deviceId)
        {
            var result = await deviceRepository.DeleteDeviceAsync(deviceId);
            return result;
        }
    }
}
