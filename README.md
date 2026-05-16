# Budget Tracker

Sistema de controle financeiro desenvolvido em .NET 9 com C# 13.0, permitindo gerenciar despesas, receitas, contatos e categorias de transações.

## 📋 Sumário

- [Tecnologias](#tecnologias)
- [Arquitetura](#arquitetura)
- [Pré-requisitos](#pré-requisitos)
- [Configuração do Banco de Dados](#configuração-do-banco-de-dados)
- [Instalação](#instalação)
- [Executando o Projeto](#executando-o-projeto)
- [Estrutura do Projeto](#estrutura-do-projeto)
- [API Endpoints](#api-endpoints)
- [Autenticação](#autenticação)

## 🚀 Tecnologias

- **.NET 9.0**
- **C# 13.0**
- **Dapper**
- **PostgreSQL**
- **JWT Authentication**
- **Swagger/OpenAPI**
- **ASP.NET Core Web API**

## 🏗️ Arquitetura

O projeto segue uma arquitetura em camadas:

- **BudgetTracker.WebApi**: Camada de apresentação (Controllers)
- **BudgetTracker.Application**: Lógica de aplicação (Services)
- **BudgetTracker.Core**: Domínio (Entities, DTOs, Interfaces)
- **BudgetTracker.Infrastructure**: Infraestrutura (Repositories, Persistence)

## 📦 Pré-requisitos

- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- [PostgreSQL 16+](https://www.postgresql.org/download/) ou PostgreSQL via Docker
- [Visual Studio 2022+](https://visualstudio.microsoft.com/) ou [VS Code](https://code.visualstudio.com/)
- [Git](https://git-scm.com/)

## 💾 Configuração do Banco de Dados

### 1. PostgreSQL via Docker (Recomendado)

```bash
docker run -d \
  --name postgres \
  -e POSTGRES_USER=admin \
  -e POSTGRES_PASSWORD=senha123 \
  -e POSTGRES_DB=budgettracker \
  -p 5432:5432 \
  -v postgres_data:/var/lib/postgresql/data \
  postgres:16
```

Ou usando Docker Compose (criar arquivo `docker-compose.yml` na raiz):

```yaml
services:
  postgres:
    image: postgres:16
    container_name: postgres
    environment:
      POSTGRES_USER: admin
      POSTGRES_PASSWORD: senha123
      POSTGRES_DB: budgettracker
    ports:
      - "5432:5432"
    volumes:
      - postgres_data:/var/lib/postgresql/data
    restart: unless-stopped

volumes:
  postgres_data:
```

Execute: `docker-compose up -d`

### 2. Criar o Banco de Dados

Conecte ao servidor PostgreSQL e execute:

```sql
CREATE DATABASE budgettracker;
```

Em seguida, conecte ao banco criado e execute os scripts de tabelas abaixo.

### 3. Executar o Script de Criação

Conecte ao banco `budgettracker` e execute o arquivo `database/init.sql`. Ele cria todas as tabelas e insere os dados de lookup necessários.

```bash
psql "sua-connection-string" -f database/init.sql
```

Ou copie o conteúdo do arquivo e cole no Query Editor do Render/Neon.

> O script usa `CREATE TABLE IF NOT EXISTS` e `ON CONFLICT DO NOTHING` — é seguro rodar mais de uma vez.

### 4. Estrutura das Tabelas (referência)

| Tabela | Descrição |
|---|---|
| `Account` | Usuários do sistema |
| `Category` | Categorias fixas (ALIMENTACAO, MORADIA, etc.) |
| `SubCategory` | Subcategorias criadas pelo usuário, vinculadas a Category e Account |
| `Contact` | Contatos (fornecedores/clientes) vinculados à Account |
| `Address` | Endereços vinculados a Contact |
| `Transactions` | Transações financeiras |
| `TypeTransaction` | Lookup: EXPENSE (1), INCOME (2) |
| `TypeContact` | Lookup: PERSONAL (1), BUSINESS (2) |
| `Recurrence` | Lookup: NONE (1), DAILY (2), BIWEEKLY (3), MONTHLY (4) |
| `ResetPassword` | Tokens de redefinição de senha |

## 🔧 Instalação

### 1. Clonar o repositório

```bash
git clone https://github.com/LiiPeee/budget-tracker.git
cd budget-tracker
```

### 2. Configurar a Connection String

Edite o arquivo `src/BudgetTracker.WebApi/appsettings.json`:

```json
{
  "ConnectionStrings": {
    "BudgetTracker": "Host=localhost;Port=5432;Database=budgettracker;Username=admin;Password=senha123;"
  },
  "Jwt": {
    "Token": "sua-chave-secreta-jwt-aqui-deve-ser-longa-e-segura",
    "Issuer": "BudgetTracker",
    "Audience": "BudgetTracker",
    "TokenExpirationMinutes": 60,
    "RefreshTokenExpirationMinutes": 60
  }
}
```

### 3. Restaurar pacotes

```bash
dotnet restore
```

## ▶️ Executando o Projeto

### Via Visual Studio

1. Abra a solução `BudgetTracker.sln`
2. Defina `BudgetTracker.WebApi` como projeto de inicialização
3. Pressione `F5` ou clique em **Run**

### Via CLI

```bash
cd src/BudgetTracker.WebApi
dotnet run
```

A API estará disponível em:

- **HTTPS**: https://localhost:7xxx
- **HTTP**: http://localhost:5xxx
- **Swagger**: https://localhost:7xxx/swagger

## 📁 Estrutura do Projeto

```
budget-tracker/
├── src/
│   ├── BudgetTracker.WebApi/          # API Controllers
│   │   ├── Controllers/
│   │   │   ├── AuthController.cs
│   │   │   ├── TransactionsController.cs
│   │   │   ├── ContactController.cs
│   │   │   ├── CategoryController.cs
│   │   │   └── SubCategoryController.cs
│   │   ├── Program.cs
│   │   └── appsettings.json
│   │
│   ├── BudgetTracker.Application/     # Application Services
│   │   └── Service/
│   │       ├── TransactionsAppService.cs
│   │       ├── AuthenticationAppService.cs
│   │       └── CategoryAppService.cs
│   │
│   ├── BudgetTracker.Core/            # Domain Layer
│   │   └── Domain/
│   │       ├── Entities/
│   │       │   ├── Account.cs
│   │       │   ├── Transactions.cs
│   │       │   ├── Category.cs
│   │       │   ├── SubCategory.cs
│   │       │   ├── Contact.cs
│   │       │   └── Address.cs
│   │       ├── Dtos/
│   │       ├── Repository/
│   │       ├── Service/
│   │       └── UnitOfWork/
│   │
│   └── BudgetTracker.Infrastructure/  # Infrastructure Layer
│       └── Persistence/
│           └── Repository/
│               ├── TransactionsRepository.cs
│               ├── CategoryRepository.cs
│               ├── ContactRepository.cs
│               └── AccountRepository.cs
└── README.md
```

## 🔌 API Endpoints

### Autenticação

- `POST /api/auth/register` - Registrar novo usuário
- `POST /api/auth/login` - Login
- `POST /api/auth/refresh-token` - Renovar token

### Transações

- `GET /api/transactions` - Listar transações
- `POST /api/transactions` - Criar transação
- `PUT /api/transactions/{id}/paid` - Marcar como pago
- `DELETE /api/transactions/{id}` - Deletar transação
- `GET /api/transactions/filter` - Filtrar por mês/ano
- `GET /api/transactions/filter-by-type` - Filtrar por tipo (despesa/receita)
- `GET /api/transactions/filter-by-category` - Filtrar por categoria
- `GET /api/transactions/economy` - Obter economia (receitas - despesas)
- `GET /api/transactions/expense` - Total de despesas do mês
- `GET /api/transactions/income` - Total de receitas do mês

### Categorias

- `GET /api/category` - Listar categorias
- `POST /api/category` - Criar categoria
- `PUT /api/category/{id}` - Atualizar categoria
- `DELETE /api/category/{id}` - Deletar categoria

### SubCategorias

- `GET /api/subcategory` - Listar subcategorias
- `POST /api/subcategory` - Criar subcategoria
- `PUT /api/subcategory/{id}` - Atualizar subcategoria
- `DELETE /api/subcategory/{id}` - Deletar subcategoria

### Contatos

- `GET /api/contact` - Listar contatos
- `POST /api/contact` - Criar contato
- `PUT /api/contact/{id}` - Atualizar contato
- `DELETE /api/contact/{id}` - Deletar contato

## 🔐 Autenticação

A API utiliza JWT (JSON Web Token) para autenticação.

### Como usar:

1. **Registre um usuário** em `/api/auth/register`

   ```json
   {
     "firstName": "João",
     "lastName": "Silva",
     "email": "joao@email.com",
     "password": "SenhaForte123!"
   }
   ```

2. **Faça login** em `/api/auth/login`

   ```json
   {
     "email": "joao@email.com",
     "password": "SenhaForte123!"
   }
   ```

3. **Copie o token JWT** retornado

4. **Adicione o header** em todas as requisições:
   ```
   Authorization: Bearer {seu-token-jwt}
   ```

### No Swagger:

1. Clique em **Authorize** (cadeado no canto superior direito)
2. Digite: `Bearer {seu-token-jwt}`
3. Clique em **Authorize**

## 📝 Exemplo de Uso

### Criar uma Transação

```json
POST /api/transactions

{
  "transactionName": "Compra de Supermercado",
  "amount": 150.00,
  "description": "Compras mensais",
  "contactName": "Supermercado ABC",
  "categoryName": "Alimentação",
  "subCategoryName": "Mercado",
  "typeTransaction": "Despesa",
  "recurrence": "Mensal",
  "numberOfInstallment": 0,
  "dateOfInstallment": null
}
```

### Filtrar Transações por Mês

```
GET /api/transactions/filter?month=12&year=2024&pageNumber=1
```

## 🎯 Funcionalidades

- ✅ Autenticação e autorização com JWT
- ✅ Gerenciamento de despesas e receitas
- ✅ Categorização de transações
- ✅ Gestão de contatos (fornecedores/clientes)
- ✅ Parcelamento de transações
- ✅ Filtros avançados (mês, ano, categoria, tipo)
- ✅ Cálculo automático de economia (receitas - despesas)
- ✅ Atualização automática de saldo ao marcar transação como paga
- ✅ Paginação de resultados
- ✅ API RESTful com Swagger/OpenAPI

## 🧪 Testes

Para executar os testes (quando disponíveis):

```bash
dotnet test
```

## 🐛 Troubleshooting

### Erro de conexão com banco de dados

- Verifique se o container PostgreSQL está rodando: `docker ps`
- Confirme a connection string no `appsettings.json`
- Teste a conexão: `docker exec -it postgres psql -U admin -d budgettracker`

### Erro de autenticação JWT

- Verifique se o token JWT no `appsettings.json` é longo e seguro
- Confirme que está enviando o header `Authorization: Bearer {token}`
- Verifique se o token não expirou

### Erro ao criar transação

- Certifique-se de que a categoria existe no banco de dados
- Verifique se o contato foi criado previamente
- Confirme que todos os campos obrigatórios estão preenchidos

## 📄 Licença

Este projeto está sob a licença MIT.

## 👤 Autor

**Luiz Cruz**

- GitHub: [@LiiPeee](https://github.com/LiiPeee)

## 🤝 Contribuindo

Contribuições são bem-vindas! Sinta-se à vontade para abrir issues e pull requests.

1. Fork o projeto
2. Crie uma branch (`git checkout -b feature/NovaFeature`)
3. Commit suas mudanças (`git commit -m 'Adiciona nova feature'`)
4. Push para a branch (`git push origin feature/NovaFeature`)
5. Abra um Pull Request

## 📞 Suporte

Se você tiver alguma dúvida ou problema, sinta-se à vontade para abrir uma [issue](https://github.com/LiiPeee/expense-tracker-v2/issues) no GitHub.

---

⭐ Se este projeto foi útil para você, considere dar uma estrela no GitHub!
