using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace App.UploadService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProcessManagerController : ControllerBase
    {
        private readonly ILogger<string> _logger;

        public ProcessManagerController(ILogger<string> logger)
        {
            _logger = logger;
        }
        [HttpPost]
        [Route("Start")]
        public async void Start()
        {
            await CallSlowApi();
        }
        private async Task CallSlowApi()
        {
            _logger.LogInformation($"Starting at {DateTime.UtcNow.TimeOfDay}");
            await Task.Delay(10000);
            _logger.LogInformation($"Done at {DateTime.UtcNow.TimeOfDay}");
        }
    }
}
