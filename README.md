# RedisTest

Bu proje, Redis kullanarak bir önbellekleme hizmeti sağlayan bir C# uygulamasıdır. Redis ile etkileşim kurmak için veri ekleme, güncelleme, alma ve silme gibi çeşitli yöntemler içerir.


## Docker ile Redis Çalıştırma

Eğer bir Redis sunucunuz yoksa, Docker kullanarak Redis'i çalıştırabilirsiniz:

```sh
docker run --name redis -d -p 6379:6379 redis
```

## Gereksinimler

- .NET 9.0 veya daha yeni bir sürüm
- Redis sunucusu
- JetBrains Rider 2024.3.4 (veya başka bir uyumlu IDE)
- Docker (isteğe bağlı, Redis'i bir konteynerde çalıştırmak için)

Redis Cache Service üzerindeki methodları inceleyebilirsiniz
