import {
  AfterViewInit,
  Component,
  computed,
  inject,
  OnInit,
  signal,
  ViewChild,
  viewChild,
} from '@angular/core';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { CommonModule } from '@angular/common';
import { MatDialog } from '@angular/material/dialog';
import { MovieDialogComponent } from './movie-dialog/movie-dialog.component';
import { MovieService } from '../../../core/services/movie.service';
import { Movie } from '../../../models/movie.model';
import { AuthService } from '../../../core/services/auth.service';
import { MatButton } from '@angular/material/button';
import { MatToolbar } from '@angular/material/toolbar';
import { MatPaginator, MatPaginatorModule, PageEvent } from '@angular/material/paginator';
import { MatInputModule } from '@angular/material/input';
import { MatIcon } from '@angular/material/icon';
import { ToastrService } from 'ngx-toastr';
import { switchMap } from 'rxjs';
import { ConfirmDialog } from '../../confirm-dialog/confirm-dialog';

@Component({
  selector: 'app-movie-component',
  standalone: true,
  imports: [RouterLink, MatButton, MatToolbar, MatPaginatorModule, MatInputModule, MatIcon],
  templateUrl: './movie.component.html',
  styleUrl: './movie.component.scss',
})
export class MovieComponent implements OnInit {
  movies = signal<Movie[]>([]);
  pageIndex = signal(0);
  pageSize = signal(5);
  searchQuery = signal('');

  movieService = inject(MovieService);
  route = inject(ActivatedRoute);
  dialog = inject(MatDialog);
  authService = inject(AuthService);
  toastr = inject(ToastrService);

  @ViewChild(MatPaginator) paginator!: MatPaginator;

  ngOnInit(): void {
    this.getAll();
  }

  filteredMovies = computed(() => {
    const query = this.searchQuery().toLowerCase().trim();
    let all = this.movies();
    return all.filter((m) => m.title.toLowerCase().includes(query));
  });

  displayedMovies = computed(() => {
    let start = this.pageIndex() * this.pageSize();
    let end = start + this.pageSize();
    return this.filteredMovies().slice(start, end);
  });

  onSearch(event: Event) {
    let inputElement = event.target as HTMLInputElement;
    let value = inputElement.value;
    this.searchQuery.set(value);
    this.pageIndex.set(0);
  }

  onPageChange(event: PageEvent) {
    this.pageIndex.set(event.pageIndex);
    this.pageSize.set(event.pageSize);
  }

  getAll() {
    this.movieService.getAll().subscribe((data) => {
      this.movies.set(data);
    });
  }

  openCreateDialog() {
    this.dialog
      .open(MovieDialogComponent, { data: { mode: 'create' } })
      .afterClosed()
      .pipe(switchMap((movieData) => this.movieService.create(movieData)))
      .subscribe({
        next: (data) => {
          this.toastr.success(`Succsesfully created new movie.`);
        },
      });
  }

  openUpdateDialog(movie: Movie) {
    const id = movie.id;
    this.dialog
      .open(MovieDialogComponent, { data: { mode: 'edit', movie } })
      .afterClosed()
      .pipe(switchMap((movieData) => this.movieService.update(id, movieData)))
      .subscribe({
        next: (res) => this.toastr.success(`Movie is succeslly updated!`),
        error: (err) => this.toastr.error(`Movie is not updated!`),
      });
  }

  openDeleteDialog(movie: Movie) {
    const id = movie.id;
    this.dialog
      .open(ConfirmDialog, {
        data: { title: 'Movie', message: `Do you want to delete ${movie.title}?` },
      })
      .afterClosed()
      .pipe(switchMap((result) => this.movieService.delete(id)))
      .subscribe({
        next: (res) => {
          this.toastr.success('Movie deleted');
        },
        error: (er) => {
          this.toastr.error('Error to deleted');
        },
      });
  }
}
