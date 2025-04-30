import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { ReactiveFormsModule, FormGroup, FormBuilder, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';

@Component({
  selector: 'app-create-rejection-reason',
  imports: [MatButtonModule, CommonModule, MatFormFieldModule, MatInputModule, ReactiveFormsModule, MatDialogModule],
  templateUrl: './create-rejection-reason.component.html',
  styleUrl: './create-rejection-reason.component.scss'
})
export class CreateRejectionReasonComponent {
  public form: FormGroup;

  constructor(
    private readonly dialogRef: MatDialogRef<CreateRejectionReasonComponent>,
    private readonly fb: FormBuilder
  ) {
    this.form = this.fb.group({
      rejectionReason: [
        '',
        [
          Validators.maxLength(2000),
        ]
      ]
    });
  }

  public submit(): void {
    if (this.form.valid) {
      this.dialogRef.close(this.form.value.rejectionReason);
    }
  }

  public cancel(): void {
    this.dialogRef.close();
  }
}
