import { Inject, inject, Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { Role } from '../../models/role.modes';

@Injectable({
  providedIn: 'root',
})
export class RoleService {
  url = environment.apiUrl + `/role/manage`;
  http = inject(HttpClient);

  getAllRoles(): Observable<Role[]> {
    return this.http.get<Role[]>(`${this.url}`);
  }

  createRole(role: string): Observable<Role> {
    return this.http.post<Role>(`${this.url}`, role);
  }

  delete(id: string): Observable<void> {
    return this.http.delete<void>(`${this.url}/${id}`);
  }
}
