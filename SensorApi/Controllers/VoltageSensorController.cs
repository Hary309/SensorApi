using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SensorApi.Data.Dto;
using SensorApi.Data.Models;
using SensorApi.Services;
using System;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace SensorApi.Controllers
{
    [ApiController]
    [Route("voltage")]
    public class VoltageSensorController : ControllerBase
    {
        ILogger<VoltageSensorController> logger;

        IVoltageSensorService voltageSensorService;

        IVoltagePredictService voltagePredictService;

        public VoltageSensorController(ILogger<VoltageSensorController> logger, IVoltageSensorService service, IVoltagePredictService predictService)
        {
            this.logger = logger;
            voltageSensorService = service;
            voltagePredictService = predictService;
        }

        [HttpGet("recent")]
        public ActionResult<VoltageSensorEntry> Recent()
        {
            return voltageSensorService.GetLatest();
        }

        [HttpGet("predict")]
        public double Predict([FromQuery] int hours = 0, [FromQuery] int minutes = 0, [FromQuery] int seconds = 0)
        {
            TimeSpan timeSpan = new TimeSpan(hours, minutes, seconds);
            var data = voltageSensorService.List();

            return voltagePredictService.Predict(timeSpan, data);
        }

        [HttpGet("ws")]
        public async Task GetAsync()
        {
            if (HttpContext.WebSockets.IsWebSocketRequest)
            {
                using var webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
                await Echo(webSocket);
            }
            else
            {
                HttpContext.Response.StatusCode = 400;
            }
        }

        private async Task Echo(WebSocket webSocket)
        {
            var buffer = new byte[1024 * 4];

            WebSocketReceiveResult result = null;

            long dataCount = 0;

            do
            {
                result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

                try
                {
                    if (result.Count > 0)
                    {
                        string data = Encoding.UTF8.GetString(buffer, 0, result.Count);
                        VoltageSensorEntryDto dto = JsonSerializer.Deserialize<VoltageSensorEntryDto>(data);
                        voltageSensorService.Add(dto.Voltage, dto.Error);
                        dataCount++;
                    }
                }
                catch (JsonException e)
                {
                    logger.LogWarning(e, "Cannot deserialize json");
                }
                catch (ArgumentException e)
                {
                    logger.LogWarning(e.Message);
                }
            } while (!result.CloseStatus.HasValue);

            logger.LogInformation($"WebSocket closed connection, added {dataCount} entries");

            await webSocket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
        }
    }
}
