import 'package:atskungfu_appmobile/domain/repositories/i_auth_repository.dart';

class ForgotPasswordUseCase {
  final IAuthRepository _repository;

  ForgotPasswordUseCase(this._repository);

  Future<bool> call(String email) async {
    return await _repository.forgotPassword(email);
  }
}
