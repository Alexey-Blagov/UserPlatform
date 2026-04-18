# UserPlatform — микросервисное приложение с Keycloak, API Gateway и Clean Architecture

## Что входит в solution

- **Keycloak** — отдельный сервер аутентификации и авторизации.
- **API Gateway (YARP)** — единая точка входа для frontend.
- **UserProfileService** — бизнес-микросервис с профилем и адресом пользователя.
- **AdminService** — защищённый административный API.
- **Frontend (React + TypeScript + Vite)** — пользовательский UI.
- **PostgreSQL** — БД для профиля и отдельная БД для Keycloak.

---

## 1. Архитектура

```text
[ React + Vite ]
       |
       v
[ API Gateway ]
       |
       +----------------------------+
       |                            |
       v                            v
[ UserProfileService ]        [ AdminService ]
       |                            
       v                            
[ PostgreSQL ]                     

[ Keycloak ] --- выдаёт токены, роли и сессии
```

### Почему именно так

- **Keycloak** снимает с твоих сервисов задачи логина, хранения паролей, выдачи JWT и управления сессиями.
- **Gateway** централизует маршрутизацию и базовую защиту входящего трафика.
- **UserProfileService** фокусируется только на бизнес-данных.
- **AdminService** отделён от пользовательского API и защищён ролью `admin`.

---

## 2. Подробная структура каталогов

```text
src/
  ApiGateway/
    UserPlatform.ApiGateway/
  BuildingBlocks/
    BuildingBlocks.Application/
    BuildingBlocks.Domain/
    BuildingBlocks.Infrastructure/
  Services/
    UserProfileService/
      UserProfileService.API/
      UserProfileService.Application/
      UserProfileService.Domain/
      UserProfileService.Infrastructure/
      UserProfileService.Contracts/
    AdminService/
      AdminService.API/
      AdminService.Application/
      AdminService.Domain/
      AdminService.Infrastructure/
      AdminService.Contracts/
  Frontend/
    user-platform-web/
deploy/
  docker-compose.yml
  keycloak/
  sql/
tests/
  UserProfileService.UnitTests/
  AdminService.UnitTests/
```

---

## 3. Что делает каждая сущность в коде

### BuildingBlocks.Domain/Entity.cs
Базовый класс для доменных сущностей.
- Хранит `Id`
- Хранит список доменных событий
- Нужен, чтобы все агрегаты имели единый базовый контракт

### BuildingBlocks.Domain/ValueObject.cs
Базовый класс для value objects.
- Реализует сравнение по значению, а не по ссылке
- Используется для `Address`

### BuildingBlocks.Application/Abstractions/ICurrentUser.cs
Абстракция текущего пользователя.
- Не даёт Application слою зависеть от `HttpContext`
- Возвращает `Subject`, `Email`, `Roles`

### BuildingBlocks.Infrastructure/Security/HttpContextCurrentUser.cs
Реализация `ICurrentUser` поверх `HttpContext`.
- Читает claims из JWT
- Используется API слоем через DI

---

## 4. UserProfileService по слоям

### Domain/Entities/UserProfile.cs
Главная доменная сущность профиля пользователя.
- `ExternalIdentityId` — идентификатор пользователя из Keycloak (`sub`)
- `FirstName`, `LastName` — базовые пользовательские данные
- `Address` — value object адреса

### Domain/ValueObjects/Address.cs
Value object адреса.
- страна, город, улица, дом, индекс
- value object нужен потому, что адрес — логически единый блок значения

### Domain/Repositories/IUserProfileRepository.cs
Контракт доступа к данным профиля.
- Application слой знает только интерфейс
- Infrastructure слой даёт реализацию через EF Core

### Application/Queries/GetMyProfile
Сценарий чтения своего профиля.
- Handler берёт `sub` из `ICurrentUser`
- Ищет профиль по `ExternalIdentityId`
- Возвращает DTO

### Application/Commands/UpsertMyAddress
Сценарий создания или обновления адреса.
- Если профиль не найден — создаётся
- Если профиль найден — адрес обновляется

### Infrastructure/Persistence/UserProfileDbContext.cs
EF Core контекст для PostgreSQL.
- описывает таблицу `user_profiles`
- настраивает owned type для `Address`

### Infrastructure/Persistence/UserProfileRepository.cs
Репозиторий EF Core.
- реализует чтение и сохранение профиля

### API/Program.cs
Точка сборки сервиса.
- регистрирует DI
- подключает EF Core
- подключает JwtBearer
- маппит роли из Keycloak claims
- включает Swagger

### API/Controllers/ProfileController.cs
HTTP-контроллер пользовательского профиля.
- `GET /api/profile/me`
- `PUT /api/profile/me/address`

---

## 5. AdminService по слоям

### Application/Queries/GetSystemSummary
Простой admin use case.
- пример защищённого endpoint
- доступ только для роли `admin`

### API/Controllers/AdminController.cs
HTTP-контроллер для admin операций.
- `GET /api/admin/summary`

---

## 6. Gateway

### Program.cs
- Подключает JWT bearer проверку
- Загружает маршруты из `appsettings.json`
- Применяет policies:
  - `authenticated`
  - `admin`

### appsettings.json
- Маршрутизирует `/api/profile/*` -> `userprofileservice`
- Маршрутизирует `/api/admin/*` -> `adminservice`

---

## 7. Frontend

### src/keycloak.ts
Конфигурация клиента Keycloak.
- URL Keycloak
- Realm
- ClientId

### src/api.ts
Axios-клиент.
- подставляет bearer token
- обновляет токен перед запросом

### src/App.tsx
Главный UI:
- профиль пользователя
- редактирование адреса
- admin page
- logout

---

## 8. Локальное развёртывание на Windows

### Требования

1. **.NET 8 SDK**
2. **Node.js 20.19+ или 22.12+**
3. **Docker Desktop**
4. **Visual Studio 2022 / VS Code / Rider**
5. **Git**

> В этом архиве код подготовлен как стартовый каркас. Я не мог прогнать здесь `dotnet build` и миграции, потому что в текущей среде нет установленного .NET SDK. Перед первым запуском проверь restore/build у себя локально.

---

## 9. Установка зависимостей

### .NET
Установи .NET 8 SDK.

Проверка:
```bash
dotnet --version
```

### Node.js
Проверка:
```bash
node --version
npm --version
```

### Docker Desktop
Установи Docker Desktop и запусти его.

Проверка:
```bash
docker --version
docker compose version
```

---

## 10. Быстрый запуск через Docker

Перейди в папку `deploy` и выполни:

```bash
docker compose up --build
```

После запуска будут доступны:

- Keycloak: `http://localhost:8081`
- Gateway: `http://localhost:5000`
- UserProfileService Swagger: `http://localhost:5001/swagger`
- AdminService Swagger: `http://localhost:5002/swagger`
- Frontend: `http://localhost:5173`

### Тестовые пользователи Keycloak
- admin / `Admin123!`
- user / `User123!`

---

## 11. Что сделать после первого запуска

### Создать таблицу профиля
В текущем каркасе SQL-файл лежит здесь:

```text
deploy/sql/001_user_profile_init.sql
```

Его можно применить вручную в `user_profile_db`, например через psql или pgAdmin.

Позже лучше заменить это на EF Core migrations.

---

## 12. Локальный запуск без Docker для backend

### UserProfileService
```bash
cd src/Services/UserProfileService/UserProfileService.API
dotnet restore
dotnet run
```

### AdminService
```bash
cd src/Services/AdminService/AdminService.API
dotnet restore
dotnet run
```

### Gateway
```bash
cd src/ApiGateway/UserPlatform.ApiGateway
dotnet restore
dotnet run
```

---

## 13. Локальный запуск frontend

```bash
cd src/Frontend/user-platform-web
npm install
npm run dev
```

---

## 14. Как проходит запрос пользователя

### Сценарий логина
1. Frontend перенаправляет пользователя в Keycloak
2. Keycloak аутентифицирует пользователя
3. Frontend получает токен
4. Frontend отправляет запросы в Gateway с bearer token

### Сценарий получения профиля
1. Frontend вызывает `GET /api/profile/me`
2. Gateway валидирует токен и проксирует запрос
3. UserProfileService достаёт `sub`
4. Репозиторий ищет профиль по `ExternalIdentityId`
5. Возвращается DTO

### Сценарий admin доступа
1. Frontend вызывает `GET /api/admin/summary`
2. Gateway и AdminService проверяют наличие роли `admin`
3. Если роль есть — ответ `200`
4. Иначе — `403`

---

## 15. Что стоит улучшить следующим шагом

1. Добавить **EF Core migrations**
2. Добавить **health checks**
3. Добавить **Serilog**
4. Добавить **Redis**
5. Добавить **Outbox pattern**
6. Добавить **integration tests** через `WebApplicationFactory`
7. Перевести frontend на полноценный layout с отдельными страницами login/profile/admin

---

## 16. Команды EF Core, которые пригодятся позже

```bash
dotnet tool install --global dotnet-ef
dotnet ef migrations add InitialCreate
dotnet ef database update
```

Для production лучше генерировать SQL-скрипты миграций и применять их осознанно.

---

## 17. Проверка ролей в Keycloak

Realm:
- `user-platform`

Роли:
- `user`
- `admin`

Клиенты:
- `frontend-web`
- `api-gateway`
- `user-profile-api`
- `admin-api`

---

## 18. Что проверить, если что-то не работает

### Frontend не открывается
- смотри `docker compose logs frontend`
- проверь порт 5173

### Токен не проходит
- проверь issuer и audience
- проверь roles mapping

### UserProfileService не видит БД
- проверь connection string
- проверь, поднят ли контейнер `userprofile-db`

### 403 на admin endpoint
- проверь, что логинишься пользователем `admin`
- проверь realm role `admin` в Keycloak

---

## 19. Почему это соответствует Clean Architecture

- **Domain** не зависит ни от EF Core, ни от ASP.NET Core
- **Application** знает только интерфейсы и use cases
- **Infrastructure** реализует внешние детали
- **API** является только транспортным слоем

Это позволяет:
- проще тестировать код
- менять БД или transport layer
- держать бизнес-логику изолированной

---

## 20. Ограничение текущего каркаса

Этот solution подготовлен как **полный стартовый шаблон**, но не как полностью обкатанный production build:
- миграции ещё не созданы
- compose и Dockerfile нужно проверить локальным `build`
- роли и claims mapping могут потребовать подстройки под конкретную конфигурацию Keycloak

Это нормальный и честный стартовый уровень для реального проекта.

