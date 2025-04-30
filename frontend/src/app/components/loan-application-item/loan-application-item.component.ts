import { CommonModule } from '@angular/common';
import { Component, Input } from '@angular/core';
import { ApplicationStatusesText, LoanApplication, LoanApplicationStatus, LoanTermViewMap, Roles, User } from '../../interfaces';
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
export class LoanApplicationItemComponent {
  @Input() loanApplication!: LoanApplication;
  @Input() role!: Roles;
  public readonly loanApplicationTerm = LoanTermViewMap;
  public readonly loanApplicationStatuses = LoanApplicationStatus;
  public readonly ApplicationStatusesText = ApplicationStatusesText;

  userData!: User | null;
  roles = Roles;

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
