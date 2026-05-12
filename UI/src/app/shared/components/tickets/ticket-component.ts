
import { HttpClient } from '@angular/common/http';
import { Component, inject, OnInit, signal } from '@angular/core';
import { Router } from '@angular/router';
import { Ticket } from '../../../models/ticket.model';
import { FormsModule } from '@angular/forms';
import { TicketService } from '../../../core/services/ticket.service';

@Component({
  selector: 'app-ticket-component',
  standalone:true,
  imports: [FormsModule],
  templateUrl: './ticket-component.html',
  styleUrl: './ticket-component.scss',
})
export class TicketComponent implements OnInit {
 allTickets = signal<Ticket[]>([])

  route = inject(Router)
  ticketService = inject(TicketService)
  
  ngOnInit(): void {
     this.ticketService.getAll().subscribe(data => {
      this.allTickets.set(data)

     })
  }

}
