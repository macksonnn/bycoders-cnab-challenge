# Quick Start Guide - CNAB System

Guia rápido para começar a usar o sistema em **5 minutos**! ⚡

## 🎯 Objetivo

Importar e visualizar transações financeiras de arquivos CNAB.

## ⚡ Start em 3 Passos

### 1️⃣ Clonar e Entrar

```bash
git clone <repository-url>
cd challenge
```

### 2️⃣ Iniciar com Docker

```bash
# Windows
run-docker.bat

# Linux/Mac
chmod +x run-docker.sh
./run-docker.sh
```

### 3️⃣ Acessar

Abra no navegador: **http://localhost:5000**

🎉 Pronto! O sistema está rodando!

## 📤 Como Usar

### Via Interface Web

1. **Abra** http://localhost:5000
2. **Arraste** o arquivo `CNAB.txt` (ou clique para selecionar)
3. **Clique** em "Enviar Arquivo"
4. **Veja** as lojas e transações aparecerem automaticamente

### Via API (cURL)

```bash
# Importar arquivo
curl -X POST http://localhost:5000/api/cnab/import \
  -F "file=@CNAB.txt"

# Listar lojas
curl http://localhost:5000/api/stores
```

### Via Swagger

1. Acesse: http://localhost:5000/swagger
2. Teste os endpoints diretamente na interface

## 📋 Arquivo de Exemplo

O projeto já vem com um arquivo `CNAB.txt` de exemplo pronto para uso!

**Conteúdo:**
```
3201903010000014200096206760174753****3153153453JOÃO MACEDO   BAR DO JOÃO       
1201903010000023200055641815708231****1231123100JOSÉ COSTA    MERCEARIA 3 IRMÃOS
...
```

## 🛑 Como Parar

```bash
# Windows
stop-docker.bat

# Linux/Mac
./stop-docker.sh
```

## 🔧 Alternativa sem Docker

### Requisitos
- .NET 8 SDK
- PostgreSQL 16+

### Passos

```bash
# 1. Configure o PostgreSQL
# Crie um banco chamado 'cnab_db'

# 2. Configure a connection string
# Edite: src/Cnab.Api/appsettings.Development.json

# 3. Restaure e execute
dotnet restore
dotnet run --project src/Cnab.Api
```

## 📊 O Que Você Verá

### Interface Principal
```
┌────────────────────────────────────────┐
│     📊 Sistema CNAB                    │
│                                        │
│  ┌──────────────────────────────────┐ │
│  │  📤 Importar Arquivo CNAB        │ │
│  │                                  │ │
│  │  [Arraste arquivo aqui]          │ │
│  │                                  │ │
│  │  [Enviar Arquivo]                │ │
│  └──────────────────────────────────┘ │
│                                        │
│  ┌──────────────────────────────────┐ │
│  │  🏪 Lojas e Transações           │ │
│  │                                  │ │
│  │  BAR DO JOÃO        R$ 400,00 ✅  │ │
│  │  👤 João Macedo                  │ │
│  │  ▼ Ver 5 transações              │ │
│  │                                  │ │
│  │  MERCEARIA 3 IRMÃOS R$ -120,00 ❌ │ │
│  │  👤 José Costa                   │ │
│  │  ▼ Ver 8 transações              │ │
│  └──────────────────────────────────┘ │
└────────────────────────────────────────┘
```

### Resposta da API
```json
[
  {
    "id": 1,
    "name": "BAR DO JOÃO",
    "owner": "JOÃO MACEDO",
    "balance": 400.00,
    "transactions": [
      {
        "id": 1,
        "type": 3,
        "typeDescription": "Financiamento",
        "occurredAt": "2019-03-01T15:31:53",
        "value": 142.00,
        "signedValue": -142.00,
        "cpf": "09620676017",
        "card": "4753****3153"
      }
    ]
  }
]
```

## 🎓 Próximos Passos

### Explore a Documentação
- 📖 [README.md](README.md) - Documentação completa
- 🏗️ [ARCHITECTURE.md](ARCHITECTURE.md) - Arquitetura do sistema
- 📁 [PROJECT_STRUCTURE.md](PROJECT_STRUCTURE.md) - Estrutura de arquivos
- 🐛 [TROUBLESHOOTING.md](TROUBLESHOOTING.md) - Solução de problemas

### Execute os Testes
```bash
dotnet test
```

### Explore o Código
```bash
# Abrir no VS Code
code .

# Ou seu editor favorito
```

## 🚨 Problemas Comuns

### Porta 5000 em uso?
```bash
# Windows
netstat -ano | findstr :5000

# Linux/Mac
lsof -i :5000
```

**Solução:** Mate o processo ou mude a porta no `docker-compose.yml`

### Docker não inicia?
```bash
# Verificar se Docker está rodando
docker ps

# Reiniciar Docker Desktop
```

### Banco não conecta?
```bash
# Ver logs do PostgreSQL
docker-compose logs postgres

# Recriar containers
docker-compose down -v
docker-compose up
```

Mais soluções: [TROUBLESHOOTING.md](TROUBLESHOOTING.md)

## 🎯 Comandos Úteis

```bash
# Ver logs em tempo real
docker-compose logs -f api

# Acessar banco de dados
docker exec -it cnab-postgres psql -U postgres -d cnab_db

# Ver containers rodando
docker-compose ps

# Rebuild completo
docker-compose up --build --force-recreate

# Parar e limpar tudo
docker-compose down -v
```

## 📞 Precisa de Ajuda?

1. ✅ Verifique [TROUBLESHOOTING.md](TROUBLESHOOTING.md)
2. ✅ Veja os logs: `docker-compose logs`
3. ✅ Abra uma [issue](../../issues)

## 🎉 Sucesso!

Se você chegou até aqui e o sistema está rodando:

**PARABÉNS!** 🎊

Você tem um sistema completo de importação CNAB rodando localmente com:
- ✅ API REST funcional
- ✅ Interface web moderna
- ✅ Banco de dados PostgreSQL
- ✅ Tudo containerizado

## 📚 Quer Aprender Mais?

### Conceitos Aplicados
- Clean Architecture
- Domain-Driven Design
- Repository Pattern
- Dependency Injection
- Entity Framework Core
- Docker & Docker Compose
- Minimal APIs
- xUnit Testing

### Recursos
- [.NET Documentation](https://docs.microsoft.com/dotnet)
- [EF Core](https://docs.microsoft.com/ef/core)
- [Docker Docs](https://docs.docker.com)
- [Clean Architecture](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html)

## ⏱️ Tempo Total

- **Setup inicial:** 2 minutos
- **Build (primeira vez):** 2-3 minutos
- **Importar arquivo:** < 1 segundo
- **Ver resultados:** imediato

**Total:** ~5 minutos do zero ao funcionamento! ⚡

---

**Dica:** Marque este arquivo ⭐ para referência futura!

Desenvolvido para **ByCodersTec Challenge** 🚀

