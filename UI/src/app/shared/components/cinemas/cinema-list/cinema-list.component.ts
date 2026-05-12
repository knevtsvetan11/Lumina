import { Component, inject, OnInit, signal } from '@angular/core';
import { Router, RouterModule } from '@angular/router';
import { Cinema } from '../../../../models/cinema.model';
import { CinemaService } from '../../../../core/services/cinema.service';

import { Movie } from '../../../../models/movie.model';
import { ScreeningComponent } from '../../screenings/screening.component';
import { AuthService } from '../../../../core/services/auth.service';
import { ConfirmDialog } from '../../../confirm-dialog/confirm-dialog';
import { MatDialog } from '@angular/material/dialog';
import { Title } from '@angular/platform-browser';
import { MatButtonModule } from '@angular/material/button';
import { MatToolbar } from '@angular/material/toolbar';
import { CinemaDialogComponent } from '../cinema-dialog-component/cinema-dialog-component';
import { switchMap } from 'rxjs';
import { ToastrService } from 'ngx-toastr';
@Component({
  selector: 'app-cinema-list-component',
  standalone: true,
  imports: [RouterModule, MatButtonModule, MatToolbar],
  templateUrl: './cinema-list.component.html',
  styleUrl: './cinema-list.component.scss',
})
export class CinemaListComponent implements OnInit {
  cinemaService = inject(CinemaService);
  router = inject(Router);
  authService = inject(AuthService);
  dialog = inject(MatDialog);
  toastr = inject(ToastrService);

  cinemas = signal<Cinema[]>([]);

  ngOnInit(): void {
    this.getAllCinemas();
  }

  openDeleteDialog(cinema: Cinema) {
    const dialogRef = this.dialog.open(ConfirmDialog, {
      data: {
        title: 'Delete Cinema',
        message: `Do you want to delete "${cinema.name}"?`,
      },
    });

    dialogRef
      .afterClosed()
      .pipe(switchMap((result) => this.cinemaService.delete(cinema.id)))
      .subscribe({
        next: (res) => {
          this.toastr.success('Cinema is deleted.');
        },
        error: (err) => {
          this.toastr.error('Error to delete cinema');
        },
      });
  }

  getAllCinemas() {
    this.cinemaService.getAllCinemas().subscribe((data) => {
      this.cinemas.set(data);
    });
  }

  getScreenings(id: string) {
    let cinemaId = id;
    this.router.navigate(['/screenings', cinemaId]);
  }

  openCreateDialog() {
    const dialogRef = this.dialog
      .open(CinemaDialogComponent, { data: { mode: 'create' } })
      .afterClosed()
      .pipe(switchMap((newCinema) => this.cinemaService.create(newCinema)))
      .subscribe({
        next: (data) => {
          this.toastr.success('Cinema is created!');
        },
        error: (err) => {
          this.toastr.error('Error to create new cinema.');
        },
      });
  }

  openUpdateDialog(cinema: Cinema) {
    const dialogRef = this.dialog.open(CinemaDialogComponent, {
      data: {
        mode: 'edit',
        name: cinema.name,
        location: cinema.location,
        id: cinema.id,
      },
    });

    dialogRef
      .afterClosed()
      .pipe(switchMap((newCinema) => this.cinemaService.update(cinema.id, newCinema)))
      .subscribe({
        next: (res) => {
          this.toastr.success('Movie is updated.');
        },
        error: (err) => {
          this.toastr.error('Error to update cinema.');
        },
      });
  }
}
