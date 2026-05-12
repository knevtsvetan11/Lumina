import { Component, inject, OnInit, signal } from '@angular/core';
import { Router, RouterModule } from '@angular/router';
import {
  FormControl,
  FormGroup,
  FormsModule,
  MaxLengthValidator,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { Login } from '../../models/login.model';
import { AuthService } from '../../core/services/auth.service';
import { ToastrService } from 'ngx-toastr';
import { RegularExpressionLiteral } from '@angular/compiler';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatProgressSpinner } from '@angular/material/progress-spinner';
import { PresenceHubService } from '../../core/services/presence.service';
import { ChatSignlrService } from '../../core/services/chat-signalr.service';

@Component({
  selector: 'app-login-component',
  standalone: true,
  imports: [
    RouterModule,
    FormsModule,
    ReactiveFormsModule,
    MatCardModule,
    MatButtonModule,
    MatProgressSpinner,
  ],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss',
})
export class LoginComponent {
  presenceHubService = inject(PresenceHubService);
  chatSupportHubService = inject(ChatSignlrService);

  authService = inject(AuthService);
  toastr = inject(ToastrService);

  router = inject(Router);
  isLoged = signal<boolean>(false);

  loginForm = new FormGroup({
    email: new FormControl('', [Validators.required, Validators.nullValidator, Validators.email]),
    password: new FormControl('', [
      Validators.required,
      Validators.nullValidator,
      Validators.minLength(14),
      Validators.maxLength(20),
    ]),
  });

  onLogin() {
    this.isLoged.set(true);

    if (this.loginForm.valid) {
      this.authService.log(this.loginForm.value).subscribe({
        next: (response) => {
          (this.loginForm.reset(),
            (sessionStorage.setItem('authToken', response.token),
            sessionStorage.setItem('userId', response.userId),
            sessionStorage.setItem('userRole', response.userRole),
            this.router.navigate(['/home']),
            this.toastr.success('Successfully loggin!')));
          this.presenceHubService.startConnection();
          this.chatSupportHubService.initHub();
        },
        error: (err) => {
          this.toastr.error('Incorrect pass or username!Try again.');
        },
      });
    }
  }
}
