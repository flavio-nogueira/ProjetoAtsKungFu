import 'package:flutter/material.dart';
import 'package:get/get.dart';
import 'package:atskungfu_appmobile/domain/usecases/forgot_password_usecase.dart';

class ForgotPasswordController extends GetxController {
  final ForgotPasswordUseCase _forgotPasswordUseCase;

  ForgotPasswordController(this._forgotPasswordUseCase);

  final formKey = GlobalKey<FormState>();
  final emailController = TextEditingController();

  final _isLoading = false.obs;

  bool get isLoading => _isLoading.value;

  Future<void> sendResetLink() async {
    if (!formKey.currentState!.validate()) return;

    _isLoading.value = true;

    try {
      final success = await _forgotPasswordUseCase.call(
        emailController.text.trim(),
      );

      if (success) {
        Get.back();
        Get.snackbar(
          'Sucesso',
          'Link de recuperação enviado para seu email',
          snackPosition: SnackPosition.BOTTOM,
          backgroundColor: Colors.green,
          colorText: Colors.white,
        );
      } else {
        Get.snackbar(
          'Erro',
          'Email não encontrado',
          snackPosition: SnackPosition.BOTTOM,
          backgroundColor: Colors.red,
          colorText: Colors.white,
        );
      }
    } catch (e) {
      Get.snackbar(
        'Erro',
        'Erro ao enviar link: $e',
        snackPosition: SnackPosition.BOTTOM,
        backgroundColor: Colors.red,
        colorText: Colors.white,
      );
    } finally {
      _isLoading.value = false;
    }
  }

  @override
  void onClose() {
    emailController.dispose();
    super.onClose();
  }
}
