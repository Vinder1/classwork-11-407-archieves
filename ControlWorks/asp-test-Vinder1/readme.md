## Контрольная
Задание 1

- Миграций нет, ибо не работает утилита. Они должны были появиться в следующих коммитах:
  - add board entity (Миграция AddBoardEntity)
  - add name to board (Миграция AddNameToBoard)



# TaskTracker API

## Описание проекта

**TaskTracker API** — это учебный backend-сервис, реализованный на ASP.NET Core (Minimal API), предназначенный для управления задачами, проектами и пользователями.

Проект используется в рамках контрольной работы и имитирует реальный production-сервис с базовой архитектурой:

- многослойная структура (Repository / Service / DTO)
- авторизация через JWT
- работа с базой данных через EF Core
- валидация через FluentValidation
- логирование
- интеграции через HttpClient
- конфигурация через IConfiguration / IOptions

---

## Назначение

Цель проекта — проверить умение:

- работать с существующим кодом
- понимать архитектуру ASP.NET Core приложения
- добавлять новую функциональность без нарушения структуры
- применять DI, middleware, validation, mapping и другие механизмы

---

## Архитектура

Проект разделён на логические слои:

- **Domain** — сущности и перечисления
- **Data** — DbContext и конфигурации EF Core
- **Repositories** — доступ к данным
- **Services** — бизнес-логика
- **Dtos** — модели запросов/ответов
- **Endpoints** — HTTP API (Minimal API)
- **Auth** — работа с JWT и текущим пользователем
- **Middleware** — обработка запросов
- **Options** — конфигурация приложения
- **Clients** — внешние HTTP интеграции

---

## Пользователи и авторизация

В системе реализована авторизация через JWT.

### Роли:

- `User`
- `Manager`
- `Admin`

### Тестовые пользователи:

| Login   | Password    | Role    |
|--------|------------|--------|
| admin  | admin123   | Admin  |
| manager| manager123 | Manager|
| user   | user123    | User   |

---

## Получение токена

**POST** `/api/auth/login`

```json
{
  "login": "admin",
  "password": "admin123"
}
```

Ответ:

```json
{
  "accessToken": "...",
  "expiresAtUtc": "...",
  "login": "admin",
  "role": "Admin"
}
```

Использование:

```
Authorization: Bearer {token}
```

---

## Основные сущности

### User
- логин, email, роль
- используется для авторизации и прав доступа

### Project
- группа задач
- содержит список задач

### TaskItem
- задача внутри проекта
- имеет статус, приоритет, дедлайн
- привязана к пользователю (создателю)

---

## Основные endpoint’ы

### Auth
- `POST /api/auth/register`
- `POST /api/auth/login`

### Users
- `GET /api/users/me`
- `GET /api/users` (только Admin)

### Projects
- `GET /api/projects`
- `POST /api/projects` (Manager/Admin)

### Tasks
- `GET /api/tasks`
- `GET /api/tasks/{id}`
- `GET /api/tasks/by-status/{status}`
- `POST /api/tasks` (Manager/Admin)
- `DELETE /api/tasks/{id}` (Admin)

---

## Конфигурация

### appsettings.json

```json
{
  "Database": {
    "ConnectionString": "Host=localhost;Port=5432;Database=tasktracker;Username=postgres;Password=postgres"
  },
  "Jwt": {
    "Issuer": "TaskTracker.Api",
    "Audience": "TaskTracker.Client",
    "SecretKey": "SUPER_SECRET_KEY_123456789",
    "ExpirationMinutes": 60
  }
}
```

---

## Запуск проекта

1. Убедитесь, что установлен PostgreSQL
2. Укажите строку подключения в `appsettings.json`
3. Примените миграции:

```
dotnet ef database update
```

4. Запустите проект:

```
dotnet run
```

5. Откройте Swagger:

```
https://localhost:{port}/swagger
```

---

## Особенности проекта

- используется PostgreSQL
- данные инициализируются через `DbSeeder`
- используется Minimal API
- используется AutoMapper
- используется FluentValidation
- реализован доступ к текущему пользователю через `ICurrentUserAccessor`

---

## Важно

Проект намеренно содержит упрощения и неполные участки кода.

В рамках контрольной работы вам может потребоваться:

- добавить новую сущность
- расширить существующие endpoint’ы
- внедрить новые сервисы
- изменить поведение текущей логики
- добавить валидацию, логирование или авторизацию

---

## Рекомендации

Перед выполнением заданий:

1. Изучите структуру проекта
2. Посмотрите, как реализованы `Projects`
3. Используйте их как пример
4. Следуйте существующей архитектуре

---

## Подсказка

Если не знаете, как реализовать задачу:

- найдите похожий код в проекте
- повторите подход
- адаптируйте под свою задачу

---

Удачи :)
