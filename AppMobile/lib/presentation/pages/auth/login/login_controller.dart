import 'package:flutter/material.dart';
import 'package:get/get.dart';
import 'package:atskungfu_appmobile/core/constants/app_routes.dart';
import 'package:atskungfu_appmobile/domain/usecases/login_usecase.dart';

class LoginController extends GetxController {
  final LoginUseCase _loginUseCase;

  LoginController(this._loginUseCase);

  final formKey = GlobalKey<FormState>();
  final emailController = TextEditingController();
  final passwordController = TextEditingController();

  final _isLoading = false.obs;
  final _obscurePassword = true.obs;

  bool get isLoading => _isLoading.value;
  bool get obscurePassword => _obscurePassword.value;

  void togglePasswordVisibility() {
    _obscurePassword.value = !_obscurePassword.value;
  }

  Future<void> login() async {
    _isLoading.value = true;

    try {
      final email = emailController.text.trim();
      final password = passwordController.text;

      if (email.isEmpty && password.isEmpty) {
        Get.offAllNamed(AppRoutes.main);
        return;
      }

      if (!formKey.currentState!.validate()) {
        _isLoading.value = false;
        return;
      }

      final user = await _loginUseCase.call(email, password);

      if (user != null) {
        Get.offAllNamed(AppRoutes.main);
      } else {
        Get.snackbar(
          'Erro',
          'Email ou senha inválidos',
          snackPosition: SnackPosition.BOTTOM,
          backgroundColor: Colors.red,
          colorText: Colors.white,
        );
      }
    } catch (e) {
      Get.snackbar(
        'Erro',
        'Erro ao fazer login: $e',
        snackPosition: SnackPosition.BOTTOM,
        backgroundColor: Colors.red,
        colorText: Colors.white,
      );
    } finally {
      _isLoading.value = false;
    }
  }

  void navigateToRegister() {
    Get.toNamed(AppRoutes.register);
  }

  void navigateToForgotPassword() {
    Get.toNamed(AppRoutes.forgotPassword);
  }

  @override
  void onClose() {
    emailController.dispose();
    passwordController.dispose();
    super.onClose();
  }
}
