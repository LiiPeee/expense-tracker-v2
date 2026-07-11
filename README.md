# Budget Tracker

Sistema de controle financeiro desenvolvido em .NET 9 com C# 13.0, permitindo gerenciar despesas, receitas, contatos e categorias de transações.

## 📋 Sumário

- [Tecnologias](#tecnologias)
- [Arquitetura](#arquitetura)
- [Pré-requisitos](#pré-requisitos)
- [Configuração do Banco de Dados](#configuração-do-banco-de-dados)
- [Migração de Produção](#-migração-de-produção)
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

Conecte ao banco `budgettracker` e execute o script abaixo. Ele cria todas as tabelas, índices e insere os dados de lookup necessários.

> O script usa `CREATE TABLE IF NOT EXISTS` e `ON CONFLICT DO NOTHING` — é seguro rodar mais de uma vez.

```sql
-- ============================================================
-- LOOKUP TABLES
-- Devem ser populadas antes de qualquer dado transacional.
-- ============================================================

CREATE TABLE IF NOT EXISTS TypeTransaction (
    Id        BIGINT      PRIMARY KEY,
    Name      VARCHAR(50) NOT NULL,
    CreatedAt TIMESTAMPTZ NOT NULL DEFAULT NOW()
);

CREATE TABLE IF NOT EXISTS TypeContact (
    Id        BIGINT      PRIMARY KEY,
    Name      VARCHAR(50) NOT NULL,
    CreatedAt TIMESTAMPTZ NOT NULL DEFAULT NOW()
);

CREATE TABLE IF NOT EXISTS Recurrence (
    Id        BIGINT      PRIMARY KEY,
    Name      VARCHAR(50) NOT NULL,
    CreatedAt TIMESTAMPTZ NOT NULL DEFAULT NOW()
);

-- ============================================================
-- CORE TABLES
-- ============================================================

CREATE TABLE IF NOT EXISTS Account (
    Id                           BIGSERIAL      PRIMARY KEY,
    FirstName                    VARCHAR(50)    NOT NULL,
    LastName                     VARCHAR(50)    NOT NULL,
    Email                        VARCHAR(64)    NOT NULL,
    Password                     VARCHAR(255)   NOT NULL,
    Balance                      NUMERIC(18, 2) NOT NULL DEFAULT 0,
    Role                         VARCHAR(20)    NOT NULL DEFAULT 'User',
    RefreshToken                 VARCHAR(500),
    RefreshTokenExpiryTime       TIMESTAMPTZ,
    EmailVerified                BOOLEAN        NOT NULL DEFAULT FALSE,
    VerifiedAt                   TIMESTAMPTZ,
    EmailVerificationToken       VARCHAR(255),
    EmailVerificationTokenExpiry TIMESTAMPTZ,
    VerifyAttempts               BIGINT         NOT NULL DEFAULT 0,
    IsActive                     BOOLEAN        NOT NULL DEFAULT TRUE,
    CreatedAt                    TIMESTAMPTZ    NOT NULL DEFAULT NOW(),
    UpdatedAt                    TIMESTAMPTZ,

    CONSTRAINT uq_account_email UNIQUE (Email)
);

CREATE TABLE IF NOT EXISTS Category (
    Id          BIGSERIAL    PRIMARY KEY,
    Name        VARCHAR(100) NOT NULL,
    Description VARCHAR(255),
    IsActive    BOOLEAN      NOT NULL DEFAULT TRUE,
    CreatedAt   TIMESTAMPTZ  NOT NULL DEFAULT NOW(),
    UpdatedAt   TIMESTAMPTZ
);

CREATE TABLE IF NOT EXISTS SubCategory (
    Id          BIGSERIAL    PRIMARY KEY,
    Name        VARCHAR(100) NOT NULL,
    Description VARCHAR(255),
    IsActive    BOOLEAN      NOT NULL DEFAULT TRUE,
    CategoryId  BIGINT,
    AccountId   BIGINT       NOT NULL,
    CreatedAt   TIMESTAMPTZ  NOT NULL DEFAULT NOW(),
    UpdatedAt   TIMESTAMPTZ,

    CONSTRAINT fk_subcategory_category
        FOREIGN KEY (CategoryId) REFERENCES Category (Id) ON DELETE SET NULL,
    CONSTRAINT fk_subcategory_account
        FOREIGN KEY (AccountId)  REFERENCES Account  (Id) ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS Contact (
    Id            BIGSERIAL    PRIMARY KEY,
    Name          VARCHAR(100) NOT NULL,
    Email         VARCHAR(255),
    Phone         VARCHAR(30),
    Document      VARCHAR(50),
    IsActive      BOOLEAN      NOT NULL DEFAULT TRUE,
    AccountId     BIGINT       NOT NULL,
    TypeContactId BIGINT       NOT NULL,
    CreatedAt     TIMESTAMPTZ  NOT NULL DEFAULT NOW(),
    UpdatedAt     TIMESTAMPTZ,

    CONSTRAINT fk_contact_account
        FOREIGN KEY (AccountId)     REFERENCES Account     (Id) ON DELETE CASCADE,
    CONSTRAINT fk_contact_typecontact
        FOREIGN KEY (TypeContactId) REFERENCES TypeContact (Id)
);

CREATE TABLE IF NOT EXISTS Address (
    Id        BIGSERIAL    PRIMARY KEY,
    Street    VARCHAR(255) NOT NULL,
    City      VARCHAR(100) NOT NULL,
    State     VARCHAR(100) NOT NULL,
    ZipCode   VARCHAR(20)  NOT NULL,
    Country   VARCHAR(100) NOT NULL,
    IsPrimary BOOLEAN      NOT NULL DEFAULT FALSE,
    ContactId BIGINT       NOT NULL,
    AccountId BIGINT       NOT NULL,
    CreatedAt TIMESTAMPTZ  NOT NULL DEFAULT NOW(),
    UpdatedAt TIMESTAMPTZ,

    CONSTRAINT fk_address_contact
        FOREIGN KEY (ContactId) REFERENCES Contact (Id) ON DELETE CASCADE,
    CONSTRAINT fk_address_account
        FOREIGN KEY (AccountId) REFERENCES Account (Id) ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS Transactions (
    Id                  BIGSERIAL      PRIMARY KEY,
    Amount              NUMERIC(18, 2) NOT NULL,
    Name                VARCHAR(100)   NOT NULL,
    Description         VARCHAR(255),
    Paid                BOOLEAN        NOT NULL DEFAULT FALSE,
    NumberOfInstallment BIGINT,
    DateOfInstallment   DATE,
    CompetenceDate      DATE           NOT NULL DEFAULT CURRENT_DATE,
    QuantityInstallment VARCHAR(50),
    RecurrenceId        BIGINT         NOT NULL,
    ContactId           BIGINT,
    SubCategoryId       BIGINT,
    AccountId           BIGINT         NOT NULL,
    CategoryId          BIGINT         NOT NULL,
    TypeTransactionId   BIGINT         NOT NULL,
    CreatedAt           TIMESTAMPTZ    NOT NULL DEFAULT NOW(),
    UpdatedAt           TIMESTAMPTZ,

    CONSTRAINT chk_transactions_amount
        CHECK (Amount > 0),
    CONSTRAINT fk_transactions_account
        FOREIGN KEY (AccountId)       REFERENCES Account         (Id) ON DELETE CASCADE,
    CONSTRAINT fk_transactions_category
        FOREIGN KEY (CategoryId)      REFERENCES Category        (Id),
    CONSTRAINT fk_transactions_subcategory
        FOREIGN KEY (SubCategoryId)   REFERENCES SubCategory     (Id) ON DELETE SET NULL,
    CONSTRAINT fk_transactions_contact
        FOREIGN KEY (ContactId)       REFERENCES Contact         (Id) ON DELETE SET NULL,
    CONSTRAINT fk_transactions_type
        FOREIGN KEY (TypeTransactionId) REFERENCES TypeTransaction (Id),
    CONSTRAINT fk_transactions_recurrence
        FOREIGN KEY (RecurrenceId)    REFERENCES Recurrence      (Id)
);

CREATE TABLE IF NOT EXISTS ResetPassword (
    Id          BIGSERIAL    PRIMARY KEY,
    AccountId   BIGINT       NOT NULL,
    HashedToken VARCHAR(500) NOT NULL,
    ExpireAt    TIMESTAMPTZ  NOT NULL,
    CreatedAt   TIMESTAMPTZ  NOT NULL DEFAULT NOW(),
    UpdatedAt   TIMESTAMPTZ,

    CONSTRAINT fk_resetpassword_account
        FOREIGN KEY (AccountId) REFERENCES Account (Id) ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS BudgetLimit (
    Id          BIGSERIAL      PRIMARY KEY,
    IsLimit     BOOLEAN        NOT NULL DEFAULT FALSE,
    Month       INTEGER        NOT NULL,
    Year        INTEGER        NOT NULL,
    CategoryId  BIGINT         NOT NULL,
    AccountId   BIGINT         NOT NULL,
    Percentage  NUMERIC(18, 2) NOT NULL DEFAULT 0,
    LimitAmount NUMERIC(18, 2) NOT NULL,
    CreatedAt   TIMESTAMPTZ    NOT NULL DEFAULT NOW(),
    UpdatedAt   TIMESTAMPTZ,

    CONSTRAINT chk_budgetlimit_month
        CHECK (Month BETWEEN 1 AND 12),
    CONSTRAINT chk_budgetlimit_limitamount
        CHECK (LimitAmount > 0),
    CONSTRAINT fk_budgetlimit_account
        FOREIGN KEY (AccountId)  REFERENCES Account  (Id) ON DELETE CASCADE,
    CONSTRAINT fk_budgetlimit_category
        FOREIGN KEY (CategoryId) REFERENCES Category (Id)
);

CREATE TABLE IF NOT EXISTS Stock (
    Id          BIGSERIAL      PRIMARY KEY,
    AccountId   BIGINT         NOT NULL,
    Ticker      VARCHAR(20)    NOT NULL,
    Title       VARCHAR(100)   NOT NULL,
    Description VARCHAR(255),
    PriceBuyed  NUMERIC(18, 2) NOT NULL,
    PriceMarket NUMERIC(18, 2) NOT NULL DEFAULT 0,
    Avarage     NUMERIC(18, 2) NOT NULL DEFAULT 0,
    Quantity    BIGINT         NOT NULL DEFAULT 0,
    CreatedAt   TIMESTAMPTZ    NOT NULL DEFAULT NOW(),
    UpdatedAt   TIMESTAMPTZ,

    CONSTRAINT fk_stock_account
        FOREIGN KEY (AccountId) REFERENCES Account (Id) ON DELETE CASCADE
);

-- ============================================================
-- INDEXES
-- ============================================================

CREATE UNIQUE INDEX IF NOT EXISTS uix_account_email               ON Account      (Email);
CREATE INDEX IF NOT EXISTS idx_account_isactive                   ON Account      (IsActive);
CREATE INDEX IF NOT EXISTS idx_account_refreshtoken               ON Account      (RefreshToken) WHERE RefreshToken IS NOT NULL;

CREATE INDEX IF NOT EXISTS idx_subcategory_accountid              ON SubCategory  (AccountId);
CREATE INDEX IF NOT EXISTS idx_subcategory_categoryid             ON SubCategory  (CategoryId)  WHERE CategoryId IS NOT NULL;

CREATE INDEX IF NOT EXISTS idx_contact_accountid                  ON Contact      (AccountId);
CREATE INDEX IF NOT EXISTS idx_contact_typecontactid              ON Contact      (TypeContactId);

CREATE INDEX IF NOT EXISTS idx_address_contactid                  ON Address      (ContactId);
CREATE INDEX IF NOT EXISTS idx_address_accountid                  ON Address      (AccountId);

CREATE INDEX IF NOT EXISTS idx_transactions_accountid             ON Transactions (AccountId);
CREATE INDEX IF NOT EXISTS idx_transactions_categoryid            ON Transactions (CategoryId);
CREATE INDEX IF NOT EXISTS idx_transactions_subcategoryid         ON Transactions (SubCategoryId)     WHERE SubCategoryId IS NOT NULL;
CREATE INDEX IF NOT EXISTS idx_transactions_contactid             ON Transactions (ContactId)          WHERE ContactId IS NOT NULL;
CREATE INDEX IF NOT EXISTS idx_transactions_typetransactionid     ON Transactions (TypeTransactionId);
CREATE INDEX IF NOT EXISTS idx_transactions_recurrenceid          ON Transactions (RecurrenceId);
CREATE INDEX IF NOT EXISTS idx_transactions_account_createdat     ON Transactions (AccountId, CreatedAt DESC);
CREATE INDEX IF NOT EXISTS idx_transactions_account_category      ON Transactions (AccountId, CategoryId);

CREATE INDEX IF NOT EXISTS idx_resetpassword_accountid            ON ResetPassword (AccountId);
CREATE INDEX IF NOT EXISTS idx_resetpassword_hashedtoken          ON ResetPassword (HashedToken);

CREATE INDEX IF NOT EXISTS idx_budgetlimit_accountid              ON BudgetLimit  (AccountId);
CREATE INDEX IF NOT EXISTS idx_budgetlimit_account_category       ON BudgetLimit  (AccountId, CategoryId);

CREATE INDEX IF NOT EXISTS idx_stock_accountid                    ON Stock        (AccountId);

-- ============================================================
-- SEED DATA
-- ============================================================

INSERT INTO TypeTransaction (Id, Name) VALUES
    (1, 'EXPENSE'),
    (2, 'INCOME')
ON CONFLICT (Id) DO NOTHING;

INSERT INTO TypeContact (Id, Name) VALUES
    (1, 'PERSONAL'),
    (2, 'BUSINESS')
ON CONFLICT (Id) DO NOTHING;

INSERT INTO Recurrence (Id, Name) VALUES
    (1, 'NONE'),
    (2, 'DAILY'),
    (3, 'BIWEEKLY'),
    (4, 'MONTHLY')
ON CONFLICT (Id) DO NOTHING;

INSERT INTO Category (Name, Description) VALUES
    ('MORADIA',        'Despesas com moradia e habitação'),
    ('TRANSPORTE',     'Despesas com transporte'),
    ('ALIMENTACAO',    'Despesas com alimentação'),
    ('SAUDE',          'Despesas com saúde'),
    ('EDUCACAO',       'Despesas com educação'),
    ('LAZER',          'Despesas com lazer e entretenimento'),
    ('BENS_PESSOAIS',  'Despesas com bens pessoais'),
    ('INVESTIMENTO',   'Investimentos em geral'),
    ('RENDA_VARIAVEL', 'Receitas de renda variável'),
    ('BENEFICIOS',     'Benefícios recebidos'),
    ('SALARIO',        'Salário e remuneração fixa'),
    ('CONFORTO',       'Despesas com conforto e qualidade de vida'),
    ('OUTROS',         'Despesas e receitas diversas')
ON CONFLICT DO NOTHING;
```

## 🔄 Migração de Produção

Caso você já tenha executado uma versão anterior do script (com nomes em `snake_case` e plural), rode o script abaixo para corrigir o banco existente **sem perder dados**.

```sql
-- ============================================================
-- BudgetTracker - Script de Migração de Produção
-- Corrige: nomes de tabelas (plural/snake_case → PascalCase)
--          nomes de colunas (snake_case → PascalCase)
-- É seguro rodar em um banco com dados existentes.
-- ============================================================

BEGIN;

-- ============================================================
-- PASSO 1: Renomear tabelas lookup
-- ============================================================
ALTER TABLE IF EXISTS type_transaction RENAME TO TypeTransaction;
ALTER TABLE IF EXISTS type_contact     RENAME TO TypeContact;
-- recurrence já está correto, apenas corrige coluna abaixo

-- ============================================================
-- PASSO 2: Renomear tabelas core
-- ============================================================
ALTER TABLE IF EXISTS accounts       RENAME TO Account;
ALTER TABLE IF EXISTS categories     RENAME TO Category;
ALTER TABLE IF EXISTS sub_categories RENAME TO SubCategory;
ALTER TABLE IF EXISTS contacts       RENAME TO Contact;
ALTER TABLE IF EXISTS addresses      RENAME TO Address;
-- transactions já está correto (plural = nome da entity)
ALTER TABLE IF EXISTS reset_passwords RENAME TO ResetPassword;

-- ============================================================
-- PASSO 3: Renomear colunas - TypeTransaction
-- ============================================================
ALTER TABLE IF EXISTS TypeTransaction RENAME COLUMN created_at TO CreatedAt;

-- ============================================================
-- PASSO 4: Renomear colunas - TypeContact
-- ============================================================
ALTER TABLE IF EXISTS TypeContact RENAME COLUMN created_at TO CreatedAt;

-- ============================================================
-- PASSO 5: Renomear colunas - Recurrence
-- ============================================================
ALTER TABLE IF EXISTS Recurrence RENAME COLUMN created_at TO CreatedAt;

-- ============================================================
-- PASSO 6: Renomear colunas - Account (era accounts)
-- ============================================================
ALTER TABLE IF EXISTS Account RENAME COLUMN first_name                      TO FirstName;
ALTER TABLE IF EXISTS Account RENAME COLUMN last_name                       TO LastName;
ALTER TABLE IF EXISTS Account RENAME COLUMN refresh_token                   TO RefreshToken;
ALTER TABLE IF EXISTS Account RENAME COLUMN refresh_token_expiry_time       TO RefreshTokenExpiryTime;
ALTER TABLE IF EXISTS Account RENAME COLUMN email_verified                  TO EmailVerified;
ALTER TABLE IF EXISTS Account RENAME COLUMN verified_at                     TO VerifiedAt;
ALTER TABLE IF EXISTS Account RENAME COLUMN email_verification_token        TO EmailVerificationToken;
ALTER TABLE IF EXISTS Account RENAME COLUMN email_verification_token_expiry TO EmailVerificationTokenExpiry;
ALTER TABLE IF EXISTS Account RENAME COLUMN verify_attempts                 TO VerifyAttempts;
ALTER TABLE IF EXISTS Account RENAME COLUMN is_active                       TO IsActive;
ALTER TABLE IF EXISTS Account RENAME COLUMN created_at                      TO CreatedAt;
ALTER TABLE IF EXISTS Account RENAME COLUMN updated_at                      TO UpdatedAt;

-- ============================================================
-- PASSO 7: Renomear colunas - Category (era categories)
-- ============================================================
ALTER TABLE IF EXISTS Category RENAME COLUMN is_active  TO IsActive;
ALTER TABLE IF EXISTS Category RENAME COLUMN created_at TO CreatedAt;
ALTER TABLE IF EXISTS Category RENAME COLUMN updated_at TO UpdatedAt;

-- ============================================================
-- PASSO 8: Renomear colunas - SubCategory (era sub_categories)
-- ============================================================
ALTER TABLE IF EXISTS SubCategory RENAME COLUMN is_active   TO IsActive;
ALTER TABLE IF EXISTS SubCategory RENAME COLUMN category_id TO CategoryId;
ALTER TABLE IF EXISTS SubCategory RENAME COLUMN account_id  TO AccountId;
ALTER TABLE IF EXISTS SubCategory RENAME COLUMN created_at  TO CreatedAt;
ALTER TABLE IF EXISTS SubCategory RENAME COLUMN updated_at  TO UpdatedAt;

-- ============================================================
-- PASSO 9: Renomear colunas - Contact (era contacts)
-- ============================================================
ALTER TABLE IF EXISTS Contact RENAME COLUMN is_active       TO IsActive;
ALTER TABLE IF EXISTS Contact RENAME COLUMN account_id      TO AccountId;
ALTER TABLE IF EXISTS Contact RENAME COLUMN type_contact_id TO TypeContactId;
ALTER TABLE IF EXISTS Contact RENAME COLUMN created_at      TO CreatedAt;
ALTER TABLE IF EXISTS Contact RENAME COLUMN updated_at      TO UpdatedAt;

-- ============================================================
-- PASSO 10: Renomear colunas - Address (era addresses)
-- ============================================================
ALTER TABLE IF EXISTS Address RENAME COLUMN is_primary TO IsPrimary;
ALTER TABLE IF EXISTS Address RENAME COLUMN zip_code   TO ZipCode;
ALTER TABLE IF EXISTS Address RENAME COLUMN contact_id TO ContactId;
ALTER TABLE IF EXISTS Address RENAME COLUMN created_at TO CreatedAt;
ALTER TABLE IF EXISTS Address RENAME COLUMN updated_at TO UpdatedAt;

-- ============================================================
-- PASSO 11: Renomear colunas - Transactions
-- ============================================================
ALTER TABLE IF EXISTS Transactions RENAME COLUMN number_of_installment TO NumberOfInstallment;
ALTER TABLE IF EXISTS Transactions RENAME COLUMN date_of_installment   TO DateOfInstallment;
ALTER TABLE IF EXISTS Transactions RENAME COLUMN quantity_installment  TO QuantityInstallment;
ALTER TABLE IF EXISTS Transactions RENAME COLUMN recurrence_id         TO RecurrenceId;
ALTER TABLE IF EXISTS Transactions RENAME COLUMN contact_id            TO ContactId;
ALTER TABLE IF EXISTS Transactions RENAME COLUMN sub_category_id       TO SubCategoryId;
ALTER TABLE IF EXISTS Transactions RENAME COLUMN account_id            TO AccountId;
ALTER TABLE IF EXISTS Transactions RENAME COLUMN category_id           TO CategoryId;
ALTER TABLE IF EXISTS Transactions RENAME COLUMN type_transaction_id   TO TypeTransactionId;
ALTER TABLE IF EXISTS Transactions RENAME COLUMN created_at            TO CreatedAt;
ALTER TABLE IF EXISTS Transactions RENAME COLUMN updated_at            TO UpdatedAt;

-- ============================================================
-- PASSO 12: Renomear colunas - ResetPassword (era reset_passwords)
-- ============================================================
ALTER TABLE IF EXISTS ResetPassword RENAME COLUMN account_id   TO AccountId;
ALTER TABLE IF EXISTS ResetPassword RENAME COLUMN hashed_token TO HashedToken;
ALTER TABLE IF EXISTS ResetPassword RENAME COLUMN expire_at    TO ExpireAt;
ALTER TABLE IF EXISTS ResetPassword RENAME COLUMN created_at   TO CreatedAt;
ALTER TABLE IF EXISTS ResetPassword RENAME COLUMN updated_at   TO UpdatedAt;

COMMIT;
```

| Tabela            | Descrição                                                           |
| ----------------- | ------------------------------------------------------------------- |
| `Account`         | Usuários do sistema                                                 |
| `Category`        | Categorias fixas (ALIMENTACAO, MORADIA, etc.)                       |
| `SubCategory`     | Subcategorias criadas pelo usuário, vinculadas a Category e Account |
| `Contact`         | Contatos (fornecedores/clientes) vinculados à Account               |
| `Address`         | Endereços vinculados a Contact                                      |
| `Transactions`    | Transações financeiras                                              |
| `TypeTransaction` | Lookup: EXPENSE (1), INCOME (2)                                     |
| `TypeContact`     | Lookup: PERSONAL (1), BUSINESS (2)                                  |
| `Recurrence`      | Lookup: NONE (1), DAILY (2), BIWEEKLY (3), MONTHLY (4)              |
| `ResetPassword`   | Tokens de redefinição de senha                                      |

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
