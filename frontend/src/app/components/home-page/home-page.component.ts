import { CommonModule } from '@angular/common';
import { Component, ElementRef, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatTabsModule } from '@angular/material/tabs';
import { Router, RouterModule } from '@angular/router';
import { LoanTerm, LoanTermViewMap, PercentByTermMap, PaymentCountByTermMap, User, ILoanApplicationCreate, Roles } from '../../interfaces';
import { Observable, Subject, takeUntil } from 'rxjs';
import { Store } from '@ngxs/store';
import { UserState } from '../../store/app.state';
import { CreateApplicationDraft, CreateLoanApplication } from '../../store/loan-application.actions';
import { CheckAuth } from '../../store/app.actions';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';


@Component({
  selector: 'app-home-page',
  imports: [CommonModule, MatButtonModule, RouterModule, MatIconModule, MatTabsModule, ReactiveFormsModule, MatFormFieldModule, MatSelectModule, MatInputModule, MatSnackBarModule],
  templateUrl: './home-page.component.html',
  styleUrl: './home-page.component.scss'
})
export class HomePageComponent implements OnInit, OnDestroy {
  @ViewChild('scrollTarget') scrollTarget!: ElementRef;
  loanTerms = Object.values(LoanTerm)
    .map((value) => ({
      value,
      label: LoanTermViewMap[value as LoanTerm]
    }));

  loanForm!: FormGroup;

  public user$: Observable<User | null> = this.store.select(UserState.currentUser);
  public user!: User | null;

  public readonly loanTermViewMap = LoanTermViewMap;
  public readonly percentByTermMap = PercentByTermMap;
  public readonly paymentCountByTermMap = PaymentCountByTermMap;
  public readonly destroy$ = new Subject<void>();

  constructor(private readonly fb: FormBuilder, private readonly store: Store, private readonly router: Router, private readonly snackBar: MatSnackBar) {
    this.loanForm = this.fb.group({
      value: [null, [Validators.required, Validators.min(0), Validators.max(20000)]],
      term: [null, Validators.required],
    });
  }

  public ngOnInit(): void {
    this.store.dispatch(new CheckAuth());
    this.user$.pipe(takeUntil(this.destroy$)).subscribe((user) => {
      this.user = user;
    });
  }

  public scrollToElement(): void {
    this.scrollTarget.nativeElement.scrollIntoView({ behavior: 'smooth' });
  }

  public getCount(): number {
    const term: LoanTerm = this.loanForm.get('term')?.value;
    return this.paymentCountByTermMap[term] || 0;
  }

  public getPercent(): number {
    const term: LoanTerm = this.loanForm.get('term')?.value;
    return this.percentByTermMap[term] || 0;
  }

  public getPaymentSize(): number {
    const value = this.loanForm.get('value')?.value;
    const term: LoanTerm = this.loanForm.get('term')?.value;
    const percent = this.getPercent();
    let paymentCount;
    if (term !== null) {
      paymentCount = this.paymentCountByTermMap[term];
    } else {
      return 0;
    }
    return this.roundUpToTwoDecimals((value + (value * percent / 100)) / paymentCount);
  }

  public createApplication(): void {
    let result: ILoanApplicationCreate;
    if (this.loanForm.valid) {
      if (this.user) {
        result = {
          value: this.loanForm.value.value,
          term: this.loanForm.value.term,
          userId: this.user.id,
        };
        if (this.user.role === Roles.Manager) {
          this.snackBar.open("Менеджер не може оформлювати займи!", "Закрити", { duration: 3000 })
        } else {
          this.store.dispatch(new CreateLoanApplication(result, {size: 10, offset: 0}));
          this.router.navigate(['/loan-applications']);
        }
      }
      else {
        result = {
          value: this.loanForm.value.value,
          term: this.loanForm.value.term
        };
        this.store.dispatch(new CreateApplicationDraft(result));
        this.router.navigate(['/register']);
      }
    }
  }


  private roundUpToTwoDecimals(value: number): number {
    return Math.ceil(value * 100) / 100;
  }

  public ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }
}
