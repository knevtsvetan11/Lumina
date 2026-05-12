import { Component, inject, signal } from '@angular/core';
import {
  FormControl,
  FormGroup,
  MaxLengthValidator,
  MaxValidator,
  ReactiveFormsModule,
  RequiredValidator,
  Validators,
} from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import {
  MatFormField,
  MatFormFieldControl,
  MatFormFieldModule,
  MatLabel,
} from '@angular/material/form-field';
import { MatInput, MatInputModule } from '@angular/material/input';
import { RoleCreateDto } from '../../../models/role-create.dto';

@Component({
  selector: 'app-create-role-dialog-component',
  standalone: true,
  imports: [MatDialogModule, MatFormFieldModule, MatFormField, ReactiveFormsModule],
  templateUrl: './create-role-dialog.component.html',
  styleUrl: './create-role-dialog.component.scss',
})
export class CreateRoleDialogComponent {
  dialogRef = inject(MatDialogRef<CreateRoleDialogComponent>);

  form = new FormGroup({
    roleName: new FormControl('', [
      Validators.required,
      Validators.minLength(4),
      Validators.maxLength(20),
    ]),
  });

  save() {
    if (this.form.valid) {
      const dto: RoleCreateDto = {
        roleName: this.form.value.roleName as string,
      };
      this.dialogRef.close(dto);
    }
  }

  cancel() {
    this.dialogRef.close();
  }
}
