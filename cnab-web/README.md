# CNAB Web - Frontend Angular

Frontend em Angular para o sistema de importação e gerenciamento de transações CNAB.

## 🚀 Tecnologias

- **Angular** 17+
- **TypeScript** 5.4+
- **SCSS** para estilização
- **Standalone Components**
- **HttpClient** para consumo de API

## 📋 Pré-requisitos

- Node.js 18+ e npm
- Angular CLI (`npm install -g @angular/cli`)
- API CNAB rodando em `http://localhost:5000`

## 🔧 Instalação

```bash
# Instalar dependências
npm install
```

## 🏃 Como Executar

### Desenvolvimento

```bash
# Iniciar servidor de desenvolvimento
npm start

# ou
ng serve

# Acesse http://localhost:4200
```

### Build de Produção

```bash
# Build para produção
npm run build

# Arquivos gerados em dist/cnab-web
```

## 📁 Estrutura do Projeto

```
src/app/
├── core/                      # Serviços e modelos
│   ├── models/
│   │   ├── store.model.ts
│   │   └── transaction.model.ts
│   └── services/
│       └── cnab-api.service.ts
├── features/                  # Páginas/Features
│   ├── upload/
│   │   ├── upload-cnab.component.ts
│   │   ├── upload-cnab.component.html
│   │   └── upload-cnab.component.scss
│   └── store-list/
│       ├── store-list.component.ts
│       ├── store-list.component.html
│       └── store-list.component.scss
├── shared/                    # Componentes compartilhados
├── app.component.ts           # Componente raiz
├── app.config.ts              # Configuração da aplicação
└── app.routes.ts              # Rotas
```

## 🌐 Funcionalidades

### Upload de CNAB
- Upload de arquivo via seleção ou drag-and-drop
- Validação de arquivo
- Feedback visual de progresso
- Redirecionamento automático após sucesso

### Listagem de Lojas
- Visualização de lojas com saldo consolidado
- Lista de transações por loja
- Formatação de valores monetários
- Indicadores visuais para receitas e despesas
- Atualização manual dos dados

## 🎨 Estilização

O projeto utiliza **CSS puro** (SCSS) sem frameworks externos:
- Variáveis CSS personalizadas
- Flexbox e Grid
- Gradientes modernos
- Animações suaves
- Design responsivo

## 🔌 Integração com API

### Endpoints Consumidos

- `POST /api/cnab/import` - Upload de arquivo CNAB
- `GET /api/stores` - Listagem de lojas e transações

### Configuração da API

Edite `src/environments/environment.ts`:

```typescript
export const environment = {
  production: false,
  apiUrl: 'http://localhost:5000'  // URL da API
};
```

## 🧪 Testes

```bash
# Executar testes unitários
npm test

# ou
ng test
```

## 📦 Build

```bash
# Build de produção
ng build --configuration production

# Build de desenvolvimento
ng build
```

## 🚀 Deploy

Os arquivos buildados estarão em `dist/cnab-web` e podem ser servidos por qualquer servidor web estático (nginx, Apache, etc).

## 📝 Notas de Desenvolvimento

- Projeto utiliza **Standalone Components** (Angular 17+)
- Lazy loading configurado para as rotas
- HttpClient configurado globalmente
- CORS deve estar habilitado na API

## 🤝 Integração com Backend .NET

Certifique-se de que:
1. A API .NET está rodando em `http://localhost:5000`
2. CORS está configurado para aceitar `http://localhost:4200`
3. Os endpoints retornam os dados no formato esperado

## 📄 Licença

MIT

## ✨ Author

Developed by **Macksonnn** © 2025

