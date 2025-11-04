# ⚡ Quick Start - CNAB Web

Inicie o projeto em **3 comandos**!

## 🚀 Opção 1: Modo Rápido (Docker + Angular)

### Terminal 1 - Backend
```bash
cd c:\src\challenge
docker-compose up
```

### Terminal 2 - Frontend
```bash
cd c:\src\challenge\cnab-web
npm install
npm start
```

### Acesse
```
http://localhost:4200
```

## 🎯 Pronto!

- Upload: `http://localhost:4200/upload`
- Lojas: `http://localhost:4200/stores`
- API: `http://localhost:5000`
- Swagger: `http://localhost:5000/swagger`

## 📝 Arquivo de Teste

Use o arquivo `CNAB.txt` que está na pasta `c:\src\challenge\`

## ⚙️ Portas Usadas

| Serviço | Porta |
|---------|-------|
| Angular | 4200 |
| .NET API | 5000 |
| PostgreSQL | 5432 |

## 🐛 Problema?

### Backend não inicia
```bash
docker ps  # Veja se está rodando
docker-compose logs  # Veja erros
```

### Frontend não inicia
```bash
cd cnab-web
rm -rf node_modules
npm install
```

### Porta ocupada
```bash
# Windows
netstat -ano | findstr :4200
netstat -ano | findstr :5000

# Mate o processo ou mude a porta
ng serve --port 4201
```

---

**Tudo funcionando?** Hora de testar! 🎉

