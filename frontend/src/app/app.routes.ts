import { Routes } from '@angular/router';
import { RegistrationFormComponent } from './components/registration-form/registration-form.component';
import { authGuard } from './guards/authGuard';
import { OrdersComponent } from './components/orders/orders.component';
import { CabinetComponent } from './components/cabinet/cabinet.component';
import { ProductsComponent } from './components/products/products.component';
import { UsersComponent } from './components/users/users.component';
import { LogsComponent } from './components/logs/logs.component';
import { ProductsDynamicComponent } from './components/products-dynamic/products-dynamic.component';

export const routes: Routes = [
  { path: 'login', component: RegistrationFormComponent },
  { path: 'register', component: RegistrationFormComponent },
  { path: 'cabinet', component: CabinetComponent, canActivate: [() => authGuard()] },
  { path: 'products', component: ProductsComponent, canActivate: [() => authGuard()] },
  { path: 'products/dynamic', component: ProductsDynamicComponent, canActivate: [() => authGuard(['Admin', 'Manager'])]},
  { path: 'orders', component: OrdersComponent, canActivate: [() => authGuard()] },
  { path: 'users', component: UsersComponent, canActivate: [() => authGuard(['Admin', 'Manager'])] },
  { path: 'logs', component: LogsComponent, canActivate: [() => authGuard(['Admin'])] },
  { path: '', redirectTo: 'products', pathMatch: 'full'}
];
