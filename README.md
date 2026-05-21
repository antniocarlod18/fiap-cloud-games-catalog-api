# Catalog API – Fiap Cloud Games

Microserviço do catálogo responsável por gerenciar os domínios de:

- Jogos
- Promoções
- Pedidos
- Carrinho
- Biblioteca de usuários

## 📦 Visão geral da solução

Esta solução é construída em .NET 10 e composta pelos seguintes projetos:

- `FiapCloudGamesCatalog.Api` – API HTTP principal e ponto de entrada do microsserviço
- `FiapCloudGamesCatalog.Application` – regras de negócio, serviços, validação e manipuladores de eventos
- `FiapCloudGamesCatalog.Domain` – entidades de domínio, agregados, eventos e contratos de repositórios
- `FiapCloudGamesCatalog.Infra.Data` – persistência em MySQL, cache Redis, Elasticsearch e integração com mensageria
- `FiapCloudGames.Contracts` – contratos de eventos e modelos compartilhados
- `FiapCloudGames.Tests` – testes unitários e de integração da aplicação

## 🚀 Funcionalidades principais

A API cobre:

- Cadastro, consulta e atualização de jogos
- Aplicação de promoções
- Criação e processamento de pedidos
- Gerenciamento de carrinho de compras
- Biblioteca de jogos do usuário
- Autenticação e autorização JWT
- Cache distribuído e busca via Elasticsearch
- Integração de eventos com Azure Service Bus via MassTransit

## ⚙️ Dependências obrigatórias

Para executar localmente, o serviço precisa das seguintes dependências:

- MySQL 8.x
- Redis
- Azure Service Bus
- Elasticsearch
- MongoDB (usado para catálogos específicos)

> Em ambientes de desenvolvimento, essas dependências podem ser orquestradas localmente ou fornecidas por serviços gerenciados.

## 🔧 Configuração

A aplicação usa `appsettings.json` e variáveis de ambiente para configuração. Os valores principais são:

- `ConnectionStrings:MySQL`
- `Authentication:Key`
- `Authentication:Issuer`
- `Authentication:Audience`
- `ElasticSearch:Uri`
- `ElasticSearch:IndexName`
- `ElasticSearch:ApiKey`
- `AzureServiceBus:ConnectionString`
- `Redis:ConnectionString`
- `MongoCatalog:ConnectionString`
- `MongoCatalog:DatabaseName`

### Exemplo de variáveis de ambiente

No Windows PowerShell:

```powershell
$env:ConnectionStrings__MySQL = 'server=localhost;uid=root;pwd=password;database=fiap_catalog'
$env:Authentication__Key = 'uma-chave-segura-aqui'
$env:Authentication__Issuer = 'fiap-cloud-games'
$env:Authentication__Audience = 'fiap-cloud-games-client'
$env:ElasticSearch__Uri = 'http://localhost:9200'
$env:Redis__ConnectionString = 'localhost:6379'
$env:AzureServiceBus__ConnectionString = '<connection-string>'
$env:MongoCatalog__ConnectionString = 'mongodb://localhost:27017'
$env:MongoCatalog__DatabaseName = 'fiap_catalog'
```

## 🧪 Executar localmente

### Restaurar dependências

```powershell
dotnet restore
```

### Compilar

```powershell
dotnet build
```

### Executar a API

```powershell
dotnet run --project FiapCloudGamesCatalog.Api/FiapCloudGamesCatalog.Api.csproj
```

A API inicia com o pipeline de middleware configurado em `FiapCloudGamesCatalog.Api/Program.cs`.

### Executar migrações do banco de dados

```powershell
dotnet run --project FiapCloudGamesCatalog.Api/FiapCloudGamesCatalog.Api.csproj -- migrate
```

## 🧭 Documentação de API

No ambiente de desenvolvimento, o Swagger UI está habilitado e pode ser acessado após iniciar o serviço.

- `http://localhost:5000/swagger` (ou porta configurada pelo ASP.NET Core)

## 🧰 Testes

Execute todos os testes da solução com:

```powershell
dotnet test
```

## 🐳 Docker

A imagem é construída a partir do `FiapCloudGamesCatalog.Api/Dockerfile`.

### Build da imagem

```powershell
docker build -f FiapCloudGamesCatalog.Api/Dockerfile -t fiap-cloud-games-catalog-api:latest .
```

### Executar o contêiner

```powershell
docker run --rm -p 8080:8080 -p 8081:8081 \
  -e ConnectionStrings__MySQL='<conn>' \
  -e Authentication__Key='<key>' \
  -e Authentication__Issuer='<issuer>' \
  -e Authentication__Audience='<audience>' \
  -e ElasticSearch__Uri='<uri>' \
  -e Redis__ConnectionString='<redis>' \
  -e AzureServiceBus__ConnectionString='<bus>' \
  -e MongoCatalog__ConnectionString='<mongo>' \
  -e MongoCatalog__DatabaseName='<db>' \
  fiap-cloud-games-catalog-api:latest
```

## ☁️ CI / CD

O pipeline está definido em `azure-pipelines.yml` e realiza as seguintes etapas:

1. Restaura pacotes .NET
2. Compila a solução em Release
3. Executa testes unitários e coleta cobertura
4. Constrói e publica imagem Docker
5. Publica manifestos Kubernetes
6. Faz deploy em cluster AKS usando o manifesto

## 📁 Estrutura do repositório

- `FiapCloudGamesCatalog.Api/` – API REST e configuração de middleware
- `FiapCloudGamesCatalog.Application/` – serviços de aplicação, validações e handlers
- `FiapCloudGamesCatalog.Domain/` – modelo de domínio e abstrações de repositório
- `FiapCloudGamesCatalog.Infra.Data/` – implementação de persistência e mensageria
- `FiapCloudGames.Contracts/` – contratos e eventos de integração
- `FiapCloudGames.Tests/` – testes automatizados

## 💡 Observações

- O projeto utiliza `MassTransit` para integração com Azure Service Bus.
- Os dados de catálogo podem ser indexados no Elasticsearch para busca avançada.
- A autorização utiliza JWT e uma política customizada `SameUserOrAdmin`.

---

Se precisar de ajuda para adaptar o ambiente local ou criar scripts de inicialização, posso ajudar a documentar isso também.
---
