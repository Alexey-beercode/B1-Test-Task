// second-task.component.ts
import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FileListComponent } from './components/files-list/files-list.component';
import { FileDetailsComponent } from './components/file-details/file-details.component';
import { UploadModalComponent } from './components/upload-modal/upload-modal.component';
import { LoadingSpinnerComponent } from '../../shared/components/loading-spinner/loading-spinner.component';
import { ErrorMessageComponent } from '../../shared/components/error-message/error-message.component';
import { BackButtonComponent } from '../../shared/components/back-button/back-button.component';

@Component({
  selector: 'app-second-task',
  standalone: true,
  imports: [
    CommonModule,
    FileListComponent,
    FileDetailsComponent,
    UploadModalComponent,
    LoadingSpinnerComponent,
    ErrorMessageComponent,
    BackButtonComponent
  ],
  templateUrl: './second-task.component.html',
  styleUrls: ['./second-task.component.css']
})
export class SecondTaskComponent {
  showModal = false;
  isLoading = false;
  errorMessage: string | null = null;
  selectedFileId: string | null = null;
  selectedFileName: string = '';

  showUploadModal(): void {
    this.showModal = true;
  }

  closeModal(): void {
    this.showModal = false;
  }

  onUploadComplete(success: boolean): void {
    if (success) {
      this.errorMessage = null;
      // Можно добавить дополнительную логику после успешной загрузки
    } else {
      this.errorMessage = 'Произошла ошибка при загрузке файла';
    }
  }

  onFileSelected(fileData: { id: string, fileName: string }): void {
    this.selectedFileId = fileData.id;
    this.selectedFileName = fileData.fileName;
  }
}
