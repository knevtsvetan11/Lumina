import { HttpClient } from '@angular/common/http';
import { inject, Injectable, signal } from '@angular/core';
import { Observable, retryWhen } from 'rxjs';
import { Movie } from '../../models/movie.model';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root',
})
export class MovieService {
  private apiUrl = environment.apiUrl + `/movies`;
  http = inject(HttpClient);

  getMoviesCount(): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/count`);
  }

  getAll(): Observable<Movie[]> {
    return this.http.get<Movie[]>(`${this.apiUrl}`);
  }

  create(movie: Movie): Observable<Movie> {
    return this.http.post<Movie>(`${this.apiUrl}/create`, movie);
  }

  update(id: string, movie: any): Observable<any> {
    return this.http.patch(`${this.apiUrl}/${id}`, movie);
  }

  getMovieDetails(id: string): Observable<Movie> {
    return this.http.get<Movie>(`${this.apiUrl}/${id}`);
  }

  delete(id: string): Observable<any> {
    return this.http.delete(`${this.apiUrl}/${id}`);
  }
}
