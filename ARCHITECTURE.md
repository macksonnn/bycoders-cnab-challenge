# Arquitetura do Sistema CNAB

## Visão Geral

O sistema CNAB foi desenvolvido seguindo os princípios de **Clean Architecture** e **Domain-Driven Design (DDD)**, garantindo separação de responsabilidades, testabilidade e manutenibilidade.

## Camadas da Aplicação

### 1. Domain (Cnab.Domain)

**Responsabilidade:** Contém as entidades e regras de negócio fundamentais do domínio.

**Componentes:**
- **Entities:** `Store`, `Transaction`
- **Enums:** `TransactionType`
- **Extensions:** `TransactionTypeExtensions` (lógica de negócio relacionada a tipos de transação)

**Características:**
- Não possui dependências externas
- Representa o núcleo do negócio
- Contém apenas regras de domínio puras

```
Cnab.Domain/
├── Entities/
│   ├── Store.cs
│   └── Transaction.cs
├── Enums/
│   └── TransactionType.cs
└── Extensions/
    └── TransactionTypeExtensions.cs
```

### 2. Application (Cnab.Application)

**Responsabilidade:** Casos de uso e lógica de aplicação.

**Componentes:**
- **Interfaces:** Contratos dos serviços (`ICnabImportService`, `IStoreService`)
- **Services:** Implementação dos casos de uso
- **DTOs:** Objetos de transferência de dados

**Características:**
- Depende apenas do Domain
- Orquestra a lógica de negócio
- Transforma dados entre camadas

```
Cnab.Application/
├── Interfaces/
│   ├── ICnabImportService.cs
│   └── IStoreService.cs
├── Services/
│   ├── CnabImportService.cs
│   ├── CnabParser.cs
│   └── StoreService.cs
├── DTOs/
│   ├── StoreDto.cs
│   └── TransactionDto.cs
└── DependencyInjection.cs
```

### 3. Infrastructure (Cnab.Infrastructure)

**Responsabilidade:** Implementações técnicas e acesso a dados.

**Componentes:**
- **Data:** `CnabDbContext` (EF Core)
- **Repositories:** Implementações de acesso a dados
- **Migrations:** Versionamento do banco de dados

**Características:**
- Depende do Domain
- Implementa abstrações do Application
- Contém detalhes de persistência

```
Cnab.Infrastructure/
├── Data/
│   └── CnabDbContext.cs
├── Repositories/
│   ├── IStoreRepository.cs
│   ├── StoreRepository.cs
│   ├── ITransactionRepository.cs
│   └── TransactionRepository.cs
├── Migrations/
│   └── 20231103000000_InitialCreate.cs
└── DependencyInjection.cs
```

### 4. API (Cnab.Api)

**Responsabilidade:** Camada de apresentação e interface HTTP.

**Componentes:**
- **Program.cs:** Configuração da aplicação e endpoints
- **wwwroot:** Arquivos estáticos (frontend)
- **appsettings:** Configurações da aplicação

**Características:**
- Depende de Application e Infrastructure
- Expõe endpoints REST
- Minimal APIs do ASP.NET Core

```
Cnab.Api/
├── Program.cs
├── appsettings.json
├── appsettings.Development.json
├── Properties/
│   └── launchSettings.json
└── wwwroot/
    └── index.html
```

## Fluxo de Dados

### Importação de CNAB

```
┌─────────────┐
│   Cliente   │
│  (Browser)  │
└──────┬──────┘
       │ HTTP POST /api/cnab/import
       ▼
┌─────────────────────────────────┐
│         Cnab.Api                │
│  (Minimal API Endpoint)         │
└──────┬──────────────────────────┘
       │ IFormFile
       ▼
┌─────────────────────────────────┐
│    Cnab.Application             │
│  CnabImportService              │
│  ├─ CnabParser.ParseLine()      │
│  └─ Validações                  │
└──────┬──────────────────────────┘
       │ Transaction entities
       ▼
┌─────────────────────────────────┐
│   Cnab.Infrastructure           │
│  ├─ StoreRepository             │
│  ├─ TransactionRepository       │
│  └─ CnabDbContext               │
└──────┬──────────────────────────┘
       │ SQL
       ▼
┌─────────────────────────────────┐
│       PostgreSQL                │
└─────────────────────────────────┘
```

### Consulta de Lojas

```
┌─────────────┐
│   Cliente   │
│  (Browser)  │
└──────┬──────┘
       │ HTTP GET /api/stores
       ▼
┌─────────────────────────────────┐
│         Cnab.Api                │
│  (Minimal API Endpoint)         │
└──────┬──────────────────────────┘
       │
       ▼
┌─────────────────────────────────┐
│    Cnab.Application             │
│  StoreService                   │
│  └─ Cálculo de saldos           │
└──────┬──────────────────────────┘
       │ Store entities
       ▼
┌─────────────────────────────────┐
│   Cnab.Infrastructure           │
│  ├─ StoreRepository             │
│  └─ Include(Transactions)       │
└──────┬──────────────────────────┘
       │ SQL
       ▼
┌─────────────────────────────────┐
│       PostgreSQL                │
└─────────────────────────────────┘
```

## Padrões Utilizados

### Repository Pattern
- Abstração do acesso a dados
- Testabilidade através de mocks
- Interfaces: `IStoreRepository`, `ITransactionRepository`

### Dependency Injection
- Inversão de controle
- Registros em `DependencyInjection.cs` de cada camada
- Ciclo de vida: `Scoped` para repositórios e serviços

### Data Transfer Objects (DTO)
- Separação entre entidades de domínio e dados expostos
- `StoreDto`, `TransactionDto`

### Extension Methods
- Lógica de negócio encapsulada
- `TransactionTypeExtensions` para cálculo de sinal

### Strategy Pattern
- Parser de CNAB (`CnabParser.ParseLine`)
- Extensível para outros formatos

## Decisões Arquiteturais

### 1. Clean Architecture
**Por quê?**
- Separação clara de responsabilidades
- Testabilidade
- Facilidade de manutenção
- Independência de frameworks

### 2. Entity Framework Core
**Por quê?**
- ORM maduro e performático
- Migrations para versionamento do BD
- Suporte robusto para PostgreSQL
- LINQ para queries type-safe

### 3. Minimal APIs
**Por quê?**
- Simplicidade
- Performance superior
- Menos boilerplate
- Ideal para APIs REST simples

### 4. PostgreSQL
**Por quê?**
- Open source
- Robusto e escalável
- Excelente suporte para .NET
- ACID compliant

### 5. Repository Pattern
**Por quê?**
- Abstração do acesso a dados
- Facilita testes unitários
- Possibilita troca de tecnologia de persistência

## Escalabilidade

### Horizontal
- API stateless (pode ser replicada)
- PostgreSQL com replicação
- Load balancer na frente da API

### Vertical
- Connection pooling (configurado no EF Core)
- Índices no banco de dados
- Paginação de resultados (implementação futura)

### Caching
**Oportunidades futuras:**
- Redis para cache de lojas
- Cache em memória para tipos de transação
- ETag para cache HTTP

## Testes

### Unitários
- Domain: Testes de extensões
- Application: Testes de serviços com mocks
- Cobertura: >80%

### Integração
- Testes de API com WebApplicationFactory
- Banco em memória (InMemory provider)

### E2E
**Futuro:**
- Testes com Playwright/Selenium
- Validação do fluxo completo

## Segurança

### Implementado
- Validação de entrada
- Tratamento de exceções
- CORS configurável

### Recomendações para Produção
- Autenticação JWT
- Rate limiting
- HTTPS obrigatório
- Validação de tamanho de arquivo
- Antivírus no upload
- Logs de auditoria

## Monitoramento

### Recomendações
- Application Insights / Prometheus
- Health checks (já implementado)
- Logs estruturados (Serilog)
- Métricas de performance

## Evolução Futura

### Fase 2
- [ ] Autenticação e autorização
- [ ] Paginação e filtros
- [ ] Logs estruturados
- [ ] Cache distribuído

### Fase 3
- [ ] Processamento assíncrono (RabbitMQ)
- [ ] Notificações em tempo real (SignalR)
- [ ] Dashboard analítico
- [ ] Exportação de relatórios

### Fase 4
- [ ] Microserviços
- [ ] Event sourcing
- [ ] CQRS
- [ ] Elasticsearch para busca

## Diagrama de Componentes

```
┌─────────────────────────────────────────────────┐
│                  Frontend HTML                  │
│            (wwwroot/index.html)                 │
└────────────────────┬────────────────────────────┘
                     │ HTTP REST
                     ▼
┌─────────────────────────────────────────────────┐
│                 Cnab.Api                        │
│  ┌───────────────────────────────────────────┐  │
│  │         Minimal API Endpoints             │  │
│  │  - POST /api/cnab/import                  │  │
│  │  - GET /api/stores                        │  │
│  │  - GET /                                  │  │
│  └────────────────┬──────────────────────────┘  │
└───────────────────┼─────────────────────────────┘
                    │
┌───────────────────┼─────────────────────────────┐
│  Cnab.Application │                             │
│  ┌────────────────▼──────────────────────────┐  │
│  │         Services Layer                    │  │
│  │  - ICnabImportService                     │  │
│  │  - IStoreService                          │  │
│  │  - CnabParser                             │  │
│  └────────────────┬──────────────────────────┘  │
└───────────────────┼─────────────────────────────┘
                    │
┌───────────────────┼─────────────────────────────┐
│ Cnab.Infrastructure│                            │
│  ┌────────────────▼──────────────────────────┐  │
│  │         Repository Layer                  │  │
│  │  - IStoreRepository                       │  │
│  │  - ITransactionRepository                 │  │
│  └────────────────┬──────────────────────────┘  │
│                   │                              │
│  ┌────────────────▼──────────────────────────┐  │
│  │         CnabDbContext (EF Core)           │  │
│  └────────────────┬──────────────────────────┘  │
└───────────────────┼─────────────────────────────┘
                    │
                    ▼
         ┌──────────────────────┐
         │     PostgreSQL       │
         │   ┌──────────────┐   │
         │   │   stores     │   │
         │   └──────┬───────┘   │
         │          │1:N         │
         │   ┌──────▼───────┐   │
         │   │ transactions │   │
         │   └──────────────┘   │
         └──────────────────────┘
```

## Referências

- [Clean Architecture - Robert C. Martin](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html)
- [Domain-Driven Design - Eric Evans](https://www.domainlanguage.com/ddd/)
- [ASP.NET Core Best Practices](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/best-practices)
- [Entity Framework Core](https://docs.microsoft.com/en-us/ef/core/)

