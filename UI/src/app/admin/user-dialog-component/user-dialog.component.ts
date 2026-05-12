import { Component, inject, OnInit } from '@angular/core';
import {
  AbstractControl,
  FormControl,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { MatButton } from '@angular/material/button';
import { MAT_DIALOG_DATA, MatDialogRef, MatDialogTitle } from '@angular/material/dialog';
import { MatFormFieldControl, MatFormFieldModule } from '@angular/material/form-field';
import { MatInput } from '@angular/material/input';
import { UserService } from '../../core/services/user.service';
import { CheckEmailResponse } from '../../models/checkEmailResponse.dto';
import { map } from 'rxjs';
import { MatOption, MatSelect } from '@angular/material/select';

@Component({
  selector: 'app-user-dialog-component',
  standalone: true,
  imports: [
    MatButton,
    MatInput,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatDialogTitle,
    MatOption,
    MatSelect,
  ],
  templateUrl: './user-dialog.component.html',
  styleUrl: './user-dialog.component.scss',
})
export class UserDialogComponent implements OnInit {
  form = new FormGroup({
    // username ?!
    email: new FormControl('', {
      nonNullable: true,
      validators: [Validators.required, Validators.email],
      asyncValidators: [this.emailExistValidator.bind(this)],
      updateOn: 'blur',
    }),
    password: new FormControl('', {
      nonNullable: true,
      validators: [
        Validators.minLength(6),
        Validators.maxLength(20),
        this.passwordValidator.bind(this),
      ],
    }),
    role: new FormControl('', {
      validators: [Validators.required],
    }),
  });

  userService = inject(UserService);
  dialogRef = inject(MatDialogRef<UserDialogComponent>);
  data = inject(MAT_DIALOG_DATA);

  ngOnInit(): void {
    if (this.data.isEditMode) this.form.patchValue(this.data);
  }

  passwordValidator(control: AbstractControl) {
    let value = control.value as string;
    if (!value) return null;

    const hasUpperCase = /[A-Z]/.test(value);
    const hasSymbol = /[!@#$%^&*(),.?":{}|<>]/.test(value);
    return hasSymbol && hasUpperCase ? null : { passwordStrength: true };
  }

  emailExistValidator(control: AbstractControl) {
    return this.userService.checkEmail(control.value).pipe(
      map((res: CheckEmailResponse) => {
        return res.exist ? { emailIsTaken: true } : null;
      }),
    );
  }

  submit() {
    this.dialogRef.close(this.form.value);
  }

  cancel() {
    this.dialogRef.close(this.form.value);
  }
}
