import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { ActivatedRoute, RouterModule } from '@angular/router';
import { Store } from '@ngxs/store';
import { Login, Register } from '../../store/app.actions';
import { Observable } from 'rxjs';
import { CommonModule } from '@angular/common';
import { MatFormFieldModule, MatError } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { MatButtonModule } from '@angular/material/button';


@Component({
  selector: 'app-registration-form',
  templateUrl: './registration-form.component.html',
  styleUrls: ['./registration-form.component.scss'],
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatProgressBarModule,
    MatError,
    MatButtonModule,
    RouterModule
  ]
})

export class RegistrationFormComponent implements OnInit {
  public isRegisterRoute = false;
  public errorMessage = '';
  public loginForm!: FormGroup;
  public isLoading$!: Observable<boolean>;
  
  constructor(   
    private readonly fb: FormBuilder,
    private readonly route: ActivatedRoute,
    private readonly store: Store
  ) {}

  public ngOnInit(): void {
    this.isRegisterRoute = this.route.snapshot.routeConfig?.path === 'register';
    this.isLoading$ = this.store.select(state => state.app.isLoading);
  
    this.loginForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(6)]],
      login: ['', [Validators.required, Validators.minLength(3)]]
    });

    if (!this.isRegisterRoute) {
      this.loginForm.removeControl('login');
    }
  }

  onSubmit() {
    const { email, password, login } = this.loginForm.value;

    if(this.isRegisterRoute) {
      this.store.dispatch(new Register({ email, password, login }))
    }
    else
    {
      this.store.dispatch(new Login({ email, password }));
    }
  }
}
