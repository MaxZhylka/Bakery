import { Component, Inject } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef, MatDialogModule } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatButtonModule } from '@angular/material/button';
import { CommonModule } from '@angular/common';
import { Store } from '@ngxs/store';
import { UserState } from '../../store/app.state';
import { Observable } from 'rxjs';
import { ILoanApplicationCreate, LoanTerm, LoanTermViewMap, PaymentCountByTermMap, PercentByTermMap, User } from '../../interfaces';

@Component({
  selector: 'app-create-loan-application',
  standalone: true,
  imports: [
    CommonModule,
    MatDialogModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatButtonModule,
    ReactiveFormsModule
  ],
  templateUrl: './craete-loan-application.component.html',
  styleUrl: './craete-loan-application.component.scss'
})
export class CreateLoanApplicationComponent {
  public form: FormGroup;
  public currentUser$!: Observable<User | null>;
  public userId: string = '';
  public readonly paymentCountByTermMap = PaymentCountByTermMap;
  public readonly percentByTermMap = PercentByTermMap;
  loanTerms = Object.values(LoanTerm)
  .map((value) => ({
    value,
    label: LoanTermViewMap[value as LoanTerm]
  }));

  constructor(
    private readonly dialogRef: MatDialogRef<CreateLoanApplicationComponent>,
    private readonly fb: FormBuilder,
    private readonly store: Store,
    @Inject(MAT_DIALOG_DATA) public data: any
  ) {
    this.form = this.fb.group({
      value: [null, [Validators.required, Validators.max(20000)]],
      term: [null, Validators.required],
    });
  }

  ngOnInit(): void {
    this.currentUser$ = this.store.select(UserState.currentUser);
    this.currentUser$.subscribe(user => {
      if (user) this.userId = user.id;
    });
  }

  public getCount(): number {
    const term: LoanTerm = this.form.get('term')?.value;
    return this.paymentCountByTermMap[term] || 0;
  }

  public getPercent(): number {
    const term: LoanTerm = this.form.get('term')?.value;
    return this.percentByTermMap[term] || 0;
  }

  public getPaymentSize(): number {
    const value = this.form.get('value')?.value;
    const term: LoanTerm = this.form.get('term')?.value;
    const percent = this.getPercent();
    let paymentCount;
    if(term !== null) {
      paymentCount = this.paymentCountByTermMap[term];
    } else {
      return 0;
    }
    return this.roundUpToTwoDecimals((value + (value * percent / 100)) / paymentCount);
  }

  private roundUpToTwoDecimals(value: number): number {
    return Math.ceil(value * 100) / 100;
  }

  public submit(): void {
    if (this.form.valid && this.userId) {
      const result: ILoanApplicationCreate = {
        value: this.form.value.value,
        term: this.form.value.term,
        userId: this.userId,
      };
      this.dialogRef.close(result);
    }
  }

  public cancel(): void {
    this.dialogRef.close();
  }
}
