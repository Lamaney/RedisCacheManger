﻿using System.Text.Json;
using ServiceStack.Redis;
using System.Threading.Tasks;

namespace RedisTest;

public class CacheService<T>(
    IRedisClientsManagerAsync cacheManager)
    : IDisposable, IAsyncDisposable, ICacheService<T>
    where T : class
{
    //Key üzerinden veri çekme
    public async Task<T?> GetAsync(string key)
    {
        await using var client = await cacheManager.GetClientAsync();
        string value = await client.GetValueAsync(key);
        return !string.IsNullOrEmpty(value) ? JsonSerializer.Deserialize<T>(value) : null;
    }

    //Key üzerinden veri günceller
    public async Task<T> UpdateAsync(T value,  string key)
    {
        await using var client = await cacheManager.GetClientAsync();
        string oldValue = await client.GetValueAsync(key);
       
        if (string.IsNullOrEmpty(oldValue))
        {
            throw new InvalidOperationException($"Bu anahtar '{key}' için veri bulunamadı.");
        }
       
        if (!await client.ReplaceAsync(key, value))
        {
            throw new InvalidOperationException($"Bu anahtar '{key}' için güncelleme işlemi yapılırken hata oluştu.");
        }
        
        return value;
    }

    //Key üzerinden veri ekler
    public async Task<T> SetAsync(string key, T value)
    {
        await using var client = await cacheManager.GetClientAsync();
        if (string.IsNullOrEmpty(key))
        {
            throw new ArgumentNullException(nameof(key));
        }

        if (!await client.AddAsync(key, value))
        {
            throw new InvalidOperationException("Veri eklenirken bir hata oluştu.");
        }

        return value;
    }
    //Key üzerinden veri siler
    public async Task RemoveAsync(string key)
    {
        await using var client = await cacheManager.GetClientAsync();
        await client.RemoveAsync(key);
    }

    //Yeni bir liste oluşturur ve elemanları ekler
    public async Task<List<T>> SetListAsync(string key, List<T> value)
    {
        if (string.IsNullOrEmpty(key))
        {
            throw new ArgumentNullException(nameof(key));
        }
        
        if (value == null || value.Count == 0)
        {
            throw new InvalidOperationException("Boş listeler eklenemez.");
        }

        await using var client = await cacheManager.GetClientAsync();

        if (await client.ContainsKeyAsync(key))
        {
            throw new InvalidOperationException($"Anahtar '{key}' ile ilişkili bir liste zaten mevcut.");
        }
        
        foreach (var item in value)
        {
            await client.AddItemToListAsync(key, JsonSerializer.Serialize(item));
        }

        return value;
    }

    //Listeyi ve elemanları getirir 
    public async Task<List<T>> GetListAsync(string key)
    {
        if (string.IsNullOrWhiteSpace(key))
        {
            throw new ArgumentNullException(nameof(key));
        }

        await using var client = await cacheManager.GetClientAsync();
        List<string> items = await client.GetAllItemsFromListAsync(key);

        if (items == null || items.Count == 0)
        {
            return [];
        }

        var entityList = new List<T>(items.Count);
        foreach (string item in items)
        {
            entityList.Add(JsonSerializer.Deserialize<T>(item));
        }
        
        return entityList;
    }

    //Listeden eleman siler
    public async Task RemoveFromListAsync(string listKey, T value)
    {
        await using var client = await cacheManager.GetClientAsync();
        string serializedValue = JsonSerializer.Serialize(value);
        await client.RemoveItemFromListAsync(listKey, serializedValue);
    }

    //Listeye eleman ekler
    public async Task<T> AddToListAsync(string listKey, T value)
    {
        if (string.IsNullOrEmpty(listKey))
        {
            throw new ArgumentNullException(nameof(listKey));
        }

        await using var client = await cacheManager.GetClientAsync();
        await client.AddItemToListAsync(listKey, JsonSerializer.Serialize(value));
        return value;
    }

    //Listeyi ve elemanlarını siler.
    public async Task<bool> DeleteListsItemsAsync(string listKey)
    {
        await using var client = await cacheManager.GetClientAsync();
        await client.RemoveAllFromListAsync(listKey);
        return true;
    }


    public void Dispose()
    {
        if (cacheManager is IDisposable cacheManagerDisposable)
            cacheManagerDisposable.Dispose();
        else
            DisposeAsync().AsTask().GetAwaiter().GetResult();
        GC.SuppressFinalize(this);
    }

    public async ValueTask DisposeAsync()
    {
        if (cacheManager is IAsyncDisposable asyncDisposable)
            await asyncDisposable.DisposeAsync();
        else if (cacheManager is IDisposable disposable)
            disposable.Dispose();
        GC.SuppressFinalize(this);
    }
}