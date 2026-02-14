

using IMS.Core.Entities;
using IMS.Core.Interfaces;
using IMS.Infrastructure.Database;

namespace IMS.Infrastructure.Repositories;
public class TraineeRepository : ITraineeRepository
{
    private readonly AppDbContext _context;

    public TraineeRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Trainee trainee)
    {
        await _context.Trainees.AddAsync(trainee);
    }

}
