namespace CryptoWorld.News.Data.Extension
{

    public interface IRepository
    {
        Task<T?> GetByIdAsync<T>(object id) where T : class;
        IQueryable<T> GetAllAsync<T>() where T : class;
        Task AddAsync<T>(T entity) where T : class;
        Task DeleteAsync<T>(T entity) where T : class;
        Task UpdateAsync<T>(T entity) where T : class;
        Task SaveChangesAsync();
        Task AddRangeAsync<T>(IEnumerable<T> entities);
        IQueryable<T> AllReadOnly<T>() where T : class;

        
    }

}
