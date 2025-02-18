import { Component, Inject } from '@angular/core';
import { FormGroup, FormBuilder, Validators, ReactiveFormsModule } from '@angular/forms';
import { Product } from '../../interfaces';
import { MAT_DIALOG_DATA, MatDialogModule, MatDialogRef } from '@angular/material/dialog'
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-create-product',
  imports: [MatDialogModule, MatFormFieldModule, ReactiveFormsModule, MatInputModule, MatButtonModule, CommonModule],
  templateUrl: './create-product.component.html',
  styleUrl: './create-product.component.scss'
})
export class CreateProductComponent {
  public form: FormGroup;
  public isEditMode: boolean;

  constructor(
    private readonly dialogRef: MatDialogRef<CreateProductComponent>,
    private readonly fb: FormBuilder,
    @Inject(MAT_DIALOG_DATA) public data: Product | null
  ) {
    this.isEditMode = !!data;
    this.form = this.fb.group({
      name: [data?.name ?? '', Validators.required],
      price: [data?.price ?? '', [Validators.required, Validators.pattern('^[0-9]+(\\.[0-9]{1,2})?$')]],
      count: [data?.productCount ?? '', [Validators.required, Validators.min(1)]],
    });
  }

  public onSubmit(): void {
    if (this.form.valid) {
      const result = { ...this.data, ...this.form.value };
      this.dialogRef.close(result);
    }
  }

  public onCancel(): void {
    this.dialogRef.close();
  }
}