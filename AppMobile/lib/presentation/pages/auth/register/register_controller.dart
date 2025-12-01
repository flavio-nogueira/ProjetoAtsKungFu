import 'package:flutter/material.dart';
import 'package:get/get.dart';
import 'package:atskungfu_appmobile/domain/usecases/register_usecase.dart';

class RegisterController extends GetxController {
  final RegisterUseCase _registerUseCase;

  RegisterController(this._registerUseCase);

  final formKey = GlobalKey<FormState>();
  final nameController = TextEditingController();
  final emailController = TextEditingController();
  final passwordController = TextEditingController();
  final confirmPasswordController = TextEditingController();

  final _isLoading = false.obs;
  final _obscurePassword = true.obs;
  final _obscureConfirmPassword = true.obs;
  final _selectedDate = Rx<DateTime?>(null);

  bool get isLoading => _isLoading.value;
  bool get obscurePassword => _obscurePassword.value;
  bool get obscureConfirmPassword => _obscureConfirmPassword.value;
  DateTime? get selectedDate => _selectedDate.value;

  void togglePasswordVisibility() {
    _obscurePassword.value = !_obscurePassword.value;
  }

  void toggleConfirmPasswordVisibility() {
    _obscureConfirmPassword.value = !_obscureConfirmPassword.value;
  }

  void setSelectedDate(DateTime date) {
    _selectedDate.value = date;
  }

  Future<void> selectDate(BuildContext context) async {
    final DateTime? picked = await showDatePicker(
      context: context,
      initialDate: DateTime(2000),
      firstDate: DateTime(1900),
      lastDate: DateTime.now(),
    );

    if (picked != null) {
      setSelectedDate(picked);
    }
  }

  String? validateConfirmPassword(String? value) {
    if (value == null || value.isEmpty) {
      return 'Confirme sua senha';
    }

    if (value != passwordController.text) {
      return 'As senhas não coincidem';
    }

    return null;
  }

  Future<void> register() async {
    if (!formKey.currentState!.validate()) return;

    if (selectedDate == null) {
      Get.snackbar(
        'Erro',
        'Selecione sua data de nascimento',
        snackPosition: SnackPosition.BOTTOM,
        backgroundColor: Colors.orange,
        colorText: Colors.white,
      );
      return;
    }

    _isLoading.value = true;

    try {
      final success = await _registerUseCase.call(
        name: nameController.text.trim(),
        email: emailController.text.trim(),
        password: passwordController.text,
        birthDate: selectedDate!,
      );

      if (success) {
        Get.back();
        Get.snackbar(
          'Sucesso',
          'Cadastro realizado com sucesso!',
          snackPosition: SnackPosition.BOTTOM,
          backgroundColor: Colors.green,
          colorText: Colors.white,
        );
      } else {
        Get.snackbar(
          'Erro',
          'Este email já está cadastrado',
          snackPosition: SnackPosition.BOTTOM,
          backgroundColor: Colors.red,
          colorText: Colors.white,
        );
      }
    } catch (e) {
      Get.snackbar(
        'Erro',
        'Erro ao realizar cadastro: $e',
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
    nameController.dispose();
    emailController.dispose();
    passwordController.dispose();
    confirmPasswordController.dispose();
    super.onClose();
  }
}
