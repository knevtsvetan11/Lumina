import { Component, Inject, input, OnInit } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { MatFormField, MatFormFieldModule } from '@angular/material/form-field';

@Component({
  selector: 'app-cinema-dialog-component',
  imports: [ReactiveFormsModule, MatFormFieldModule, MatFormField],
  templateUrl: './cinema-dialog-component.html',
  styleUrl: './cinema-dialog-component.scss',
})
export class CinemaDialogComponent implements OnInit {
  ///Create & Edit Cinema

  form = new FormGroup({
    id: new FormControl(''),
    name: new FormControl('', Validators.required),
    location: new FormControl('', Validators.required),
  });

  constructor(
    @Inject(MAT_DIALOG_DATA) public data: any,
    private dialogRef: MatDialogRef<CinemaDialogComponent>,
  ) {}

  ngOnInit(): void {
    if (this.data.mode == 'edit') this.form.patchValue(this.data);
  }

  confirm() {
    if (this.form.valid) this.dialogRef.close(this.form.value);
  }

  cancel() {
    this.dialogRef.close(this.form.value);
  }
}
