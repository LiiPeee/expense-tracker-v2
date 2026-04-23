# Expense Tracker V2

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
- **Entity Framework Core**
- **SQL Server**
- **JWT Authentication**
- **Swagger/OpenAPI**
- **ASP.NET Core Web API**

## 🏗️ Arquitetura

O projeto segue uma arquitetura em camadas:

- **ExpenseTrackerV2.WebApi**: Camada de apresentação (Controllers)
- **ExpenseTrackerV2.Application**: Lógica de aplicação (Services)
- **ExpenseTrackerV2.Core**: Domínio (Entities, DTOs, Interfaces)
- **ExpenseTrackerV2.Infrastructure**: Infraestrutura (Repositories, Persistence)

## 📦 Pré-requisitos

- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- [SQL Server 2019+](https://www.microsoft.com/sql-server/sql-server-downloads) ou SQL Server Docker
- [Visual Studio 2022+](https://visualstudio.microsoft.com/) ou [VS Code](https://code.visualstudio.com/)
- [Git](https://git-scm.com/)

## 💾 Configuração do Banco de Dados

### 1. SQL Server via Docker (Recomendado)

```bash
docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=Abc12345!" -p 1433:1433 --name sqlserver -d mcr.microsoft.com/mssql/server:2019-latest
```

Ou usando Docker Compose (criar arquivo `docker-compose.yml` na raiz):

```yaml
services:
  sqlserver:
    image: mcr.microsoft.com/mssql/server:2022-latest
    container_name: sqlserver
    environment:
      ACCEPT_EULA: "Y"
      SA_PASSWORD: "Abc12345!"
      MSSQL_PID: "Developer"
    ports:
      - "1433:1433"
    volumes:
      - sql_data:/var/opt/mssql
    restart: unless-stopped

volumes:
  sql_data:
```

Execute: `docker-compose up -d`

### 2. Criar o Banco de Dados

Crie o banco com este comando isolado:

```sql
CREATE DATABASE ExpenseTracker;
```

Depois, abra uma nova consulta conectada ao banco `ExpenseTracker` e execute os scripts de tabelas abaixo.

Se o seu cliente SQL permitir trocar o contexto da conexão por comando, você pode executar antes:

```sql
USE ExpenseTracker;
```

### 3. Estrutura das Tabelas

#### Tabela: Account

```sql
CREATE TABLE Account (
    Id BIGINT PRIMARY KEY IDENTITY(1,1),
    FirstName NVARCHAR(100) NOT NULL,
    LastName NVARCHAR(100) NOT NULL,
    Email NVARCHAR(255) NOT NULL UNIQUE,
    Password NVARCHAR(MAX) NOT NULL,
    Balance DECIMAL(18,2) NOT NULL DEFAULT 0,
    Role NVARCHAR(50) NOT NULL DEFAULT 'User',
    RefreshToken NVARCHAR(MAX) NULL,
    RefreshTokenExpiryTime DATETIMEOFFSET NULL,
    IsActive BIT NOT NULL DEFAULT 1,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    UpdatedAt DATETIME2 NULL
);
```

#### Tabela: Category

```sql
CREATE TABLE Category (
    Id BIGINT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(100) NOT NULL,
    Description NVARCHAR(MAX) NULL,
    IsActive BIT NOT NULL DEFAULT 1,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    UpdatedAt DATETIME2 NULL
);

-- Inserir categorias padrão
INSERT INTO Category (Name, Description, IsActive) VALUES
('ALIMENTACAO', 'Gastos com alimentação', 1),
('CONFORTO', 'Gastos com conforto', 1),
('MORADIA', 'Gastos com moradia', 1),
('TRANSPORTE', 'Gastos com transporte', 1),
('SAUDE', 'Gastos com saúde', 1),
('EDUCACAO', 'Gastos com educação', 1),
('LAZER', 'Gastos com lazer', 1),
('BENS_PESSOAIS', 'Gastos com bens pessoais', 1),
('INVESTIMENTO', 'Investimentos', 1),
('OUTROS', 'Outros gastos', 1),
('RENDA_VARIAVEL', 'Renda variável', 1),
('BENEFICIOS', 'Benefícios', 1),
('SALARIO', 'Salário', 1);
```

#### Tabela: SubCategory

```sql
CREATE TABLE SubCategory (
    Id BIGINT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(100) NOT NULL,
    Description NVARCHAR(MAX) NULL,
    IsActive BIT NOT NULL DEFAULT 1,
    CategoryId BIGINT NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    UpdatedAt DATETIME2 NULL,
    FOREIGN KEY (CategoryId) REFERENCES Category(Id)
);
```

#### Tabela: TypeContact

```sql
CREATE TABLE TypeContact (
    Id BIGINT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(100) NOT NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    UpdatedAt DATETIME2 NULL
);

-- Inserir tipos de contato
INSERT INTO TypeContact (Name) VALUES ('PERSONAL'), ('BUSINESS');
```

#### Tabela: Contact

```sql
CREATE TABLE Contact (
    Id BIGINT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(200) NOT NULL,
    Email NVARCHAR(255) NULL,
    Phone NVARCHAR(20) NULL,
    Document NVARCHAR(50) NULL,
    IsActive BIT NOT NULL DEFAULT 1,
    AccountId BIGINT NOT NULL,
    TypeContactId BIGINT NOT NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    UpdatedAt DATETIME2 NULL,
    FOREIGN KEY (AccountId) REFERENCES Account(Id),
    FOREIGN KEY (TypeContactId) REFERENCES TypeContact(Id)
);
```

#### Tabela: Address

```sql
CREATE TABLE Address (
    Id BIGINT PRIMARY KEY IDENTITY(1,1),
    Street NVARCHAR(200) NOT NULL,
    City NVARCHAR(100) NOT NULL,
    State NVARCHAR(50) NOT NULL,
    ZipCode NVARCHAR(20) NOT NULL,
    Country NVARCHAR(100) NOT NULL,
    IsPrimary BIT NOT NULL DEFAULT 0,
    ContactId BIGINT NOT NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    UpdatedAt DATETIME2 NULL,
    FOREIGN KEY (ContactId) REFERENCES Contact(Id)
);
```

#### Tabela: TypeTransaction

```sql
CREATE TABLE TypeTransaction (
    Id BIGINT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(100) NOT NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    UpdatedAt DATETIME2 NULL
);

-- Inserir tipos de transação
INSERT INTO TypeTransaction (Name) VALUES ('EXPENSE'), ('INCOME');
```

#### Tabela: Recurrence

```sql
CREATE TABLE Recurrence (
    Id BIGINT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(50) NOT NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    UpdatedAt DATETIME2 NULL
);

-- Inserir tipos de recorrência
INSERT INTO Recurrence (Name) VALUES ('NONE'), ('DAILY'), ('BiWeekly'), ('Monthly');
```

#### Tabela: Transactions

```sql
CREATE TABLE Transactions (
    Id BIGINT PRIMARY KEY IDENTITY(1,1),
    Amount DECIMAL(18,2) NOT NULL,
    Name NVARCHAR(200) NOT NULL,
    Description NVARCHAR(MAX) NOT NULL,
    Paid BIT NOT NULL DEFAULT 0,
    NumberOfInstallment BIGINT NULL,
    DateOfInstallment DATETIME2 NULL,
    QuantityInstallment NVARCHAR(20) NULL,
    RecurrenceId BIGINT NOT NULL,
    ContactId BIGINT NULL,
    AccountId BIGINT NOT NULL,
    CategoryId BIGINT NOT NULL,
    TypeTransactionId BIGINT NOT NULL,
    SubCategoryId BIGINT NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    UpdatedAt DATETIME2 NULL,
    FOREIGN KEY (AccountId) REFERENCES Account(Id),
    FOREIGN KEY (CategoryId) REFERENCES Category(Id),
    FOREIGN KEY (ContactId) REFERENCES Contact(Id),
    FOREIGN KEY (SubCategoryId) REFERENCES SubCategory(Id),
    FOREIGN KEY (RecurrenceId) REFERENCES Recurrence(Id),
    FOREIGN KEY (TypeTransactionId) REFERENCES TypeTransaction(Id)
);

CREATE TABLE ResetPassowrd(
    Id BIGINT PRIMARY KEY IDENTITY(1,1),
    AccountId BIGINT NOT NULL,
    HashedToken VARCHAR(100) NULL,
   	FOREIGN KEY (AccountId) REFERENCES Account(Id)
);
CREATE TABLE  ResetPassword(
    Id BIGINT PRIMARY KEY IDENTITY(1,1),
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    UpdatedAt DATETIME2 NULL,
    AccountId BIGINT NOT NULL,
    HashedToken VARCHAR(100),
   	ExpireAt DATETIME NULL,
);

```

### 4. Script Completo de Criação

Use os comandos abaixo em duas etapas para evitar erros em clientes que não suportam `GO`.

#### Etapa 1: criar o banco

Execute este comando isoladamente:

```sql
IF DB_ID('ExpenseTracker') IS NULL
BEGIN
  CREATE DATABASE ExpenseTracker;
END;
```

#### Etapa 2: criar tabelas e dados iniciais

Abra uma nova consulta já conectada ao banco `ExpenseTracker` e execute:

```sql
CREATE TABLE Account (
  Id BIGINT PRIMARY KEY IDENTITY(1,1),
  FirstName NVARCHAR(100) NOT NULL,
  LastName NVARCHAR(100) NOT NULL,
  Email NVARCHAR(255) NOT NULL UNIQUE,
  Password NVARCHAR(MAX) NOT NULL,
  Balance DECIMAL(18,2) NOT NULL DEFAULT 0,
  Role NVARCHAR(50) NOT NULL DEFAULT 'User',
  RefreshToken NVARCHAR(MAX) NULL,
  RefreshTokenExpiryTime DATETIMEOFFSET NULL,
  IsActive BIT NOT NULL DEFAULT 1,
  CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
  UpdatedAt DATETIME2 NULL
);

CREATE TABLE Category (
  Id BIGINT PRIMARY KEY IDENTITY(1,1),
  Name NVARCHAR(100) NOT NULL,
  Description NVARCHAR(MAX) NULL,
  IsActive BIT NOT NULL DEFAULT 1,
  CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
  UpdatedAt DATETIME2 NULL
);

CREATE TABLE SubCategory (
  Id BIGINT PRIMARY KEY IDENTITY(1,1),
  Name NVARCHAR(100) NOT NULL,
  Description NVARCHAR(MAX) NULL,
  IsActive BIT NOT NULL DEFAULT 1,
  CategoryId BIGINT NULL,
  CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
  UpdatedAt DATETIME2 NULL,
  FOREIGN KEY (CategoryId) REFERENCES Category(Id)
);

CREATE TABLE TypeContact (
  Id BIGINT PRIMARY KEY IDENTITY(1,1),
  Name NVARCHAR(100) NOT NULL,
  CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
  UpdatedAt DATETIME2 NULL
);

CREATE TABLE Contact (
  Id BIGINT PRIMARY KEY IDENTITY(1,1),
  Name NVARCHAR(200) NOT NULL,
  Email NVARCHAR(255) NULL,
  Phone NVARCHAR(20) NULL,
  Document NVARCHAR(50) NULL,
  IsActive BIT NOT NULL DEFAULT 1,
  AccountId BIGINT NOT NULL,
  TypeContactId BIGINT NOT NULL,
  CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
  UpdatedAt DATETIME2 NULL,
  FOREIGN KEY (AccountId) REFERENCES Account(Id),
  FOREIGN KEY (TypeContactId) REFERENCES TypeContact(Id)
);

CREATE TABLE Address (
  Id BIGINT PRIMARY KEY IDENTITY(1,1),
  Street NVARCHAR(200) NOT NULL,
  City NVARCHAR(100) NOT NULL,
  State NVARCHAR(50) NOT NULL,
  ZipCode NVARCHAR(20) NOT NULL,
  Country NVARCHAR(100) NOT NULL,
  IsPrimary BIT NOT NULL DEFAULT 0,
  ContactId BIGINT NOT NULL,
  CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
  UpdatedAt DATETIME2 NULL,
  FOREIGN KEY (ContactId) REFERENCES Contact(Id)
);

CREATE TABLE TypeTransaction (
  Id BIGINT PRIMARY KEY IDENTITY(1,1),
  Name NVARCHAR(100) NOT NULL,
  CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
  UpdatedAt DATETIME2 NULL
);

CREATE TABLE Recurrence (
  Id BIGINT PRIMARY KEY IDENTITY(1,1),
  Name NVARCHAR(50) NOT NULL,
  CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
  UpdatedAt DATETIME2 NULL
);

CREATE TABLE Transactions (
  Id BIGINT PRIMARY KEY IDENTITY(1,1),
  Amount DECIMAL(18,2) NOT NULL,
  Name NVARCHAR(200) NOT NULL,
  Description NVARCHAR(MAX) NOT NULL,
  Paid BIT NOT NULL DEFAULT 0,
  NumberOfInstallment BIGINT NULL,
  DateOfInstallment DATETIME2 NULL,
  QuantityInstallment NVARCHAR(20) NULL,
  RecurrenceId BIGINT NOT NULL,
  ContactId BIGINT NULL,
  AccountId BIGINT NOT NULL,
  CategoryId BIGINT NOT NULL,
  TypeTransactionId BIGINT NOT NULL,
  SubCategoryId BIGINT NULL,
  CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
  UpdatedAt DATETIME2 NULL,
  FOREIGN KEY (AccountId) REFERENCES Account(Id),
  FOREIGN KEY (CategoryId) REFERENCES Category(Id),
  FOREIGN KEY (ContactId) REFERENCES Contact(Id),
  FOREIGN KEY (SubCategoryId) REFERENCES SubCategory(Id),
  FOREIGN KEY (RecurrenceId) REFERENCES Recurrence(Id),
  FOREIGN KEY (TypeTransactionId) REFERENCES TypeTransaction(Id)
);

INSERT INTO Category (Name, Description, IsActive) VALUES
('ALIMENTACAO', 'Gastos com alimentação', 1),
('CONFORTO', 'Gastos com conforto', 1),
('MORADIA', 'Gastos com moradia', 1),
('TRANSPORTE', 'Gastos com transporte', 1),
('SAUDE', 'Gastos com saúde', 1),
('EDUCACAO', 'Gastos com educação', 1),
('LAZER', 'Gastos com lazer', 1),
('BENS_PESSOAIS', 'Gastos com bens pessoais', 1),
('INVESTIMENTO', 'Investimentos', 1),
('OUTROS', 'Outros gastos', 1),
('RENDA_VARIAVEL', 'Renda variável', 1),
('BENEFICIOS', 'Benefícios', 1),
('SALARIO', 'Salário', 1);

INSERT INTO TypeContact (Name) VALUES ('PERSONAL'), ('BUSINESS');
INSERT INTO TypeTransaction (Name) VALUES ('EXPENSE'), ('INCOME');
INSERT INTO Recurrence (Name) VALUES ('NONE'), ('DAILY'), ('BiWeekly'), ('Monthly');
```

## 🔧 Instalação

### 1. Clonar o repositório

```bash
git clone https://github.com/LiiPeee/expense-tracker-v2.git
cd expense-tracker-v2
```

### 2. Configurar a Connection String

Edite o arquivo `src/ExpenseTrackerV2.WebApi/appsettings.json`:

```json
{
  "ConnectionStrings": {
    "ExpenseTrackerV2": "Server=localhost,1433;Database=ExpenseTracker;User Id=sa;Password=SUA_SENHA;TrustServerCertificate=True;"
  },
  "Jwt": {
    "Token": "sua-chave-secreta-jwt-aqui-deve-ser-longa-e-segura",
    "Issuer": "ExpenseTrackerV2",
    "Audience": "ExpenseTrackerV2",
    "TokenExpirationMinutes": 60,
    "RefreshTokenExpirationMinutes": 60
  }
}
```

### 3. Restaurar pacotes

```bash
dotnet restore
```

### 4. Aplicar Migrations (se houver)

```bash
cd src/ExpenseTrackerV2.Infrastructure
dotnet ef database update --startup-project ../ExpenseTrackerV2.WebApi
```

## ▶️ Executando o Projeto

### Via Visual Studio

1. Abra a solução `ExpenseTrackerV2.sln`
2. Defina `ExpenseTrackerV2.WebApi` como projeto de inicialização
3. Pressione `F5` ou clique em **Run**

### Via CLI

```bash
cd src/ExpenseTrackerV2.WebApi
dotnet run
```

A API estará disponível em:

- **HTTPS**: https://localhost:7xxx
- **HTTP**: http://localhost:5xxx
- **Swagger**: https://localhost:7xxx/swagger

## 📁 Estrutura do Projeto

```
expense-tracker-v2/
├── src/
│   ├── ExpenseTrackerV2.WebApi/          # API Controllers
│   │   ├── Controllers/
│   │   │   ├── AuthController.cs
│   │   │   ├── TransactionsController.cs
│   │   │   ├── ContactController.cs
│   │   │   ├── CategoryController.cs
│   │   │   └── SubCategoryController.cs
│   │   ├── Program.cs
│   │   └── appsettings.json
│   │
│   ├── ExpenseTrackerV2.Application/     # Application Services
│   │   └── Service/
│   │       ├── TransactionsAppService.cs
│   │       ├── AuthenticationAppService.cs
│   │       └── CategoryAppService.cs
│   │
│   ├── ExpenseTrackerV2.Core/            # Domain Layer
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
│   └── ExpenseTrackerV2.Infrastructure/  # Infrastructure Layer
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

- Verifique se o SQL Server está rodando
- Confirme a connection string no `appsettings.json`
- Teste a conexão com SQL Server Management Studio

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
