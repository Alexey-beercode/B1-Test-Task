import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BankStatementFilesService } from '../../../../services/bank-statement-files.service';
import { ExportService } from '../../../../services/export.service';
import { BankStatementListItemResponse } from '../../../../models/responses/bank-statement-list-item.response';
import { ExportBankStatementRequest } from '../../../../models/requests/export-bank-statement.request';
import { LoadingSpinnerComponent } from '../../../../shared/components/loading-spinner/loading-spinner.component';

@Component({
  selector: 'app-file-list',
  templateUrl: 'files-list.component.html',
  styleUrls: ['./files-list.component.css'],
  standalone: true,
  imports: [CommonModule, LoadingSpinnerComponent]
})
export class FileListComponent implements OnInit {
  @Output() fileSelected = new EventEmitter<{ id: string, fileName: string }>();

  files: BankStatementListItemResponse[] = [];
  isLoading = false;
  error: string | null = null;
  currentPage = 1;
  pageSize = 10;

  constructor(
    private bankStatementFilesService: BankStatementFilesService,
    private exportService: ExportService
  ) {}

  ngOnInit(): void {
    this.loadFiles();
  }

  loadFiles(): void {
    this.isLoading = true;
    this.error = null;

    this.bankStatementFilesService.getFilesList(
    ).subscribe({
      next: (files) => {
        this.files = files;
        this.isLoading = false;
      },
      error: (error) => {
        this.error = 'Не удалось загрузить список файлов';
        this.isLoading = false;
      }
    });
  }

  onFileClick(file: BankStatementListItemResponse): void {
    this.fileSelected.emit({
      id: file.id,
      fileName: file.fileName
    });
  }

  downloadFile(id: string, fileName: string, event: Event): void {
    event.stopPropagation(); // Предотвращаем всплытие события клика
    const request: ExportBankStatementRequest = {
      fileId: id,
      exportFormat: 'Excel'
    };

    this.exportService.exportFile(request).subscribe({
      next: (blob) => {
        const url = window.URL.createObjectURL(blob);
        const link = document.createElement('a');
        link.href = url;
        link.download = fileName;
        link.click();
        window.URL.revokeObjectURL(url);
      },
      error: (error) => {
        this.error = 'Не удалось скачать файл';
      }
    });
  }

  refreshList(): void {
    this.loadFiles();
  }

  getEntriesCountText(count: number): string {
    return `${count} ${this.getDeclension(count, ['запись', 'записи', 'записей'])}`;
  }

  private getDeclension(number: number, titles: [string, string, string]): string {
    const cases = [2, 0, 1, 1, 1, 2];
    return titles[(number % 100 > 4 && number % 100 < 20) ? 2 : cases[(number % 10 < 5) ? number % 10 : 5]];
  }

  changePage(newPage: number): void {
    this.currentPage = newPage;
    this.loadFiles();
  }
}
