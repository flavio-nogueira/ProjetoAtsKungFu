import 'package:atskungfu_appmobile/domain/repositories/i_auth_repository.dart';

class RegisterUseCase {
  final IAuthRepository _repository;

  RegisterUseCase(this._repository);

  Future<bool> call({
    required String name,
    required String email,
    required String password,
    required DateTime birthDate,
  }) async {
    return await _repository.register(
      name: name,
      email: email,
      password: password,
      birthDate: birthDate,
    );
  }
}
