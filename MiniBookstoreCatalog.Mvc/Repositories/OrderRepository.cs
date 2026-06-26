using Microsoft.EntityFrameworkCore;
using MiniBookstoreCatalog.Mvc.Data;
using MiniBookstoreCatalog.Mvc.Models;

namespace MiniBookstoreCatalog.Mvc.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly AppDbContext _context;

    public OrderRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddOrderAsync(Order order)
    {
        await _context.Orders.AddAsync(order);
    }

    public async Task<Order?> GetOrderByIdAsync(int id)
    {
        return await _context.Orders
            .Include(x => x.OrderItems)
            .ThenInclude(x => x.Book)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}

