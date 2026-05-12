import { HttpClient } from '@angular/common/http';
import { Login } from '../../models/login.model';
import { Observable } from 'rxjs';
import { booleanAttribute, inject, Injectable, OnInit, signal } from '@angular/core';
import { Token } from '@angular/compiler';
import { Register } from '../../models/register.model';
import { jwtDecode } from 'jwt-decode';
import { Router } from '@angular/router';
import { environment } from '../../../environments/environment';
import { LoginResponse } from '../../models/login.response';

@Injectable({
  providedIn: 'root',
})
export class AuthService implements OnInit {
  private apiUrl = environment.apiUrl + `/auth`;
  http = inject(HttpClient);
  router = inject(Router);
  public userRole = signal<string>('');

  ngOnInit(): void {
    this.getRole();
  }

  getRole() {
    const role = sessionStorage.getItem('userRole');
    if (role !== null) this.userRole.set(role);
  }

  isLoggedIn(): boolean {
    let token = sessionStorage.getItem('authToken');
    if (token) {
      return true;
    }
    return false;
  }

  isAdmin() {
    const role = sessionStorage.getItem('userRole');
    if (role === 'Admin') return true;
    else return false;
  }

  getUserId() {
    return sessionStorage.getItem('userId');
  }

  log(loginRequest: any): Observable<LoginResponse> {
    return this.http.post<LoginResponse>(`${this.apiUrl}/login`, loginRequest);
  }

  logout() {
    let token = sessionStorage.getItem('authToken');
    if (token) {
    }
    sessionStorage.clear();
    this.router.navigate(['/login']);
  }

  register(formRequest: any): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/register`, formRequest);
  }
}
