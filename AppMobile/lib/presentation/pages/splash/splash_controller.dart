import 'package:get/get.dart';
import 'package:atskungfu_appmobile/core/constants/app_routes.dart';
import 'package:atskungfu_appmobile/domain/usecases/get_current_user_usecase.dart';

class SplashController extends GetxController {
  final GetCurrentUserUseCase? _getCurrentUserUseCase;

  SplashController([this._getCurrentUserUseCase]);

  @override
  void onInit() {
    super.onInit();
    _navigateToNextPage();
  }

  Future<void> _navigateToNextPage() async {
    await Future.delayed(const Duration(seconds: 2));

    if (_getCurrentUserUseCase != null) {
      final user = await _getCurrentUserUseCase.call();

      if (user != null) {
        Get.offAllNamed(AppRoutes.main);
        return;
      }
    }

    Get.offAllNamed(AppRoutes.login);
  }
}
