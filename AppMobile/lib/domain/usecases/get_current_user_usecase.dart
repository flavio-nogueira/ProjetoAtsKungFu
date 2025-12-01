import 'package:atskungfu_appmobile/domain/entities/user_entity.dart';
import 'package:atskungfu_appmobile/domain/repositories/i_auth_repository.dart';

class GetCurrentUserUseCase {
  final IAuthRepository _repository;

  GetCurrentUserUseCase(this._repository);

  Future<UserEntity?> call() async {
    return await _repository.getCurrentUser();
  }
}
