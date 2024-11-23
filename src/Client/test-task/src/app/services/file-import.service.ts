import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class FileImportService {
  private readonly apiUrl = `${environment.apiUrls.fileImport}`;

  constructor(private http: HttpClient) {}

  importFiles(): Observable<void> {
    return this.http.post<void>(`${this.apiUrl}/import`, {});
  }
}
