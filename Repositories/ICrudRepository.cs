namespace cortado.Repositories;

public interface ICrudRepository<TModel, TDetailsDto>
{
    Task<IEnumerable<TModel>> GetAllAsync();
    Task<TDetailsDto?> GetByIdAsync(int id);
    Task<TModel> CreateAsync(TModel user);
    Task<TModel> UpdateAsync(TModel user);
    Task<bool> DeleteAsync(int id);
}