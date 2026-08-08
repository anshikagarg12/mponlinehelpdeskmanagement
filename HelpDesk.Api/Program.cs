using HelpDesk.Api.Data;
using HelpDesk.Api.Models;
using HelpDesk.Api.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Controllers + Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Entity Framework Core + SQL Server
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Repository Pattern
builder.Services.AddScoped<ITicketRepository, TicketRepository>();

var app = builder.Build();

// Apply migrations and seed sample data on startup (development convenience).
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
    DbSeeder.Seed(db);
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.MapControllers();

app.Run();

/// <summary>
/// Seeds a handful of sample tickets so the MVC dashboard has data to display.
/// </summary>
public static class DbSeeder
{
    public static void Seed(AppDbContext db)
    {
        if (db.Tickets.Any())
        {
            return;
        }

        db.Tickets.AddRange(
            new Ticket { Title = "Cannot connect to VPN", Description = "VPN client fails with error 809 from home network.", Priority = "High", Status = "Open", RaisedBy = "priya.sharma", CreatedDate = DateTime.Now.AddDays(-2) },
            new Ticket { Title = "Outlook keeps crashing", Description = "Outlook closes unexpectedly when opening large attachments.", Priority = "Medium", Status = "In Progress", RaisedBy = "rahul.verma", CreatedDate = DateTime.Now.AddDays(-1) },
            new Ticket { Title = "Request new mouse", Description = "Existing mouse scroll wheel is not working.", Priority = "Low", Status = "Closed", RaisedBy = "ananya.iyer", CreatedDate = DateTime.Now.AddDays(-5) },
            new Ticket { Title = "Printer on 3rd floor jammed", Description = "Paper jam that cannot be cleared, needs technician.", Priority = "Medium", Status = "Open", RaisedBy = "vikram.singh", CreatedDate = DateTime.Now.AddHours(-6) },
            new Ticket { Title = "Software install: Node.js", Description = "Need Node.js LTS installed for a new project.", Priority = "Low", Status = "Open", RaisedBy = "sana.khan", CreatedDate = DateTime.Now.AddHours(-3) }
        );

        db.SaveChanges();
    }
}
