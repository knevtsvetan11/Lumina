import { HttpClient } from '@angular/common/http';
import { Cinema } from '../../models/cinema.model';
import { Observable } from 'rxjs';
import { inject, Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root',
})
export class CinemaService {
  private urls = environment.apiUrl + `/cinemas`;
  http = inject(HttpClient);

  create(data: any): Observable<any> {
    return this.http.post(`${this.urls}`, data);
  }

  delete(id: string): Observable<any> {
    return this.http.delete(`${this.urls}/${id}`);
  }

  update(id: string, data: any): Observable<any> {
    return this.http.patch(`${this.urls}/${id}`, data);
  }

  getCinemasCount(): Observable<any> {
    return this.http.get(`${this.urls}/count`);
  }

  getAllCinemas(): Observable<any> {
    return this.http.get(`${this.urls}/getAll`);
  }
}
