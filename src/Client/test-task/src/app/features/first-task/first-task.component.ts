import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FileImportComponent } from './components/file-import/file-import.component';
import { BackButtonComponent } from '../../shared/components/back-button/back-button.component';
import { LoadingSpinnerComponent } from '../../shared/components/loading-spinner/loading-spinner.component';
import { ErrorMessageComponent } from '../../shared/components/error-message/error-message.component';
import {FileGeneratorComponent} from './components/file-gereator/file-generator.component';
import {FileMergeComponent} from './components/file-merge/file-merge.component';
import {StatisticsComponent} from './components/statistics/statistics.component';

@Component({
  selector: 'app-first-task',
  templateUrl: './first-task.component.html',
  styleUrls: ['./first-task.component.css'],
  standalone: true,
  imports: [
    CommonModule,
    BackButtonComponent,
    LoadingSpinnerComponent,
    ErrorMessageComponent,
    FileGeneratorComponent,
    FileImportComponent,
    FileMergeComponent,
    StatisticsComponent,
  ],
})
export class FirstTaskComponent {
  currentView: 'generator' | 'importer' | 'merger' | 'statistics' | null = null;
  canMerge = false;
  canImport = false;
  canShowStatistics = false;
  isLoading = false;
  errorMessage: string | null = null;

  showGenerator(): void {
    console.log('showGenerator вызван'); // Отладочное сообщение
    this.currentView = 'generator';
  }

  showImporter(): void {
    console.log('showImporter вызван'); // Отладочное сообщение
    this.currentView = 'importer';
  }

  onGenerationComplete(success: boolean): void {
    if (success) {
      this.canImport = true;
      this.canMerge=true;
    }
  }

  onLoadingChange(loading: boolean): void {
    this.isLoading = loading;
  }

  onErrorChange(error: string | null): void {
    this.errorMessage = error;
  }

  mergeFiles(): void {
    this.currentView = 'merger';
  }

  showStatistics(): void {
    this.currentView = 'statistics';
    this.canShowStatistics = true;
  }
}
