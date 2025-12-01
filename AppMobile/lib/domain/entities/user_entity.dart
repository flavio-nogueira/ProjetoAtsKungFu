class UserEntity {
  final String id;
  final String name;
  final String email;
  final DateTime birthDate;

  UserEntity({
    required this.id,
    required this.name,
    required this.email,
    required this.birthDate,
  });

  int get age {
    final now = DateTime.now();
    int age = now.year - birthDate.year;
    if (now.month < birthDate.month ||
        (now.month == birthDate.month && now.day < birthDate.day)) {
      age--;
    }
    return age;
  }
}
