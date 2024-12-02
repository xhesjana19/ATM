using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Threading;
using System.Threading.Tasks;

namespace ATM.Core.Data
{
    /// <summary>
    /// Represents DB context.
    /// </summary>
    public interface IDbContext
    {
        #region Methods

        /// <summary>
        /// Creates a DbSet that can be used to query and save instances of entity.
        /// </summary>
        /// <typeparam name="TEntity">Entity type.</typeparam>
        /// <returns>A set for the given entity type.</returns>
        DbSet<TEntity> Set<TEntity>() where TEntity : BaseEntity;

        /// <summary>
        /// Saves all changes made in this context to the database.
        /// </summary>
        /// <returns>The number of state entries written to the database.</returns>
        int SaveChanges();

        /// <summary>
        /// Saves all changes made in this context to the database.
        /// </summary>
        /// <param name="acceptAllChangesOnSuccess">
        /// Indicates whether SaveAllChanges on EF is called when the data is sent to the db for.
        /// </param>
        /// <returns>The number of state entries written to the database.</returns>
        int SaveChanges(bool acceptAllChangesOnSuccess);

        /// <summary>
        /// Saves all changes asynchronously made in this context to the database.
        /// </summary>
        /// <param name="cancellationToken">A token to observe while waiting for the task to complete.</param>
        /// <returns>The number of state entries written to the database.</returns>
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Saves all changes asynchronously made in this context to the database.
        /// </summary>
        /// <param name="acceptAllChangesOnSuccess">
        /// Indicates whether SaveAllChanges on EF is called when the data is sent to the db for.
        /// </param>
        /// <param name="cancellationToken">A token to observe while waiting for the task to complete.</param>
        /// <returns>The number of state entries written to the database.</returns>
        Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default);

        /// <summary>
        /// Detach an entity from the context.
        /// </summary>
        /// <typeparam name="TEntity">Entity type.</typeparam>
        /// <param name="entity">Entity.</param>
        void Detach<TEntity>(TEntity entity) where TEntity : BaseEntity;


        EntityEntry Entry(object entity);

        EntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;

        #endregion
    }
}