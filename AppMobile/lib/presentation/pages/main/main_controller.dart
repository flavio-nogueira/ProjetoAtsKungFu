import 'package:flutter/material.dart';
import 'package:get/get.dart';
import 'package:atskungfu_appmobile/core/constants/app_routes.dart';
import 'package:atskungfu_appmobile/domain/usecases/logout_usecase.dart';

class MainController extends GetxController {
  final LogoutUseCase _logoutUseCase;

  MainController(this._logoutUseCase);

  final _currentIndex = 0.obs;

  int get currentIndex => _currentIndex.value;

  void changeTab(int index) {
    _currentIndex.value = index;
  }

  Future<void> logout() async {
    final confirm = await Get.dialog<bool>(
      AlertDialog(
        title: const Text('Sair'),
        content: const Text('Deseja realmente sair?'),
        actions: [
          TextButton(
            onPressed: () => Get.back(result: false),
            child: const Text('Cancelar'),
          ),
          TextButton(
            onPressed: () => Get.back(result: true),
            child: const Text('Sair'),
          ),
        ],
      ),
    );

    if (confirm == true) {
      await _logoutUseCase.call();
      Get.offAllNamed(AppRoutes.login);
    }
  }
}
