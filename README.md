
## Вимоги
- .NET SDK 9.x
- Node.js 18+ і npm
- SQL Server LocalDB (типово використовується `(localdb)\MSSQLLocalDB`)

## Структура
- `backend/TemplatePL` — Web API (контролер `TemplatesController` з маршрутами `/api/Templates/...`).
- `backend/TemplateBLL`, `backend/TemplateDAL` — бізнес-логіка та доступ до даних (EF Core, SQL Server LocalDB).
- `frontend/` — застосунок Vite + React (порт за замовчуванням 5173).

## Запуск бекенду
1. Відкрийте рішення в корені (`BootsikTest.sln`) або перейдіть у папку `backend/TemplatePL`.
2. Переконайтеся, що рядок підключення у `backend/TemplatePL/Program.cs` вказує на доступний SQL Server:
   ```csharp
   options.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=TemplatesDb;Trusted_Connection=True;");
   ```
3. Застосуйте міграції (під час першого запуску):
   - Через Package Manager Console/CLI (варіанти):
     - `dotnet ef database update` з каталогу `backend/TemplateDAL` або `backend/TemplatePL` із додаванням `--project`/`--startup-project`.
4. Запустіть Web API у режимі Development (доступні профілі HTTPS і HTTP):
   - HTTPS: `https://localhost:7030`
   - HTTP: `http://localhost:5211`

Swagger доступний у Development за адресою `/swagger`.

## Запуск фронтенду

### Налаштування проксі Vite (важливо)
Файл: `frontend/vite.config.ts`

Зараз налаштовано проксування всіх запитів до `/api` на HTTPS‑профіль бекенду:
```ts
server: {
  port: 5173,
  proxy: {
    '/api': {
      target: 'https://localhost:7030',
      changeOrigin: true,
      secure: false
    }
  }
}
```
Це підходить, якщо бекенд запущено на `https://localhost:7030` (див. `launchSettings.json`). Якщо ви запускаєте по HTTP (наприклад, `http://localhost:5211`), замініть `target` на HTTP:
```ts
proxy: {
  '/api': {
    target: 'http://localhost:5211',
    changeOrigin: true
  }
}
```

## Основні ендпоїнти API
Базовий маршрут контролера — `/api/Templates`:
- `GET /api/Templates/GetAll`
- `GET /api/Templates/GetById/{id}`
- `POST /api/Templates/Create` — тіло запиту: `{ name: string, htmlContent: string }`
- `PUT /api/Templates/Update/{id}` — тіло запиту: `{ name: string, htmlContent: string }`
- `DELETE /api/Templates/Delete/{id}`
- `POST /api/Templates/{id}/generate` — тіло запиту: `{ data: Record<string,string> }`, повертає PDF (`application/pdf`).