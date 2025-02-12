using Microsoft.AspNetCore.Mvc;
using RedisTest.Models;

namespace RedisTest.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserCacheController(ICacheService<User> cacheService) : ControllerBase
    {
        [HttpGet("get/{key}")]
        public async Task<User?> Get([FromRoute] string key)
        {
            return await cacheService.GetAsync(key);
        }
        
        [HttpPost("set/{key}")]
        public async Task<User> Set([FromRoute] string key, [FromBody] User user)
        {
            return await cacheService.SetAsync(key, user);
        }
        
        [HttpPut("update/{key}")]
        public async Task<User> Update([FromRoute] string key, [FromBody] User user)
        {
            return await cacheService.UpdateAsync(user, key);
        }
        
        [HttpDelete("delete/{key}")]
        public async Task Delete([FromRoute] string key)
        {
            await cacheService.RemoveAsync(key);
        }
        
        [HttpPost("setList/{key}")]
        public async Task<List<User>> SetList([FromRoute] string key, [FromBody] List<User> users)
        {
            return await cacheService.SetListAsync(key, users);
        }
        
        [HttpGet("getList/{key}")]
        public async Task<List<User>> GetList([FromRoute] string key)
        {
            return await cacheService.GetListAsync(key);
        }
        
        [HttpDelete("removeFromList/{listKey}")]
        public async Task RemoveFromList([FromRoute] string listKey, [FromBody] User user)
        {
            await cacheService.RemoveFromListAsync(listKey, user);
        }
        
        [HttpPost("addToList/{listKey}")]
        public async Task<User> AddToList([FromRoute] string listKey, [FromBody] User user)
        {
            return await cacheService.AddToListAsync(listKey, user);
        }
        
        [HttpDelete("deleteListItems/{listKey}")]
        public async Task<bool> DeleteListItems([FromRoute] string listKey)
        {
            return await cacheService.DeleteListsItemsAsync(listKey);
        }
    }
}
