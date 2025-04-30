import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { BehaviorSubject, Observable, tap } from 'rxjs';
import { environment } from '../../environments/environment';
import { SettingsData } from '../../interfaces';

@Injectable({
  providedIn: 'root'
})
export class BackupService {
  private readonly baseUrl = environment.apiUrl + '/backup';

  private readonly settingsSubject = new BehaviorSubject<SettingsData | null>(null);

  public settings$ = this.settingsSubject.asObservable();


  constructor(private readonly http: HttpClient) { }

  downloadBackup(): Observable<Blob> {
    return this.http.get(`${this.baseUrl}/download`, { responseType: 'blob' });
  }

  restoreBackup(file: File): Observable<any> {
    const formData = new FormData();
    formData.append('file', file);
    return this.http.post(`${this.baseUrl}/restore`, formData);
  }

  loadSettings(): Observable<SettingsData> {
    return this.http.get<SettingsData>(`${this.baseUrl}/settings`)
      .pipe(
        tap(settings => this.settingsSubject.next(settings))
      );
  }

  updateSettings(settings: SettingsData): Observable<void> {
    return this.http.put<void>(`${this.baseUrl}/settings`, settings)
      .pipe(
        tap(() => {
          this.settingsSubject.next(settings);
        })
      );
  }
}
