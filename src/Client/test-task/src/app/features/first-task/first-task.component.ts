import { Component } from '@angular/core';
import { FileGeneratorService } from '../../services/file-generator.service';
import { FileMergeService } from '../../services/file-merge.service';
import { FileImportService } from '../../services/file-import.service';
import { StatisticsService } from '../../services/statistics.service';
import {BackButtonComponent} from '../../shared/components/back-button/back-button.component';

@Component({
  selector: 'app-first-task',
  templateUrl: './first-task.component.html',
  standalone: true,
  imports: [
    BackButtonComponent
  ],
  styleUrls: ['./first-task.component.css']
})
export class FirstTaskComponent {
  loading = false;
  errorMessage: string | null = null;
  canMerge = false; // Условие для возможности слияния
  canImport = false; // Условие для возможности импорта
  canShowStatistics = false; // Условие для статистики

  constructor(
    private fileGeneratorService: FileGeneratorService,
    private fileMergeService: FileMergeService,
    private fileImportService: FileImportService,
    private statisticsService: StatisticsService
  ) {}

  generateFiles() {
    this.loading = true;
    this.fileGeneratorService.generateFiles().subscribe({
      next: (response) => {
        this.loading = false;
        this.canMerge = true;
      },
      error: (error) => {
        this.loading = false;
        this.errorMessage = 'Ошибка при генерации файлов.';
      }
    });
  }

  mergeFiles() {
    // Логика для слияния файлов
  }

  importFiles() {
    // Логика для импорта файлов
  }

  getStatistics() {
    // Логика для получения статистики
  }
}
