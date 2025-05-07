import { Component } from '@angular/core';
import { BackupService } from '../../services/backup-service/backup-service.service';
import { FormsModule } from '@angular/forms';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatSnackBar } from '@angular/material/snack-bar';


@Component({
  selector: 'app-backup-page',
  imports: [FormsModule, MatInputModule, MatFormFieldModule, MatButtonModule, MatCardModule],
  templateUrl: './backup-page.component.html',
  styleUrl: './backup-page.component.scss'
})
export class BackupPageComponent {

  selectedFile: File | null = null;
  selectedFileName: string | null = null;

  constructor(private readonly backupService: BackupService, private readonly snackBar: MatSnackBar) { }

  downloadBackup(): void {
    this.backupService.downloadBackup()
      .subscribe(blob => {
        const filename = `backup_${new Date().toISOString().replace(/[:.-]/g, '')}.bak`;

        const downloadLink = document.createElement('a');
        const url = URL.createObjectURL(blob);
        downloadLink.href = url;
        downloadLink.download = filename;
        downloadLink.click();
        URL.revokeObjectURL(url);
      });
  }

  onFileSelected(event: Event): void {
    const input = event.target as HTMLInputElement;

    if (input.files && input.files.length > 0) {
      this.selectedFile = input.files[0];
      this.selectedFileName = input.files[0].name;
    } else {
      this.selectedFileName = null;
    }
  }

  restoreBackup(): void {
    console.log(this.selectedFile)
    if (!this.selectedFile) {
      alert('Оберіть файл для відновлення.');
      return;
    }

    this.backupService.restoreBackup(this.selectedFile).pipe()
      .subscribe((response) => this.snackBar.open(
        'Базу даних відновлено!',
        '✖',
        { duration: 5000 }
      )
      );
  }
}
