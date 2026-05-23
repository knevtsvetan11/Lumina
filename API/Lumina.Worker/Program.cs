
using Lumina.Worker;
using Lumina.Worker.Configuration;
using Lumina.Worker.Data;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Reflection;
using Worker.Consumers;

var settings = new HostApplicationBuilderSettings
{
    Args = args,
    EnvironmentName = Environments.Development
};
//stop connect with docker rabbitMQ
//to do create console app
var builder = Host.CreateApplicationBuilder(settings);

// Force the builder to look for User Secrets explicitly
if (builder.Environment.IsDevelopment())
{
    builder.Configuration.AddUserSecrets(Assembly.GetExecutingAssembly(), optional: true);
}

string connectionString = builder.Configuration.GetConnectionString("WorkerDbContextConnection" ??  
    throw new ArgumentNullException("Connection string must be not empty")) ;

builder.Services.Configure<MailtrapSettings>(builder.Configuration.GetSection("MailtrapSettings"));


builder.Services.AddDbContext<WorkerDbContext>(options => options.UseSqlServer(connectionString));



builder.Services.AddMassTransit(x =>
{

    x.AddConsumer<SendWelcomeEmailConsumer>();

    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("localhost", "/", h => {
            h.Username("guest"); 
            h.Password("guest"); 

        });

        cfg.ConfigureEndpoints(context);
    });
});

var app  = builder.Build();
    
 await app.RunAsync();
///     