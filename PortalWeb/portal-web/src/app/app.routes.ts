import { Routes } from '@angular/router';
import { LoginComponent } from './core/login/login.component';
import { LayoutComponent } from './core/layout/layout.component';
import { authGuard } from './core/auth.guard';
import { ProfessorComponent } from './cadastro/professor/professor.component';
import { AlunoComponent } from './cadastro/aluno/aluno.component';
import { AulasComponent } from './cadastro/aulas/aulas.component';

export const routes: Routes = [
  { path: 'login', component: LoginComponent },
  {
    path: 'app',
    component: LayoutComponent,
    canActivate: [authGuard],
    children: [
      { path: '', redirectTo: 'cadastro/professor', pathMatch: 'full' },
      { path: 'cadastro/professor', component: ProfessorComponent },
      { path: 'cadastro/aluno', component: AlunoComponent },
      { path: 'cadastro/aulas', component: AulasComponent }
    ]
  },
  { path: '', redirectTo: '/login', pathMatch: 'full' },
  { path: '**', redirectTo: '/login' }
];
