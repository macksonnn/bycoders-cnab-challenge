# CNAB System

![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?logo=dotnet)
![Angular](https://img.shields.io/badge/Angular-17-DD0031?logo=angular)
![PostgreSQL](https://img.shields.io/badge/PostgreSQL-16-336791?logo=postgresql)
![Docker](https://img.shields.io/badge/Docker-Ready-2496ED?logo=docker)

Financial transaction management system for CNAB (Brazilian Banking Automation Center) file import.

## 🚀 Quick Start

### Prerequisites
- Docker and Docker Compose
- Node.js 18+ and npm (for Angular frontend)

### Running the Backend (.NET API + PostgreSQL)

```bash
# Start backend services
docker-compose up --build
```

Wait for the message:
```
cnab-api     | Now listening on: http://[::]:5000
cnab-postgres| database system is ready to accept connections
```

**Access:**
- API: http://localhost:5000
- Swagger: http://localhost:5000/swagger

### Running the Frontend (Angular)

Open a **new terminal** and run:

```bash
# Navigate to Angular project
cd cnab-web

# Install dependencies (first time only)
npm install

# Start development server
npm start
```

Wait for:
```
✔ Browser application bundle generation complete.
** Angular Live Development Server is listening on localhost:4200
```

**Access:**
- Frontend: http://localhost:4200

## 📊 Architecture

```
┌─────────────────────┐       HTTP        ┌──────────────────┐
│   Angular Frontend  │ ──────────────>   │   .NET Backend   │
│   localhost:4200    │                   │   localhost:5000 │
└─────────────────────┘                   └──────────────────┘
                                                    │
                                             ┌──────▼──────┐
                                             │  PostgreSQL │
                                             │     :5432   │
                                             └─────────────┘
```

## 🐛 Troubleshooting

### If you encounter any errors:

**Step 1: Stop all containers**
```bash
docker-compose down -v
```

**Step 2: Rebuild and start again**
```bash
docker-compose up --build
```

**Step 3: Restart Angular** (if needed)
```bash
cd cnab-web
npm start
```

### Common Issues

**Port already in use:**
```bash
# Check what's using the port
# Windows:
netstat -ano | findstr :5000
netstat -ano | findstr :4200

# Linux/Mac:
lsof -i :5000
lsof -i :4200
```

**Docker not running:**
- Start Docker Desktop
- Wait for it to be fully initialized
- Try again

**Angular dependencies error:**
```bash
cd cnab-web
rm -rf node_modules
npm install
```

## 📝 Usage

### 1. Upload CNAB File

**Via Web Interface (recommended):**
1. Access http://localhost:4200
2. Click or drag-drop the `CNAB.txt` file
3. Click "Send File"
4. Wait for success message

**Via API:**
```bash
curl -X POST http://localhost:5000/api/cnab/import \
  -F "file=@CNAB.txt"
```

### 2. View Stores and Transactions

**Via Web Interface:**
- Automatically redirected after upload
- Or access http://localhost:4200/stores

**Via API:**
```bash
curl http://localhost:5000/api/stores
```

## 🗂️ Project Structure

```
.
├── src/                    # .NET Backend
│   ├── Cnab.Api/
│   ├── Cnab.Application/
│   ├── Cnab.Domain/
│   └── Cnab.Infrastructure/
│
├── cnab-web/              # Angular Frontend
│   ├── src/app/
│   ├── package.json
│   └── angular.json
│
├── tests/                 # Unit Tests
├── docker-compose.yml     # Docker orchestration
├── CNAB.txt              # Sample file
└── README.md             # This file
```

## ⚠️ Security Note

The PostgreSQL credentials in the configuration files (`postgres/postgres`) are **for local development only** and are isolated in Docker containers. 

**For production:**
- Use environment variables
- Never commit real credentials
- Use Azure Key Vault, AWS Secrets Manager, or similar

## 🔧 Technology Stack

### Backend
- .NET 8
- ASP.NET Core (Minimal APIs)
- Entity Framework Core
- PostgreSQL 16

### Frontend
- Angular 17
- TypeScript
- SCSS
- Standalone Components

## 📡 API Endpoints

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/` | Health check |
| POST | `/api/cnab/import` | Import CNAB file |
| GET | `/api/stores` | List stores with balance |
| GET | `/swagger` | API documentation |

## 🧪 Running Tests

```bash
# Backend tests
dotnet test

# Frontend tests
cd cnab-web
npm test
```

## 🛠️ Development

### Backend only
```bash
dotnet run --project src/Cnab.Api
```

### Frontend only
```bash
cd cnab-web
npm start
```

### Both with hot-reload
```bash
# Terminal 1 - Backend
dotnet watch run --project src/Cnab.Api

# Terminal 2 - Frontend
cd cnab-web
npm start
```

## 📄 CNAB File Format

Each line must have exactly **81 characters**:

| Position | Field | Size | Description |
|----------|-------|------|-------------|
| 1 | Type | 1 | Transaction type (1-9) |
| 2-9 | Date | 8 | YYYYMMDD format |
| 10-19 | Value | 10 | Amount in cents |
| 20-30 | CPF | 11 | Customer ID |
| 31-42 | Card | 12 | Card number |
| 43-48 | Time | 6 | HHMMSS format |
| 49-62 | Owner | 14 | Store owner name |
| 63-81 | Store | 19 | Store name |

### Transaction Types

| Code | Description | Type | Sign |
|------|-------------|------|------|
| 1 | Debit | Revenue | + |
| 2 | Boleto | Expense | - |
| 3 | Financing | Expense | - |
| 4 | Credit | Revenue | + |
| 5 | Loan Receipt | Revenue | + |
| 6 | Sales | Revenue | + |
| 7 | TED Receipt | Revenue | + |
| 8 | DOC Receipt | Revenue | + |
| 9 | Rent | Expense | - |

## 🔄 Full Reset

If something goes wrong, perform a complete reset:

```bash
# 1. Stop everything
docker-compose down -v

# 2. Clean Docker
docker system prune -f

# 3. Clean .NET build
dotnet clean

# 4. Clean Angular
cd cnab-web
rm -rf node_modules dist

# 5. Reinstall and restart
npm install
cd ..
docker-compose up --build

# 6. In another terminal
cd cnab-web
npm start
```

## 📤 Deploy to GitHub

Want to upload this project to your GitHub? Follow the [**GITHUB_SETUP.md**](GITHUB_SETUP.md) guide!

## 📚 Additional Documentation

- [**GITHUB_SETUP.md**](GITHUB_SETUP.md) - How to push to GitHub
- [**ARCHITECTURE.md**](ARCHITECTURE.md) - System architecture details
- [**CONTRIBUTING.md**](CONTRIBUTING.md) - Contribution guidelines
- [**TROUBLESHOOTING.md**](TROUBLESHOOTING.md) - Detailed troubleshooting
- [**cnab-web/README.md**](cnab-web/README.md) - Angular project details
- [**cnab-web/INTEGRATION.md**](cnab-web/INTEGRATION.md) - Integration guide

## 📞 Support

If you encounter issues:
1. Check [TROUBLESHOOTING.md](TROUBLESHOOTING.md)
2. Run `docker-compose down -v` and try again
3. Check Docker logs: `docker-compose logs`
4. Open an issue on GitHub

## 📜 License

MIT License - See [LICENSE](LICENSE) file for details.

## ✨ Author

Developed by **Macksonnn** © 2025

---

**Made with** ❤️ **using .NET 8, Angular 17, and PostgreSQL**
