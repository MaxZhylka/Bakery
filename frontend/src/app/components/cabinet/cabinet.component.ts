import { Component } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatCardModule } from '@angular/material/card'
import { Store } from '@ngxs/store';
import { Logout } from '../../store/app.actions';
import { ReportService } from '../../services/report-service/report.service';

@Component({
  selector: 'app-cabinet',
  imports: [MatButtonModule, MatIconModule, MatCardModule],
  templateUrl: './cabinet.component.html',
  styleUrl: './cabinet.component.scss'
})
export class CabinetComponent {

  constructor(private readonly store: Store, private readonly reportService: ReportService) { }

  public logout(): void {
    this.store.dispatch(new Logout());
  }

  downloadProductReport(): void {
    this.reportService.saveProductReport();
  }

  downloadCustomerReport(): void {
    this.reportService.saveCustomerReport();
  }

  downloadAllOrdersReport(): void {
    this.reportService.saveAllOrdersReport();
  }

  downloadOrderTrendsByCustomer(): void {
    this.reportService.saveOrderTrendsByCustomer();
  }

  downloadOrderTrendsByProduct(): void {
    this.reportService.saveOrderTrendsByProduct();
  }
}
