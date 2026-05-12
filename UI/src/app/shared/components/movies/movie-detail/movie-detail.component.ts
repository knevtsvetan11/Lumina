import { Component, inject, OnInit, signal } from '@angular/core';
import { Movie } from '../../../../models/movie.model';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router, RouterModule } from '@angular/router'; // 👉 добави Router тук
import { FormsModule } from '@angular/forms';
import { AuthService } from '../../../../core/services/auth.service';
import { WatchlistService } from '../../../../core/services/watchlist.service';
import { MovieService } from '../../../../core/services/movie.service';
import { isEmpty } from 'rxjs';

@Component({
  selector: 'app-movie-detail',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule],
  templateUrl: './movie-detail.component.html',
  styleUrls: ['./movie-detail.component.scss'],
})
export class MovieDetailComponent implements OnInit {
  movie = signal<Movie | null>(null);
  inWatchlist = signal<boolean>(false);

  route = inject(ActivatedRoute);
  movieService = inject(MovieService);
  router = inject(Router);
  authService = inject(AuthService);
  watchlistService = inject(WatchlistService);

  ngOnInit() {
    const id = this.route.snapshot.paramMap.get('id');
    if (id !== null) {
      this.movieService.getMovieDetails(id).subscribe((data) => {
        this.movie.set(data);
      });
    }
  }

  goBack() {
    this.router.navigate(['/movies']);
  }

  addToWatchlist(id: string) {
    let movieId = id;
    return this.watchlistService.addToWatchlist(movieId).subscribe({
      next: (res) => {
        if (res.inWatchlist) {
          this.inWatchlist.set(true);
          this.movieService.getMovieDetails(movieId);
          this.isInWatchlist(movieId);
        }
      },
      error: (err) => {
        console.log('Error to added in watchlist.');
      },
    });
  }

  removeFromWatchlist(id: string) {
    let movieId = id;
    return this.watchlistService.removeFromWatchlist(movieId).subscribe({
      next: (res) => {
        if (res.isDeleted) {
          this.inWatchlist.set(false);
          this.movieService.getAll();
        }
      },
      error: (err) => {
        console.error('Error removing movie from watchlist.', err);
      },
    });
  }

  isInWatchlist(id: string) {
    this.watchlistService.isInWatchlist(id).subscribe((res) => {
      this.inWatchlist.set(res.isExist);
    });
  }
}
