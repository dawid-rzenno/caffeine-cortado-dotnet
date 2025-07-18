using cortado.Models;

namespace cortado.Repositories;

public interface ICrudRepository<T>
{
    Task<IEnumerable<T>> GetAllAsync();
    Task<T?> GetByIdAsync(int id);
    Task<T> CreateAsync(User user);
    Task<T> UpdateAsync(User user);
    Task<bool> DeleteAsync(int id);
}