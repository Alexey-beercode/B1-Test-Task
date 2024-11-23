import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Statistics } from '../models/responses/statistics.response';
import { environment } from '../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class StatisticsService {
  private readonly apiUrl = `${environment.apiUrls.statistics}`;

  constructor(private http: HttpClient) {}

  getStatistics(): Observable<Statistics> {
    return this.http.get<Statistics>(this.apiUrl);
  }
}
