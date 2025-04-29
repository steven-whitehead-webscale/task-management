using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.HttpsPolicy;
using TaskManagement.Application.Services;
using TaskManagement.BusinessLogic.Models;
using TaskManagement.BusinessLogic.Queries;
using TaskManagement.Common.Models;
using TaskManagement.DataAccessLayer.Commands;
using TaskManagement.DataAccessLayer.Queries;
using TaskManagement.Infrastructure.Logging;

var builder = WebApplication.CreateBuilder(args);

// Explicitly configure the URLs
builder.WebHost.UseUrls("http://localhost:5000", "https://localhost:5001");

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Task Management API",
        Version = "v1",
        Description = "A simple task management API using CQRS pattern"
    });
});

// Configure HTTPS redirection
builder.Services.AddHttpsRedirection(options =>
{
    options.HttpsPort = 5001;
});

// Create shared in-memory store
var commonTasks = new List<TaskManagement.Common.Models.Task>();
var businessLogicTasks = new List<TaskManagement.BusinessLogic.Models.Task>();

// Register application services
builder.Services.AddSingleton(commonTasks); // Share the same list for both commands and queries
builder.Services.AddSingleton(businessLogicTasks); // Share the same list for business logic tasks
builder.Services.AddScoped<ITaskService, TaskService>();
builder.Services.AddScoped<ITaskQueries, TaskQueries>();
builder.Services.AddScoped<ITaskCommandHandlers, TaskCommandHandlers>();
builder.Services.AddScoped<ITaskQueryHandlers, TaskQueryHandlers>();
builder.Services.AddScoped<ILoggingService, LoggingService>();

var app = builder.Build();

Console.WriteLine("\n=== Application Startup ===");
Console.WriteLine($"Environment: {app.Environment.EnvironmentName}");
Console.WriteLine($"Content Root: {app.Environment.ContentRootPath}");
Console.WriteLine($"Web Root: {app.Environment.WebRootPath}");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Task Management API V1");
    });
}

// Add detailed logging middleware
app.Use(async (context, next) =>
{
    Console.WriteLine($"\nRequest Path: {context.Request.Path}");
    Console.WriteLine($"Request Method: {context.Request.Method}");
    Console.WriteLine($"Content Type: {context.Response.ContentType}");
    
    await next();
    
    Console.WriteLine($"Response Status Code: {context.Response.StatusCode}");
});

// Use HTTPS redirection
app.UseHttpsRedirection();

// Serve static files with detailed options
app.UseStaticFiles(new StaticFileOptions
{
    OnPrepareResponse = ctx =>
    {
        Console.WriteLine($"\nServing static file: {ctx.File.Name}");
        Console.WriteLine($"File exists: {ctx.File.Exists}");
        Console.WriteLine($"Content Type: {ctx.Context.Response.ContentType}");
    }
});

// Configure default files
app.UseDefaultFiles(new DefaultFilesOptions
{
    DefaultFileNames = new List<string> { "index.html" }
});

app.UseAuthorization();
app.MapControllers();

// Add a route to the root path
app.MapGet("/", context =>
{
    Console.WriteLine("\nRoot path requested, redirecting to index.html");
    context.Response.Redirect("/index.html");
    return System.Threading.Tasks.Task.CompletedTask;
});

// Add a test endpoint
app.MapGet("/test", () => "API is working!");

// Log the URLs the application is listening on
var urls = app.Urls;
Console.WriteLine("\n=== Application URLs ===");
Console.WriteLine("Application is listening on the following URLs:");
foreach (var url in urls)
{
    Console.WriteLine($"  {url}");
}

Console.WriteLine("\n=== Static Files ===");
Console.WriteLine("Static files are being served from: " + Path.Combine(Directory.GetCurrentDirectory(), "wwwroot"));
Console.WriteLine("Available static files:");
foreach (var file in Directory.GetFiles(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot"), "*.*", SearchOption.AllDirectories))
{
    Console.WriteLine($"  {Path.GetFileName(file)}");
}

Console.WriteLine("\n=== Application Started ===");
Console.WriteLine("Press Ctrl+C to stop the application");

app.Run();
