using IoTApplication.Models;
using IoTApplication.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Devices;

namespace IoTApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PropertiesController : ControllerBase
    {
        private readonly IUpdatePropertiesRepository _updatePropertiesRepository;
        private readonly RegistryManager _registryManager;

        public PropertiesController(IUpdatePropertiesRepository updatePropertiesRepository)
        {
            _updatePropertiesRepository = updatePropertiesRepository;
            _registryManager = RegistryManager.CreateFromConnectionString("HostName=charithasri.azure-devices.net;SharedAccessKeyName=iothubowner;SharedAccessKey=WgGdamxvXm2vHXHqZWAZZ7FliYbjDp4PXna0IJly1jE=");
        }

        [HttpGet("{deviceId}")]
        public async Task<ActionResult<DeviceTwinModel>> GetDeviceTwin(string deviceId)
        {
            var twin = await _updatePropertiesRepository.GetDeviceTwinAsync(deviceId);
            if (twin == null)
            {
                return NotFound();
            }

            return Ok(twin);
        }

        [HttpPost("reported")]
        public async Task<IActionResult> UpdateReportedProperties(string deviceId, Properties updatePropertiesModel)
        {
            await _updatePropertiesRepository.UpdateReportedProperties(deviceId, updatePropertiesModel);
            return Ok();
        }

        [HttpPost("desired")]
        public async Task<IActionResult> UpdateDesiredProperties(string deviceId, [FromBody] Properties updatePropertiesModel)
        {
            await _updatePropertiesRepository.UpdateDesiredPropertiesAsync(deviceId, updatePropertiesModel);
            return Ok();
        }
    }
}
