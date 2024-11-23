import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { MergeResult } from '../models/responses/merge-result.response';
import { environment } from '../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class FileMergeService {
  private readonly apiUrl = `${environment.apiUrls.fileMerge}`;

  constructor(private http: HttpClient) {}

  mergeFiles(patternToExclude: string): Observable<MergeResult> {
    return this.http.post<MergeResult>
    (`${this.apiUrl}/merge/${patternToExclude}`, {});
  }
}
