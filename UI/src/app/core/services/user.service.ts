import { inject, Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient, HttpParams } from '@angular/common/http';
import { MatDialog } from '@angular/material/dialog';
import { Observable } from 'rxjs';
import { User } from '../../models/user.model';
import { CheckEmailResponse } from '../../models/checkEmailResponse.dto';
import { CreateUserDto } from '../../models/createUser.dto';
import { jwtDecode } from 'jwt-decode';
import { ReactiveFormsModule } from '@angular/forms';
import { UserSearchRequest } from '../../models/user.search.request';

@Injectable({
  providedIn: 'root',
})
export class UserService {
  url = environment.apiUrl + `/users`;
  http = inject(HttpClient);
  dialog = inject(MatDialog);

  getUsersCount(): Observable<number> {
    return this.http.get<number>(`${this.url}/count`);
  }

  toggleIsActiveUser(user: User): Observable<boolean> {
    return this.http.patch<boolean>(`${this.url}/${user.id}`, {});
  }

  checkEmail(email: string): Observable<CheckEmailResponse> {
    return this.http.get<CheckEmailResponse>(`${this.url}/${email}`);
  }

  getAll(): Observable<User[]> {
    return this.http.get<User[]>(`${this.url}`);
  }

  create(data: CreateUserDto): Observable<CreateUserDto> {
    return this.http.post<CreateUserDto>(`${this.url}/`, data);
  }

  edit() {}

  delete(user: User): Observable<any> {
    let id = user.id;
    return this.http.delete<any>(`${this.url}/${id}`);
  }

  getUsersPaged(req: UserSearchRequest): Observable<User[]> {
    const params = new HttpParams()
      .set('pageIndex', req.pageIndex.toString())
      .set('pageSize', req.pageSize.toString())
      .set('searchData', req.searchData ?? '')
      .set('filterColumn', req.filterColumn ?? '');
    return this.http.get<User[]>(`${this.url}/paged`, { params });
  }
}
