import { Routes } from '@angular/router';
import { RegistrationFormComponent } from './components/registration-form/registration-form.component';
import { authGuard } from './guards/authGuard';
import { CabinetComponent } from './components/cabinet/cabinet.component';
import { UsersComponent } from './components/users/users.component';
import { LogsComponent } from './components/logs/logs.component';
import { LoansComponent } from './components/loans/loans.component';
import { LoanApplicationsComponent } from './components/loan-applications/loan-applications.component';
import { PaymentComponent } from './components/payment/payment.component';
import { HomePageComponent } from './components/home-page/home-page.component';

export const routes: Routes = [
  { path: 'login', component: RegistrationFormComponent },
  { path: 'register', component: RegistrationFormComponent },
  { path: 'cabinet', component: CabinetComponent, canActivate: [() => authGuard()] },
  // { path: 'products', component: ProductsComponent, canActivate: [() => authGuard()] },
  // { path: 'products/dynamic', component: ProductsDynamicComponent, canActivate: [() => authGuard(['Admin', 'Manager'])]},
  // { path: 'orders', component: OrdersComponent, canActivate: [() => authGuard()] },
  { path: 'loans', component: LoansComponent, canActivate: [() => authGuard()] },
  { path: 'loan-applications', component: LoanApplicationsComponent, canActivate: [() => authGuard()] },
  { path: 'payments', component: PaymentComponent, canActivate: [() => authGuard()] },
  { path: 'users', component: UsersComponent, canActivate: [() => authGuard(['Admin', 'Manager'])] },
  { path: 'logs', component: LogsComponent, canActivate: [() => authGuard(['Admin'])] },
  { path: '', component: HomePageComponent, pathMatch: 'full'}
];
