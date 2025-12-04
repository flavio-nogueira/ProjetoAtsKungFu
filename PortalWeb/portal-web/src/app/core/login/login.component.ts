import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { AuthService } from '../auth.service';

@Component({
  selector: 'app-login',
  imports: [CommonModule, FormsModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss'
})
export class LoginComponent {
  username: string = 'flavio.nogueira.alfa@outlook.com.br';
  password: string = '@Fn.2025@';
  errorMessage: string = '';

  constructor(
    private authService: AuthService,
    private router: Router
  ) {}

  onSubmit(): void {
    if (this.authService.login(this.username, this.password)) {
      this.router.navigate(['/app']);
    } else {
      this.errorMessage = 'Usuário ou senha inválidos';
    }
  }

  forgotPassword(event: Event): void {
    event.preventDefault();
    alert('Funcionalidade de recuperação de senha será implementada em breve.');
  }
}
