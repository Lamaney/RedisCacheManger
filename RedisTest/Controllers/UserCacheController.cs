﻿using Microsoft.AspNetCore.Mvc;
using RedisTest.Models;

namespace RedisTest.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserCacheController(IRedisCacheService<User> redisCacheService) : ControllerBase
    {
        [HttpGet("get/{key}")]
        public async Task<User?> Get([FromRoute] string key)
        {
            return await redisCacheService.GetAsync(key);
        }
        
        [HttpPost("set/{key}")]
        public async Task<User> Set([FromRoute] string key, [FromBody] User user)
        {
            return await redisCacheService.SetAsync(key, user);
        }
        
        [HttpPut("update/{key}")]
        public async Task<User> Update([FromRoute] string key, [FromBody] User user)
        {
            return await redisCacheService.UpdateAsync(user, key);
        }
        
        [HttpDelete("delete/{key}")]
        public async Task Delete([FromRoute] string key)
        {
            await redisCacheService.RemoveAsync(key);
        }
        
        [HttpPost("setList/{key}")]
        public async Task<List<User>> SetList([FromRoute] string key, [FromBody] List<User> users)
        {
            return await redisCacheService.SetListAsync(key, users);
        }
        
        [HttpGet("getList/{key}")]
        public async Task<List<User>> GetList([FromRoute] string key)
        {
            return await redisCacheService.GetListAsync(key);
        }
        
        [HttpDelete("removeFromList/{listKey}")]
        public async Task RemoveFromList([FromRoute] string listKey, [FromBody] User user)
        {
            await redisCacheService.RemoveFromListAsync(listKey, user);
        }
        
        [HttpPost("addToList/{listKey}")]
        public async Task<User> AddToList([FromRoute] string listKey, [FromBody] User user)
        {
            return await redisCacheService.AddToListAsync(listKey, user);
        }
        
        [HttpDelete("deleteListItems/{listKey}")]
        public async Task<bool> DeleteListItems([FromRoute] string listKey)
        {
            return await redisCacheService.DeleteListsItemsAsync(listKey);
        }
        
        [HttpPost("setHash/{hashKey}/{entityKey}")]
        public async Task<bool> SetHash([FromRoute] string hashKey, [FromRoute] string entityKey, [FromBody] User user)
        {
            return await redisCacheService.AddEntryToHash(hashKey, entityKey, user);
        }
        
        [HttpGet("getEntityFromHash/{hashKey}/{entityKey}")]
        public async Task<User?> GetEntityFromHash([FromRoute] string hashKey, [FromRoute] string entityKey)
        {
            return await redisCacheService.GetEntityFromHashAsync(hashKey, entityKey);
        }
        
        [HttpPut("updateHashEntry/{hashKey}/{entityKey}")]
        public async Task<bool> UpdateHashEntry([FromRoute] string hashKey, [FromRoute] string entityKey, [FromBody] User user)
        {
            return await redisCacheService.UpdateHashEntryAsync(hashKey, entityKey, user);
        }
        
        [HttpDelete("deleteHashEntry/{hashKey}/{entityKey}")]
        public async Task<bool> DeleteHashEntry([FromRoute] string hashKey, [FromRoute] string entityKey)
        {
            return await redisCacheService.DeleteHashEntryAsync(hashKey, entityKey);
        }
        
        [HttpDelete("deleteHash/{hashKey}")]
        public async Task<bool> DeleteHash([FromRoute] string hashKey)
        {
            return await redisCacheService.DeleteHashAsync(hashKey);
        }
    }
}
