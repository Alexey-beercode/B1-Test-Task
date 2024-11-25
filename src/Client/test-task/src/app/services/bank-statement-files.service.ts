import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { UploadBankStatementResponse } from '../models/responses/upload-bank-statement.response';
import { BankStatementListItemResponse } from '../models/responses/bank-statement-list-item.response';
import { BankStatementDetailsResponse } from '../models/responses/bank-statement-details.response';
import { GetBankStatementsRequest } from '../models/requests/get-bank-statements.request';
import { UploadBankStatementRequest } from '../models/requests/upload-bank-statement.request';
import { environment } from '../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class BankStatementFilesService {
  private readonly apiUrl = `${environment.apiUrls.bankStatementFiles}`;

  constructor(private http: HttpClient) {}

// В BankStatementFilesService
  uploadFile(file: File): Observable<UploadBankStatementResponse> {
    const formData = new FormData();
    formData.append('FileContent', file);
    formData.append('FileName', file.name);

    return this.http.post<UploadBankStatementResponse>(
      `${this.apiUrl}/upload`,
      formData
    );
  }

  getFilesList(): Observable<BankStatementListItemResponse[]> {

    return this.http.get<BankStatementListItemResponse[]>(`${this.apiUrl}/files`);
  }

  getFileDetails(fileId: string): Observable<BankStatementDetailsResponse> {
    return this.http.get<BankStatementDetailsResponse>(`${this.apiUrl}/files/${fileId}`);
  }
}
