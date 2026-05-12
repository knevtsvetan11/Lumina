import { inject, Injectable, NgZone, signal } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { environment } from '../../../environments/environment';
import { Observable } from 'rxjs';
import { Message } from '../../models/message.model';
import { HttpClient } from '@angular/common/http';
import { MessageRequest } from '../../models/message.request';
import { AuthService } from './auth.service';

@Injectable({
  providedIn: 'root',
})
export class ChatSignlrService {
  private http = inject(HttpClient);
  private url = environment.apiUrl + `/messages`;
  private hubConnection: signalR.HubConnection | undefined;
  private ngZone = inject(NgZone);
  private authService = inject(AuthService);
  public messages = signal<Message[]>([]);
  public activeMessages = signal<Message[]>([]);

  private readonly audioPath = 'assets/mixkit-correct-answer-tone-2870.wav';

  private notificationSound = new Audio(this.audioPath);

  constructor() {
    this.notificationSound.load();
  }

  public initHub(): void {
    if (!this.authService.isLoggedIn()) return;

    if (
      this.hubConnection &&
      this.hubConnection.state !== signalR.HubConnectionState.Disconnected
    ) {
      return;
    }
    this.startUserConnection();
  }

  private playIncomingSound() {
    this.notificationSound.currentTime = 0;
    this.notificationSound.volume = 0.5;
    this.notificationSound.play().catch((err) => console.error('Audio blocked by browser'));
  }

  startUserConnection(): void {
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl('https://localhost:7093/supportChatHub', {
        accessTokenFactory: () => {
          const token = sessionStorage.getItem('authToken');
          return token ? token : '';
        },
      })
      .withAutomaticReconnect()
      .build();

    this.hubConnection?.on('ReceiveMessage', (newMessage: Message) => {
      this.ngZone.run(() => {
        
        this.messages.update((prev) => [...prev, newMessage]);
      
        if (this.activeMessages.length === 0) this.loadActiveMessages();
        this.activeMessages.update((prevList) => {
          const index = prevList.findIndex((x) => x.senderId === newMessage.senderId);

          if (index !== -1) {
            const newList = [...prevList];
            newList[index] = {
              ...newList[index],
              message: newMessage.message,
              sentAt: newMessage.sentAt,
            };

            const updatedUser = newList.splice(index, 1)[0];
            return [updatedUser, ...newList];
          }
          this.playIncomingSound();
          return prevList;
        });
      });
    });

    this.hubConnection.start().then(() => {
    });
  }

  public stopConnection(): void {
    if (this.hubConnection) {
      this.hubConnection
        .stop()
        .then(() => {
          this.hubConnection = undefined;
        })
        .catch((err) => console.error('Error while stopping connection: ', err));
    }
  }

  joinRoom(userId: string) {
    if (userId !== null) {
      this.hubConnection
        ?.invoke('JoinRoom', userId)
        .then(() => console.log('Now we are inside the room'))
        .catch((err) => console.error('Error to get inside in room: ', err));
    }
  }

  sendMessage(request: MessageRequest): Observable<any> {
    return this.http.post(`${this.url}/message`, request);
  }

  loadChatHistory(groupId: string, oldestMessageDateTime?: string): Observable<Message[]> {
    return this.http.get<Message[]>(`${this.url}/history/${groupId}/${oldestMessageDateTime}`);
  }

  loadActiveMessages(): void {
    this.http.get<Message[]>(`https://localhost:7093/api/admin/chats/active/messages`).subscribe({
      next: (data) => {
        this.activeMessages.set(data);
      },
      error: (err) => {
        console.error(`Error : ${err}`);
      },
    });
  }
}
