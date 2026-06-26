using Microsoft.EntityFrameworkCore;
using MiniBookstoreCatalog.Mvc.Data;
using MiniBookstoreCatalog.Mvc.Models;
using MiniBookstoreCatalog.Mvc.ViewModels;

namespace MiniBookstoreCatalog.Mvc.Services;

public class OrderService : IOrderService
{
    private readonly AppDbContext _context;

    public OrderService(
        AppDbContext context)
    {
        _context = context;
    }

    public async Task CreateOrderAsync(
        OrderCreateViewModel model)
    {
        await using var transaction =
            await _context.Database
                .BeginTransactionAsync();

        try
        {
            var book =
                await _context.Books
                    .FirstOrDefaultAsync(x =>
                        x.Id == model.BookId);

            if (book == null)
                throw new Exception(
                    "Book not found");

            if (book.Stock <
                model.Quantity)
            {
                throw new Exception(
                    "Out of stock");
            }

            var order = new Order
            {
                CreatedAt = DateTime.Now,
                TotalAmount =
                    book.Price *
                    model.Quantity
            };

            _context.Orders.Add(order);

            await _context.SaveChangesAsync();

            _context.OrderItems.Add(
                new OrderItem
                {
                    OrderId = order.Id,
                    BookId = book.Id,
                    Quantity =
                        model.Quantity,
                    UnitPrice =
                        book.Price
                });

            book.Stock -= model.Quantity;

            await _context.SaveChangesAsync();

            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}

