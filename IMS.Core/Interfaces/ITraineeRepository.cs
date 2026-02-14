using IMS.Core.Entities;

namespace IMS.Core.Interfaces;
public interface ITraineeRepository
{
    Task AddAsync(Trainee trainee);
}
