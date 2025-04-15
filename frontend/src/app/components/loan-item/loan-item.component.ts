import { Component, Input, OnChanges, OnInit, SimpleChanges } from '@angular/core';
import { Loan, LoanStatus, Roles } from '../../interfaces';
import { CommonModule, CurrencyPipe } from '@angular/common';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatDialog } from '@angular/material/dialog';
import { GetMoneyFromComponent } from '../get-money-from/get-money-from.component';
import { Store } from '@ngxs/store';
import { GetMoney } from '../../store/loan-application.actions';
import { CreatePaymentComponent } from '../craete-payment/craete-payment.component';
import { CreatePayment } from '../../store/payment.actions';

@Component({
  selector: 'app-loan-item',
  imports: [CurrencyPipe, CommonModule, MatButtonModule, MatIconModule],
  templateUrl: './loan-item.component.html',
  styleUrl: './loan-item.component.scss'
})
export class LoanItemComponent implements OnChanges {
  @Input() loan!: Loan;
  @Input() role!: Roles;
  public readonly loanStatuses = LoanStatus;
  roles = Roles;

  constructor(private readonly dialog: MatDialog, private readonly store: Store) { }

  public ngOnChanges(changes: SimpleChanges): void {
    if (changes['loan']?.currentValue) {
      const loan: Loan = changes['loan'].currentValue;
      changes['loan'].currentValue.status = this.getUpdatedLoanStatus(loan);
    }
  }

  public getMoney(): void {
    const dialogRef = this.dialog.open(GetMoneyFromComponent, {
      width: '400px',
    })

    dialogRef.afterClosed().subscribe((cardNumber) => {
      if (cardNumber) {
        this.store.dispatch(new GetMoney(this.loan.id));
      }
    });
  }

  public payMoney(): void {
    const dialogRef = this.dialog.open(CreatePaymentComponent, {
      width: '400px',
      data: { maxValue: this.loan.leftValue }
    })

    dialogRef.afterClosed().subscribe((valueToPay) => {
      if (valueToPay) {
        this.store.dispatch(new CreatePayment({ value: valueToPay, loanId: this.loan.id, userId: this.loan.userId }, { size: 10, offset: 0 }));
      }
    });
  }

  private getUpdatedLoanStatus(loan: Loan): LoanStatus {
    const loanDate = new Date(loan.nextPaymentDate);
    const currentDate = new Date();

    const diffInMs = Math.abs(currentDate.getTime() - loanDate.getTime());
    const diffInDays = diffInMs / (1000 * 60 * 60 * 24);

    if (diffInDays < 30 && loan.status === LoanStatus.Active) {
      return LoanStatus.NeedPayment;
    }

    return loan.status;
  }
}
