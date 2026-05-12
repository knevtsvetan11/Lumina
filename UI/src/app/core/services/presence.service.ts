import { inject, Injectable, NgZone, signal } from '@angular/core';
import * as signalR from '@microsoft/signalr';

@Injectable({
  providedIn: 'root',
})
export class PresenceHubService {
  private hubConnection: signalR.HubConnection | undefined;
  public usersOnline = signal<string[]>([]);
  private ngZone = inject(NgZone);

  public startConnection(): void {
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl('https://localhost:7093/presenceHub', {
        accessTokenFactory: () => {
          const token = sessionStorage.getItem('authToken');
          return token ? token : '';
        },
      })
      .withAutomaticReconnect()
      .build();

    this.hubConnection.on('UserIsOnline', (userId) => {
      this.usersOnline.update((users) => [users, ...userId]);
    });

    this.hubConnection.on('GetOnlineUsers', (newUsersOnline: string[]) => {
      this.ngZone.run(() => {
        this.usersOnline.set(newUsersOnline);
      });
    });

    this.hubConnection.on('UserIsOffline', (userId: string) => {
      this.ngZone.run(() => {
        this.usersOnline.update((currentUsers) => currentUsers.filter((id) => id !== userId));
      });
    });

    this.hubConnection
      .start()
      .then(() => console.log('PresenceHubService starting connection to bekend hub.'))
      .catch(() => console.error('PresenceHubService can`t starting connection to bekend hub.'));
  }

  stopConnection() {
    this.hubConnection?.stop();
  }
}
