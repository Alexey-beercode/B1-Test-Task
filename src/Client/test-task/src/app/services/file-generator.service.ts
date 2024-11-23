import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class FileGeneratorService {
  private readonly apiUrl = `${environment.apiUrls.fileGenerator}`;

  constructor(private http: HttpClient) {}

  generateFiles(): Observable<{ message: string; directory: string }> {
    return this.http.post<{ message: string; directory: string }>
    (`${this.apiUrl}/generate`, {});
  }
}
