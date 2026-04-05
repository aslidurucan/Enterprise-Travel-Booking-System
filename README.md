# Enterprise Travel Booking System

> ⚠️ **Bu proje aktif geliştirme aşamasındadır.**

.NET 8 ile geliştirilmiş dağıtık bir araç kiralama ve rezervasyon sistemi. Microservices, Clean Architecture, CQRS ve Event-Driven mimari pattern'lerini içermektedir.

---

## Mimari

```
EnterpriseTravelBooking/
├── Catalog.API                  # Araç yönetimi servisi
│   ├── Catalog.Domain
│   ├── Catalog.Application
│   └── Catalog.Infrastructure
├── Rentals.API                  # Kiralama servisi
│   ├── Rentals.Domain
│   ├── Rentals.Application
│   └── Rentals.Infrastructure
├── Notification.Service         # Bildirim servisi (RabbitMQ consumer)
├── EnterpriseTravelBooking.Shared  # Paylaşılan event modelleri
└── docker-compose.yml
```

## Kullanılan Teknolojiler

- **.NET 8** — Web API
- **PostgreSQL** — Veritabanı
- **RabbitMQ + MassTransit** — Servisler arası event-driven iletişim
- **MediatR** — CQRS pattern
- **FluentValidation** — İstek doğrulama
- **Redis** — Distributed caching
- **JWT** — Kimlik doğrulama ve yetkilendirme
- **Entity Framework Core** — ORM
- **Serilog + Seq** — Structured logging
- **Docker Compose** — Altyapı yönetimi

---

## Mevcut Özellikler

### Catalog.API
- [x] Araç CRUD işlemleri
- [x] JWT kimlik doğrulama (Register / Login)
- [x] Rol tabanlı yetkilendirme (Admin / User)
- [x] Sayfalandırmalı araç listeleme
- [x] Soft delete mekanizması
- [x] Redis ile response caching
- [x] FluentValidation pipeline
- [x] Global exception handling
- [x] RabbitMQ üzerinden VehicleCreated / Updated / Deleted event yayını

### Rentals.API
- [x] Kiralama oluşturma (JWT korumalı)
- [x] Tüm kiralamaları listeleme
- [x] RabbitMQ ile araç verisini senkronize etme (VehicleCreated / Updated / Deleted)
- [x] Clean Architecture katmanları (Domain / Application / Infrastructure)
- [x] FluentValidation pipeline
- [x] Global exception handling

### Notification.Service
- [x] RabbitMQ üzerinden RentalCreatedEvent dinleme
- [x] MassTransit consumer

---

## Geliştirme Aşamasındaki Özellikler

- [ ] Catalog.API — VehicleUpdated / VehicleDeleted event consumer (Rentals tarafında)
- [ ] Rentals.API — GetAllRentals endpoint yetkilendirmesi
- [ ] Güvenlik iyileştirmeleri (JWT secret key, RabbitMQ credentials)
- [ ] Kullanıcı servisi (ayrı mikroservis)
- [ ] API Gateway
- [ ] Health check endpoint'leri
- [ ] CI/CD pipeline

---

## Kurulum

### Gereksinimler
- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- [Docker Desktop](https://www.docker.com/products/docker-desktop)

### Çalıştırma

```bash
# Altyapıyı başlat (PostgreSQL, RabbitMQ, Redis, Seq)
docker-compose up -d

# Servisleri çalıştır
dotnet run --project Catalog.API
dotnet run --project Rentals.API
dotnet run --project Notification.Service
```

### Seq (Log Arayüzü)
`http://localhost:5341` adresinden erişilebilir.
