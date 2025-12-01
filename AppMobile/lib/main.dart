import 'package:flutter/material.dart';
import 'package:get/get.dart';
import 'package:atskungfu_appmobile/core/constants/app_routes.dart';
import 'package:atskungfu_appmobile/core/di/dependency_injection.dart';
import 'package:atskungfu_appmobile/core/routes/app_pages.dart';
import 'package:atskungfu_appmobile/core/theme/app_theme.dart';

void main() async {
  WidgetsFlutterBinding.ensureInitialized();
  await DependencyInjection.init();
  runApp(const MyApp());
}

class MyApp extends StatelessWidget {
  const MyApp({super.key});

  @override
  Widget build(BuildContext context) {
    return GetMaterialApp(
      title: 'AtsKungFu',
      debugShowCheckedModeBanner: false,
      theme: AppTheme.lightTheme,
      initialRoute: AppRoutes.splash,
      getPages: AppPages.routes,
    );
  }
}
