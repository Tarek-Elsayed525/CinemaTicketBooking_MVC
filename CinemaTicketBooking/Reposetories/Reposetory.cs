using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq.Expressions;

namespace CinemaTicketBooking.Reposetories
{
    public class Reposetory<T> : IReposetory<T> where T : class
    {
        private readonly ApplicationDbContext _context;// = new ApplicationDbContext();
        private readonly DbSet<T> _dbSet;

        public Reposetory(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        // query method to get entities with optional filtering, including related entities, and tracking options
        public IQueryable<T> Query(Expression<Func<T, bool>> filter = null,
            Expression<Func<T, object>>[] includes = null,
            bool isTracked = true
            )
        {
            var entities = _dbSet.AsQueryable();

            //filter 
            if (filter != null)
            {
                entities = entities.Where(filter);
            }
            //include

            if (includes != null)
            {
                foreach (var include in includes)
                {
                    entities = entities.Include(include);
                }
            }

            // Tracking
            if (!isTracked)
            {
                entities = entities.AsNoTracking();
            }

            return entities;
        }
        // Add a new entity to the database asynchronously
        public async Task<EntityEntry<T>> AddAsync(T entity)
        {
            return await _dbSet.AddAsync(entity);
        }

        // Get all entities with optional filtering, including related entities, and tracking options
        public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> filter = null,
            Expression<Func<T, object>>[] includes = null,
            bool isTracked = true
            )
        {
            var entities = Query(filter, includes, isTracked);
            return await entities.ToListAsync();
        }
        // Get a single entity based on the provided filter, including related entities, and tracking options
        public async Task<T> GetOneAsync(Expression<Func<T, bool>> filter = null,
            Expression<Func<T, object>>[] includes = null,
            bool isTracked = true
            )
        {
            var entities = Query(filter, includes, isTracked);
            return await entities.FirstOrDefaultAsync();
        }

        // Update an existing entity in the database
        public void Update(T entity)
        {
            _dbSet.Update(entity);
        }

        // Remove a entity from the database
        public void Remove(T entity)
        {
            _dbSet.Remove(entity);
        }



        public void RemoveRange(IEnumerable<T> entities)
        {
            _dbSet.RemoveRange(entities);
        }

        // Save changes to the database asynchronously and return the number of affected rows, or -1 in case of an exception
        public async Task<int> SaveAsync()
        {
            try
            {
                return await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return -1;
            }
        }



    }
}
