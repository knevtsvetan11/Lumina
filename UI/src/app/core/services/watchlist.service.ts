import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Watchlist } from '../../models/watchlist.model';
import { Movie } from '../../models/movie.model';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root',
})
export class WatchlistService {
  url = environment.apiUrl + `/watchlists`;

  http = inject(HttpClient);

  addToWatchlist(movieId: string): Observable<{ inWatchlist: boolean }> {
    return this.http.post<{ inWatchlist: boolean }>(`${this.url}/${movieId}`, {});
  }

  showWatchlist(): Observable<Watchlist[]> {
    return this.http.get<Watchlist[]>(`${this.url}/ShowWatchlist`);
  }

  removeFromWatchlist(movieId: string): Observable<{ isDeleted: boolean }> {
    return this.http.delete<{ isDeleted: boolean }>(`${this.url}/${movieId}`);
  }

  isInWatchlist(id: string): Observable<{ isExist: boolean }> {
    let movieId = id;
    return this.http.get<{ isExist: boolean }>(
      `${this.url}/IsMovieExistInWatchlist?movieId=${movieId}`,
    );
  }
}
