using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Lumina.Data.Common;
using Microsoft.EntityFrameworkCore;

namespace Lumina.Data.Models;

public  class Movie
{
    [Key]
    public Guid Id { get; set; } 


    [Required(ErrorMessage = "Title is required.")]
    [StringLength(EntityConstants.Movie.TitleMaxLength,ErrorMessage = "Title cannot exceed 100 characters.")]
    public string Title { get; set; } = null!;


    [Required(ErrorMessage = "Genre is required.")]
    [StringLength(EntityConstants.Movie.GenreMaxLength, ErrorMessage = "Genre cannot exceed 50 characters.")]
    public string Genre { get; set; } = null!;


    [Required(ErrorMessage = "Release date is required.")]
    public DateTime  ReleaseDate { get; set; }


    [Required(ErrorMessage = "Director is required.")]
    [StringLength(EntityConstants.Movie.DirectorNameMaxLength, ErrorMessage = "Director cannot exceed 100 characters.")]

    public string Director { get; set; } = null!;

    public int Duration { get; set; }

    public string Description { get; set; } = null!;

    public string? ImageUrl { get; set; }

    public bool IsDeleted { get; set; } = false;

    public virtual  ICollection<ApplicationUserMovie> UserWatchlists { get; set; } = 
          new HashSet<ApplicationUserMovie>();

    public virtual ICollection<Screening> ScreeningProjections { get; set; } = 
        new HashSet<Screening>();
}
