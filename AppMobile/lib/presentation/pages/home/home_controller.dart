import 'package:get/get.dart';
import 'package:atskungfu_appmobile/domain/entities/user_entity.dart';
import 'package:atskungfu_appmobile/domain/usecases/get_current_user_usecase.dart';

class HomeController extends GetxController {
  final GetCurrentUserUseCase _getCurrentUserUseCase;

  HomeController(this._getCurrentUserUseCase);

  final _user = Rx<UserEntity?>(null);
  final _isLoading = true.obs;

  UserEntity? get user => _user.value;
  bool get isLoading => _isLoading.value;

  @override
  void onInit() {
    super.onInit();
    _loadUser();
  }

  Future<void> _loadUser() async {
    _isLoading.value = true;
    try {
      final currentUser = await _getCurrentUserUseCase.call();
      _user.value = currentUser;
    } catch (e) {
      _user.value = null;
    } finally {
      _isLoading.value = false;
    }
  }
}
