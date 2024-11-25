import { Component, Output, EventEmitter } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BankStatementFilesService } from '../../../../services/bank-statement-files.service';
import {LoadingSpinnerComponent} from '../../../../shared/components/loading-spinner/loading-spinner.component';

@Component({
  selector: 'app-upload-modal',
  templateUrl: './upload-modal.component.html',
  styleUrls: ['./upload-modal.component.css'],
  standalone: true,
  imports: [CommonModule, LoadingSpinnerComponent]
})
export class UploadModalComponent {
  @Output() close = new EventEmitter<void>();
  @Output() uploadComplete = new EventEmitter<boolean>();

  selectedFile: File | null = null;
  isLoading = false;
  errorMessage: string | null = null;

  constructor(private bankStatementFilesService: BankStatementFilesService) {}

  onFileSelected(event: any): void {
    const file = event.target.files[0];

    // Добавляем проверку размера
    const maxSize = 10 * 1024 * 1024; // 10MB
    if (file.size > maxSize) {
      this.errorMessage = 'Размер файла не должен превышать 10MB';
      this.selectedFile = null;
      return;
    }

    // Добавляем проверку расширения
    const validExtensions = ['.xlsx', '.xls', '.csv'];
    const extension = '.' + file.name.split('.').pop().toLowerCase();
    if (!validExtensions.includes(extension)) {
      this.errorMessage = 'Пожалуйста, выберите файл с расширением .xls, .xlsx или .csv';
      this.selectedFile = null;
      return;
    }

    this.selectedFile = file;
    this.errorMessage = null;
  }

  uploadFile(): void {
    if (!this.selectedFile) return;

    this.isLoading = true;
    this.errorMessage = null;

    console.log('Selected file:', this.selectedFile);
    console.log('File name:', this.selectedFile?.name);
    console.log('File size:', this.selectedFile?.size);
    this.bankStatementFilesService.uploadFile(this.selectedFile).subscribe({
      next: () => {
        this.isLoading = false;
        this.uploadComplete.emit(true);
        this.close.emit();
      },
      error: (error) => {
        this.isLoading = false;
        if (error.error?.errors) {
          const errorMessages = [];
          for (const field in error.error.errors) {
            errorMessages.push(...error.error.errors[field]);
          }
          this.errorMessage = errorMessages.join('\n');
        } else {
          this.errorMessage = 'Ошибка при загрузке файла. Пожалуйста, попробуйте снова.';
        }
        this.uploadComplete.emit(false);
      }
    });
  }

  closeModal(): void {
    this.close.emit();
  }
}
