using Lumina.Worker.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumina.Worker.Data;

public  class WorkerDbContext : DbContext  
{

    public WorkerDbContext(DbContextOptions<WorkerDbContext> options) : base(options)
    {
        
    }

    public virtual  DbSet<EmailLog> EmailLogs { get; set; }

}
