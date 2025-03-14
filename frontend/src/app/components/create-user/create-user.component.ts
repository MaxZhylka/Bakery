import { Component, Inject } from '@angular/core';
import { FormGroup, FormBuilder, Validators, ReactiveFormsModule } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatSelectModule } from '@angular/material/select';
import { CommonModule } from '@angular/common';

interface User {
  name: string;
  role: string;
  email: string;
  password: string;
}

@Component({
  selector: 'app-create-user',
  standalone: true,
  imports: [MatDialogModule, MatFormFieldModule, ReactiveFormsModule, MatInputModule, MatButtonModule, MatSelectModule, CommonModule],
  templateUrl: './create-user.component.html',
  styleUrl: './create-user.component.scss'
})
export class CreateUserComponent {
  public form: FormGroup;
  public isEditMode: boolean;
  public roles = [
    { value: 'Admin', viewValue: 'Адміністратор' },
    { value: 'Manager', viewValue: 'Менеджер' },
    { value: 'User', viewValue: 'Користувач' }
  ];

  constructor(
    private readonly dialogRef: MatDialogRef<CreateUserComponent>,
    private readonly fb: FormBuilder,
    @Inject(MAT_DIALOG_DATA) public data: User | null
  ) {
    this.isEditMode = !!data;

    const formConfig: { [key: string]: any } = {
      name: [data?.name ?? '', Validators.required],
      role: [data?.role ?? '', Validators.required],
      email: [data?.email ?? '', [Validators.required, Validators.email]],
    };
  
    if (!this.isEditMode) {
      formConfig['password'] = ['', [Validators.required, Validators.minLength(6)]];
    }
  
    this.form = this.fb.group(formConfig);
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
