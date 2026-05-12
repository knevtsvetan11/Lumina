import { inject, Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Permission } from '../../models/permission.model';
import { UserPermissionDto } from '../../models/user-permissions.dto';
import { UserPermissionUpdateDto } from '../../models/user-permissions-update.dto';

@Injectable({
  providedIn: 'root',
})
export class PermissionService {
  url = environment.apiUrl + `/permissions`;

  http = inject(HttpClient);

  saveUserPermissions(data: any): Observable<any> {
    return this.http.patch(`${this.url}/update`, data);
  }

  getAllPermissions(): Observable<string[]> {
    return this.http.get<string[]>(`${this.url}`);
  }

  getUserPermissions(id: string): Observable<UserPermissionDto[]> {
    return this.http.get<UserPermissionDto[]>(`${this.url}/user/${id}`);
  }

  getPermissionsByRoleId(id: string): Observable<string[]> {
    return this.http.get<string[]>(`${this.url}/${id}`);
  }

  update(roleId: string, newPermissions: string[]): Observable<void> {
    return this.http.patch<void>(`${this.url}/${roleId}`, newPermissions);
  }
}
