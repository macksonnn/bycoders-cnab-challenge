# Estrutura do Projeto CNAB

## Árvore de Diretórios

```
challenge/
│
├── 📁 src/                                    # Código fonte da aplicação
│   ├── 📁 Cnab.Domain/                        # Camada de Domínio
│   │   ├── 📁 Entities/
│   │   │   ├── Store.cs                       # Entidade Loja
│   │   │   └── Transaction.cs                 # Entidade Transação
│   │   ├── 📁 Enums/
│   │   │   └── TransactionType.cs             # Tipos de transação CNAB
│   │   ├── 📁 Extensions/
│   │   │   └── TransactionTypeExtensions.cs   # Lógica de receita/despesa
│   │   └── Cnab.Domain.csproj
│   │
│   ├── 📁 Cnab.Application/                   # Camada de Aplicação
│   │   ├── 📁 DTOs/
│   │   │   ├── StoreDto.cs                    # DTO de Loja
│   │   │   └── TransactionDto.cs              # DTO de Transação
│   │   ├── 📁 Interfaces/
│   │   │   ├── ICnabImportService.cs          # Interface de importação
│   │   │   └── IStoreService.cs               # Interface de consulta
│   │   ├── 📁 Services/
│   │   │   ├── CnabImportService.cs           # Serviço de importação
│   │   │   ├── CnabParser.cs                  # Parser de linhas CNAB
│   │   │   └── StoreService.cs                # Serviço de consulta
│   │   ├── DependencyInjection.cs             # Registro de DI
│   │   └── Cnab.Application.csproj
│   │
│   ├── 📁 Cnab.Infrastructure/                # Camada de Infraestrutura
│   │   ├── 📁 Data/
│   │   │   └── CnabDbContext.cs               # Contexto do EF Core
│   │   ├── 📁 Repositories/
│   │   │   ├── IStoreRepository.cs            # Interface do repositório
│   │   │   ├── StoreRepository.cs             # Implementação Store
│   │   │   ├── ITransactionRepository.cs      # Interface do repositório
│   │   │   └── TransactionRepository.cs       # Implementação Transaction
│   │   ├── 📁 Migrations/
│   │   │   ├── 20231103000000_InitialCreate.cs           # Migration inicial
│   │   │   ├── 20231103000000_InitialCreate.Designer.cs
│   │   │   └── CnabDbContextModelSnapshot.cs
│   │   ├── DependencyInjection.cs             # Registro de DI
│   │   └── Cnab.Infrastructure.csproj
│   │
│   └── 📁 Cnab.Api/                           # Camada de API
│       ├── 📁 Properties/
│       │   └── launchSettings.json            # Configurações de launch
│       ├── 📁 wwwroot/
│       │   └── index.html                     # Frontend web
│       ├── Program.cs                         # Configuração da API
│       ├── appsettings.json                   # Configurações padrão
│       ├── appsettings.Development.json       # Configurações de dev
│       └── Cnab.Api.csproj
│
├── 📁 tests/                                  # Testes
│   └── 📁 Cnab.Tests/
│       ├── 📁 Domain/
│       │   └── TransactionTypeExtensionsTests.cs
│       ├── 📁 Application/
│       │   ├── CnabParserTests.cs             # Testes do parser
│       │   ├── CnabImportServiceTests.cs      # Testes de importação
│       │   └── StoreServiceTests.cs           # Testes de consulta
│       ├── GlobalUsings.cs
│       └── Cnab.Tests.csproj
│
├── 📁 .vscode/                                # Configurações VS Code
│   ├── launch.json                            # Configurações de debug
│   ├── tasks.json                             # Tasks do VS Code
│   └── settings.json                          # Settings do workspace
│
├── 📄 Cnab.sln                                # Solução do Visual Studio
├── 📄 Dockerfile                              # Build da imagem Docker
├── 📄 docker-compose.yml                      # Orquestração de containers
├── 📄 .dockerignore                           # Arquivos ignorados no Docker
├── 📄 .gitignore                              # Arquivos ignorados no Git
├── 📄 .editorconfig                           # Configurações de editor
│
├── 📄 README.md                               # Documentação principal
├── 📄 ARCHITECTURE.md                         # Documentação de arquitetura
├── 📄 CONTRIBUTING.md                         # Guia de contribuição
├── 📄 PROJECT_STRUCTURE.md                    # Este arquivo
├── 📄 LICENSE                                 # Licença MIT
│
├── 📄 CNAB.txt                                # Arquivo de exemplo
│
├── 🔧 run-docker.bat                          # Script Windows
├── 🔧 run-docker.sh                           # Script Linux/Mac
├── 🔧 stop-docker.bat                         # Script Windows
└── 🔧 stop-docker.sh                          # Script Linux/Mac
```

## Descrição dos Diretórios

### `/src` - Código Fonte

#### `Cnab.Domain` 🎯
**O que é:** Núcleo do sistema, contém as regras de negócio puras.

**Responsabilidades:**
- Definir entidades do domínio
- Estabelecer enums e value objects
- Conter lógica de negócio fundamental

**Não deve:**
- Ter dependências externas
- Conhecer infraestrutura
- Depender de frameworks

#### `Cnab.Application` 🔄
**O que é:** Camada de casos de uso e orquestração.

**Responsabilidades:**
- Implementar casos de uso (importar CNAB, listar lojas)
- Fazer parsing de dados externos
- Transformar entre entidades e DTOs

**Depende de:**
- Cnab.Domain

#### `Cnab.Infrastructure` 🗄️
**O que é:** Implementações técnicas e acesso a dados.

**Responsabilidades:**
- Gerenciar persistência (EF Core)
- Implementar repositórios
- Configurar banco de dados

**Depende de:**
- Cnab.Domain

#### `Cnab.Api` 🌐
**O que é:** Interface HTTP da aplicação.

**Responsabilidades:**
- Expor endpoints REST
- Servir frontend estático
- Configurar middleware e serviços

**Depende de:**
- Cnab.Application
- Cnab.Infrastructure

### `/tests` - Testes Automatizados

#### `Cnab.Tests` ✅
**O que contém:**
- Testes unitários de todas as camadas
- Uso de mocks (Moq) para isolamento
- Asserções com FluentAssertions

**Estrutura:**
- `/Domain` - Testes de lógica de domínio
- `/Application` - Testes de serviços e parser

### Arquivos de Configuração

| Arquivo | Propósito |
|---------|-----------|
| `Cnab.sln` | Solução do Visual Studio |
| `*.csproj` | Arquivos de projeto .NET |
| `Dockerfile` | Build da imagem Docker da API |
| `docker-compose.yml` | Orquestração API + PostgreSQL |
| `.editorconfig` | Padrões de código |
| `appsettings.json` | Configurações da aplicação |

### Arquivos de Documentação

| Arquivo | Conteúdo |
|---------|----------|
| `README.md` | Visão geral e quick start |
| `ARCHITECTURE.md` | Decisões arquiteturais detalhadas |
| `CONTRIBUTING.md` | Como contribuir com o projeto |
| `PROJECT_STRUCTURE.md` | Estrutura de diretórios (este arquivo) |
| `LICENSE` | Licença MIT |

### Scripts Auxiliares

| Script | Plataforma | Função |
|--------|-----------|---------|
| `run-docker.bat` | Windows | Inicia containers |
| `run-docker.sh` | Linux/Mac | Inicia containers |
| `stop-docker.bat` | Windows | Para containers |
| `stop-docker.sh` | Linux/Mac | Para containers |

## Fluxo de Dependências

```
        ┌─────────────┐
        │   Cnab.Api  │  (Depende de tudo)
        └──────┬──────┘
               │
       ┌───────┴────────┐
       │                │
┌──────▼──────┐  ┌─────▼────────┐
│Cnab.Application│ │ Cnab.Infra   │
└──────┬──────┘  └─────┬────────┘
       │                │
       └───────┬────────┘
               │
        ┌──────▼──────┐
        │ Cnab.Domain │  (Não depende de nada)
        └─────────────┘
```

## Tamanhos Aproximados

| Projeto | Arquivos | Linhas de Código |
|---------|----------|------------------|
| Cnab.Domain | 4 | ~150 |
| Cnab.Application | 8 | ~400 |
| Cnab.Infrastructure | 8 | ~350 |
| Cnab.Api | 4 | ~200 |
| Cnab.Tests | 5 | ~500 |
| **Total** | **29** | **~1,600** |

## Como Navegar

### Para entender o domínio:
1. Comece em `src/Cnab.Domain/Entities/`
2. Veja os enums em `src/Cnab.Domain/Enums/`
3. Entenda a lógica em `src/Cnab.Domain/Extensions/`

### Para entender a lógica de negócio:
1. Veja os casos de uso em `src/Cnab.Application/Services/`
2. Entenda o parser em `src/Cnab.Application/Services/CnabParser.cs`

### Para entender a persistência:
1. Veja o contexto em `src/Cnab.Infrastructure/Data/`
2. Entenda os repositórios em `src/Cnab.Infrastructure/Repositories/`
3. Veja as migrations em `src/Cnab.Infrastructure/Migrations/`

### Para entender a API:
1. Abra `src/Cnab.Api/Program.cs`
2. Veja os endpoints definidos
3. Teste com `src/Cnab.Api/wwwroot/index.html`

### Para entender os testes:
1. Comece com `tests/Cnab.Tests/Application/CnabParserTests.cs`
2. Veja testes de integração em `tests/Cnab.Tests/Application/`

## Convenções de Nomenclatura

### Arquivos
- **Entities:** `NomeDaEntidade.cs` (ex: `Store.cs`)
- **Interfaces:** `INomeDoServico.cs` (ex: `IStoreRepository.cs`)
- **Testes:** `ClasseEmTeste + Tests.cs` (ex: `CnabParserTests.cs`)
- **DTOs:** `NomeDto.cs` (ex: `StoreDto.cs`)

### Namespaces
```csharp
Cnab.{Camada}.{Subcategoria}

Exemplos:
- Cnab.Domain.Entities
- Cnab.Application.Services
- Cnab.Infrastructure.Repositories
```

## Próximos Passos

Para adicionar uma nova funcionalidade:

1. **Domain:** Adicione entidades/enums se necessário
2. **Application:** Crie o serviço e interface
3. **Infrastructure:** Implemente repositório se necessário
4. **API:** Adicione o endpoint
5. **Tests:** Adicione testes unitários
6. **Docs:** Atualize README.md

## Comandos Úteis

```bash
# Ver estrutura no terminal
tree /F                           # Windows
tree -L 3                         # Linux/Mac

# Contar linhas de código
find . -name "*.cs" | xargs wc -l # Linux/Mac
(Get-ChildItem -R -Filter *.cs | Get-Content | Measure-Object -Line).Lines  # PowerShell

# Buscar por padrão
grep -r "palavra" src/            # Linux/Mac
Select-String -Pattern "palavra" -Path "src\*" -Recurse  # PowerShell
```

## Referências

- [Clean Architecture](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html)
- [.NET Project Structure](https://docs.microsoft.com/en-us/dotnet/core/tools/)
- [EF Core Migrations](https://docs.microsoft.com/en-us/ef/core/managing-schemas/migrations/)

