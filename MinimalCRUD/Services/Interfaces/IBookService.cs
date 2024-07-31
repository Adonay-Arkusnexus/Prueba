using MinimalCRUD.Models;

namespace MinimalCRUD.Services.Interfaces
{
    public interface IBookService
    {
        Task<Book> CrearLibro(BookRequest request);
    }
}
