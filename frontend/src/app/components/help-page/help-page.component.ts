import { Component, OnInit } from '@angular/core';
import { first } from 'rxjs';
import { BackupService } from '../../services/backup-service/backup-service.service';
import { SettingsData } from '../../interfaces';
import { CommonModule } from '@angular/common';


@Component({
  selector: 'app-help-page',
  imports: [CommonModule],
  templateUrl: './help-page.component.html',
  styleUrl: './help-page.component.scss'
})
export class HelpPageComponent implements OnInit {
    public settings!: SettingsData;
    constructor(private readonly backupService: BackupService) {}

    public ngOnInit(): void {
        this.backupService.loadSettings().pipe(first()).subscribe((settings)=>{
          this.settings = settings;
        })
    }

}
