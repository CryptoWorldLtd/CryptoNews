namespace CryptoWorld.News.Data.Extension
{
    public class Repository : IRepository
    {

        private readonly ApplicationDbContext
        public Task AddAsync<T>(T entity) where T : class
        {
            try
            {

            }
            catch (Exception)
            {

                throw;
            }
        }

        public Task DeleteAsync<T>(Guid id) where T : class
        {
            throw new NotImplementedException();
        }

        public Task<T> GetByIdAsync<T>(Guid id) where T : class
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync<T>(T entity) where T : class
        {
            throw new NotImplementedException();
        }
    }
}
