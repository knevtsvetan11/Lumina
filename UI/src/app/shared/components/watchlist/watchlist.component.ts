import { Component, inject, OnInit, signal, Signal } from '@angular/core';
import { Router } from '@angular/router';

import { Movie } from '../../../models/movie.model';
import { Watchlist } from '../../../models/watchlist.model';
import { WatchlistService } from '../../../core/services/watchlist.service';

@Component({
  selector: 'app-watchlist-component',
  standalone:true,
  imports: [],
  templateUrl: './watchlist.component.html',
  styleUrl: './watchlist.component.scss',
})
export class WatchlistComponent implements OnInit{
   watchlists = signal<Watchlist[]>([]);

   router = inject(Router)
   watchlistService = inject(WatchlistService)
  

  ngOnInit(): void {
    this.showWatchlist();
  }

  showWatchlist(){
    return this.watchlistService.showWatchlist().subscribe({
      next: (data) => { this.watchlists.set(data)} ,
      error: (err) => { console.error('Error to show watchlist.')}
    });
  }

  removeFromWatchlist(movieId:string){
    
     this.watchlistService.removeFromWatchlist(movieId).subscribe({ 
      next: (res) =>  { 
        if(res.isDeleted){
          this.showWatchlist()
        }
      } ,
      error: (err) => console.error('Error to remove movie from watchlist.')
     }); 
  }

}
