import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

interface Aluno {
  id: number;
  nome: string;
  email: string;
  telefone: string;
  dataNascimento: string;
  matricula: string;
  turma: string;
}

@Component({
  selector: 'app-aluno',
  imports: [CommonModule, FormsModule],
  templateUrl: './aluno.component.html',
  styleUrl: './aluno.component.scss'
})
export class AlunoComponent {
  alunos: Aluno[] = [
    { id: 1, nome: 'Ana Silva', email: 'ana@example.com', telefone: '(11) 98765-1111', dataNascimento: '2005-03-15', matricula: 'MAT001', turma: '9º Ano A' },
    { id: 2, nome: 'Carlos Souza', email: 'carlos@example.com', telefone: '(11) 98765-2222', dataNascimento: '2006-07-20', matricula: 'MAT002', turma: '8º Ano B' },
    { id: 3, nome: 'Beatriz Lima', email: 'beatriz@example.com', telefone: '(11) 98765-3333', dataNascimento: '2005-11-10', matricula: 'MAT003', turma: '9º Ano A' },
    { id: 4, nome: 'Daniel Oliveira', email: 'daniel@example.com', telefone: '(11) 98765-4444', dataNascimento: '2007-02-28', matricula: 'MAT004', turma: '7º Ano C' }
  ];

  aluno: Aluno = this.getEmptyAluno();
  showModal = false;
  isEditing = false;
  searchTerm = '';

  getEmptyAluno(): Aluno {
    return {
      id: 0,
      nome: '',
      email: '',
      telefone: '',
      dataNascimento: '',
      matricula: '',
      turma: ''
    };
  }

  get filteredAlunos(): Aluno[] {
    if (!this.searchTerm) {
      return this.alunos;
    }
    return this.alunos.filter(a =>
      a.nome.toLowerCase().includes(this.searchTerm.toLowerCase()) ||
      a.email.toLowerCase().includes(this.searchTerm.toLowerCase()) ||
      a.matricula.toLowerCase().includes(this.searchTerm.toLowerCase()) ||
      a.turma.toLowerCase().includes(this.searchTerm.toLowerCase())
    );
  }

  openAddModal(): void {
    this.aluno = this.getEmptyAluno();
    this.isEditing = false;
    this.showModal = true;
  }

  openEditModal(aluno: Aluno): void {
    this.aluno = { ...aluno };
    this.isEditing = true;
    this.showModal = true;
  }

  closeModal(): void {
    this.showModal = false;
    this.aluno = this.getEmptyAluno();
  }

  saveAluno(): void {
    if (this.isEditing) {
      const index = this.alunos.findIndex(a => a.id === this.aluno.id);
      if (index !== -1) {
        this.alunos[index] = { ...this.aluno };
      }
    } else {
      this.aluno.id = Math.max(...this.alunos.map(a => a.id), 0) + 1;
      this.alunos.push({ ...this.aluno });
    }
    this.closeModal();
  }

  deleteAluno(id: number): void {
    if (confirm('Tem certeza que deseja excluir este aluno?')) {
      this.alunos = this.alunos.filter(a => a.id !== id);
    }
  }

  calculateAge(birthDate: string): number {
    const today = new Date();
    const birth = new Date(birthDate);
    let age = today.getFullYear() - birth.getFullYear();
    const monthDiff = today.getMonth() - birth.getMonth();
    if (monthDiff < 0 || (monthDiff === 0 && today.getDate() < birth.getDate())) {
      age--;
    }
    return age;
  }
}
