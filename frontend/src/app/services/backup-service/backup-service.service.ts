import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class BackupService {
  private readonly baseUrl = environment.apiUrl + '/backup';

  constructor(private readonly http: HttpClient) { }

  downloadBackup(backupFolderPath: string): Observable<Blob> {
    const params = new HttpParams().set('backupFolderPath', backupFolderPath);
    return this.http.get(`${this.baseUrl}/download`, { params, responseType: 'blob' });
  }

  restoreBackup(file: File): Observable<any> {
    const formData = new FormData();
    formData.append('file', file);
    return this.http.post(`${this.baseUrl}/restore`, formData);
  }
}
