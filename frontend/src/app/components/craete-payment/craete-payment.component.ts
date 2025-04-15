import { CommonModule } from '@angular/common';
import { Component, Inject } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MAT_DIALOG_DATA, MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';

@Component({
  selector: 'app-create-payment',
  imports: [MatButtonModule, CommonModule, MatFormFieldModule, MatInputModule, ReactiveFormsModule, MatDialogModule],
  templateUrl: './craete-payment.component.html',
  styleUrl: './craete-payment.component.scss'
})

export class CreatePaymentComponent {
  public form: FormGroup;

  constructor(
    private readonly dialogRef: MatDialogRef<CreatePaymentComponent>,
    private readonly fb: FormBuilder,
    @Inject(MAT_DIALOG_DATA) public data: any
  ) {
    this.form = this.fb.group({
      value: [
        '',
        [
          Validators.required,
          Validators.max(data.maxValue),
        ]
      ]
    });
  }

  public submit(): void {
    if (this.form.valid) {
      this.dialogRef.close(this.form.value.value);
    }
  }

  public cancel(): void {
    this.dialogRef.close();
  }
}