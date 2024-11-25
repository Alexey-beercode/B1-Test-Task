// file-details.component.ts
import { Component, Input, OnInit } from '@angular/core';
import { BankStatementEntriesService } from '../../../../services/bank-statement-entries.service';
import { BankStatementEntryResponse } from '../../../../models/responses/bank-statement-entry.response';
import { finalize } from 'rxjs/operators';
import { CommonModule, DecimalPipe } from '@angular/common';
import {GetBankStatementsRequest} from '../../../../models/requests/get-bank-statements.request';

@Component({
  selector: 'app-file-details',
  templateUrl: './file-details.component.html',
  standalone: true,
  imports: [
    CommonModule,
    DecimalPipe
  ],
  styleUrls: ['./file-details.component.css']
})
export class FileDetailsComponent implements OnInit {
  @Input() fileId: string = '';
  @Input() fileName: string = '';

  tableData: BankStatementEntryResponse[] = [];
  isLoading: boolean = false;
  error: string | null = null;

  // Pagination
  currentPage: number = 1;
  pageSize: number = 10;
  totalItems: number = 0;

  constructor(private bankStatementService: BankStatementEntriesService) {}

  ngOnInit() {
    this.loadBankStatements();
  }

  private loadBankStatements() {
    if (!this.fileId) return;

    this.isLoading = true;
    this.error = null;

    const request:GetBankStatementsRequest = {
      page: this.currentPage,
      pageSize: this.pageSize,
      bankStatementId: this.fileId
    };

    this.bankStatementService.getPagedEntries(request)
      .pipe(
        finalize(() => this.isLoading = false)
      )
      .subscribe({
        next: (data) => {
          this.tableData = data;
        },
        error: (error) => {
          console.error('Error loading bank statements:', error);
          this.error = 'Произошла ошибка при загрузке данных';
        }
      });
  }

  onPageChange(page: number) {
    this.currentPage = page;
    this.loadBankStatements();
  }
}
