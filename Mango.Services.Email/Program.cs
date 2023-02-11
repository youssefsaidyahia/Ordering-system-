
using Mango.Services.Email.DbContexts;
using Mango.Services.Email.Extentions;
using Mango.Services.Email.Messaging;
using Mango.Services.Email.Repository;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IEmailRepository, EmailRepository>();
builder.Services.AddHostedService<RabbitMQPaymentConsumer>();
var optionBulider = new DbContextOptionsBuilder<ApplicationDbContext>();
optionBulider.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
builder.Services.AddSingleton(new EmailRepository(optionBulider.Options));
builder.Services.AddSingleton<IAzureServiceBusConsumer, AzureServiceBusConsumer>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.UseAzureServiceBusConsumer();
app.Run();
