import { inject, Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { ToastrService } from 'ngx-toastr';

@Injectable({
  providedIn: 'root',
})
export class SignalrService {
  private hubConnection: signalR.HubConnection | undefined;

  toastr = inject(ToastrService);
  public startConnection(): void {
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl('https://localhost:7093/notificationHub')
      .withAutomaticReconnect()
      .build();

    this.hubConnection.on('MovieCreated', (movie) => {
      this.toastr.info(`${movie.title} is now available!`, 'New Movie!', {
        tapToDismiss: true,
        newestOnTop: true,
      });
    });

    this.hubConnection.on('MovieUpdated', (movie) => {
      this.toastr.info(`${movie.title} is now Updated!`, 'Updated Movie!', {
        tapToDismiss: true,
        newestOnTop: true,
      });
    });

    this.hubConnection.on('MovieDeleted', (movie) => {
      this.toastr.info(`${movie.title} is now Deleted!`, 'Deleted Movie!', {
        tapToDismiss: true,
        newestOnTop: true,
      });
    });

    this.hubConnection.on('CinemaCreated', (cinema) => {
      this.toastr.info(`${cinema.name} is now available!`, 'New Cinema!', {
        tapToDismiss: true,
        newestOnTop: true,
      });
    });

    this.hubConnection.on('CinemaUpdated', (cinema) => {
      this.toastr.info(`${cinema.name} is now updated!`, 'Updated Cinema!', {
        tapToDismiss: true,
        newestOnTop: true,
      });
    });

    this.hubConnection.on('CinemaDeleted', (cinema) => {
      this.toastr.info(`Cinema: ${cinema.name} is deleted.`, 'Deleted Cinema!', {
        tapToDismiss: true,
        newestOnTop: true,
      });
    });

    this.hubConnection
      .start()
      .then(() => console.log('Successfully Connected!'))
      .catch((err) => console.error('Connection Failed: ', err));
  }
}
