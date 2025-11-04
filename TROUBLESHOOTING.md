# Guia de Troubleshooting - CNAB

Este guia contém soluções para problemas comuns encontrados durante o desenvolvimento e execução do projeto.

## 📦 Problemas com Docker

### Container não inicia

**Sintoma:** `docker-compose up` falha ou containers param imediatamente

**Soluções:**

1. **Verificar se as portas estão em uso**
```bash
# Windows
netstat -ano | findstr :5000
netstat -ano | findstr :5432

# Linux/Mac
lsof -i :5000
lsof -i :5432
```

2. **Limpar containers antigos**
```bash
docker-compose down -v
docker system prune -a
```

3. **Verificar logs**
```bash
docker-compose logs api
docker-compose logs postgres
```

### PostgreSQL não está pronto

**Sintoma:** API tenta conectar antes do PostgreSQL estar pronto

**Solução:** O `docker-compose.yml` já tem `healthcheck` configurado. Se ainda assim falhar:
```bash
docker-compose down
docker-compose up --force-recreate
```

### Erro de permissão no volume

**Sintoma:** `permission denied` no volume do PostgreSQL

**Solução Linux/Mac:**
```bash
sudo chown -R $USER:$USER postgres_data
```

## 🗄️ Problemas com Banco de Dados

### Erro de conexão ao PostgreSQL

**Sintoma:** `Npgsql.NpgsqlException: Connection refused`

**Verificações:**

1. **PostgreSQL está rodando?**
```bash
docker ps | grep postgres
```

2. **Connection string correta?**
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=cnab_db;Username=postgres;Password=postgres"
  }
}
```

3. **Firewall bloqueando?**
```bash
# Windows: Adicionar exceção no firewall para porta 5432
# Linux: sudo ufw allow 5432
```

### Migrations não aplicadas

**Sintoma:** Tabelas não existem no banco

**Solução:**
```bash
cd src/Cnab.Api
dotnet ef database update --project ../Cnab.Infrastructure
```

### Erro "relation does not exist"

**Sintoma:** `ERROR: relation "stores" does not exist`

**Solução:**
```bash
# Dropar e recriar o banco
docker-compose down -v
docker-compose up

# Ou manualmente aplicar migrations
dotnet ef database update --project src/Cnab.Infrastructure --startup-project src/Cnab.Api
```

### Dados corrompidos

**Sintoma:** Dados inconsistentes no banco

**Solução:**
```bash
# Resetar banco de dados
docker-compose down -v
docker volume rm challenge_postgres_data
docker-compose up
```

## 🔨 Problemas com Build

### Erro ao restaurar pacotes

**Sintoma:** `error NU1101: Unable to find package`

**Solução:**
```bash
# Limpar cache do NuGet
dotnet nuget locals all --clear

# Restaurar novamente
dotnet restore
```

### Erro de compilação

**Sintoma:** `CS0246: The type or namespace name '...' could not be found`

**Verificações:**

1. **Referências de projeto corretas?**
```bash
dotnet list reference
```

2. **SDK do .NET 8 instalado?**
```bash
dotnet --version  # Deve ser 8.0.x
```

3. **Rebuild completo**
```bash
dotnet clean
dotnet build
```

## 🌐 Problemas com API

### API não responde

**Sintoma:** Timeout ou connection refused

**Verificações:**

1. **API está rodando?**
```bash
docker ps  # Ou dotnet run
```

2. **URL correta?**
- Com Docker: `http://localhost:5000`
- Local: `http://localhost:5000`

3. **Verificar logs**
```bash
docker-compose logs api -f
```

### Erro 500 ao importar arquivo

**Sintoma:** Internal Server Error ao fazer upload

**Verificações:**

1. **Formato do arquivo correto?**
- Cada linha deve ter exatamente 81 caracteres
- Campos devem estar nas posições corretas

2. **Banco de dados acessível?**
```bash
docker-compose logs postgres
```

3. **Ver logs detalhados**
```json
// appsettings.Development.json
{
  "Logging": {
    "LogLevel": {
      "Default": "Debug"
    }
  }
}
```

### CORS bloqueando requisições

**Sintoma:** `No 'Access-Control-Allow-Origin' header`

**Solução:** CORS já está configurado para aceitar todas as origens em desenvolvimento. Para produção:
```csharp
// Program.cs
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("https://seudominio.com")
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});
```

## 🧪 Problemas com Testes

### Testes falhando

**Sintoma:** `dotnet test` falha

**Verificações:**

1. **Dependências instaladas?**
```bash
dotnet restore
```

2. **Executar testes específicos**
```bash
dotnet test --filter "FullyQualifiedName~CnabParserTests"
```

3. **Modo verbose**
```bash
dotnet test --logger "console;verbosity=detailed"
```

### Mock não funciona

**Sintoma:** `NullReferenceException` em testes

**Verificação:**
```csharp
// Certifique-se de configurar todos os métodos usados
_mockRepository
    .Setup(x => x.GetByNameAndOwnerAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
    .ReturnsAsync((Store?)null);
```

## 📄 Problemas com Arquivo CNAB

### Erro ao fazer parse

**Sintoma:** `ArgumentException` ao processar linha

**Verificações:**

1. **Codificação do arquivo**
- Deve ser UTF-8
- Sem BOM (Byte Order Mark)

2. **Fim de linha**
- Pode ser LF ou CRLF
- Não pode ter linhas vazias no meio

3. **Comprimento da linha**
```bash
# Verificar tamanho das linhas
cat CNAB.txt | awk '{print length($0)}'  # Deve retornar 81
```

### Dados não aparecem após importação

**Sintoma:** Upload com sucesso mas `/api/stores` retorna vazio

**Verificações:**

1. **Transação foi comitada?**
```csharp
// Verificar se SaveChangesAsync foi chamado
await _context.SaveChangesAsync();
```

2. **Verificar diretamente no banco**
```bash
docker exec -it cnab-postgres psql -U postgres -d cnab_db -c "SELECT COUNT(*) FROM stores;"
```

## 🔧 Problemas com Entity Framework

### Erro de migration

**Sintoma:** `The migration '...' has already been applied`

**Solução:**
```bash
# Ver migrations aplicadas
dotnet ef migrations list --project src/Cnab.Infrastructure --startup-project src/Cnab.Api

# Reverter migration
dotnet ef database update PreviousMigration --project src/Cnab.Infrastructure --startup-project src/Cnab.Api

# Remover migration
dotnet ef migrations remove --project src/Cnab.Infrastructure --startup-project src/Cnab.Api
```

### Erro "The entity type X is not part of the model"

**Sintoma:** Entity não reconhecida pelo EF Core

**Verificação:**
```csharp
// Certifique-se de ter DbSet no contexto
public DbSet<Store> Stores => Set<Store>();
public DbSet<Transaction> Transactions => Set<Transaction>();
```

## 🐛 Debug

### Debugar com VS Code

1. **Configurar breakpoints**
2. **F5** para iniciar debug
3. Usar `launch.json` configurado no projeto

### Debugar Docker

**Solução:**
```bash
# Adicionar variável de ambiente no docker-compose.yml
environment:
  - ASPNETCORE_ENVIRONMENT=Development
  - Logging__LogLevel__Default=Debug
```

### Ver queries SQL

**Solução:**
```json
// appsettings.Development.json
{
  "Logging": {
    "LogLevel": {
      "Microsoft.EntityFrameworkCore.Database.Command": "Information"
    }
  }
}
```

## 📱 Problemas com Frontend

### Página em branco

**Sintoma:** `http://localhost:5000` não carrega

**Verificações:**

1. **Arquivos estáticos habilitados?**
```csharp
// Program.cs
app.UseDefaultFiles();
app.UseStaticFiles();
```

2. **Arquivo existe?**
```bash
ls src/Cnab.Api/wwwroot/index.html
```

3. **Verificar console do browser** (F12)

### Upload não funciona

**Sintoma:** Erro ao fazer upload de arquivo

**Verificações:**

1. **Antiforgery desabilitado?**
```csharp
.DisableAntiforgery()
```

2. **FormData corretamente configurado?**
```javascript
const formData = new FormData();
formData.append('file', selectedFile);
```

3. **CORS configurado?**

## 🚀 Performance

### API lenta

**Verificações:**

1. **Índices no banco?**
```sql
CREATE INDEX idx_transactions_store_id ON transactions(store_id);
CREATE INDEX idx_stores_name_owner ON stores(name, owner);
```

2. **Connection pool configurado?**
```csharp
options.UseNpgsql(connectionString, o => o.MaxBatchSize(100));
```

3. **AsNoTracking em queries de leitura?**
```csharp
.AsNoTracking()
.ToListAsync();
```

## 📞 Onde Pedir Ajuda

### Logs Úteis

```bash
# Logs da API
docker-compose logs api -f

# Logs do PostgreSQL
docker-compose logs postgres -f

# Logs do .NET
dotnet run --verbosity detailed
```

### Informações para reportar

Ao reportar um problema, inclua:
- Versão do .NET (`dotnet --version`)
- Sistema operacional
- Logs relevantes
- Passos para reproduzir
- Comportamento esperado vs atual

### Recursos

- [Issues do GitHub](../../issues)
- [Documentação do .NET](https://docs.microsoft.com/dotnet)
- [Stack Overflow](https://stackoverflow.com/questions/tagged/.net)
- [Entity Framework Core Docs](https://docs.microsoft.com/ef/core)

## ✅ Checklist de Diagnóstico

Quando algo não funciona, verifique na ordem:

- [ ] Docker está rodando?
- [ ] Containers estão up? (`docker ps`)
- [ ] PostgreSQL está healthy? (`docker-compose ps`)
- [ ] Migrations foram aplicadas? (check logs da API)
- [ ] Connection string está correta?
- [ ] Portas estão disponíveis?
- [ ] Firewall não está bloqueando?
- [ ] Pacotes NuGet restaurados?
- [ ] Build sem erros?
- [ ] Arquivo CNAB tem formato correto?
- [ ] Logs da API mostram algum erro?

## 🔄 Reset Completo

Se nada mais funcionar:

```bash
# Parar tudo
docker-compose down -v

# Limpar Docker
docker system prune -a -f

# Limpar build
dotnet clean
rm -rf */bin */obj

# Reconstruir tudo
dotnet restore
dotnet build

# Subir novamente
docker-compose up --build
```

---

💡 **Dica:** Mantenha este documento atualizado com novos problemas e soluções encontrados!

