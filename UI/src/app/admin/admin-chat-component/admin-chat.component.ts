import { Component, inject, OnInit, signal } from '@angular/core';
import { MatCardActions, MatCardModule } from '@angular/material/card';
import { RouterModule } from '@angular/router';
import { Message } from '../../models/message.model';
import { ChatSignlrService } from '../../core/services/chat-signalr.service';
import { MatIcon, MatIconModule } from '@angular/material/icon';
import { MatButton } from '@angular/material/button';
import { MatInput, MatInputModule } from '@angular/material/input';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MessageRequest } from '../../models/message.request';
import { DatePipe } from '@angular/common';
import { MatProgressSpinner } from '@angular/material/progress-spinner';
import { PresenceHubService } from '../../core/services/presence.service';

@Component({
  selector: 'app-admin.chat-component',
  standalone: true,
  imports: [
    RouterModule,
    MatCardModule,
    MatIconModule,
    MatIcon,
    MatButton,
    MatCardActions,
    ReactiveFormsModule,
    MatInputModule,
    DatePipe,
    MatProgressSpinner,
  ],
  templateUrl: './admin-chat.component.html',
  styleUrl: './admin-chat.component.scss',
})
export class AdminChatComponent implements OnInit {
  chatSignalrService = inject(ChatSignlrService);
  presenceHubService = inject(PresenceHubService);

  public groupChatHistory = this.chatSignalrService.messages;
  public onlineUsers = this.presenceHubService.usersOnline;

  adminId = sessionStorage.getItem('userId') ?? '';
  receiverId = signal('');
  isFetching = signal<boolean>(false);

  chatForm = new FormGroup({
    text: new FormControl('', [Validators.max(1000), Validators.required, Validators.minLength(1)]),
  });

  ngOnInit(): void {
    this.chatSignalrService.loadActiveMessages();
  }

  onScroll(event: Event) {
    const element = event.target as HTMLElement;
    let oldestMessageDateTime = this.groupChatHistory()[0];
    const elementScrollValue = element.scrollTop;

    if (elementScrollValue < 1 && !this.isFetching()) {
      this.isFetching.set(true);

      this.chatSignalrService
        .loadChatHistory(this.receiverId(), oldestMessageDateTime.sentAt)
        .subscribe({
          next: (moreMesseges) => {
            this.groupChatHistory.update((prev) => [...moreMesseges, ...prev]);
            this.isFetching.set(false);
          },
          error: (err) => {
            console.error(err);
          },
        });
    }
  }

  loadChatHistory(msg: Message) {
    this.chatSignalrService.joinRoom(msg.chatGroupId);
    this.receiverId.set(msg.chatGroupId);

    this.chatSignalrService.loadChatHistory(msg.chatGroupId, msg.sentAt).subscribe({
      next: (data) => {
        this.groupChatHistory.set(data);
      },
      error: (err) => {
        console.error(`${err}`);
      },
    });
  }

  sendMessage() {
    if (this.chatForm.valid) {
      const request: MessageRequest = {
        groupId: this.receiverId(),
        text: this.chatForm.value.text ?? '',
      };
      if (this.chatForm.valid) {
        this.chatSignalrService.sendMessage(request).subscribe({
          next: () => {
            (this.chatForm.reset(), 'Success send message to server.');
          },
          error: () => {
            'Error to send message!';
          },
        });
      }
    }
  }
}
