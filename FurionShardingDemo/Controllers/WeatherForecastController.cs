using Furion.DatabaseAccessor;
using FurionShardingDemo.EfCores;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FurionShardingDemo.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IRepository<TodoItem> _todoItemRepository;

        public WeatherForecastController(ILogger<WeatherForecastController> logger,IRepository<TodoItem> todoItemRepository)
        {
            _logger = logger;
            _todoItemRepository = todoItemRepository;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }
        [HttpGet]
        public async Task<IActionResult> Add()
        {
            await _todoItemRepository.InsertAsync(new TodoItem() { Id = Guid.NewGuid().ToString("n"), Name = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") });
            await _todoItemRepository.SaveNowAsync();
            return Ok();
        }
        [HttpGet]
        public async Task<IActionResult> List()
        {
            var list = await _todoItemRepository.AsQueryable().ToListAsync();
            return Ok(list);
        }
    }
}