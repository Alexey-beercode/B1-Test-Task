import {HttpTransportType, HubConnection, HubConnectionBuilder, LogLevel} from '@microsoft/signalr';
import {BehaviorSubject, Observable} from 'rxjs';
import {environment} from '../environments/environment';
import {ImportProgress} from '../models/responses/import-progress';
import {Injectable} from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class ImportProgressService {
  private hubConnection: HubConnection;
  private progressSubject = new BehaviorSubject<ImportProgress | null>(null);
  private readonly apiUrl = `${environment.apiUrls.base}`;

  constructor() {
    this.hubConnection = new HubConnectionBuilder()
      .withUrl(`${this.apiUrl}/importProgressHub`, {
        skipNegotiation: false, // Изменено на false
        transport: HttpTransportType.WebSockets | HttpTransportType.LongPolling, // Добавлен fallback
        withCredentials: true
      })
      .configureLogging(LogLevel.Debug)
      .withAutomaticReconnect({
        nextRetryDelayInMilliseconds: retryContext => {
          if (retryContext.previousRetryCount === 0) {
            return 0;
          } else if (retryContext.previousRetryCount < 3) {
            return 2000;
          }
          return 5000;
        }
      })
      .build();

    this.hubConnection.on('ReceiveProgress', (processed: number, total: number, status: string) => {
      const percentage = Math.round((processed / total) * 100);
      this.progressSubject.next({ processed, total, status, percentage });
    });
  }

  public async startConnection(): Promise<void> {
    try {
      await this.hubConnection.start();
      console.log('SignalR Connected successfully');
    } catch (err) {
      console.error('Error while starting connection: ', err);
      throw err;
    }
  }

  public stopConnection(): Promise<void> {
    return this.hubConnection.stop();
  }

  public getProgress(): Observable<ImportProgress | null> {
    return this.progressSubject.asObservable();
  }
}
