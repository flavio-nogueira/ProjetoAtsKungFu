import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { AuthService } from '../auth.service';

interface MenuItem {
  label: string;
  icon: string;
  route?: string;
  children?: MenuItem[];
  expanded?: boolean;
}

@Component({
  selector: 'app-layout',
  imports: [CommonModule, RouterModule],
  templateUrl: './layout.component.html',
  styleUrl: './layout.component.scss'
})
export class LayoutComponent {
  sidebarCollapsed = false;

  menuItems: MenuItem[] = [
    {
      label: 'Cadastro',
      icon: 'fas fa-folder',
      expanded: true,
      children: [
        { label: 'Professor', icon: 'fas fa-chalkboard-teacher', route: '/app/cadastro/professor' },
        { label: 'Aluno', icon: 'fas fa-user-graduate', route: '/app/cadastro/aluno' },
        { label: 'Aulas', icon: 'fas fa-book', route: '/app/cadastro/aulas' }
      ]
    }
  ];

  constructor(public authService: AuthService) {}

  toggleSidebar(): void {
    this.sidebarCollapsed = !this.sidebarCollapsed;
  }

  toggleMenuItem(item: MenuItem): void {
    if (item.children) {
      item.expanded = !item.expanded;
    }
  }

  logout(): void {
    this.authService.logout();
  }
}
