import 'package:atskungfu_appmobile/domain/entities/user_entity.dart';
import 'package:atskungfu_appmobile/domain/repositories/i_auth_repository.dart';

class LoginUseCase {
  final IAuthRepository _repository;

  LoginUseCase(this._repository);

  Future<UserEntity?> call(String email, String password) async {
    return await _repository.login(email, password);
  }
}
