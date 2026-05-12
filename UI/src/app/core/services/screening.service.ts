import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { inject, Injectable } from '@angular/core';
import { Screening } from '../../models/screening.model';
import { ScreeningShowtimeDto } from '../../models/screening-showtime.dto';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root',
})
export class ScreeningService {
  private apiUrl = environment.apiUrl + `/screenings`;
  http = inject(HttpClient);

  delete(id: string): Observable<any> {
    return this.http.delete(`${this.apiUrl}/${id}`);
  }

  edit(screning: any): Observable<any> {
    return this.http.put(`${this.apiUrl}/${screning.id}`, screning);
  }

  create(screening: any): Observable<any> {
    return this.http.post(`${this.apiUrl}`, screening);
  }

  getScreeningsCount(): Observable<any> {
    return this.http.get(`${this.apiUrl}`);
  }

  getCinemaScreenings(id: string): Observable<Screening[]> {
    return this.http.get<Screening[]>(`${this.apiUrl}/cinema/${id}/screenings`);
  }

  getShowtime(cinemaId: string, movieId: string): Observable<ScreeningShowtimeDto> {
    return this.http.get<ScreeningShowtimeDto>(`${this.apiUrl}/${cinemaId}/${movieId}`);
  }
}
