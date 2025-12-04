# 📱 INTEGRAÇÃO FLUTTER - API ATS KUNG FU

Guia completo para integrar sua aplicação Flutter com a API de autenticação.

---

## 📦 Dependências Necessárias

Adicione ao seu `pubspec.yaml`:

```yaml
dependencies:
  flutter:
    sdk: flutter

  # HTTP Client
  dio: ^5.4.0

  # Armazenamento seguro de tokens
  flutter_secure_storage: ^9.0.0

  # Gerenciamento de estado (escolha um)
  provider: ^6.1.1
  # OU
  # riverpod: ^2.4.9

  # Injeção de dependência
  get_it: ^7.6.4

  # JSON Serialization
  json_annotation: ^4.8.1

dev_dependencies:
  # JSON Code Generation
  build_runner: ^2.4.6
  json_serializable: ^6.7.1
```

Execute:
```bash
flutter pub get
```

---

## 🔐 1. Modelos (DTOs)

### `lib/models/auth/login_dto.dart`
```dart
import 'package:json_annotation/json_annotation.dart';

part 'login_dto.g.dart';

@JsonSerializable()
class LoginDto {
  final String email;
  final String senha;
  final bool lembrarMe;

  LoginDto({
    required this.email,
    required this.senha,
    this.lembrarMe = false,
  });

  factory LoginDto.fromJson(Map<String, dynamic> json) =>
      _$LoginDtoFromJson(json);

  Map<String, dynamic> toJson() => _$LoginDtoToJson(this);
}
```

### `lib/models/auth/register_usuario_dto.dart`
```dart
import 'package:json_annotation/json_annotation.dart';

part 'register_usuario_dto.g.dart';

@JsonSerializable()
class RegisterUsuarioDto {
  final String nomeCompleto;
  final String email;
  final String? cpf;
  final String? telefone;
  final DateTime? dataNascimento;
  final String senha;
  final String confirmarSenha;

  RegisterUsuarioDto({
    required this.nomeCompleto,
    required this.email,
    this.cpf,
    this.telefone,
    this.dataNascimento,
    required this.senha,
    required this.confirmarSenha,
  });

  factory RegisterUsuarioDto.fromJson(Map<String, dynamic> json) =>
      _$RegisterUsuarioDtoFromJson(json);

  Map<String, dynamic> toJson() => _$RegisterUsuarioDtoToJson(this);
}
```

### `lib/models/auth/usuario_dto.dart`
```dart
import 'package:json_annotation/json_annotation.dart';

part 'usuario_dto.g.dart';

@JsonSerializable()
class UsuarioDto {
  final String id;
  final String nomeCompleto;
  final String email;
  final String? cpf;
  final String? telefone;
  final DateTime? dataNascimento;
  final String? fotoPerfil;
  final bool emailConfirmado;
  final bool telefoneConfirmado;
  final DateTime dataCriacao;
  final DateTime? dataUltimoLogin;

  UsuarioDto({
    required this.id,
    required this.nomeCompleto,
    required this.email,
    this.cpf,
    this.telefone,
    this.dataNascimento,
    this.fotoPerfil,
    required this.emailConfirmado,
    required this.telefoneConfirmado,
    required this.dataCriacao,
    this.dataUltimoLogin,
  });

  factory UsuarioDto.fromJson(Map<String, dynamic> json) =>
      _$UsuarioDtoFromJson(json);

  Map<String, dynamic> toJson() => _$UsuarioDtoToJson(this);
}
```

### `lib/models/auth/token_response_dto.dart`
```dart
import 'package:json_annotation/json_annotation.dart';
import 'usuario_dto.dart';

part 'token_response_dto.g.dart';

@JsonSerializable()
class TokenResponseDto {
  final String accessToken;
  final String refreshToken;
  final String tokenType;
  final int expiresIn;
  final DateTime expiresAt;
  final UsuarioDto? usuario;

  TokenResponseDto({
    required this.accessToken,
    required this.refreshToken,
    required this.tokenType,
    required this.expiresIn,
    required this.expiresAt,
    this.usuario,
  });

  factory TokenResponseDto.fromJson(Map<String, dynamic> json) =>
      _$TokenResponseDtoFromJson(json);

  Map<String, dynamic> toJson() => _$TokenResponseDtoToJson(this);
}
```

**Gerar código:**
```bash
flutter pub run build_runner build --delete-conflicting-outputs
```

---

## 🔧 2. Armazenamento Seguro de Tokens

### `lib/services/storage/token_storage_service.dart`
```dart
import 'package:flutter_secure_storage/flutter_secure_storage.dart';

class TokenStorageService {
  static const _storage = FlutterSecureStorage();

  static const _accessTokenKey = 'access_token';
  static const _refreshTokenKey = 'refresh_token';
  static const _expiresAtKey = 'expires_at';

  Future<void> saveTokens({
    required String accessToken,
    required String refreshToken,
    required DateTime expiresAt,
  }) async {
    await Future.wait([
      _storage.write(key: _accessTokenKey, value: accessToken),
      _storage.write(key: _refreshTokenKey, value: refreshToken),
      _storage.write(key: _expiresAtKey, value: expiresAt.toIso8601String()),
    ]);
  }

  Future<String?> getAccessToken() async {
    return await _storage.read(key: _accessTokenKey);
  }

  Future<String?> getRefreshToken() async {
    return await _storage.read(key: _refreshTokenKey);
  }

  Future<DateTime?> getExpiresAt() async {
    final expiresAtStr = await _storage.read(key: _expiresAtKey);
    if (expiresAtStr == null) return null;
    return DateTime.parse(expiresAtStr);
  }

  Future<bool> isTokenExpired() async {
    final expiresAt = await getExpiresAt();
    if (expiresAt == null) return true;
    return DateTime.now().isAfter(expiresAt);
  }

  Future<void> clearTokens() async {
    await Future.wait([
      _storage.delete(key: _accessTokenKey),
      _storage.delete(key: _refreshTokenKey),
      _storage.delete(key: _expiresAtKey),
    ]);
  }

  Future<bool> hasValidToken() async {
    final token = await getAccessToken();
    if (token == null) return false;
    return !(await isTokenExpired());
  }
}
```

---

## 🌐 3. Serviço de API com Dio

### `lib/services/api/api_client.dart`
```dart
import 'package:dio/dio.dart';
import '../storage/token_storage_service.dart';

class ApiClient {
  static const String baseUrl = 'https://168.231.95.240:7073/api';

  late final Dio _dio;
  final TokenStorageService _tokenStorage;

  ApiClient(this._tokenStorage) {
    _dio = Dio(BaseOptions(
      baseUrl: baseUrl,
      connectTimeout: const Duration(seconds: 30),
      receiveTimeout: const Duration(seconds: 30),
      headers: {
        'Content-Type': 'application/json',
        'Accept': 'application/json',
      },
    ));

    // Interceptor para adicionar token automaticamente
    _dio.interceptors.add(InterceptorsWrapper(
      onRequest: (options, handler) async {
        final token = await _tokenStorage.getAccessToken();
        if (token != null) {
          options.headers['Authorization'] = 'Bearer $token';
        }
        return handler.next(options);
      },
      onError: (error, handler) async {
        // Se erro 401 e temos refresh token, tenta renovar
        if (error.response?.statusCode == 401) {
          final refreshToken = await _tokenStorage.getRefreshToken();
          if (refreshToken != null) {
            try {
              // Renovar token
              final response = await _refreshToken(refreshToken);

              // Salvar novos tokens
              await _tokenStorage.saveTokens(
                accessToken: response['accessToken'],
                refreshToken: response['refreshToken'],
                expiresAt: DateTime.parse(response['expiresAt']),
              );

              // Retentar requisição original
              final opts = error.requestOptions;
              opts.headers['Authorization'] = 'Bearer ${response['accessToken']}';
              final cloneReq = await _dio.fetch(opts);
              return handler.resolve(cloneReq);
            } catch (e) {
              // Falha ao renovar, faz logout
              await _tokenStorage.clearTokens();
              return handler.reject(error);
            }
          }
        }
        return handler.next(error);
      },
    ));
  }

  Dio get dio => _dio;

  Future<Map<String, dynamic>> _refreshToken(String refreshToken) async {
    final response = await _dio.post('/Auth/refresh', data: {
      'accessToken': await _tokenStorage.getAccessToken(),
      'refreshToken': refreshToken,
    });
    return response.data;
  }
}
```

---

## 🔐 4. Serviço de Autenticação

### `lib/services/api/auth_service.dart`
```dart
import 'package:dio/dio.dart';
import '../../models/auth/login_dto.dart';
import '../../models/auth/register_usuario_dto.dart';
import '../../models/auth/token_response_dto.dart';
import '../../models/auth/usuario_dto.dart';
import 'api_client.dart';
import '../storage/token_storage_service.dart';

class AuthService {
  final ApiClient _apiClient;
  final TokenStorageService _tokenStorage;

  AuthService(this._apiClient, this._tokenStorage);

  /// Registra novo usuário
  Future<TokenResponseDto> register(RegisterUsuarioDto dto) async {
    try {
      final response = await _apiClient.dio.post(
        '/Auth/register',
        data: dto.toJson(),
      );

      final tokenResponse = TokenResponseDto.fromJson(response.data);

      await _tokenStorage.saveTokens(
        accessToken: tokenResponse.accessToken,
        refreshToken: tokenResponse.refreshToken,
        expiresAt: tokenResponse.expiresAt,
      );

      return tokenResponse;
    } on DioException catch (e) {
      throw _handleError(e);
    }
  }

  /// Realiza login
  Future<TokenResponseDto> login(LoginDto dto) async {
    try {
      final response = await _apiClient.dio.post(
        '/Auth/login',
        data: dto.toJson(),
      );

      final tokenResponse = TokenResponseDto.fromJson(response.data);

      await _tokenStorage.saveTokens(
        accessToken: tokenResponse.accessToken,
        refreshToken: tokenResponse.refreshToken,
        expiresAt: tokenResponse.expiresAt,
      );

      return tokenResponse;
    } on DioException catch (e) {
      throw _handleError(e);
    }
  }

  /// Obtém dados do usuário atual
  Future<UsuarioDto> me() async {
    try {
      final response = await _apiClient.dio.get('/Auth/me');
      return UsuarioDto.fromJson(response.data);
    } on DioException catch (e) {
      throw _handleError(e);
    }
  }

  /// Logout (revoga refresh token)
  Future<void> logout() async {
    try {
      final refreshToken = await _tokenStorage.getRefreshToken();
      if (refreshToken != null) {
        await _apiClient.dio.post('/Auth/revoke', data: {
          'refreshToken': refreshToken,
          'motivo': 'Logout do usuário',
        });
      }
    } catch (e) {
      // Ignora erros no logout
    } finally {
      await _tokenStorage.clearTokens();
    }
  }

  /// Altera senha
  Future<void> changePassword({
    required String senhaAtual,
    required String novaSenha,
  }) async {
    try {
      await _apiClient.dio.post('/Auth/change-password', data: {
        'senhaAtual': senhaAtual,
        'novaSenha': novaSenha,
        'confirmarNovaSenha': novaSenha,
      });
    } on DioException catch (e) {
      throw _handleError(e);
    }
  }

  /// Solicita recuperação de senha
  Future<String> forgotPassword(String email) async {
    try {
      final response = await _apiClient.dio.post(
        '/Auth/forgot-password',
        data: {'email': email},
      );
      return response.data['message'];
    } on DioException catch (e) {
      throw _handleError(e);
    }
  }

  /// Reseta senha com token
  Future<void> resetPassword({
    required String email,
    required String token,
    required String novaSenha,
  }) async {
    try {
      await _apiClient.dio.post('/Auth/reset-password', data: {
        'email': email,
        'token': token,
        'novaSenha': novaSenha,
        'confirmarNovaSenha': novaSenha,
      });
    } on DioException catch (e) {
      throw _handleError(e);
    }
  }

  /// Verifica se está autenticado
  Future<bool> isAuthenticated() async {
    return await _tokenStorage.hasValidToken();
  }

  String _handleError(DioException e) {
    if (e.response?.data != null && e.response?.data['message'] != null) {
      return e.response!.data['message'];
    }

    switch (e.type) {
      case DioExceptionType.connectionTimeout:
      case DioExceptionType.receiveTimeout:
        return 'Tempo limite excedido. Verifique sua conexão.';
      case DioExceptionType.badResponse:
        return 'Erro no servidor: ${e.response?.statusCode}';
      default:
        return 'Erro de conexão. Tente novamente.';
    }
  }
}
```

---

## 🏗️ 5. Configuração de Injeção de Dependência

### `lib/core/di/service_locator.dart`
```dart
import 'package:get_it/get_it.dart';
import '../../services/storage/token_storage_service.dart';
import '../../services/api/api_client.dart';
import '../../services/api/auth_service.dart';

final getIt = GetIt.instance;

void setupServiceLocator() {
  // Storage
  getIt.registerLazySingleton(() => TokenStorageService());

  // API Client
  getIt.registerLazySingleton(() => ApiClient(getIt<TokenStorageService>()));

  // Services
  getIt.registerLazySingleton(() => AuthService(
        getIt<ApiClient>(),
        getIt<TokenStorageService>(),
      ));
}
```

### `lib/main.dart`
```dart
import 'package:flutter/material.dart';
import 'core/di/service_locator.dart';

void main() {
  setupServiceLocator();
  runApp(const MyApp());
}

class MyApp extends StatelessWidget {
  const MyApp({super.key});

  @override
  Widget build(BuildContext context) {
    return MaterialApp(
      title: 'ATS Kung Fu',
      theme: ThemeData(
        primarySwatch: Colors.blue,
        useMaterial3: true,
      ),
      home: const LoginPage(),
    );
  }
}
```

---

## 📲 6. Exemplos de Uso nas Telas

### `lib/pages/login_page.dart`
```dart
import 'package:flutter/material.dart';
import '../core/di/service_locator.dart';
import '../services/api/auth_service.dart';
import '../models/auth/login_dto.dart';

class LoginPage extends StatefulWidget {
  const LoginPage({super.key});

  @override
  State<LoginPage> createState() => _LoginPageState();
}

class _LoginPageState extends State<LoginPage> {
  final _formKey = GlobalKey<FormState>();
  final _emailController = TextEditingController();
  final _senhaController = TextEditingController();
  bool _lembrarMe = false;
  bool _isLoading = false;

  final _authService = getIt<AuthService>();

  Future<void> _login() async {
    if (!_formKey.currentState!.validate()) return;

    setState(() => _isLoading = true);

    try {
      final loginDto = LoginDto(
        email: _emailController.text,
        senha: _senhaController.text,
        lembrarMe: _lembrarMe,
      );

      final response = await _authService.login(loginDto);

      if (mounted) {
        // Navegar para home
        Navigator.pushReplacementNamed(context, '/home');

        ScaffoldMessenger.of(context).showSnackBar(
          SnackBar(
            content: Text('Bem-vindo, ${response.usuario?.nomeCompleto}!'),
            backgroundColor: Colors.green,
          ),
        );
      }
    } catch (e) {
      if (mounted) {
        ScaffoldMessenger.of(context).showSnackBar(
          SnackBar(
            content: Text(e.toString()),
            backgroundColor: Colors.red,
          ),
        );
      }
    } finally {
      if (mounted) {
        setState(() => _isLoading = false);
      }
    }
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(title: const Text('Login')),
      body: Padding(
        padding: const EdgeInsets.all(16.0),
        child: Form(
          key: _formKey,
          child: Column(
            mainAxisAlignment: MainAxisAlignment.center,
            children: [
              TextFormField(
                controller: _emailController,
                decoration: const InputDecoration(
                  labelText: 'Email',
                  border: OutlineInputBorder(),
                ),
                keyboardType: TextInputType.emailAddress,
                validator: (value) {
                  if (value == null || value.isEmpty) {
                    return 'Email é obrigatório';
                  }
                  return null;
                },
              ),
              const SizedBox(height: 16),
              TextFormField(
                controller: _senhaController,
                decoration: const InputDecoration(
                  labelText: 'Senha',
                  border: OutlineInputBorder(),
                ),
                obscureText: true,
                validator: (value) {
                  if (value == null || value.isEmpty) {
                    return 'Senha é obrigatória';
                  }
                  return null;
                },
              ),
              const SizedBox(height: 8),
              CheckboxListTile(
                title: const Text('Lembrar-me'),
                value: _lembrarMe,
                onChanged: (value) {
                  setState(() => _lembrarMe = value ?? false);
                },
              ),
              const SizedBox(height: 16),
              SizedBox(
                width: double.infinity,
                height: 48,
                child: ElevatedButton(
                  onPressed: _isLoading ? null : _login,
                  child: _isLoading
                      ? const CircularProgressIndicator()
                      : const Text('Entrar'),
                ),
              ),
              const SizedBox(height: 16),
              TextButton(
                onPressed: () {
                  Navigator.pushNamed(context, '/register');
                },
                child: const Text('Não tem conta? Registre-se'),
              ),
            ],
          ),
        ),
      ),
    );
  }

  @override
  void dispose() {
    _emailController.dispose();
    _senhaController.dispose();
    super.dispose();
  }
}
```

---

## 🔄 7. Refresh Token Automático

O `ApiClient` já implementa refresh automático! Quando uma requisição retorna 401:
1. Tenta renovar o token automaticamente
2. Se sucesso, retenta a requisição original
3. Se falha, limpa os tokens e redireciona para login

---

## 📝 8. Checklist de Integração

- [ ] Adicionar dependências ao `pubspec.yaml`
- [ ] Criar todos os modelos (DTOs)
- [ ] Gerar código com `build_runner`
- [ ] Implementar `TokenStorageService`
- [ ] Implementar `ApiClient` com interceptors
- [ ] Implementar `AuthService`
- [ ] Configurar injeção de dependência
- [ ] Criar tela de login
- [ ] Criar tela de registro
- [ ] Testar login com usuário admin
- [ ] Testar renovação automática de token
- [ ] Testar logout

---

## 🎯 Endpoints Disponíveis

| Endpoint | Método | Autenticação | Descrição |
|----------|--------|--------------|-----------|
| `/api/Auth/register` | POST | Não | Registrar novo usuário |
| `/api/Auth/login` | POST | Não | Login |
| `/api/Auth/refresh` | POST | Não | Renovar token |
| `/api/Auth/revoke` | POST | Não | Revogar token (logout) |
| `/api/Auth/forgot-password` | POST | Não | Solicitar recuperação de senha |
| `/api/Auth/reset-password` | POST | Não | Resetar senha com token |
| `/api/Auth/change-password` | POST | **Sim** | Alterar senha |
| `/api/Auth/me` | GET | **Sim** | Obter dados do usuário |

---

## ⚠️ Importantes

### HTTPS em Desenvolvimento
Para testar com API HTTPS em desenvolvimento:

#### Android (`android/app/src/main/AndroidManifest.xml`):
```xml
<application
    android:usesCleartextTraffic="true">
```

#### iOS (`ios/Runner/Info.plist`):
```xml
<key>NSAppTransportSecurity</key>
<dict>
    <key>NSAllowsArbitraryLoads</key>
    <true/>
</dict>
```

### Credenciais de Teste
- **Email**: `flavio.nogueira.alfa@outlook.com.br`
- **Senha**: `@Fn.2025@`

---

✅ **Pronto! Sua aplicação Flutter está integrada com a API!**
