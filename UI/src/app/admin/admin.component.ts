import { CommonModule } from '@angular/common';
import { Component, inject, OnInit, signal } from '@angular/core';
import { Router, RouterModule } from '@angular/router';
import { MovieService } from '../core/services/movie.service';
import { CinemaService } from '../core/services/cinema.service';
import { ScreeningService } from '../core/services/screening.service';
import { MatButton } from '@angular/material/button';
import { UserService } from '../core/services/user.service';
import { subscribeOn } from 'rxjs';
import { MatIcon, MatIconModule } from '@angular/material/icon';

@Component({
  selector: 'app-admin-component',
  standalone: true,
  imports: [CommonModule, RouterModule, MatButton, MatIcon, MatIconModule],
  templateUrl: './admin.component.html',
  styleUrl: './admin.component.scss',
})
export class AdminComponent implements OnInit {
  movieService = inject(MovieService);
  cinemaService = inject(CinemaService);
  screeningService = inject(ScreeningService);
  userService = inject(UserService);

  moviesCount = signal<number>(0);
  cinemasCount = signal<number>(0);
  screeningsCount = signal<number>(0);
  usersCount = signal<number>(0);

  ngOnInit(): void {
    this.getMoviesCount();
    this.getCinemaCount();
    this.getScreeningsCount();
    this.getUsersCount();
  }

  getUsersCount() {
    this.userService.getUsersCount().subscribe((res) => {
      if (res) {
        this.usersCount.set(res);
      }
    });
  }

  getMoviesCount() {
    this.movieService.getMoviesCount().subscribe((res) => {
      if (res != null) {
        this.moviesCount.set(res);
      }
    });
  }

  getCinemaCount() {
    this.cinemaService.getCinemasCount().subscribe((res) => {
      if (res != null) {
        this.cinemasCount.set(res);
      }
    });
  }

  getScreeningsCount() {
    this.screeningService.getScreeningsCount().subscribe((res) => {
      if (res != null) {
        this.screeningsCount.set(res);
      }
    });
  }
}
