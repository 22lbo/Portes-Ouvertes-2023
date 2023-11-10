using Draw_SignalR_Hub;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSignalR();

var app = builder.Build();

app.MapHub<DrawingHub>("/DrawingHub");

app.Run();
