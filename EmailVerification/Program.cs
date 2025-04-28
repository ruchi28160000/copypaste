
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<EmailService>();  // Register EmailService

builder.Services.AddSingleton<UserService>();   // Register UserService

builder.Services.AddControllers();

var app = builder.Build();

app.UseAuthorization();

app.MapControllers();

app.Run();

 