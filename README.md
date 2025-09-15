# Desafio Técnico – Agenda (.NET 8 + Vue 3)

CRUD de contatos (nome, e-mail, telefone) com backend em **.NET 8 / EF Core (SQLite)** e frontend em **Vue 3 / Vite / Pinia / PrimeVue**.

## 📦 Estrutura

```
Desafio_Tecnico/
├─ agenda/
│  ├─ backend/              # API .NET 8 (EF Core + Swagger + JWT)
│  └─ frontend/frontend/    # Vue 3 + Vite + Pinia + PrimeVue
└─ Desafio_Tecnico.sln
```

---

## ✅ Pré-requisitos

- **.NET 8 SDK**  
- **Node.js 20+** (ou 22+)  
- **NPM**  
- **dotnet-ef** (para migrations)  

SQLite já está embutido via arquivo (`agenda.db`).

---

## ▶️ Rodando o **backend** (.NET 8)

```bash
cd agenda/backend
dotnet restore
dotnet ef database update
dotnet run --urls http://localhost:3000
```

Swagger: [http://localhost:3000/swagger](http://localhost:3000/swagger)

---

## ▶️ Rodando o **frontend** (Vue 3 + Vite)

```bash
cd agenda/frontend/frontend
npm install
npm run dev
```

Acesse: [http://localhost:5173](http://localhost:5173)

### Variáveis de ambiente (opcional)
Arquivo `.env`:
```ini
VITE_API_BASE_URL=http://localhost:3000
```

---

## 🔐 Autenticação JWT

- Endpoint: `POST /api/auth/login`  
- Body: `{ "email": "admin@local", "password": "Admin@123" }`  
- Resposta: `{ "token": "..." }`

Use o token no header `Authorization: Bearer ...` para acessar `/api/contacts`.

---

## 📘 Endpoints principais

- `POST /api/auth/login` → retorna `{ token }`
- `GET /api/contacts?term=&page=1&size=10`
- `GET /api/contacts/{id}`
- `POST /api/contacts` → `{ name, email, phone }`
- `PUT /api/contacts/{id}`
- `DELETE /api/contacts/{id}`

---

## 🧪 Testes

- **Frontend** (Vitest):
```bash
cd agenda/frontend/frontend
npm run test:unit
```

- **Backend**: não há testes implementados (sugestão: xUnit).

---

## 🐳 Docker (exemplo opcional)

**Dockerfile (API)** – `agenda/backend/Dockerfile`:
```dockerfile
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY . .
RUN dotnet publish -c Release -o /out

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /out .
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080
ENTRYPOINT ["dotnet","Agenda.Api.dll"]
```

**docker-compose.yml** – `agenda/docker-compose.yml`:
```yaml
services:
  api:
    build: ./backend
    ports:
      - "3000:8080"
    environment:
      ASPNETCORE_ENVIRONMENT: "Development"
      ConnectionStrings__Default: "Data Source=/app/agenda.db"
      Jwt__Issuer: "AgendaApi"
      Jwt__Audience: "AgendaClient"
      Jwt__Secret: "sua-chave-segura"
    volumes:
      - api_data:/app

volumes:
  api_data:
```

Rodar:
```bash
cd agenda
docker compose up --build
```

---

## 📌 Status
- ✅ CRUD com EF Core + Swagger  
- ✅ Front em Vue 3 + Pinia + PrimeVue  
- ⚠️ Autenticação JWT implementada mas precisa ativar `UseAuthentication`/`UseAuthorization`  
- ⚠️ PrimeVue pouco explorado nos componentes  
- ❌ Sem testes backend  
- ❌ Dockerfile/compose não entregues no zip (apenas exemplos acima)  
