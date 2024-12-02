
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ATM.Core.Data
{
    /// <summary>
    /// Represents an entity repository.
    /// </summary>
    /// <typeparam name="TEntity">Entity type.</typeparam>
    public interface IRepository<TEntity> where TEntity : BaseEntity
    {
        #region Methods
        
        /// <summary>
        /// Insert entity.
        /// </summary>
        /// <param name="entity">Entity.</param>
        void Insert(TEntity entity);

        /// <summary>
        /// Asynchronously insert entity.
        /// </summary>
        /// <param name="entity">Entity.</param>
        /// <param name="cancellationToken">A token to observe while waiting for the task to complete.</param>
        Task InsertAsync(TEntity entity, CancellationToken cancellationToken = default);

        /// <summary>
        /// Insert entities.
        /// </summary>
        /// <param name="entities">Entities.</param>
        void Insert(IEnumerable<TEntity> entities);

        /// <summary>
        /// Asynchronously insert entities.
        /// </summary>
        /// <param name="entities">Entities.</param>
        /// <param name="cancellationToken">A token to observe while waiting for the task to complete.</param>
        Task InsertAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);

        /// <summary>
        /// Edit entity.
        /// </summary>
        /// <param name="entity">Entity.</param>
        void Update(TEntity entity);

        /// <summary>
        /// Asynchronously update entity.
        /// </summary>
        /// <param name="entity">Entity.</param>
        /// <param name="cancellationToken">A token to observe while waiting for the task to complete.</param>
        Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);

        /// <summary>
        /// Edit entities.
        /// </summary>
        /// <param name="entities">Entities.</param>
        void Update(IEnumerable<TEntity> entities);

        /// <summary>
        /// Asynchronously update entities.
        /// </summary>
        /// <param name="entities">Entities.</param>
        /// <param name="cancellationToken">A token to observe while waiting for the task to complete.</param>
        Task UpdateAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);

        /// <summary>
        /// Delete entity.
        /// </summary>
        /// <param name="entity">Entity.</param>
        void Delete(TEntity entity);

        /// <summary>
        /// Asynchronously delete entity.
        /// </summary>
        /// <param name="entity">Entity.</param>
        /// <param name="cancellationToken">A token to observe while waiting for the task to complete.</param>
        Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default);

        /// <summary>
        /// Delete entities.
        /// </summary>
        /// <param name="entities">Entities.</param>
        void Delete(IEnumerable<TEntity> entities);

        /// <summary>
        /// Asynchronously delete entities.
        /// </summary>
        /// <param name="entities">Entities.</param>
        /// <param name="cancellationToken">A token to observe while waiting for the task to complete.</param>
        Task DeleteAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);

        /// <summary>
        /// Asynchronously get entity by identifier.
        /// </summary>
        /// <param name="id">Identifier.</param>
        /// <param name="cancellationToken">A token to observe while waiting for the task to complete.</param>
        /// <returns>Entity.</returns>
        Task<TEntity> GetByIdAsync(object id, CancellationToken cancellationToken = default);
        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether IDbContext.SaveChanges() should be
        /// called after every operation.
        /// </summary>
        bool AutoSaveChanges { get; set; }

        /// <summary>
        /// Gets a table.
        /// </summary>
        IQueryable<TEntity> Table { get; }

        /// <summary>
        /// Gets a table with no-tracking enabled. Should be used only for read only operations.
        /// </summary>
        IQueryable<TEntity> TableNoTracking { get; }

        #endregion
    }
}