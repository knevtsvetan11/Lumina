import { Component, Inject, inject, OnInit, signal } from '@angular/core';

import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { Screening } from '../../../models/screening.model';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { ConstantPool } from '@angular/compiler';
import { MatSnackBar } from '@angular/material/snack-bar';
import { TicketService } from '../../../core/services/ticket.service';
import { BuyTicketsComponent } from '../tickets/tickets-dialog/buy-tickets-dialog.component';
import { ScreeningService } from '../../../core/services/screening.service';
import { AuthService } from '../../../core/services/auth.service';
import { MatToolbar } from '@angular/material/toolbar';
import { MatButton } from '@angular/material/button';
import { ScreeningDialogComponent } from './screening-dialog/screening-dialog.component';
import { ConfirmDialog } from '../../confirm-dialog/confirm-dialog';
@Component({
  selector: 'app-screening-component',
  standalone: true,
  imports: [RouterModule, MatToolbar, MatButton],
  templateUrl: './screening.component.html',
  styleUrl: './screening.component.scss',
})
export class ScreeningComponent implements OnInit {
  screenings = signal<Screening[]>([]);
  cinemaName = '';
  screeningService = inject(ScreeningService);
  ticketService = inject(TicketService);
  route = inject(ActivatedRoute);
  dialog = inject(MatDialog);
  snackBar = inject(MatSnackBar);
  authService = inject(AuthService);

  selectedOption: any = null;
  handleBuy: any;

  ngOnInit(): void {
    const cinemaId = this.route.snapshot.paramMap.get('id');

    this.getCinemaScreenings(cinemaId);
  }

  openDeleteDialog(screening: Screening) {
    const cinemaId = screening.cinemaId;
    const dialogRef = this.dialog.open(ConfirmDialog, {
      data: { message: 'Do you want to delete screening', title: 'Delete Screening' },
    });

    dialogRef.afterClosed().subscribe((data) => {
      if (data) {
        this.screeningService.delete(screening.id).subscribe({
          next: (res) => {
            this.screeningService
              .getCinemaScreenings(cinemaId)
              .subscribe((res) => this.screenings.set(res));
          },
        });
      }
    });
  }

  openCreateDialog() {
    const dialogRef = this.dialog.open(ScreeningDialogComponent);

    dialogRef.afterClosed().subscribe({
      next: (data) => {
        this.screeningService.create(data).subscribe({
          next: (res) => {
            alert('New screening is successfully created.');
          },
          error: (err) => {
            console.error(err);
          },
        });
      },
      error: (err) => {
        console.error(err);
      },
    });
  }

  openEditDialog(screening: Screening) {
    const cinemaId = screening.cinemaId;
    const dialogRef = this.dialog.open(ScreeningDialogComponent, {
      data: {
        id: screening.id,
        cinemaId: screening.cinemaId,
        movieId: screening.movieId,
        availableTickets: screening.availableTickets,
        showtime: screening.showtime,
      },
    });

    dialogRef.afterClosed().subscribe((data) => {
      if (data) {
        this.screeningService.edit(data).subscribe({
          next: (res) => {
            this.screeningService.getCinemaScreenings(cinemaId).subscribe((data) => {
              if (data) {
                this.screenings.set(data);
              }
            });
          },
          error: (err) => console.error(err),
        });
      }
    });
  }

  openBuyDialog(screening: Screening) {
    let screeningId = screening.id;
    let cinemaId = screening.cinemaId;
    let movieId = screening.movieId;

    const dialogRef = this.dialog.open(BuyTicketsComponent, {
      data: {
        cinemaId,
        movieId,
      },
    });

    dialogRef.afterClosed().subscribe((result) => {
      if (!result) return;

      this.ticketService.buyTicket(screeningId, result.tickets()).subscribe((success) => {
        if (success) {
          this.snackBar.open('Successfully buy ticket!');
        }
      });
    });
  }

  getCinemaScreenings(id: any) {
    this.screeningService.getCinemaScreenings(id).subscribe((data) => {
      this.screenings.set(data);
    });
  }
}
