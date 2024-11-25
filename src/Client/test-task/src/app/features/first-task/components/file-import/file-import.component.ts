import { Component, EventEmitter, Output, OnInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FileImportService } from '../../../../services/file-import.service';
import { ImportProgressService} from '../../../../services/import-progress.service';
import { LoadingSpinnerComponent } from '../../../../shared/components/loading-spinner/loading-spinner.component';
import { ErrorMessageComponent } from '../../../../shared/components/error-message/error-message.component';
import { Subscription } from 'rxjs';
import { ImportProgress } from '../../../../models/responses/import-progress';

@Component({
  selector: 'app-file-import',
  templateUrl: './file-import.component.html',
  standalone: true,
  imports: [
    LoadingSpinnerComponent,
    ErrorMessageComponent,
    CommonModule
  ],
  styleUrls: ['./file-import.component.css']
})
export class FileImportComponent implements OnInit, OnDestroy {
  loading = false;
  errorMessage: string | null = null;
  success = false;
  progress: ImportProgress | null = null;
  private progressSubscription?: Subscription;

  @Output() loadingChange = new EventEmitter<boolean>();
  @Output() errorChange = new EventEmitter<string | null>();
  @Output() importComplete = new EventEmitter<boolean | null>();

  constructor(
    private fileImportService: FileImportService,
    private importProgressService: ImportProgressService
  ) {}

  async ngOnInit() {
    try {
      console.log('Starting SignalR connection...');
      await this.importProgressService.startConnection();

      this.progressSubscription = this.importProgressService.getProgress()
        .subscribe({
          next: (progress) => {
            console.log('Progress received:', progress);
            this.progress = progress;

            // Если прогресс достиг 100%, устанавливаем success в true
            if (progress && progress.percentage === 100) {
              this.success = true;
              this.loading = false;
              this.loadingChange.emit(false);
              this.importComplete.emit(true);
            }
          },
          error: (error) => {
            console.error('Progress subscription error:', error);
            this.errorMessage = 'Ошибка при получении обновлений прогресса';
            this.errorChange.emit(this.errorMessage);
          }
        });

      this.importFiles();
    } catch (error) {
      console.error('Failed to start SignalR connection:', error);
      this.errorMessage = 'Не удалось подключиться к серверу для отслеживания прогресса';
      this.errorChange.emit(this.errorMessage);
    }
  }

  ngOnDestroy() {
    this.progressSubscription?.unsubscribe();
    this.importProgressService.stopConnection().catch(console.error);
  }

  importFiles(): void {
    this.loading = true;
    this.errorMessage = null;
    this.success = false;
    this.progress = null;
    this.loadingChange.emit(true);
    this.errorChange.emit(null);

    this.fileImportService.importFiles().subscribe({
      next: () => {
        // Успешное завершение будет обработано через SignalR
        console.log('Import request sent successfully');
      },
      error: (error) => {
        console.error('Import error:', error);
        this.loading = false;
        this.errorMessage = 'Ошибка при импорте файлов.';
        this.loadingChange.emit(false);
        this.errorChange.emit(this.errorMessage);
        this.importComplete.emit(false);
      }
    });
  }
}
