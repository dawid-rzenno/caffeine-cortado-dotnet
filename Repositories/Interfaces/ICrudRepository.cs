namespace cortado.Repositories.Interfaces;

public interface ICrudRepository<TModel, TDetailsDto>
{
    public Task<IEnumerable<TModel>> GetAllAsync(
        string sort,
        string sortBy,
        int size,
        int page,
        string term,
        bool globalSearch
    )
    {
        throw new NotImplementedException();
    }
    
    public Task<TDetailsDto?> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }
    
    public Task<TModel> CreateAsync(TModel user)
    {
        throw new NotImplementedException();
    }
    
    public Task<TModel> UpdateAsync(TModel user)
    {
        throw new NotImplementedException();
    }
    
    public Task<bool> DeleteAsync(int id)
    {
        throw new NotImplementedException();
    }
}