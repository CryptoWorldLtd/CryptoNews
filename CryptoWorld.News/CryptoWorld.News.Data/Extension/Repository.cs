using Microsoft.EntityFrameworkCore;
using Serilog;

namespace CryptoWorld.News.Data.Extension
{
    public class Repository : IRepository
    {
        private readonly ApplicationDbContext dbContext;
        public Repository(ApplicationDbContext _dbContext)
        {
            dbContext = _dbContext;
        }

        private DbSet<T> DbSet<T>() where T : class
        {
            return dbContext.Set<T>();
        }
        public async Task AddAsync<T>(T entity) where T : class
        {
            try
            {
                await DbSet<T>().AddAsync(entity);
              
            }
            catch (Exception ex)
            {
                Log.Error("Error while adding entity in db");
                throw new Exception($"Error while adding entity  in db, {ex}");
            }
        }

        public async Task AddRangeAsync<T>(IEnumerable<T> entities)
        {
            try
            {
               await dbContext.AddRangeAsync(entities);
            }
            catch (Exception ex)
            {
                Log.Error("Error while adding range in db!");
                throw new Exception($"Error while adding range in db ,{ex}");
            }
        }

        public IQueryable<T> AllReadOnly<T>() where T : class
        {
            try
            {
                return DbSet<T>()
                .AsNoTracking();
            }
            catch ( Exception ex)
            {
                Log.Error("Error while searching for collection from T!");
                throw new Exception($"Error while searching for collection from T!, {ex}");
            }
        }

        public async Task DeleteAsync<T>(T entity) where T : class
        {
            try
            {
                await Task.Run(() => dbContext.Set<T>().Remove(entity));
            }
            catch (Exception ex)
            {
                Log.Error("Error while deleting entity!");
                throw new Exception($"Error while deleting entity, {ex}");
            }
        }
        public async Task<T> GetByIdAsync<T>(object id) where T : class
        {
            try
            {
                return await DbSet<T>().FindAsync(id);
            }
            catch (Exception ex)
            {
                Log.Error("Error while searching for entity in db");
                throw new Exception($"Error while searching for entity in db, {ex}");
            }
        }

        public async Task SaveChangesAsync()
        {
            try
            {
                await dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Log.Error("Error while saving dates in db!");
               throw new Exception($"Error while saving dates in db {ex}");
            }
        }

        public async Task UpdateAsync<T>(T entity) where T : class
        {
            try
            {
                await Task.Run(() => DbSet<T>().Update(entity));
            }
            catch (Exception ex)
            {
                Log.Error("Error while updating entity in db!");
                throw new Exception ($"Error while updating entity in db {ex}");
            }
        }

        IQueryable<T> IRepository.GetAllAsync<T>()
        {
            try
            {
                return DbSet<T>();
            }
            catch (Exception ex)
            {
                Log.Error("Error while searching for collection from <T>!");
                throw new Exception($"Error while searching for collection from <T>, {ex}");
            }
        }
    }
}
