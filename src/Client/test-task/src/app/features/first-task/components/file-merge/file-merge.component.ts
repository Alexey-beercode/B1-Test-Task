import { Component, EventEmitter, Output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { FileMergeService } from '../../../../services/file-merge.service';
import { LoadingSpinnerComponent } from '../../../../shared/components/loading-spinner/loading-spinner.component';
import { ErrorMessageComponent } from '../../../../shared/components/error-message/error-message.component';
import { MergeResult } from '../../../../models/responses/merge-result.response';

@Component({
  selector: 'app-file-merge',
  templateUrl: './file-merge.component.html',
  styleUrls: ['./file-merge.component.css'],
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    LoadingSpinnerComponent,
    ErrorMessageComponent
  ]
})
export class FileMergeComponent {
  loading = false;
  errorMessage: string | null = null;
  mergeResult: MergeResult | null = null;
  excludePattern = '';

  @Output() loadingChange = new EventEmitter<boolean>();
  @Output() errorChange = new EventEmitter<string | null>();
  @Output() mergeComplete = new EventEmitter<boolean>();

  constructor(private fileMergeService: FileMergeService) {}

  mergeFiles(): void {
    if (!this.excludePattern) return;

    this.loading = true;
    this.errorMessage = null;
    this.mergeResult = null;
    this.loadingChange.emit(true);
    this.errorChange.emit(null);

    this.fileMergeService.mergeFiles(this.excludePattern).subscribe({
      next: (result) => {
        this.loading = false;
        this.mergeResult = result;
        this.loadingChange.emit(false);
        this.mergeComplete.emit(true);
      },
      error: (error) => {
        this.loading = false;
        this.errorMessage = 'Ошибка при объединении файлов';
        this.loadingChange.emit(false);
        this.errorChange.emit(this.errorMessage);
        this.mergeComplete.emit(false);
      }
    });
  }
}
