import 'package:get/get.dart';
import 'package:shared_preferences/shared_preferences.dart';
import 'package:atskungfu_appmobile/data/datasources/i_local_storage.dart';
import 'package:atskungfu_appmobile/data/datasources/local_storage_impl.dart';
import 'package:atskungfu_appmobile/data/repositories/auth_repository_impl.dart';
import 'package:atskungfu_appmobile/domain/repositories/i_auth_repository.dart';
import 'package:atskungfu_appmobile/domain/usecases/forgot_password_usecase.dart';
import 'package:atskungfu_appmobile/domain/usecases/get_current_user_usecase.dart';
import 'package:atskungfu_appmobile/domain/usecases/login_usecase.dart';
import 'package:atskungfu_appmobile/domain/usecases/logout_usecase.dart';
import 'package:atskungfu_appmobile/domain/usecases/register_usecase.dart';
import 'package:atskungfu_appmobile/presentation/pages/auth/forgot_password/forgot_password_controller.dart';
import 'package:atskungfu_appmobile/presentation/pages/auth/login/login_controller.dart';
import 'package:atskungfu_appmobile/presentation/pages/auth/register/register_controller.dart';
import 'package:atskungfu_appmobile/presentation/pages/home/home_controller.dart';
import 'package:atskungfu_appmobile/presentation/pages/main/main_controller.dart';

class DependencyInjection {
  static Future<void> init() async {
    final sharedPreferences = await SharedPreferences.getInstance();

    Get.lazyPut<ILocalStorage>(
      () => LocalStorageImpl(sharedPreferences),
      fenix: true,
    );

    Get.lazyPut<IAuthRepository>(
      () => AuthRepositoryImpl(Get.find<ILocalStorage>()),
      fenix: true,
    );

    Get.lazyPut(() => LoginUseCase(Get.find<IAuthRepository>()), fenix: true);
    Get.lazyPut(() => RegisterUseCase(Get.find<IAuthRepository>()),
        fenix: true);
    Get.lazyPut(() => ForgotPasswordUseCase(Get.find<IAuthRepository>()),
        fenix: true);
    Get.lazyPut(() => GetCurrentUserUseCase(Get.find<IAuthRepository>()),
        fenix: true);
    Get.lazyPut(() => LogoutUseCase(Get.find<IAuthRepository>()), fenix: true);

    Get.lazyPut(
      () => LoginController(Get.find<LoginUseCase>()),
      fenix: true,
    );

    Get.lazyPut(
      () => RegisterController(Get.find<RegisterUseCase>()),
      fenix: true,
    );

    Get.lazyPut(
      () => ForgotPasswordController(Get.find<ForgotPasswordUseCase>()),
      fenix: true,
    );

    Get.lazyPut(
      () => HomeController(Get.find<GetCurrentUserUseCase>()),
      fenix: true,
    );

    Get.lazyPut(
      () => MainController(Get.find<LogoutUseCase>()),
      fenix: true,
    );
  }
}
