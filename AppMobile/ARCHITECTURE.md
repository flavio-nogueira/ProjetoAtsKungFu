# Arquitetura do Projeto

## Clean Architecture

Este projeto segue os princípios da Clean Architecture, dividindo o código em camadas bem definidas e independentes.

## Camadas

### 1. Domain Layer (Camada de Domínio)
A camada mais interna e independente. Contém a lógica de negócio pura.

**Entities** (`lib/domain/entities/`)
- Objetos de negócio puros sem dependências externas
- Exemplo: `UserEntity` representa um usuário no sistema

**Use Cases** (`lib/domain/usecases/`)
- Cada use case representa uma ação específica no sistema
- Segue o SRP (Single Responsibility Principle)
- Exemplos:
  - `LoginUseCase`: Responsável apenas por autenticar o usuário
  - `RegisterUseCase`: Responsável apenas por registrar um novo usuário
  - `ForgotPasswordUseCase`: Responsável apenas por recuperar senha

**Repository Interfaces** (`lib/domain/repositories/`)
- Define contratos para acesso a dados
- Segue DIP (Dependency Inversion Principle)
- Exemplo: `IAuthRepository` define métodos sem implementação

### 2. Data Layer (Camada de Dados)
Implementa o acesso aos dados e comunica com fontes externas.

**Models** (`lib/data/models/`)
- Extensões das entidades com serialização/deserialização
- Conversão entre JSON e objetos Dart
- Exemplo: `UserModel` extends `UserEntity`

**Data Sources** (`lib/data/datasources/`)
- Interfaces e implementações para diferentes fontes de dados
- `ILocalStorage`: Interface para armazenamento local
- `LocalStorageImpl`: Implementação usando SharedPreferences

**Repositories** (`lib/data/repositories/`)
- Implementações concretas das interfaces do domínio
- Exemplo: `AuthRepositoryImpl` implements `IAuthRepository`

### 3. Presentation Layer (Camada de Apresentação)
Responsável pela interface do usuário e interação.

**Pages** (`lib/presentation/pages/`)
- Telas da aplicação organizadas por funcionalidade
- Cada tela em sua própria pasta com controller

**Controllers** (`lib/presentation/controllers/`)
- Gerenciamento de estado usando GetX
- Lógica de apresentação separada da view
- Exemplo: `LoginController` gerencia estado da tela de login

**Widgets** (`lib/presentation/widgets/`)
- Componentes reutilizáveis de interface

### 4. Core (Núcleo)
Funcionalidades compartilhadas por todas as camadas.

**Constants** (`lib/core/constants/`)
- Constantes da aplicação
- `AppRoutes`: Rotas da aplicação
- `StorageKeys`: Chaves para armazenamento

**Dependency Injection** (`lib/core/di/`)
- Configuração de injeção de dependência
- Registro de dependências no GetX

**Routes** (`lib/core/routes/`)
- Configuração de navegação
- Mapeamento de rotas para páginas

**Theme** (`lib/core/theme/`)
- Tema visual da aplicação
- Cores, estilos e componentes

**Utils** (`lib/core/utils/`)
- Utilitários e helpers
- `AppValidators`: Validações de formulários

## Fluxo de Dados

```
UI (Page)
  ↓
Controller
  ↓
Use Case
  ↓
Repository Interface
  ↓
Repository Implementation
  ↓
Data Source
  ↓
External (SharedPreferences, API, etc.)
```

## Princípios SOLID Implementados

### Single Responsibility Principle (SRP)
- Cada classe tem uma única responsabilidade
- Use Cases específicos para cada ação
- Controllers dedicados para cada tela

### Open/Closed Principle (OCP)
- Classes abertas para extensão, fechadas para modificação
- Interfaces permitem novas implementações sem alterar código existente

### Liskov Substitution Principle (LSP)
- Implementações podem substituir suas interfaces
- `AuthRepositoryImpl` pode ser substituído por outra implementação de `IAuthRepository`

### Interface Segregation Principle (ISP)
- Interfaces pequenas e específicas
- `ILocalStorage` tem apenas métodos relacionados a armazenamento
- `IAuthRepository` tem apenas métodos relacionados a autenticação

### Dependency Inversion Principle (DIP)
- Dependências de abstrações, não de implementações concretas
- Use Cases dependem de `IAuthRepository`, não de `AuthRepositoryImpl`
- Controllers recebem use cases via injeção de dependência

## Benefícios desta Arquitetura

1. **Testabilidade**: Cada camada pode ser testada independentemente
2. **Manutenibilidade**: Mudanças em uma camada não afetam as outras
3. **Escalabilidade**: Fácil adicionar novas funcionalidades
4. **Separação de Responsabilidades**: Código organizado e limpo
5. **Independência de Frameworks**: Lógica de negócio isolada
6. **Reutilização de Código**: Components e use cases reutilizáveis

## Exemplo Prático: Fluxo de Login

1. Usuário insere credenciais na `LoginPage`
2. `LoginController` valida os dados
3. Controller chama `LoginUseCase`
4. Use Case usa `IAuthRepository` (interface)
5. `AuthRepositoryImpl` (implementação) é executado
6. Repository usa `ILocalStorage` para verificar dados
7. `LocalStorageImpl` acessa SharedPreferences
8. Resultado retorna pela cadeia até o Controller
9. Controller atualiza a UI através do GetX

## Injeção de Dependência

O projeto usa GetX para gerenciar dependências:

```dart
// Registro de dependências
Get.lazyPut<ILocalStorage>(() => LocalStorageImpl(sharedPreferences));
Get.lazyPut<IAuthRepository>(() => AuthRepositoryImpl(Get.find()));
Get.lazyPut(() => LoginUseCase(Get.find()));
Get.lazyPut(() => LoginController(Get.find()));
```

Benefícios:
- Facilita testes (mock de dependências)
- Reduz acoplamento
- Segue princípio DIP
- Permite trocar implementações facilmente
