using Microsoft.OpenApi.Models;
using RedisTest;
using ServiceStack.Redis;

var builder = WebApplication.CreateBuilder(args); 

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Redis API", Version = "v1" });
});

string? redisConnectionString = builder.Configuration.GetConnectionString("RedisConnection");
builder.Services.AddSingleton<IRedisClientsManagerAsync>(new RedisManagerPool(redisConnectionString));
builder.Services.AddSingleton(typeof(IRedisCacheService<>), typeof(RedisRedisCacheService<>));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Redis API v1"));
}

app.UseHttpsRedirection();
app.MapControllers();
app.Run();