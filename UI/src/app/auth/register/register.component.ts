import { Component, ErrorHandler, Inject, inject } from '@angular/core';
import { AuthService } from '../../core/services/auth.service';
import { Router, RouterModule } from '@angular/router';
import { Register } from '../../models/register.model';

import {
  FormControl,
  FormGroup,
  FormsModule,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { validateHorizontalPosition } from '@angular/cdk/overlay';
import { MatIcon } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';

@Component({
  selector: 'app-register-component',
  imports: [RouterModule, FormsModule, ReactiveFormsModule, MatIcon, MatInputModule],
  templateUrl: './register.component.html',
  styleUrl: './register.component.scss',
})
export class RegisterComponent {
  authService = inject(AuthService);
  router = inject(Router);
  toastr = inject(ToastrService);

  registerForm = new FormGroup({
    firstName: new FormControl('', [Validators.required]),
    lastName: new FormControl('', [Validators.required]),
    username: new FormControl('', [Validators.required, Validators.minLength(3)]),
    email: new FormControl('', [
      Validators.required,
      Validators.pattern(/^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$/),
    ]),
    phoneNumber: new FormControl('', [Validators.pattern(/^[0-9+ ]{10,15}$/)]),
    password: new FormControl('', [Validators.required, Validators.minLength(8)]),
  });

  register() {
    if (this.registerForm.valid) {
      this.authService.register(this.registerForm.value).subscribe({
        next: (response) => {
          this.toastr.success('Successfully registered!', 'Success');
          this.router.navigate(['/login']);
          this.registerForm.reset();
        },
        error: (err) => {
          console.error(`Register response error:`, err);
          this.toastr.success('Unsuccessly registrate!', 'Error try again');
        },
      });
    }
  }
}
