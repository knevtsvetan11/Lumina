import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Ticket } from '../../models/ticket.model';
import { environment } from '../../../environments/environment';
import { observableToBeFn } from 'rxjs/internal/testing/TestScheduler';
@Injectable({
  providedIn: 'root',
})
export class TicketService {
  url = environment.apiUrl + `/tickets`;
  http = inject(HttpClient);

  buyTicket(screeningId: string, tickets: number): Observable<{ success: boolean }> {
    return this.http.post<{ success: boolean }>(`${this.url}/buy`, { screeningId, tickets });
  }

  getAll(): Observable<Ticket[]> {
    return this.http.get<Ticket[]>(`${this.url}/getAll`);
  }

  update(data: any): Observable<any> {
    return this.http.patch<any>(`${this.url}/update`, { data });
  }

  delete(id: string): Observable<any> {
    return this.http.delete(`${this.url}/delete/${id}`);
  }
}
