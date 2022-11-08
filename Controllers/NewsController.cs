using Microsoft.AspNetCore.Mvc;
using RedisCaching.CachingServices;
using RedisCaching.Models;

namespace RedisCaching.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewsController : ControllerBase
    {
        private readonly AppDbContext _dbContext;
        private readonly ICacheService _cacheService;
        private static object _lock = new object();


        public NewsController(AppDbContext appDbContext, ICacheService cacheService)
        {
            _dbContext = appDbContext;
            _cacheService = cacheService;
        }

        // It is better to use DTO object instead of direct model. 
        // News ======> NewsDTO  
        // Use AutoMapper to map model and DTO object

        // Get
        [HttpGet("news")]
        public IEnumerable<News> Get()
        {
            var cacheData = _cacheService.GetData<IEnumerable<News>>("news");

            if (cacheData != null)
            {
                return cacheData;
            }
            lock (_lock)
            {
                var expirationTime = DateTimeOffset.Now.AddMinutes(5.0);
                cacheData = _dbContext.News.ToList();
                _cacheService.SetData<IEnumerable<News>>("news", cacheData, expirationTime);
            }
            return cacheData;
        }

        // Get by id
        [HttpGet("newsbyid")]
        public News Get(string id)
        {
            News news;
            var cacheData = _cacheService.GetData<IEnumerable<News>>("news");
            if (cacheData != null)
            {
                news = cacheData.SingleOrDefault(c => c.Id == id);
                return news;
            }

            news = _dbContext.News.SingleOrDefault(c => c.Id == id);
            return news;
        }

        // Add
        [HttpPost("Add")]
        public async Task<News> Post(News news)
        {
            news.Id = Guid.NewGuid().ToString();
            news.PubDate = DateTime.Now;
            var obj = await _dbContext.News.AddAsync(news);
            _cacheService.RemoveData("news");
            _dbContext.SaveChanges();
            return obj.Entity;
        }

        // Update
        [HttpPut("Update")]
        public void Put(News news)
        {
            _dbContext.News.Update(news);
            _cacheService.RemoveData("news");
            _dbContext.SaveChanges();
        }

        // Delete
        [HttpDelete("Delete")]
        public void Delete(string id)
        {
            var filterData = _dbContext.News.SingleOrDefault(c => c.Id == id);
            _dbContext.Remove(filterData);
            _cacheService.RemoveData("news");
            _dbContext.SaveChanges();
        }

    }
}