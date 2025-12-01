# AtsKungFu - App Mobile

Aplicativo mobile desenvolvido em Flutter seguindo Clean Architecture e princípios SOLID.

## Funcionalidades

- Tela Splash
- Autenticação (Login, Cadastro, Recuperação de Senha)
- Home com informações do usuário
- Tela "Quem Fez" (About)
- Navegação com Bottom Navigation Bar

## Arquitetura

O projeto segue a **Clean Architecture** com três camadas principais:

### Domain Layer
- **Entities**: Modelos de negócio puros
- **Use Cases**: Casos de uso específicos (SRP - Single Responsibility Principle)
- **Repository Interfaces**: Contratos para acesso a dados (DIP - Dependency Inversion Principle)

### Data Layer
- **Models**: Implementação das entidades com serialização
- **Repositories**: Implementação concreta dos repositórios
- **Data Sources**: Interfaces e implementações para acesso a dados locais

### Presentation Layer
- **Pages**: Telas da aplicação
- **Controllers**: Gerenciamento de estado com GetX
- **Widgets**: Componentes reutilizáveis

## Princípios SOLID Aplicados

1. **Single Responsibility Principle (SRP)**: Cada UseCase tem uma única responsabilidade
2. **Open/Closed Principle (OCP)**: Extensível através de interfaces
3. **Liskov Substitution Principle (LSP)**: Implementações podem ser substituídas
4. **Interface Segregation Principle (ISP)**: Interfaces específicas e coesas
5. **Dependency Inversion Principle (DIP)**: Dependência de abstrações, não de implementações

## Estrutura de Pastas

```
lib/
├── core/
│   ├── constants/     # Constantes da aplicação
│   ├── di/            # Dependency Injection
│   ├── routes/        # Configuração de rotas
│   ├── theme/         # Tema da aplicação
│   └── utils/         # Utilitários e validadores
├── data/
│   ├── datasources/   # Interfaces e implementações de data sources
│   ├── models/        # Modelos de dados
│   └── repositories/  # Implementações de repositórios
├── domain/
│   ├── entities/      # Entidades de negócio
│   ├── repositories/  # Interfaces de repositórios
│   └── usecases/      # Casos de uso
└── presentation/
    ├── controllers/   # Controladores GetX
    ├── pages/         # Telas da aplicação
    └── widgets/       # Widgets reutilizáveis
```

## Tecnologias Utilizadas

- **Flutter**: Framework multiplataforma
- **GetX**: Gerenciamento de estado e navegação
- **SharedPreferences**: Armazenamento local
- **Intl**: Internacionalização e formatação de datas

## Como Executar

1. Certifique-se de ter o Flutter instalado:
```bash
flutter --version
```

2. Instale as dependências:
```bash
flutter pub get
```

3. Execute o aplicativo:
```bash
flutter run
```

## Telas

1. **Splash**: Tela inicial com animação
2. **Login**: Autenticação com email e senha
3. **Cadastro**: Nome, email, data de nascimento e senha
4. **Esqueci a Senha**: Recuperação de senha por email
5. **Home**: Dashboard com informações do usuário
6. **Quem Fez**: Informações sobre o aplicativo e desenvolvedores

## Validações

- Email: Formato válido
- Senha: Mínimo 6 caracteres
- Nome: Mínimo 3 caracteres
- Data de Nascimento: Idade mínima 13 anos

## Desenvolvido por

Equipe AtsKungFu