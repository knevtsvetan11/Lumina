import { Component, Inject, inject, OnInit } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { SignalrService } from './core/services/signalr.service';
import { ChatComponent } from './shared/components/chat-component/chat.component';
import { AuthService } from './core/services/auth.service';
import { PresenceHubService } from './core/services/presence.service';
import { ChatSignlrService } from './core/services/chat-signalr.service';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet,ChatComponent], 
  template: `
    <h1>Cinema</h1>
    <router-outlet></router-outlet>
    <app-chat>
  `

})
export class App implements OnInit {

 private precenseHubService = inject(PresenceHubService) // SignalR
 private signalR = inject(SignalrService)
 private chatSupportHubService = inject(ChatSignlrService)
 authService = inject(AuthService)

  ngOnInit(): void {

    this.chatSupportHubService.initHub();
    if(this.authService.isLoggedIn()){
    this.signalR.startConnection();
    this.precenseHubService.startConnection();
    }
  }

 

}
