namespace cortado.Repositories;

public interface ICrudRepository<T>
{
    Task<IEnumerable<T>> GetAllAsync();
    Task<T?> GetByIdAsync(int id);
    Task<T> CreateAsync(T user);
    Task<T> UpdateAsync(T user);
    Task<bool> DeleteAsync(int id);
}