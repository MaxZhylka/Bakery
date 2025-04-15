import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';

@Component({
  selector: 'app-get-money-from',
  imports: [MatButtonModule, CommonModule, MatFormFieldModule, MatInputModule, ReactiveFormsModule, MatDialogModule],
  templateUrl: './get-money-from.component.html',
  styleUrl: './get-money-from.component.scss'
})

export class GetMoneyFromComponent {
  public form: FormGroup;

  constructor(
    private readonly dialogRef: MatDialogRef<GetMoneyFromComponent>,
    private readonly fb: FormBuilder
  ) {
    this.form = this.fb.group({
      cardNumber: [
        '',
        [
          Validators.required,
          Validators.maxLength(16),
          Validators.minLength(16),
          Validators.pattern(/^\d{16}$/)
        ]
      ]
    });
  }

  public submit(): void {
    if (this.form.valid) {
      this.dialogRef.close(this.form.value.cardNumber);
    }
  }

  public cancel(): void {
    this.dialogRef.close();
  }
}