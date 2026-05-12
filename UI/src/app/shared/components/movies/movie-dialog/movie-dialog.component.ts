import { CommonModule } from '@angular/common';
import { Component, Inject, inject, OnInit, signal } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialog, MatDialogRef } from '@angular/material/dialog';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { FormBuilder, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { Movie } from '../../../../models/movie.model';
import { Title } from '@angular/platform-browser';

@Component({
  selector: 'app-movie-dialog.component',
  standalone: true,
  imports: [
    CommonModule,
    MatButtonModule,
    MatFormFieldModule,
    MatInputModule,
    FormsModule,
    ReactiveFormsModule,
  ],
  templateUrl: './movie-dialog.component.html',
  styleUrl: './movie-dialog.component.scss',
})
export class MovieDialogComponent implements OnInit {
  private fb = inject(FormBuilder);
  mode = '';

  form = this.fb.group({
    title: [''],
    genre: [''],
    releaseDate: [''],
    director: [''],
    duration: [0],
    description: [''],
    imageUrl: [''],
  });

  constructor(
    public dialogRef: MatDialogRef<MovieDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any,
  ) {
    this.mode = data;
  }

  ngOnInit(): void {
    if (this.data.mode == 'edit') {
      this.form.patchValue(this.data.movie);
    }
  }

  onSubmit() {
    if (this.form.valid) this.dialogRef.close(this.form.value);
  }
}
