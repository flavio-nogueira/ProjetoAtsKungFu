import 'dart:convert';
import 'package:atskungfu_appmobile/core/constants/storage_keys.dart';
import 'package:atskungfu_appmobile/data/datasources/i_local_storage.dart';
import 'package:atskungfu_appmobile/data/models/user_model.dart';
import 'package:atskungfu_appmobile/domain/entities/user_entity.dart';
import 'package:atskungfu_appmobile/domain/repositories/i_auth_repository.dart';

class AuthRepositoryImpl implements IAuthRepository {
  final ILocalStorage _localStorage;

  AuthRepositoryImpl(this._localStorage);

  @override
  Future<UserEntity?> login(String email, String password) async {
    final usersJson = await _localStorage.getString(StorageKeys.users);

    if (usersJson == null) return null;

    final List<dynamic> usersList = jsonDecode(usersJson);

    final userMap = usersList.firstWhere(
      (user) => user['email'] == email && user['password'] == password,
      orElse: () => null,
    );

    if (userMap == null) return null;

    final user = UserModel.fromJson(userMap);
    await _localStorage.saveString(
      StorageKeys.currentUser,
      jsonEncode(user.toJson()),
    );

    return user;
  }

  @override
  Future<bool> register({
    required String name,
    required String email,
    required String password,
    required DateTime birthDate,
  }) async {
    final usersJson = await _localStorage.getString(StorageKeys.users);
    List<dynamic> usersList = usersJson != null ? jsonDecode(usersJson) : [];

    final userExists = usersList.any((user) => user['email'] == email);
    if (userExists) return false;

    final newUser = {
      'id': DateTime.now().millisecondsSinceEpoch.toString(),
      'name': name,
      'email': email,
      'password': password,
      'birthDate': birthDate.toIso8601String(),
    };

    usersList.add(newUser);
    await _localStorage.saveString(StorageKeys.users, jsonEncode(usersList));

    return true;
  }

  @override
  Future<bool> forgotPassword(String email) async {
    final usersJson = await _localStorage.getString(StorageKeys.users);

    if (usersJson == null) return false;

    final List<dynamic> usersList = jsonDecode(usersJson);
    final userExists = usersList.any((user) => user['email'] == email);

    return userExists;
  }

  @override
  Future<void> logout() async {
    await _localStorage.remove(StorageKeys.currentUser);
  }

  @override
  Future<UserEntity?> getCurrentUser() async {
    final userJson = await _localStorage.getString(StorageKeys.currentUser);

    if (userJson == null) return null;

    return UserModel.fromJson(jsonDecode(userJson));
  }
}
