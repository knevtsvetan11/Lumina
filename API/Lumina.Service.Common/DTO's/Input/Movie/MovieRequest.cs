using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lumina.Common.Constants;

namespace Lumina.Service.Common.DTO_s.Input;

public record MovieRequest
{
    

    [Required(ErrorMessage = "Title is required.")]
    [StringLength(ApplicationConstants.Movie.TitleMaxLength, ErrorMessage = "Title cannot exceed 100 characters.")]
    public string Title { get; init; }

    [Required(ErrorMessage = "Genre is required.")]
    [StringLength(ApplicationConstants.Movie.GenreMaxLength, ErrorMessage = "Genre cannot exceed 50 characters.")]
    public string Genre { get; init; }

    [Required(ErrorMessage = "Release date is required.")]
    public DateTime ReleaseDate { get; init; }

    [Required(ErrorMessage = "Director is required.")]
    [StringLength(ApplicationConstants.Movie.DirectorNameMaxLength, ErrorMessage = "Director cannot exceed 100 characters.")]
    public string Director { get; init; }

    public int Duration { get; init; }

    public string Description { get; init; }

    public string? ImageUrl { get; init; }

}

