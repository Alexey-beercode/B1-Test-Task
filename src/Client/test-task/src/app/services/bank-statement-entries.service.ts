import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { BankStatementEntryResponse } from '../models/responses/bank-statement-entry.response';
import { GetBankStatementsRequest } from '../models/requests/get-bank-statements.request';
import {environment} from '../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class BankStatementEntriesService {
  private readonly apiUrl = `${environment.apiUrls.bankStatementEntries}`;

  constructor(private http: HttpClient) {}

  getEntriesByFileId(fileId: string): Observable<BankStatementEntryResponse[]> {
    return this.http.get<BankStatementEntryResponse[]>
    (`${this.apiUrl}/${fileId}`);
  }

  getPagedEntries(request: GetBankStatementsRequest): Observable<BankStatementEntryResponse[]> {
    const params = new HttpParams()
      .set('page', request.page.toString())
      .set('pageSize', request.pageSize.toString())
        .set('bankStatementId', request.bankStatementId);

    return this.http.get<BankStatementEntryResponse[]>
    (this.apiUrl, { params });
  }
}
