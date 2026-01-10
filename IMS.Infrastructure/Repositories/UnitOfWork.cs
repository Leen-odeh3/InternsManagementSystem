using IMS.Application.Abstractions;
using IMS.Infrastructure.Database;
using Microsoft.EntityFrameworkCore.Storage;


namespace IMS.Infrastructure.Repositories;
public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;
    private IDbContextTransaction? _transaction;

    public UnitOfWork(AppDbContext context)
    {
        _context = context;
    }

    public async Task BeginTransactionAsync()
    {
        _transaction = await _context.Database.BeginTransactionAsync();
    }

    public async Task CommitAsync()
    {
        await _transaction!.CommitAsync();
    }

    public async Task RollbackAsync()
    {
        await _transaction!.RollbackAsync();
    }
}
