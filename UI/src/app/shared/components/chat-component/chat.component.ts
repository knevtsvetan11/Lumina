import {
  Component,
  effect,
  ElementRef,
  inject,
  OnInit,
  signal,
  viewChild,
  ViewChild,
} from '@angular/core';
import { ChatSignlrService } from '../../../core/services/chat-signalr.service';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { Message } from '../../../models/message.model';
import { ActivatedRoute, Scroll } from '@angular/router';
import { AuthService } from '../../../core/services/auth.service';
import { MatIcon } from '@angular/material/icon';
import { DatePipe } from '@angular/common';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import {
  FormControl,
  FormGroup,
  MaxLengthValidator,
  MaxValidator,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { MessageRequest } from '../../../models/message.request';

@Component({
  selector: 'app-chat',
  standalone: true,
  imports: [
    MatButtonModule,
    MatCardModule,
    MatIcon,
    DatePipe,
    ReactiveFormsModule,
    MatProgressSpinnerModule,
  ],
  templateUrl: './chat.component.html',
  styleUrl: './chat.component.scss',
})
export class ChatComponent implements OnInit {
  chatSignlrService = inject(ChatSignlrService);
  public authService = inject(AuthService);
  route = inject(ActivatedRoute);

  isOpen = signal<boolean>(false);
  public messages = this.chatSignlrService.messages;
  userId = signal<string>('');
  isFetching = signal<boolean>(false);
  @ViewChild('scrollMe') private myScrollContainer!: ElementRef;

  chatForm = new FormGroup({
    message: new FormControl('', [
      Validators.required,
      Validators.maxLength(1000),
      Validators.minLength(1),
    ]),
  });

  ngOnInit(): void {
    this.chatSignlrService.loadActiveMessages();
  }

  onScroll(event: Event) {
    const element = event.target as HTMLElement;
    let oldestMessageDateTime = this.messages()[0];
    const elementScrollValue = element.scrollTop;

    if (elementScrollValue < 1 && !this.isFetching()) {
      this.isFetching.set(true);

      this.chatSignlrService
        .loadChatHistory(this.userId(), oldestMessageDateTime.sentAt)
        .subscribe({
          next: (moreMesseges) => {
            this.messages.update((prev) => [...moreMesseges, ...prev]);
            this.isFetching.set(false);
          },
          error: (err) => {
            console.error(err);
          },
        });
    }
  }

  toggleChat() {
    const loginUserId = sessionStorage.getItem('userId') ?? '';
    this.userId.set(loginUserId);

    if (this.isOpen()) {
      this.isOpen.set(false);
    } else {
      if (this.messages().length === 0) {
        this.loadHistory();
      }
      if (loginUserId !== null) this.chatSignlrService.joinRoom(loginUserId);
      this.isOpen.set(true);
    }

    setTimeout(() => {
      if (this.myScrollContainer) {
        const element = this.myScrollContainer.nativeElement;

        element.scrollTo({
          top: element.scrollHeight,
          behavior: 'smooth',
        });
      }
    }, 200);
  }

  loadHistory() {
    const messages = this.messages();
    let oldestMessageDateTime = '';
    const groupId = this.userId();

    if (messages.length > 0) {
      const sortedMessages = [...messages].sort(
        (a, b) => new Date(a.sentAt).getTime() - new Date(b.sentAt).getTime(),
      );

      oldestMessageDateTime = sortedMessages[0].sentAt;
    }

    if (groupId != null) {
      this.chatSignlrService.loadChatHistory(groupId, oldestMessageDateTime).subscribe({
        next: (response) => {
          if (response !== null) this.messages.set(response);
        },
        error: (err) => {
          console.error('Error to loading chat history.');
        },
      });

      setTimeout(() => {
        const element = document.getElementById('chat-window');
        element?.scrollTo({ top: element.scrollHeight, behavior: 'smooth' });
      }, 100);
    }
  }

  sendMessage() {
    if (this.chatForm.valid) {
      const request: MessageRequest = {
        groupId: 'SUPPORT_TEAM',
        text: this.chatForm.value.message ?? '',
      };
      this.chatSignlrService.sendMessage(request).subscribe({
        next: (res) => {
          (this.chatForm.reset(), 'Sucsses send message.');
        },
        error: (err) => {
          'Error to send message.';
        },
      });

      setTimeout(() => {
        const element = document.getElementById('chat-window');
        element?.scrollTo({ top: element.scrollHeight, behavior: 'smooth' });
      }, 100);
    }
  }
}
