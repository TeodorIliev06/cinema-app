namespace CinemaApp.Data.Repositories
{
    using Microsoft.EntityFrameworkCore;

    using Contracts;
    using Models.Contracts;

    public class BaseRepository<TType, TId> : IRepository<TType, TId>
        where TType : class, ISoftDeletable
    {
        private readonly CinemaDbContext dbContext;
        private readonly DbSet<TType> dbSet;

        public BaseRepository(CinemaDbContext dbContext)
        {
            this.dbContext = dbContext;
            this.dbSet = this.dbContext.Set<TType>();
        }

        public TType? GetById(TId id)
        {
            return this.dbSet.Find(id);
        }

        public async Task<TType?> GetByIdAsync(TId id)
        {
            return await this.dbSet.FindAsync(id);
        }

        public IEnumerable<TType> GetAll()
        {
            return this.dbSet.ToArray();
        }

        public async Task<IEnumerable<TType>> GetAllAsync()
        {
            return await this.dbSet.ToArrayAsync();
        }

        public IQueryable<TType> GetAllAttached()
        {
            return this.dbSet.AsQueryable();
        }

        public void Add(TType item)
        {
            this.dbSet.Add(item);
            this.dbContext.SaveChanges();
        }

        public async Task AddAsync(TType item)
        {
            await this.dbSet.AddAsync(item);
            await this.dbContext.SaveChangesAsync();
        }

        public bool Delete(TId id)
        {
            var entity = this.GetById(id);

            if (entity == null)
            {
                return false;
            }

            this.dbSet.Remove(entity);
            this.dbContext.SaveChanges();

            return true;
        }

        public async Task<bool> DeleteAsync(TId id)
        {
            var entity = await this.GetByIdAsync(id);

            if (entity == null)
            {
                return false;
            }

            this.dbSet.Remove(entity);
            await this.dbContext.SaveChangesAsync();

            return true;
        }

        public bool SoftDelete(TId id)
        {
            var entity = this.GetById(id);

            if (entity == null)
            {
                return false;
            }

            entity.IsDeleted = true;
            this.dbContext.SaveChanges();

            return true;
        }

        public async Task<bool> SoftDeleteAsync(TId id)
        {
            var entity = await this.GetByIdAsync(id);

            if (entity == null)
            {
                return false;
            }

            entity.IsDeleted = true;
            await this.dbContext.SaveChangesAsync();

            return true;
        }

        public bool Update(TType item)
        {
            try
            {
                this.dbSet.Attach(item);
                this.dbContext.Entry(item).State = EntityState.Modified;
                this.dbContext.SaveChanges();

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public async Task<bool> UpdateAsync(TType item)
        {
            try
            {
                this.dbSet.Attach(item);
                this.dbContext.Entry(item).State = EntityState.Modified;
                await this.dbContext.SaveChangesAsync();

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}
