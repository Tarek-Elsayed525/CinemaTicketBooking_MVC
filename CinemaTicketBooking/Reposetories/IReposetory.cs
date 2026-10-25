using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq.Expressions;

namespace CinemaTicketBooking.Reposetories
{
    public interface IReposetory<T> where T : class
    {
        IQueryable<T> Query(Expression<Func<T, bool>> filter = null,
           Expression<Func<T, object>>[] includes = null,
           bool isTracked = true
           );

        Task<EntityEntry<T>> AddAsync(T entity);

        Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> filter = null,
            Expression<Func<T, object>>[] includes = null,
            bool isTracked = true
            );
        Task<T> GetOneAsync(Expression<Func<T, bool>> filter = null,
           Expression<Func<T, object>>[] includes = null,
           bool isTracked = true
           );

        void Update(T entity);

        void Remove(T entity);



        void RemoveRange(IEnumerable<T> entities);

        // Save changes to the database asynchronously and return the number of affected rows, or -1 in case of an exception
        Task<int> SaveAsync();
    }
}
