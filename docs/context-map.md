# CinemaTicketingSystem — DDD Context Map

## Bounded Contexts

| Bounded Context | Klasör | Temel Aggregate(ler) | Sorumluluk |
|---|---|---|---|
| **Catalog** | `BoundedContexts/Catalog` | `Cinema`, `Movie`, `CinemaHall` | Sinema ve film kataloğunu yönetir |
| **Scheduling** | `BoundedContexts/Scheduling` | `Schedule`, `MovieSnapshot`, `CinemaHallSnapshot` | Film gösterim programlarını oluşturur ve yönetir |
| **Ticketing** | `BoundedContexts/Ticketing` | `SeatHold`, `Reservation`, `TicketIssuance`, `Ticket` | Koltuk kilitleme, rezervasyon ve bilet kesme |
| **Purchases** | `BoundedContexts/Purchases` | `Purchase` | Ödeme ve satın alma işlemleri |
| **Accounts** | `BoundedContexts/Accounts` | `User`, `RefreshToken` | Kullanıcı kimliği ve oturum yönetimi |
| **Shared Kernel** | `CinemaTicketingSytem.Domain.Core` | — | Ortak base sınıflar, value object'ler, event arayüzleri |

---

## Context Map (İlişkiler)

```
┌─────────────────────────────────────────────────────────────────────────────┐
│                              SHARED KERNEL                                  │
│  Entity<T>, AggregateRoot<T>, ValueObject, IDomainEvent, IIntegrationEvent  │
│  Price, Duration, SeatPosition, ScreeningTechnology                         │
└───────────────────────────┬─────────────────────────────────────────────────┘
                            │  (hepsi kalıtır / kullanır)
        ┌───────────────────┼────────────────────────────┐
        │                   │                            │
        ▼                   ▼                            ▼

┌──────────────────┐   ┌──────────────────┐   ┌─────────────────┐
│    ACCOUNTS BC   │   │    CATALOG BC    │   │   PURCHASES BC  │
│  (Upstream / OHS)│   │  (Upstream / PL) │   │  (Upstream / PL)│
│                  │   │                  │   │                 │
│ • User           │   │ • Cinema         │   │ • Purchase      │
│ • RefreshToken   │   │ • CinemaHall     │   │                 │
│                  │   │ • Movie          │   │ PayerId ──────► │
│ UserId ─────────►│   │                  │   │  (= UserId)     │
└──────────────────┘   └────────┬─────────┘   └────────┬────────┘
        │ U/D                   │ U/D (PL+ACL)          │ U/D (Event)
        │ (CustomerId)          │ Integration Events    │ PurchaseCreated
        │                       ▼                       │ IntegrationEvent
        │              ┌────────────────┐               │
        │              │ SCHEDULING BC  │               │
        │              │ (Downstream)   │               │
        │              │                │               │
        │              │ • Schedule     │               │
        │              │ • MovieSnapshot│◄──────────────┤
        │              │ • HallSnapshot │  (ACL: kendi  │
        │              │                │   kopyasını   │
        │              │                │   tutar)      │
        │              └───────┬────────┘               │
        │                      │ U/D (Query)            │
        │                      │ IScheduleQueryService  │
        │                      ▼                        │
        │              ┌───────────────────────────┐    │
        └─────────────►│        TICKETING BC        │◄──┘
          U/D          │       (Downstream)         │
          (CustomerId) │                            │
                       │  ┌─────────────────────┐  │
                       │  │  Holds Sub-domain    │  │
                       │  │  • SeatHold          │  │
                       │  └──────────┬──────────-┘  │
                       │             │               │
                       │  ┌──────────▼────────────┐  │
                       │  │ Reservations Sub-domain│  │
                       │  │ • Reservation          │  │
                       │  │ • ReservationSeat      │  │
                       │  └──────────┬─────────────┘  │
                       │             │               │
                       │  ┌──────────▼────────────┐  │
                       │  │ Issuance Sub-domain   │  │
                       │  │ • TicketIssuance      │  │
                       │  │ • Ticket              │  │
                       │  └───────────────────────┘  │
                       │                            │
                       │ + ICatalogQueryService     │
                       │   (Catalog'u sorgular)     │
                       └───────────────────────────-┘
```

---

## İlişki Detayları

### 1. Catalog → Scheduling  
**Pattern: Customer-Supplier + Published Language + Anti-Corruption Layer (ACL)**

Catalog BC, iki integration event yayınlar:

| Integration Event | Scheduling'in Tepkisi |
|---|---|
| `MovieCreatedIntegrationEvent` | `MovieCreatedIntegrationEventHandler` → `MovieSnapshot` oluşturur |
| `CinemaHallCreatedIntegrationEvent` | `CinemaHallCreatedIntegrationEventHandler` → `CinemaHallSnapshot` oluşturur |

**ACL detayı:** Scheduling, Catalog'un `Movie` ve `CinemaHall` nesnelerine doğrudan bağımlı değildir. Bunun yerine kendi ihtiyaçlarına göre şekillendirilmiş `MovieSnapshot` ve `CinemaHallSnapshot` sınıflarını (yerel kopya) tutar. Bu ACL sayesinde Catalog'un iç değişiklikleri Scheduling'i etkilemez.

---

### 2. Catalog → Ticketing  
**Pattern: Customer-Supplier (Open Host Service / Query)**

`TicketIssuanceAppService` ve `ReservationAppService`, `ICatalogQueryService.GetCinemaInfo()` üzerinden Catalog'dan salon ve film bilgisi sorgular. Ticketing, Catalog'un upstream olduğunu kabul eder (conformist değil; kendi response modeline dönüştürür).

---

### 3. Scheduling → Ticketing  
**Pattern: Customer-Supplier (Open Host Service / Query)**

`ReservationAppService` ve `TicketIssuanceAppService`, `IScheduleQueryService.GetScheduleInfo()` ile gösterim saatini ve salon bilgisini okur. Ticketing, rezervasyon/bilet oluşturmadan önce gösterim zamanı kontrolünü bu servise delege eder.

---

### 4. Purchases → Ticketing  
**Pattern: Event-Driven Integration (Published Language)**

```
Purchase.Create()
    └─► PurchaseCreatedIntegrationEvent (yayınlanır)
            └─► PurchaseCreatedIntegrationEventHandler (Ticketing dinler)
                    ├─► TicketIssuance.Confirm()   (bilet onaylanır)
                    └─► SeatHold sil               (koltuk kilidi kaldırılır)
```

Purchases BC, ödeme tamamlandığında event yayınlar. Ticketing bu event'i tüketerek `TicketIssuance`'ı onaylar ve ilgili `SeatHold`'ları siler.

---

### 5. Accounts → Ticketing  
**Pattern: Upstream/Downstream (Conformist)**

Ticketing BC içindeki `CustomerId` value object'i, Accounts BC'nin `UserId`'sini sarmalar. Ticketing, Accounts modelini sorgulamaz; yalnızca kullanıcı kimliğini ID olarak taşır.

---

### 6. Accounts → Purchases  
**Pattern: Upstream/Downstream (Conformist)**

`PayerId` value object'i, Accounts BC'deki `UserId`'yi temsil eder. Satın alma işlemi sahibini bu ID ile ilişkilendirir.

---

## Ticketing BC İçi Akış

Ticketing BC, üç iç alt-etki alanına sahiptir. Bunlar birbirine değil, ortak servisleri (repository'ler) üzerinden koordine edilir:

```
1. SeatHold (Holds)
   Kullanıcı koltuğu seçtiğinde kısa süreli (5 dk) kilit oluşturulur.
   Domain Events: SeatHoldStarted, SeatHoldConfirmed

2. Reservation (Reservations)
   Aktif ve süresi dolmamış SeatHold'lar üzerinden rezervasyon oluşturulur.
   Domain Events: ReservationCreated, SeatReserved, ReservationConfirmed,
                  ReservationExpired, ReservationCanceled, SeatReservationReleased

3. TicketIssuance (Issuance)
   Rezervasyon ve SeatHold kontrol edildikten sonra bilet kesilebilir duruma gelir.
   Purchase tamamlandığında Confirmed statüsüne geçer.
   Domain Events: TicketIssuanceCreated, TicketIssuanceConfirmed, TicketReleased, TicketUsed
```

**Çakışma koruması:** Hem `ReservationAppService` hem `TicketIssuanceAppService`, yeni kayıt oluşturmadan önce mevcut onaylı `TicketIssuance` listesindeki koltuk pozisyonlarını **ve** aktif `SeatHold` listesini birleştirerek (`Concat + DistinctBy`) çakışma kontrolü yapar.

---

## Integration Event Akışı (Özet)

```
[Catalog BC]
    Movie.ctor()          ──► MovieCreatedIntegrationEvent
    Cinema.AddHall()      ──► CinemaHallCreatedIntegrationEvent

[Purchases BC]
    Purchase.ctor()       ──► PurchaseCreatedIntegrationEvent

[Scheduling BC dinler]
    MovieCreatedIntegrationEvent      → MovieSnapshot kaydet
    CinemaHallCreatedIntegrationEvent → CinemaHallSnapshot kaydet

[Ticketing BC dinler]
    PurchaseCreatedIntegrationEvent   → TicketIssuance onayla + SeatHold sil
```

---

## Domain Event Akışı (Özet)

```
[Catalog BC]
    Movie.ctor()              → MovieCreatedEvent
    Movie.StartShowing()      → MovieShowingStartedEvent
    Movie.EndShowing()        → MovieShowingEndedEvent
    Cinema.AddHall()          → CinemaHallCreatedEvent
    Movie.AssignToHall()      → MovieAssignedToHallEvent

[Ticketing / Holds]
    SeatHold.ctor()           → SeatHoldStarted
    SeatHold.ConfirmHold()    → SeatHoldConfirmed

[Ticketing / Reservations]
    Reservation.ctor()        → ReservationCreatedEvent
    Reservation.AddSeat()     → SeatReservedEvent
    Reservation.RemoveSeat()  → SeatReservationReleasedEvent
    Reservation.Confirm()     → ReservationConfirmedEvent
    Reservation.Cancel()      → ReservationCanceledEvent
    Reservation.Expire()      → ReservationExpiredEvent

[Ticketing / Issuance]
    TicketIssuance.ctor()     → TicketIssuanceCreatedEvent
    TicketIssuance.Confirm()  → TicketIssuanceConfirmedEvent
    Ticket.MarkAsUsed()       → TicketUsedEvent
```

---

## Upstream / Downstream Özeti

```
Accounts ──(U)──► Ticketing   (CustomerId = UserId)
Accounts ──(U)──► Purchases   (PayerId = UserId)
Catalog  ──(U)──► Scheduling  (Integration Event + ACL)
Catalog  ──(U)──► Ticketing   (ICatalogQueryService)
Scheduling──(U)──► Ticketing  (IScheduleQueryService)
Purchases──(U)──► Ticketing   (PurchaseCreatedIntegrationEvent)
```

**Ticketing BC en fazla bağımlılığa sahip downstream context'tir.**  
Catalog, Scheduling, Purchases ve Accounts'tan bilgi/event tüketir.
