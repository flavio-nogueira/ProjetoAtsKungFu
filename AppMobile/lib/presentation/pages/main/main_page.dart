import 'package:flutter/material.dart';
import 'package:get/get.dart';
import 'package:atskungfu_appmobile/presentation/pages/about/about_page.dart';
import 'package:atskungfu_appmobile/presentation/pages/home/home_page.dart';
import 'package:atskungfu_appmobile/presentation/pages/main/main_controller.dart';

class MainPage extends GetView<MainController> {
  const MainPage({super.key});

  @override
  Widget build(BuildContext context) {
    final List<Widget> pages = [
      const HomePage(),
      const AboutPage(),
    ];

    return Obx(() => Scaffold(
          appBar: AppBar(
            title: Text(
              controller.currentIndex == 0 ? 'Home' : 'Quem Fez',
            ),
            actions: [
              IconButton(
                icon: const Icon(Icons.logout),
                onPressed: controller.logout,
                tooltip: 'Sair',
              ),
            ],
          ),
          body: IndexedStack(
            index: controller.currentIndex,
            children: pages,
          ),
          bottomNavigationBar: BottomNavigationBar(
            currentIndex: controller.currentIndex,
            onTap: controller.changeTab,
            items: const [
              BottomNavigationBarItem(
                icon: Icon(Icons.home),
                label: 'Home',
              ),
              BottomNavigationBarItem(
                icon: Icon(Icons.info),
                label: 'Quem Fez',
              ),
            ],
          ),
        ));
  }
}
