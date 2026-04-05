# EnterpriseTravelBooking — Kod İnceleme Raporu

Baştan sona yapılan inceleme sonucunda tespit edilen tüm eksiklikler, servis ve öncelik sırasına göre listelenmiştir.

---

## 🔴 KRİTİK — Hemen Düzeltilmeli

### Rentals.API

**1. Mimari Yapı: Tek Proje (En Büyük Sorun)**
- Sorun: CatalogService 4 ayrı proje (Domain / Application / Infrastructure / API) ile Clean Architecture uygularken, Rentals.API tek `.csproj` altında sadece klasörlerle taklit ediyor.
- Sonuç: Katmanlar arası bağımlılık kuralı derleyici tarafından zorlanamıyor. Application, Infrastructure'a serbestçe erişebiliyor.
- Çözüm: `Rentals.Domain`, `Rentals.Application`, `Rentals.Infrastructure`, `Rentals.API` şeklinde 4 ayrı proje oluşturulmalı.

**2. GetAllRentalsQueryHandler — Katman İhlali**
- Sorun: Application katmanında olan `GetAllRentalsQueryHandler`, `IRentalRepository` interface'i yerine doğrudan `RentalsDbContext`'e (Infrastructure) bağımlı.
- Dosya: `Application/Features/Rentals/Queries/GetAllRentals/GetAllRentalsQueryHandler.cs`
- Çözüm: `RentalsDbContext _context` kaldırılmalı, `IRentalRepository` inject edilmeli. `IRentalRepository.GetAllRentalsAsync()` metodu kullanılmalı.

**3. Notification.Service — RabbitMQ Host Hardcoded**
- Sorun: `rabbitMqHost` config'den okunuyor ama hiç kullanılmıyor. `cfg.Host("localhost", ...)` hardcoded yazılmış.
- Dosya: `Notification.Service/Program.cs` satır 17
- Sonuç: Docker ortamında container, `localhost`'u kendisi olarak görür, RabbitMQ container'ına ulaşamaz. Bağlantı kopar.
- Çözüm: `cfg.Host("localhost", ...)` → `cfg.Host(rabbitMqHost, ...)` olmalı.

**4. Notification.Service — appsettings.json Boş**
- Sorun: `appsettings.json` içinde yalnızca Logging ayarları var. SMTP ve RabbitMQ ayarları tanımlanmamış.
- Sonuç: Servis başladığında config bulamaz, `null` değerlerle SMTP bağlantısı kurmaya çalışır, hata verir.
- Çözüm: Aşağıdaki yapı eklenmeli:
```json
{
  "RabbitMQ": {
    "Host": "localhost",
    "Username": "guest",
    "Password": "guest"
  },
  "Smtp": {
    "Host": "smtp.gmail.com",
    "Port": "587",
    "Username": "...",
    "Password": "..."
  }
}
```

**5. Notification.Service — Exception Yutulması (Mesaj Kaybı)**
- Sorun: `RentalCreatedConsumer.Consume()` içinde exception yakalanıp loglanıyor ama tekrar fırlatılmıyor.
- Dosya: `Consumers/RentalCreatedConsumer.cs`
- Sonuç: Mail gönderimi başarısız olursa mesaj sessizce kayboluyor. MassTransit retry mekanizması devreye giremiyor.
- Çözüm: `catch` bloğunda loglama yapıldıktan sonra `throw;` eklenmeli, ya da MassTransit'in retry policy'si yapılandırılmalı.

---

## 🟠 ÖNEMLİ — Kısa Sürede Düzeltilmeli

### CatalogService

**6. JwtProvider — DateTime.Now Yerine UtcNow**
- Sorun: Token üretilirken `expires: DateTime.Now.AddMinutes(expiryMinutes)` kullanılmış.
- Dosya: `Catalog.Infrastructure/Security/JwtProvider.cs`
- Sonuç: JWT standardı UTC kullanır. Sunucu farklı timezone'daysa token süresi hatalı hesaplanır.
- Çözüm: `DateTime.Now` → `DateTime.UtcNow`

**7. UserRepository — Index Kullanılmayan Sorgu**
- Sorun: `u.Username.ToLower() == username.ToLower()` ifadesi veritabanındaki index'i bypass eder. Her satır için `ToLower()` hesaplanır.
- Dosya: `Catalog.Infrastructure/Repositories/UserRepository.cs`
- Sonuç: Kullanıcı tablosu büyüdükçe login sorgusu yavaşlar.
- Çözüm: `EF.Functions.ILike(u.Username, username)` kullanılmalı (PostgreSQL case-insensitive index tarar).

**8. UpdateVehicleCommandHandler — UpdatedDate Set Edilmiyor**
- Sorun: Araç güncellendiğinde `vehicleToUpdate.UpdatedDate` set edilmiyor.
- Dosya: `Catalog.Application/Features/Vehicles/Commands/UpdateVehicle/UpdateVehicleCommandHandler.cs`
- Çözüm 1: Handler içinde `vehicleToUpdate.UpdatedDate = DateTime.UtcNow;` eklenmeli.
- Çözüm 2 (Daha iyi): `CatalogDbContext.SaveChangesAsync` override'ına `EntityState.Modified` için otomatik `UpdatedDate` set edilmeli:
```csharp
foreach (var entry in ChangeTracker.Entries<BaseEntity>())
{
    if (entry.State == EntityState.Modified)
        entry.Entity.UpdatedDate = DateTime.UtcNow;
}
```

**9. Repository<T> — UpdateByIdAsync NotImplementedException**
- Sorun: `IRepository<T>` interface'inde tanımlı `UpdateByIdAsync` metodu `throw new NotImplementedException()` fırlatıyor.
- Dosya: `Catalog.Infrastructure/Repositories/Repository.cs`
- Sonuç: Derlenir ama çalışma zamanında exception fırlatır. Interface sözleşmesi yerine getirilmemiş.
- Çözüm: Implement edilmeli ya da interface'den kaldırılmalı.

**10. RegisterCommand — Dışarıdan Role Alınması (Güvenlik)**
- Sorun: `RegisterCommand`'da `Role` alanı request body'den geliyor. Herkes `"Role": "Admin"` göndererek Admin olabilir.
- Dosya: `Catalog.Application/Features/Auth/Commands/Register/RegisterCommand.cs`
- Çözüm: `Role` command'dan kaldırılmalı, handler içinde `Role = "User"` olarak hardcode edilmeli. Admin rolü ayrı bir endpoint ile verilmeli.

**11. RegisterCommandHandler — Username Unique Kontrolü Eksik**
- Sorun: Email uniqueness kontrol ediliyor ama username kontrol edilmiyor.
- Dosya: `Catalog.Application/Features/Auth/Commands/Register/RegisterCommandHandler.cs`
- Sonuç: Aynı username ile kayıt olunmaya çalışılırsa veritabanı seviyesinde hata fırlar (500), uygulama seviyesinde yakalanmıyor.
- Çözüm: `IUserRepository`'e `ExistsByUsernameAsync` eklenmeli ve handler'da kullanılmalı.

**12. Cache Invalidation Eksik**
- Sorun: Araç eklendiğinde, güncellendiğinde veya silindiğinde Redis cache temizlenmiyor.
- Etkilenen Handler'lar: `CreateVehicleCommandHandler`, `UpdateVehicleCommandHandler`, `DeleteVehicleCommandHandler`
- Sonuç: Kullanıcılar 10 dakika boyunca eski veriyi görür.
- Çözüm: İlgili handler'larda işlem sonrası `await _cache.RemoveAsync("VehiclesList");` çağrılmalı.

**13. GetVehicleByIdQuery — Cache Yok**
- Sorun: `GetVehicleByIdQuery`, `ICacheableQuery` implement etmiyor.
- Çözüm: `ICacheableQuery` implement edilmeli, `CacheKey => $"Vehicle_{Id}"` kullanılmalı. `UpdateVehicle` ve `DeleteVehicle` handler'larında bu key de invalidate edilmeli.

### Rentals.API

**14. GetAllRentalsQuery — DTO Kullanılmıyor**
- Sorun: Query doğrudan `List<Rental>` (domain entity) döndürüyor. `IsAvailable`, `CreatedAt` gibi iç alanlar dışarıya sızıyor.
- Dosya: `Application/Features/Rentals/Queries/GetAllRentals/GetAllRentalsQuery.cs`
- Çözüm: `RentalDto` oluşturulmalı, handler domain entity'yi DTO'ya map'lemeli.

**15. VehicleCreatedEvent — Yerel Kopya ve Yanlış Namespace**
- Sorun: `Application/Events/VehicleCreatedEvent.cs` dosyası hem gereksiz (Shared'deki kullanılıyor) hem de namespace'i yanlış (`Catalog.Application.Events`).
- Dosya: `Application/Events/VehicleCreatedEvent.cs`
- Çözüm: Bu dosya silinmeli.

### Notification.Service

**16. Worker.cs — Gereksiz Kod**
- Sorun: `Worker.cs` proje şablonundan kalan boilerplate kod. Her saniye log atıyor, hiçbir iş yapmıyor.
- Çözüm: `Worker.cs` silinmeli veya anlamlı bir health-check mekanizmasına dönüştürülmeli. `Program.cs`'deki `builder.Services.AddHostedService<Worker>()` satırı da kaldırılmalı.

---

## 🟡 KÜÇÜK — Teknik Borç

### Shared

**17. RentalCreatedEvent — Nullable Uyarıları**
- Sorun: `CustomerEmail` ve `VehicleInfo` property'lerinde `= string.Empty` default değer eksik.
- Dosya: `EnterpriseTravelBooking.Shared/Events/RentalCreatedEvent.cs`
- Çözüm: `public string CustomerEmail { get; init; } = string.Empty;`

### CatalogService

**18. Vehicle Entity — Nullable Uyarıları**
- Sorun: `Brand`, `Model`, `Currency` property'lerinde `= string.Empty` eksik. `<Nullable>enable</Nullable>` açık olduğu için derleyici uyarısı üretiyor.
- Dosya: `Catalog.Domain/Entities/Vehicle.cs`

**19. DeleteVehicle — Event Publish Edilmiyor**
- Sorun: Araç silindiğinde `VehicleDeletedEvent` yayınlanmıyor.
- Sonuç: Aktif kiralaması olan bir araç silinirse `RentalsService` haberdar olamıyor.
- Çözüm: `VehicleDeletedEvent` Shared'e eklenmeli, `DeleteVehicleCommandHandler`'da publish edilmeli. `RentalsService`'te consumer yazılmalı.

**20. UpdateVehicle — Event Publish Edilmiyor**
- Sorun: Araç fiyatı güncellendiğinde `VehicleUpdatedEvent` yayınlanmıyor.
- Sonuç: `RentalsService`'teki `RentableVehicle.DailyPrice` güncelleme almıyor, eski fiyatla kiralama hesaplanıyor.
- Çözüm: `VehicleUpdatedEvent` Shared'e eklenmeli, handler'da publish edilmeli, `RentalsService`'te consumer yazılmalı.

**21. UpdateVehicle — RESTful Route Eksik**
- Sorun: `[HttpPut]` attribute'unda `{id}` route parametresi yok. ID body'den geliyor.
- Dosya: `Catalog.API/Controllers/VehiclesController.cs`
- Çözüm: `[HttpPut("{id}")]` olmalı, ID route'tan alınıp command'a set edilmeli.

**22. UpdateVehicleCommand — Validator Yok**
- Sorun: `CreateVehicleCommand` için validator yazılmış ama `UpdateVehicleCommand` için yazılmamış. Boş string veya negatif fiyatla güncelleme yapılabilir.
- Çözüm: `UpdateVehicleCommandValidator` oluşturulmalı, `CreateVehicleCommandValidator` ile aynı kurallar uygulanmalı.

**23. DeleteVehicle — Validator Yok**
- Sorun: `DeleteVehicleCommand` için validator yok. Boş `Guid` ile istek gönderilebilir.
- Çözüm: `DeleteVehicleCommandValidator` oluşturulmalı:
```csharp
RuleFor(x => x.Id).NotEmpty().WithMessage("Silinecek araç ID'si boş olamaz.");
```

**24. GetAllVehicles — Tüm Veri Çekiliyor**
- Sorun: `GetAllVehiclesQuery`, 1000 araçlık seed data ile tüm tabloyu tek seferde getiriyor.
- Dosya: `Catalog.Application/Features/Vehicles/Queries/GetAllVehicles`
- Çözüm: Bu endpoint kaldırılmalı ya da sayfalama zorunlu hale getirilmeli. `GetPagedVehicles` kullanımı teşvik edilmeli.

**25. GetPagedVehicles — PageSize Sınırı Validator'da Değil Handler'da**
- Sorun: `PageSize > 50` kontrolü `GetPagedVehiclesQueryHandler` içinde yapılıyor. Ama `CacheKey` hesaplanmadan önce değiştirilmiyor, cache tutarsızlığına yol açıyor.
- Çözüm: Bu kural `GetPagedVehiclesQueryValidator`'a taşınmalı. Handler'daki manuel kontroller kaldırılmalı.

**26. CatalogDbContextFactory — Hardcoded Şifre**
- Sorun: `Password=123456` connection string'e gömülmüş.
- Dosya: `Catalog.Infrastructure/Persistence/CatalogDbContextFactory.cs`
- Not: Sadece migration araçları için kullanılıyor ama repository'e commit edilmemeli.
- Çözüm: `dotnet user-secrets` kullanılmalı ya da bu değer `.gitignore`'a eklenmeli.

**27. GlobalExceptionHandler — Yorum Numaralandırma Hatası**
- Sorun: `CatalogService` `GlobalExceptionHandler`'ında iki adet "3." numaralı yorum satırı var.
- Dosya: `Catalog.API/Exceptions/GlobalExceptionHandler.cs`

### Notification.Service

**28. SmtpClient Deprecated**
- Sorun: `System.Net.Mail.SmtpClient` .NET'te deprecated olarak işaretlenmiş.
- Çözüm: `MailKit` kütüphanesi kullanılmalı (`MimeKit` ile birlikte). Microsoft'un resmi tavsiyesi bu yönde.

---

## 📋 Özet Tablo

| # | Servis | Dosya | Öncelik |
|---|--------|-------|---------|
| 1 | Rentals.API | Tüm proje yapısı | 🔴 Kritik |
| 2 | Rentals.API | GetAllRentalsQueryHandler | 🔴 Kritik |
| 3 | Notification.Service | Program.cs | 🔴 Kritik |
| 4 | Notification.Service | appsettings.json | 🔴 Kritik |
| 5 | Notification.Service | RentalCreatedConsumer.cs | 🔴 Kritik |
| 6 | CatalogService | JwtProvider.cs | 🟠 Önemli |
| 7 | CatalogService | UserRepository.cs | 🟠 Önemli |
| 8 | CatalogService | UpdateVehicleCommandHandler.cs | 🟠 Önemli |
| 9 | CatalogService | Repository.cs | 🟠 Önemli |
| 10 | CatalogService | RegisterCommand.cs | 🟠 Önemli |
| 11 | CatalogService | RegisterCommandHandler.cs | 🟠 Önemli |
| 12 | CatalogService | Create/Update/DeleteVehicleCommandHandler | 🟠 Önemli |
| 13 | CatalogService | GetVehicleByIdQuery.cs | 🟠 Önemli |
| 14 | Rentals.API | GetAllRentalsQuery.cs | 🟠 Önemli |
| 15 | Rentals.API | Application/Events/VehicleCreatedEvent.cs | 🟠 Önemli |
| 16 | Notification.Service | Worker.cs | 🟠 Önemli |
| 17 | Shared | RentalCreatedEvent.cs | 🟡 Küçük |
| 18 | CatalogService | Vehicle.cs | 🟡 Küçük |
| 19 | CatalogService | DeleteVehicleCommandHandler.cs | 🟡 Küçük |
| 20 | CatalogService | UpdateVehicleCommandHandler.cs | 🟡 Küçük |
| 21 | CatalogService | VehiclesController.cs | 🟡 Küçük |
| 22 | CatalogService | UpdateVehicleCommand | 🟡 Küçük |
| 23 | CatalogService | DeleteVehicleCommand | 🟡 Küçük |
| 24 | CatalogService | GetAllVehiclesQuery | 🟡 Küçük |
| 25 | CatalogService | GetPagedVehiclesQueryHandler | 🟡 Küçük |
| 26 | CatalogService | CatalogDbContextFactory.cs | 🟡 Küçük |
| 27 | CatalogService | GlobalExceptionHandler.cs | 🟡 Küçük |
| 28 | Notification.Service | RentalCreatedConsumer.cs | 🟡 Küçük |
