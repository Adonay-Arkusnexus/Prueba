using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using MinimalCRUD.Context;
using MinimalCRUD.Models;
using MinimalCRUD.Services;
using MinimalCRUD.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApiContext>(opt => opt.UseInMemoryDatabase ("api"));

builder.Services.AddScoped<IBookService, BookService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Ejemplo minimal APIs", Description = "Codigo de ejemplo" });
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Todo API v1");
});

//Un mapeo que al acceder al URL de forma asincrona inyectandole el cobtexto devuelve lo que haya en BookEntity
app.MapGet("/api/books", async (ApiContext context) => Results.Ok(await context.BookEntity.ToListAsync()));

//app.MapGet("/api/book/{id}", async (int id, ApiContext context) =>
//{
//    var book = await context.BookEntity.FindAsync(id);

//    if (book != null)
//    {
//        Results.Ok(book);
//    }
//kkkkhhkhkhkhkhkhkhAAAA
//    Results.NotFound();
//});

app.MapPost("/api/book", async (BookRequest request, IBookService bookService) =>
{
    var createBook = await bookService.CrearLibro(request);

    return Results.Created($"/books/{createBook.Id}", createBook);
});

app.MapDelete("/api/book/{id}", async (int id, ApiContext context) =>
{
    var book = await context.BookEntity.FindAsync(id);
    if (book is null)
    {
        return Results.NotFound();
    }

    context.BookEntity.Remove(book);

    await context.SaveChangesAsync();

    return Results.NoContent();
});

app.MapPut("/api/book", async (int id, BookRequest request, ApiContext context) =>
{
    var book = await context.BookEntity.FindAsync(id);
    if (book is null)
    {
        return Results.NotFound();
    }

    if (request.Name != null)
    {
        book.Name = request.Name;
    }

    if (request.Isbn != null)
    {
        book.ISBN = request.Isbn;
    }

    await context.SaveChangesAsync();

    return Results.Ok(book);
});



app.Run();
