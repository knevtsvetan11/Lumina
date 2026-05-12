import {
  Component,
  EventEmitter,
  Inject,
  inject,
  Injectable,
  Input,
  OnInit,
  Output,
  signal,
} from '@angular/core';
import { FormsModule } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { MatOption, MatSelectModule } from '@angular/material/select';
import { MatFormField } from '@angular/material/form-field';
import { MatOptionModule } from '@angular/material/core';
import { ScreeningService } from '../../../../core/services/screening.service';
@Component({
  selector: 'app-buy-modal',
  imports: [FormsModule, MatSelectModule],
  standalone: true,
  templateUrl: './buy-tickets-dialog.component.html',
  styleUrl: './buy-tickets-dialog.component.scss',
})
export class BuyTicketsComponent implements OnInit {
  @Input() screening: any;

  ticketsCount = signal<number>(1);
  selectedShowtime = signal<string | null>(null);
  showtimes = signal<string[]>([]); // use signal

  constructor(
    private screeningService: ScreeningService,
    @Inject(MAT_DIALOG_DATA) public data: { cinemaId: string; movieId: string },
    private dialogRef: MatDialogRef<BuyTicketsComponent>,
  ) {}
  ngOnInit(): void {}

  loadShowtimes(cinemaId: string, movieId: string) {
    this.screeningService.getShowtime(cinemaId, movieId).subscribe({
      next: (dto) => this.showtimes.set(dto.showtime),
      error: (err) => console.error('Error to load showtimes', err),
    });
  }

  confirm() {
    this.dialogRef.close({
      tickets: this.ticketsCount,
    });
  }

  close() {
    this.dialogRef.close();
  }
}
