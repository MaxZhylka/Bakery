import { Component, OnInit, OnDestroy } from '@angular/core';
import { Subject, takeUntil } from 'rxjs';
import { SettingsData } from '../../interfaces';
import { BackupService } from '../../services/backup-service/backup-service.service';
import { MatFormField } from '@angular/material/form-field';
import { FormsModule } from '@angular/forms';
import { MatInputModule } from '@angular/material/input';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { CommonModule } from '@angular/common';
import { MatSnackBar } from '@angular/material/snack-bar';


@Component({
  selector: 'app-settings-page',
  templateUrl: './settings-page.component.html',
  imports: [MatFormField, FormsModule, MatInputModule, MatDatepickerModule, MatCardModule, MatIconModule, MatButtonModule, CommonModule],
  styleUrls: ['./settings-page.component.scss']
})
export class SettingsPageComponent implements OnInit, OnDestroy {
  public settings!: SettingsData;

  public destroy$: Subject<void> = new Subject();

  constructor(private readonly backupService: BackupService, private readonly snackBar: MatSnackBar) { }

  ngOnInit(): void {
    this.backupService.loadSettings().pipe(takeUntil(this.destroy$)).subscribe(s => this.settings = s);
  }


  onSave(): void {
    if (this.settings.startReportDate && this.settings.endReportDate) {
      this.backupService.updateSettings(this.settings).pipe(takeUntil(this.destroy$))
        .subscribe({
          next: () => this.snackBar.open(
            'Успішно оновлено',
            '✖',
            { duration: 5000 }
          ),
          error: () => this.snackBar.open(
            'Помилка оновлення',
            '✖',
            { duration: 5000 }
          )
        }
        )
    }
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }
}
