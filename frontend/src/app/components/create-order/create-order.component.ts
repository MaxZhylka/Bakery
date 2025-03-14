import { Component, Inject } from '@angular/core';
import { FormGroup, FormBuilder, Validators, ReactiveFormsModule } from '@angular/forms';
import { ICreateOrder, Product, User} from '../../interfaces';
import { MAT_DIALOG_DATA, MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { CommonModule } from '@angular/common';
import { Store } from '@ngxs/store';
import { UserState } from '../../store/app.state';
import { Observable } from 'rxjs';


@Component({
  selector: 'app-create-order',
  imports: [MatDialogModule, MatFormFieldModule, ReactiveFormsModule, MatInputModule, MatButtonModule, CommonModule],
  templateUrl: './create-order.component.html',
  styleUrl: './create-order.component.scss'
})
export class CreateOrderComponent {
  public form: FormGroup;
  public productPrice!: number;
  public currentUser$!: Observable<User | null>;
  public customerId?: string = '';

  constructor(
    private readonly dialogRef: MatDialogRef<CreateOrderComponent>,
    private readonly fb: FormBuilder,
    private readonly store: Store,
    @Inject(MAT_DIALOG_DATA) public product: Product
  ) {
    this.productPrice = this.product.price;
    this.form = this.fb.group({
      productCount: [1, [Validators.required, Validators.min(1)]],
      price: [{ value: this.productPrice, disabled: true }],
    });
  }

  public ngOnInit(): void {
    this.currentUser$ = this.store.select(UserState.currentUser);
    this.currentUser$.subscribe(user => this.customerId = user?.id);

    this.form.get('productCount')?.valueChanges.subscribe(count => {
      const totalPrice = count * this.productPrice;
      this.form.get('price')?.setValue(totalPrice.toFixed(2), { emitEvent: false });
    });
  }

  public onSubmit(): void {
    if (this.form.valid && this.product.id && this.customerId) {
      const order: ICreateOrder = {
        productId: this.product.id,
        productCount: this.form.value.productCount,
        price: this.form.get('price')?.value ?? 0,
        customerId: this.customerId,
      };
      this.dialogRef.close(order);
    }
  }

  public onCancel(): void {
    this.dialogRef.close();
  }
}