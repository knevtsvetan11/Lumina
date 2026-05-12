
import { Component, inject, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { bootstrapApplication } from '@angular/platform-browser';
import { ActivatedRouteSnapshot, Router, RouterModule } from '@angular/router';
import { jwtDecode } from 'jwt-decode';
import { AuthService } from '../../../core/services/auth.service';
import { MatButton } from '@angular/material/button';
import { MatToolbar } from '@angular/material/toolbar';
import { MatSelect } from '@angular/material/select';
import { UserService } from '../../../core/services/user.service';
import { ReturnStatement, Token } from '@angular/compiler';
import { PresenceHubService } from '../../../core/services/presence.service';
import { ChatSignlrService } from '../../../core/services/chat-signalr.service';
@Component({
  selector: 'app-home-component',
  standalone: true,
  imports: [RouterModule, FormsModule,MatButton,MatToolbar],
  templateUrl: './home.component.html',
  styleUrl: './home.component.scss',
})
export class HomeComponent implements OnInit {
  options = ['Menu', 'cinemas', 'movies', 'tickets'];
  selectedOption = this.options[0];

  chatHubService = inject(ChatSignlrService)
  presenceHubService = inject(PresenceHubService)
  authService = inject(AuthService)
  router = inject(Router)
  userService = inject(UserService)

 ngOnInit(): void {

  }
  getEmail(){
  }

  isAdmin(){
    const role = sessionStorage.getItem('userRole')
    if (role == 'Admin')
      return  true
    return false
  }
  navigate(value: string) {
    this.router.navigate([`/${value}`]);
  }

  isLoggedIn(): boolean {
    return this.authService.isLoggedIn();
  }
  logout() {
   this.authService.logout();
   this.presenceHubService.stopConnection()
   this.chatHubService.stopConnection()
  }

}
