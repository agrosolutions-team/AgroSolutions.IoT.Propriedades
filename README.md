# AgroSolutions.IoT.Propriedades

API para gerenciamento de propriedades rurais e talhões utilizando Clean Architecture e Domain-Driven Design (DDD).

## 🏗️ Arquitetura

Este projeto segue os princípios de **Clean Architecture** e **DDD**, dividido em 4 camadas:

- **Domain**: Entidades de negócio e contratos (interfaces)
- **Application**: Casos de uso, DTOs e serviços de aplicação
- **Infrastructure**: Implementação de persistência (EF Core, PostgreSQL)
- **Api**: Controllers e configurações de API

## 🔧 Tecnologias

- .NET 8
- ASP.NET Core Web API
- Entity Framework Core 8
- PostgreSQL
- JWT Authentication
- Swagger/OpenAPI

## 📋 Pré-requisitos

- .NET 8 SDK
- PostgreSQL 12+
- Visual Studio 2022 ou VS Code

## 🚀 Como Executar

### 1. Configurar o Banco de Dados

Edite o `appsettings.json` ou configure a variável de ambiente `CONNECTION_STRING`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=agrosolutions_propriedades;Username=postgres;Password=sua_senha"
  }
}
```

### 2. Restaurar Dependências

```bash
dotnet restore
```

### 3. Executar a API

As migrations serão aplicadas automaticamente na inicialização:

```bash
cd AgroSolutions.IoT.Propriedades.Api
dotnet run
```

A API estará disponível em:
- HTTPS: https://localhost:7001
- HTTP: http://localhost:5001
- Swagger: https://localhost:7001/swagger

## 🔐 Autenticação

Todos os endpoints requerem autenticação JWT. O token deve ser emitido pela API de Identidade (`AgroSolutions.IoT.Identidade`).

### Claims Obrigatórias:
- `sub`: ID do produtor (Guid)
- `email`: Email do produtor
- `nome`: Nome do produtor

### Como Usar no Swagger:

1. Obtenha um token JWT da API de Identidade
2. Clique no botão "Authorize" no Swagger
3. Digite: `Bearer {seu_token}`
4. Clique em "Authorize"

## 📡 Endpoints

### Propriedades

- `POST /api/propriedades` - Criar nova propriedade com talhões
- `GET /api/propriedades` - Listar propriedades do produtor autenticado
- `GET /api/propriedades/{id}` - Obter propriedade específica

### Talhões

- `POST /api/propriedades/{propriedadeId}/talhoes` - Adicionar talhão à propriedade
- `GET /api/propriedades/{propriedadeId}/talhoes` - Listar talhões da propriedade

## 📊 Modelo de Dados

### Propriedade
- Id (Guid - auto-gerado pelo PostgreSQL)
- Nome
- Descrição (opcional)
- ProdutorId (Guid - extraído do JWT)
- Lista de Talhões

### Talhão
- Id (Guid - auto-gerado pelo PostgreSQL)
- Nome
- AreaEmHectares (decimal)
- CulturaPlantada
- PropriedadeId (FK)

## 🌱 Dados de Seed

O banco é inicializado automaticamente com dados de exemplo:

**Propriedade Modelo**
- Id: `00000000-0000-0000-0000-000000000001`
- Nome: Propriedade Modelo
- Descrição: Propriedade criada automaticamente via migration
- ProdutorId: `00000000-0000-0000-0000-000000000001`

**Talhão 01**
- Id: `00000000-0000-0000-0000-000000000002`
- Nome: Talhão 01
- Área: 10 hectares
- Cultura: Soja

## 🧪 Exemplo de Request

### Criar Propriedade

```json
POST /api/propriedades
Authorization: Bearer {token}

{
  "nome": "Fazenda Santa Maria",
  "descricao": "Propriedade rural para cultivo de grãos",
  "talhoes": [
    {
      "nome": "Talhão Norte",
      "areaEmHectares": 50.5,
      "culturaPlantada": "Milho"
    },
    {
      "nome": "Talhão Sul",
      "areaEmHectares": 30.0,
      "culturaPlantada": "Soja"
    }
  ]
}
```

## 🔒 Regras de Negócio

- Uma propriedade deve ter ao menos um talhão
- Apenas o produtor dono pode acessar suas propriedades
- Área do talhão deve ser maior que zero
- Cultura plantada é obrigatória
- IDs (Guid) são gerados automaticamente pelo PostgreSQL

## 📁 Estrutura do Projeto

```
AgroSolutions.IoT.Propriedades/
├── AgroSolutions.IoT.Propriedades.Domain/
│   ├── Entities/
│   │   ├── Propriedade.cs
│   │   └── Talhao.cs
│   ├── Contracts/
│   │   ├── IPropriedadeRepository.cs
│   │   └── ITalhaoRepository.cs
│   └── Exceptions/
│       └── DomainException.cs
├── AgroSolutions.IoT.Propriedades.Application/
│   ├── DTOs/
│   │   ├── Requests/
│   │   └── Responses/
│   └── Services/
│       ├── PropriedadeService.cs
│       └── TalhaoService.cs
├── AgroSolutions.IoT.Propriedades.Infrastructure/
│   ├── Data/
│   │   ├── PropriedadesDbContext.cs
│   │   └── Configurations/
│   ├── Repositories/
│   └── Migrations/
└── AgroSolutions.IoT.Propriedades.Api/
    ├── Controllers/
    │   ├── PropriedadesController.cs
    │   └── TalhoesController.cs
    ├── Program.cs
    └── appsettings.json
```

## 📝 Licença

Este projeto faz parte do sistema AgroSolutions IoT.
