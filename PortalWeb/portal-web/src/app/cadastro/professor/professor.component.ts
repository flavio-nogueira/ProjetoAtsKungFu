import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

interface Professor {
  id: number;
  nome: string;
  email: string;
  telefone: string;
  especialidade: string;
  dataAdmissao: string;
}

@Component({
  selector: 'app-professor',
  imports: [CommonModule, FormsModule],
  templateUrl: './professor.component.html',
  styleUrl: './professor.component.scss'
})
export class ProfessorComponent {
  professores: Professor[] = [
    { id: 1, nome: 'João Silva', email: 'joao@example.com', telefone: '(11) 98765-4321', especialidade: 'Matemática', dataAdmissao: '2020-01-15' },
    { id: 2, nome: 'Maria Santos', email: 'maria@example.com', telefone: '(11) 91234-5678', especialidade: 'Português', dataAdmissao: '2019-03-20' },
    { id: 3, nome: 'Pedro Costa', email: 'pedro@example.com', telefone: '(11) 99876-5432', especialidade: 'Física', dataAdmissao: '2021-06-10' }
  ];

  professor: Professor = this.getEmptyProfessor();
  showModal = false;
  isEditing = false;
  searchTerm = '';

  getEmptyProfessor(): Professor {
    return {
      id: 0,
      nome: '',
      email: '',
      telefone: '',
      especialidade: '',
      dataAdmissao: ''
    };
  }

  get filteredProfessores(): Professor[] {
    if (!this.searchTerm) {
      return this.professores;
    }
    return this.professores.filter(p =>
      p.nome.toLowerCase().includes(this.searchTerm.toLowerCase()) ||
      p.email.toLowerCase().includes(this.searchTerm.toLowerCase()) ||
      p.especialidade.toLowerCase().includes(this.searchTerm.toLowerCase())
    );
  }

  openAddModal(): void {
    this.professor = this.getEmptyProfessor();
    this.isEditing = false;
    this.showModal = true;
  }

  openEditModal(professor: Professor): void {
    this.professor = { ...professor };
    this.isEditing = true;
    this.showModal = true;
  }

  closeModal(): void {
    this.showModal = false;
    this.professor = this.getEmptyProfessor();
  }

  saveProfessor(): void {
    if (this.isEditing) {
      const index = this.professores.findIndex(p => p.id === this.professor.id);
      if (index !== -1) {
        this.professores[index] = { ...this.professor };
      }
    } else {
      this.professor.id = Math.max(...this.professores.map(p => p.id), 0) + 1;
      this.professores.push({ ...this.professor });
    }
    this.closeModal();
  }

  deleteProfessor(id: number): void {
    if (confirm('Tem certeza que deseja excluir este professor?')) {
      this.professores = this.professores.filter(p => p.id !== id);
    }
  }
}
