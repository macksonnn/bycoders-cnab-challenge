# Guia de Integração - Frontend Angular + Backend .NET

Este guia explica como executar o frontend Angular integrado com o backend .NET.

## 🎯 Arquitetura

```
┌─────────────────────┐       HTTP        ┌──────────────────┐
│   Angular Frontend  │ ──────────────>   │   .NET Backend   │
│   localhost:4200    │                   │   localhost:5000 │
└─────────────────────┘                   └──────────────────┘
         │                                          │
         │                                          │
         └──────────────────┬───────────────────────┘
                            │
                     ┌──────▼──────┐
                     │  PostgreSQL │
                     │     :5432   │
                     └─────────────┘
```

## 🚀 Como Executar (Passo a Passo)

### Opção 1: Backend com Docker + Frontend Local

#### 1️⃣ Inicie o Backend (.NET + PostgreSQL)

```bash
# Na pasta raiz do projeto .NET (challenge/)
cd c:\src\challenge
docker-compose up --build
```

Aguarde até ver:
```
cnab-api     | Now listening on: http://[::]:5000
cnab-postgres| database system is ready to accept connections
```

#### 2️⃣ Inicie o Frontend Angular

```bash
# Abra OUTRO terminal
cd c:\src\challenge\cnab-web

# Instale dependências (primeira vez)
npm install

# Inicie o servidor de desenvolvimento
npm start
```

Aguarde até ver:
```
✔ Browser application bundle generation complete.
** Angular Live Development Server is listening on localhost:4200
```

#### 3️⃣ Acesse a Aplicação

Abra o navegador em: **http://localhost:4200**

### Opção 2: Tudo Local (sem Docker)

#### 1️⃣ PostgreSQL

```bash
# Certifique-se de ter PostgreSQL rodando
# Crie o banco: cnab_db
```

#### 2️⃣ Backend .NET

```bash
cd c:\src\challenge
dotnet restore
dotnet run --project src/Cnab.Api
```

#### 3️⃣ Frontend Angular

```bash
cd c:\src\challenge\cnab-web
npm install
npm start
```

## 🔌 Configuração de Endpoints

### Frontend (Angular)

Edite `cnab-web/src/environments/environment.ts`:

```typescript
export const environment = {
  production: false,
  apiUrl: 'http://localhost:5000'  // URL do backend
};
```

### Backend (.NET)

O CORS já está configurado em `src/Cnab.Api/Program.cs` para aceitar qualquer origem em desenvolvimento.

Para produção, ajuste:

```csharp
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("https://seu-dominio.com")
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});
```

## 📊 Fluxo de Dados

### Upload de Arquivo

```
1. Usuário seleciona arquivo em http://localhost:4200/upload
2. Angular envia POST para http://localhost:5000/api/cnab/import
3. Backend processa e salva no PostgreSQL
4. Retorna sucesso
5. Angular redireciona para /stores
```

### Listagem de Lojas

```
1. Usuário acessa http://localhost:4200/stores
2. Angular faz GET em http://localhost:5000/api/stores
3. Backend consulta PostgreSQL
4. Retorna JSON com lojas e transações
5. Angular renderiza os dados
```

## 🧪 Testando a Integração

### Teste Manual

1. ✅ Backend rodando em `http://localhost:5000`
2. ✅ Frontend rodando em `http://localhost:4200`
3. ✅ Acesse `http://localhost:4200/upload`
4. ✅ Faça upload do arquivo `CNAB.txt` (está na raiz do projeto .NET)
5. ✅ Verifique se foi redirecionado para `/stores`
6. ✅ Veja as lojas e transações listadas

### Teste via cURL

```bash
# Upload de arquivo
curl -X POST http://localhost:5000/api/cnab/import \
  -F "file=@CNAB.txt"

# Listar lojas
curl http://localhost:5000/api/stores
```

### Teste via Swagger

Acesse: `http://localhost:5000/swagger`

## 🐛 Troubleshooting

### Erro: CORS blocked

**Sintoma:** Console do navegador mostra erro de CORS

**Solução:**
- Verifique se o backend está com CORS configurado
- Confirme que a URL no `environment.ts` está correta

### Erro: Failed to load resource

**Sintoma:** Angular não consegue chamar a API

**Verificações:**
1. Backend está rodando? `http://localhost:5000`
2. URL está correta no `environment.ts`?
3. Firewall bloqueando?

### Erro: Can't resolve 'environment'

**Solução:**
```bash
cd cnab-web
rm -rf node_modules
npm install
```

### Erro: Port 4200 already in use

**Solução:**
```bash
# Pare o processo na porta 4200
# Windows
netstat -ano | findstr :4200

# Ou use outra porta
ng serve --port 4201
```

## 📦 Build para Produção

### Backend

```bash
cd c:\src\challenge
dotnet publish src/Cnab.Api -c Release -o ./publish
```

### Frontend

```bash
cd cnab-web
ng build --configuration production
```

Arquivos estarão em `dist/cnab-web/browser`

### Deploy Conjunto

1. Faça build do Angular
2. Copie `dist/cnab-web/browser/*` para `src/Cnab.Api/wwwroot/`
3. Configure rotas fallback no .NET para SPA
4. Publique o .NET

Ou mantenha separados e use nginx como proxy reverso.

## 🔄 Hot Reload

- **Angular:** Atualização automática ao salvar arquivos
- **.NET:** Use `dotnet watch run` no lugar de `dotnet run`

## 📁 Estrutura de Pastas Final

```
c:\src\challenge\
├── src/                    # Backend .NET
│   ├── Cnab.Api/
│   ├── Cnab.Application/
│   ├── Cnab.Domain/
│   └── Cnab.Infrastructure/
├── cnab-web/              # Frontend Angular
│   ├── src/
│   │   ├── app/
│   │   ├── assets/
│   │   └── environments/
│   ├── package.json
│   └── angular.json
├── tests/
├── docker-compose.yml
└── README.md
```

## ✅ Checklist de Integração

- [ ] PostgreSQL rodando (porta 5432)
- [ ] Backend .NET rodando (porta 5000)
- [ ] Frontend Angular rodando (porta 4200)
- [ ] CORS configurado no backend
- [ ] Environment.ts configurado no frontend
- [ ] Upload de arquivo funciona
- [ ] Listagem de lojas funciona
- [ ] Console do navegador sem erros

## 🎉 Pronto!

Agora você tem uma aplicação full-stack completa:
- ✅ Frontend Angular moderno
- ✅ Backend .NET 8 robusto
- ✅ PostgreSQL para persistência
- ✅ Docker para facilitar deploy

---

**Dúvidas?** Consulte:
- Backend: `challenge/README.md`
- Frontend: `cnab-web/README.md`

