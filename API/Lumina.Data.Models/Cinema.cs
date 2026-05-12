using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumina.Data.Models;

public class Cinema
{

    public Guid Id { get; set; }


    public string Name { get; set; } = null!;


    public string Location { get; set; } = null!;


    public bool IsDeleted { get; set; }


    public ICollection<Screening> ScreeningMovies { get; set; } = new HashSet<Screening>();  

    public Guid? ManagerId { get; set; } 

    public virtual Manager? Manager { get; set; } 
}
