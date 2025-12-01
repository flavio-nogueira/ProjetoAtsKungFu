import 'package:atskungfu_appmobile/domain/entities/user_entity.dart';

abstract class IAuthRepository {
  Future<UserEntity?> login(String email, String password);
  Future<bool> register({
    required String name,
    required String email,
    required String password,
    required DateTime birthDate,
  });
  Future<bool> forgotPassword(String email);
  Future<void> logout();
  Future<UserEntity?> getCurrentUser();
}
