using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace Caching1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BCDController : ControllerBase
    {
        private static readonly string[] TestingNames = new[]
        {
        "Vadher", "Bhavesh", "samarth", "vamarth", "sidhant"
    };

        private readonly ILogger<BCDController> _logger;
        private readonly IMemoryCache _cache;

        public BCDController(ILogger<BCDController> logger, IMemoryCache cache)
        {
            _logger = logger;
            _cache = cache;

        }


        [HttpGet(Name = "GetTest")]
        public IEnumerable<SampModel> Get()
        {
            const string cacheKey = "TestKey";
            const double cacheExpirationHours = 1;

            if (!_cache.TryGetValue(cacheKey, out IEnumerable<SampModel> samps))
            {
                samps = Enumerable.Range(1, 5).Select(index => new SampModel
                {
                    Name = TestingNames[index]
                })
                .ToArray();

                var cacheEntryOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(cacheExpirationHours)
                };

                _cache.Set(cacheKey, samps, cacheEntryOptions);
            }

            return samps;
        }
    }
}