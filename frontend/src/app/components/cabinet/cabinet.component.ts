import { Component, OnInit } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatCardModule } from '@angular/material/card'
import { Store } from '@ngxs/store';
import { Logout } from '../../store/app.actions';
import { ReportService } from '../../services/report-service/report.service';
import { filter, Observable, Subject, takeUntil } from 'rxjs';
import { User, Roles } from '../../interfaces';
import { UserState } from '../../store/app.state';
import { MatDatepickerModule} from '@angular/material/datepicker';
import { CommonModule } from '@angular/common';
import { MatFormFieldModule } from '@angular/material/form-field';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-cabinet',
  imports: [MatButtonModule, MatIconModule, MatCardModule, CommonModule, MatDatepickerModule, MatFormFieldModule, FormsModule],
  templateUrl: './cabinet.component.html',
  styleUrl: './cabinet.component.scss'
})
export class CabinetComponent implements OnInit {

  user$!: Observable<User | null>;
  destroy$: Subject<void> = new Subject();
  userData!: User | null;
  roles = Roles;
  loanApplicationsReportRange: any = { from: null, to: null };
  moneyFlowReportRange: any = { from: null, to: null };
  monthlyPaymentsReportRange: any = { from: null, to: null };
  userApplicationsReportRange: any = { from: null, to: null };

  constructor(private readonly store: Store, private readonly reportService: ReportService) { }

  public ngOnInit(): void {
    this.user$ = this.store.select(UserState.currentUser);
    this.user$.pipe(filter(user => Boolean(user)), takeUntil(this.destroy$))
      .subscribe(user => this.userData = user);
  }

  public ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }


  public logout(): void {
    this.store.dispatch(new Logout());
  }

  downloadCompletedLoansReport(): void {
    this.reportService.saveCompletedLoansReport();
  }

  downloadLoanApplicationsReport(): void {
    this.reportService.saveLoanApplicationsReport(this.loanApplicationsReportRange.from, this.loanApplicationsReportRange.to);
  }

  downloadMoneyFlowReport(): void {
    this.reportService.saveMoneyFlowReport(this.moneyFlowReportRange.from, this.moneyFlowReportRange.to);
  }

  downloadMonthlyPaymentsReport(): void {
    this.reportService.saveMonthlyPaymentsReport(this.monthlyPaymentsReportRange.from, this.monthlyPaymentsReportRange.to);
  }

  downloadUserApplicationsReport(): void {
    this.reportService.saveUserApplicationsReport(this.userApplicationsReportRange.from, this.userApplicationsReportRange.to);
  }
}
