import 'package:get/get.dart';
import 'package:atskungfu_appmobile/core/constants/app_routes.dart';
import 'package:atskungfu_appmobile/presentation/pages/auth/forgot_password/forgot_password_page.dart';
import 'package:atskungfu_appmobile/presentation/pages/auth/login/login_page.dart';
import 'package:atskungfu_appmobile/presentation/pages/auth/register/register_page.dart';
import 'package:atskungfu_appmobile/presentation/pages/main/main_page.dart';
import 'package:atskungfu_appmobile/presentation/pages/splash/splash_page.dart';

class AppPages {
  static final routes = [
    GetPage(
      name: AppRoutes.splash,
      page: () => const SplashPage(),
    ),
    GetPage(
      name: AppRoutes.login,
      page: () => const LoginPage(),
    ),
    GetPage(
      name: AppRoutes.register,
      page: () => const RegisterPage(),
    ),
    GetPage(
      name: AppRoutes.forgotPassword,
      page: () => const ForgotPasswordPage(),
    ),
    GetPage(
      name: AppRoutes.main,
      page: () => const MainPage(),
    ),
  ];
}
