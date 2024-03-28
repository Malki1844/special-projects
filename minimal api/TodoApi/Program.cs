using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.OpenApi.Models;
using TodoApi;

var builder = WebApplication.CreateBuilder(args);

//שורה זו אומרת לתוכנית לטעון את הגדרות התצורה מקובץ appsettings.json. 
var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();
//כאן, אנו מביאים את מחרוזת החיבור בשם 'ToDoDB' מקובץ התצורה. מחרוזת חיבור זו משמשת לחיבור למסד הנתונים.
var connectionString = configuration.GetConnectionString("ToDoDB");
//שורה זו מוסיפה DbContext (הקשר של מסד נתונים) בשם ToDoDbContext לשירותים באפליקציה. זה מגדיר את DbContext להשתמש בספק MySQL עם מחרוזת החיבור שאוחזרה קודם לכן.

builder.Services.AddDbContext<ToDoDbContext>(options =>
{
    //מזהה אוטומטית את גרסת השרת בהתבסס על מחרוזת החיבור. AutoDetect
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
});
// var policy = "policy";
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c=>{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "SwaggerinMinApi", Version = "1.0" });
});
builder.Services.AddControllers();
var app = builder.Build();
app.UseHttpsRedirection();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "SwaggerinMinApi");
        c.RoutePrefix = "";
    });
}
app.UseAuthorization();
app.UseRouting();
app.UseCors();

app.MapGet("/items", (ToDoDbContext dbContext) =>
{
    var items = dbContext.Items.ToList();
    return items;
});

app.MapPost("/",async (Item todo, ToDoDbContext dbContext) =>
{
    await dbContext.Items.AddAsync(todo);
    await dbContext.SaveChangesAsync();
    // return TypedResults.Created($"{todo.Id}", todo);
    return Results.Ok(todo);
});
app.MapPut("/{id}", async (int id, bool isComplete, ToDoDbContext dbContext) =>
{
    var todo = await dbContext.Items.FindAsync(id);
    if (todo is null) return Results.NotFound();
    
    todo.IsComplete = isComplete;
    await dbContext.SaveChangesAsync();
    
    return Results.Ok(todo);
});
app.MapDelete("/{id}", async (int id, ToDoDbContext dbContext) =>
{
    if (await dbContext.Items.FindAsync(id) is Item todo)
    {
        dbContext.Items.Remove(todo);
        await dbContext.SaveChangesAsync();
        return Results.Ok();   
    }

    return Results.NotFound();
});
app.Run();
