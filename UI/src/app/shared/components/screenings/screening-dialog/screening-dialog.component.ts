import { Component, Inject, OnInit, signal } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { MatFormField, MatFormFieldModule } from '@angular/material/form-field';
import { MovieService } from '../../../../core/services/movie.service';
import { CinemaService } from '../../../../core/services/cinema.service';
import { Movie } from '../../../../models/movie.model';
import { Cinema } from '../../../../models/cinema.model';
import { MatOption, MatSelect } from '@angular/material/select';
import { MatButton } from '@angular/material/button';

@Component({
  selector: 'app-screening-dialog',
  imports: [MatFormFieldModule,MatFormField,ReactiveFormsModule,MatSelect,MatOption,MatButton],
  templateUrl: './screening-dialog.component.html',
  styleUrl: './screening-dialog.component.scss',
})
export class ScreeningDialogComponent implements OnInit {

  movies = signal<Movie[]| null>([])
  cinemas = signal<Cinema[]| null>([])


  form = new FormGroup({
    id: new FormControl(''),
    movieId: new FormControl(''),
    cinemaId: new FormControl(''),
    availableTickets: new FormControl(''),
    showtime: new FormControl('')
  })

  constructor(@Inject(MAT_DIALOG_DATA) public data:any,
   private dialogRef:MatDialogRef<ScreeningDialogComponent>,
   private movieService:MovieService,private cinemaService:CinemaService) {
    
  }

  ngOnInit(): void {

    this.loadMovies();
    this.loadCinemas();
    
    if(this.data){
     this.form.patchValue(this.data)
    }

  }

  loadMovies(){
     this.movieService.getAll().subscribe(res => {
      if(res != null)
       this.movies.set(res)
     })
  }

  loadCinemas(){
  this.cinemaService.getAllCinemas().subscribe(res => {
      if(res != null)
       this.cinemas.set(res)
     }) 
  }


  confirm(){
     this.dialogRef.close(this.form.value)
  }

  cancel(){
     this.dialogRef.close(this.form.value)
  }
  
}
