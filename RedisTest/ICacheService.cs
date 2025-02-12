﻿namespace RedisTest;

public interface ICacheService<T> where T : class
{
    Task<T?> GetAsync(string key);
    Task<T> UpdateAsync(T value,  string key);
    Task<T> SetAsync(string key, T value);
    Task RemoveAsync(string key);
    Task<List<T>> SetListAsync(string key, List<T> value);
    Task<List<T>> GetListAsync(string key);
    Task RemoveFromListAsync(string listKey, T value);
    Task<T> AddToListAsync(string listKey, T value);
    Task<bool> DeleteListsItemsAsync(string listKey);
}
