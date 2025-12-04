import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

interface Aula {
  id: number;
  titulo: string;
  disciplina: string;
  professor: string;
  turma: string;
  diaSemana: string;
  horarioInicio: string;
  horarioFim: string;
  sala: string;
}

@Component({
  selector: 'app-aulas',
  imports: [CommonModule, FormsModule],
  templateUrl: './aulas.component.html',
  styleUrl: './aulas.component.scss'
})
export class AulasComponent {
  aulas: Aula[] = [
    { id: 1, titulo: 'Matemática Básica', disciplina: 'Matemática', professor: 'João Silva', turma: '9º Ano A', diaSemana: 'Segunda-feira', horarioInicio: '08:00', horarioFim: '09:00', sala: 'Sala 101' },
    { id: 2, titulo: 'Gramática e Redação', disciplina: 'Português', professor: 'Maria Santos', turma: '8º Ano B', diaSemana: 'Terça-feira', horarioInicio: '09:00', horarioFim: '10:00', sala: 'Sala 102' },
    { id: 3, titulo: 'Mecânica Clássica', disciplina: 'Física', professor: 'Pedro Costa', turma: '9º Ano A', diaSemana: 'Quarta-feira', horarioInicio: '10:00', horarioFim: '11:00', sala: 'Laboratório 1' },
    { id: 4, titulo: 'Química Orgânica', disciplina: 'Química', professor: 'Ana Paula', turma: '7º Ano C', diaSemana: 'Quinta-feira', horarioInicio: '14:00', horarioFim: '15:00', sala: 'Laboratório 2' }
  ];

  aula: Aula = this.getEmptyAula();
  showModal = false;
  isEditing = false;
  searchTerm = '';

  diasSemana = ['Segunda-feira', 'Terça-feira', 'Quarta-feira', 'Quinta-feira', 'Sexta-feira'];

  getEmptyAula(): Aula {
    return {
      id: 0,
      titulo: '',
      disciplina: '',
      professor: '',
      turma: '',
      diaSemana: '',
      horarioInicio: '',
      horarioFim: '',
      sala: ''
    };
  }

  get filteredAulas(): Aula[] {
    if (!this.searchTerm) {
      return this.aulas;
    }
    return this.aulas.filter(a =>
      a.titulo.toLowerCase().includes(this.searchTerm.toLowerCase()) ||
      a.disciplina.toLowerCase().includes(this.searchTerm.toLowerCase()) ||
      a.professor.toLowerCase().includes(this.searchTerm.toLowerCase()) ||
      a.turma.toLowerCase().includes(this.searchTerm.toLowerCase())
    );
  }

  openAddModal(): void {
    this.aula = this.getEmptyAula();
    this.isEditing = false;
    this.showModal = true;
  }

  openEditModal(aula: Aula): void {
    this.aula = { ...aula };
    this.isEditing = true;
    this.showModal = true;
  }

  closeModal(): void {
    this.showModal = false;
    this.aula = this.getEmptyAula();
  }

  saveAula(): void {
    if (this.isEditing) {
      const index = this.aulas.findIndex(a => a.id === this.aula.id);
      if (index !== -1) {
        this.aulas[index] = { ...this.aula };
      }
    } else {
      this.aula.id = Math.max(...this.aulas.map(a => a.id), 0) + 1;
      this.aulas.push({ ...this.aula });
    }
    this.closeModal();
  }

  deleteAula(id: number): void {
    if (confirm('Tem certeza que deseja excluir esta aula?')) {
      this.aulas = this.aulas.filter(a => a.id !== id);
    }
  }

  getDayColor(day: string): string {
    const colors: { [key: string]: string } = {
      'Segunda-feira': 'primary',
      'Terça-feira': 'success',
      'Quarta-feira': 'info',
      'Quinta-feira': 'warning',
      'Sexta-feira': 'danger'
    };
    return colors[day] || 'secondary';
  }
}
