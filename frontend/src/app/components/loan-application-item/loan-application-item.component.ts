import { CommonModule } from '@angular/common';
import { Component, Input, OnChanges, SimpleChanges } from '@angular/core';
import { ApplicationStatusesText, LoanApplication, LoanApplicationStatus, LoanTermViewMap, LoanTermViewMap2, Roles, User } from '../../interfaces';
import { MatIconModule } from '@angular/material/icon';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatButtonModule } from '@angular/material/button';
import { Store } from '@ngxs/store';
import { ApproveApplication, RejectApplication } from '../../store/loan-application.actions';
import { CreateRejectionReasonComponent } from '../create-rejection-reason/create-rejection-reason.component';
import { MatDialog } from '@angular/material/dialog';

@Component({
  selector: 'app-loan-application-item',
  imports: [CommonModule, MatIconModule, MatTooltipModule, MatButtonModule],
  templateUrl: './loan-application-item.component.html',
  styleUrl: './loan-application-item.component.scss'
})
export class LoanApplicationItemComponent implements OnChanges {
  @Input() loanApplication!: LoanApplication;
  @Input() role!: Roles;
  public termInNumber!: number;
  public readonly loanApplicationTerm = LoanTermViewMap2;
  public readonly loanApplicationStatuses = LoanApplicationStatus;
  public readonly ApplicationStatusesText = ApplicationStatusesText;

  userData!: User | null;
  roles = Roles;

  public ngOnChanges(changes: SimpleChanges): void {
    if (changes["loanApplication"]) {
      this.termInNumber = changes["loanApplication"].currentValue.term;
    }
  }
  constructor(private readonly store: Store, private readonly dialog: MatDialog) { }

  public approve(): void {
    this.store.dispatch(new ApproveApplication(this.loanApplication.id));
  }

  public reject(): void {
    const dialogRef = this.dialog.open(CreateRejectionReasonComponent, {
      width: '400px',
    })

    dialogRef.afterClosed().subscribe((reason) => {
      this.store.dispatch(new RejectApplication(this.loanApplication.id, reason));
    });

  }
}
