import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../environments/environment';
import { ExportBankStatementRequest } from '../models/requests/export-bank-statement.request';

@Injectable({
  providedIn: 'root'
})
export class ExportService {
  private readonly apiUrl = `${environment.apiUrls.export}`;

  constructor(private http: HttpClient) {}

  exportFile(request: ExportBankStatementRequest): Observable<Blob> {
    return this.http.post(this.apiUrl, request, {
      responseType: 'blob'
    });
  }
}
