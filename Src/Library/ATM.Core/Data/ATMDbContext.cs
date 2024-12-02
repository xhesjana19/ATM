using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ATM.Core.Services.Authentication;
using ATM.Core.Data.Mapping;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;

namespace ATM.Core.Data
{
    /// <summary>
    /// Represents base object context.
    /// </summary>
    public class ATMDbContext : DbContext, IDbContext
    {
        private readonly IAuthenticationService _authenticationService;

        #region Ctor
        
        public ATMDbContext(DbContextOptions<ATMDbContext> options, IAuthenticationService authenticationService) : base(options)
        {
            _authenticationService = authenticationService;
        }


        #endregion
       
        #region SaveChanges Overrides

        /// <inheritdoc cref="DbContext"/>
        public override int SaveChanges()
        {
            //todo do it better 
            // UpdateAuditAndDeleteStatus().RunSynchronously(); runs errors
            //UpdateAuditAndDeleteStatus();
            return base.SaveChanges();
        }
        
        /// <inheritdoc cref="DbContext"/>
        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            //todo do it better 
            // UpdateAuditAndDeleteStatus().RunSynchronously(); runs errors
            //UpdateAuditAndDeleteStatus();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        /// <inheritdoc cref="DbContext"/>
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            //await UpdateAuditAndDeleteStatus();
            return await base.SaveChangesAsync(cancellationToken);
        }

        /// <inheritdoc cref="DbContext"/>
        public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = new CancellationToken())
        {
            //await UpdateAuditAndDeleteStatus();
            return await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }


        #endregion

        #region Utilities

        /// <inheritdoc cref="DbContext"/>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //dynamically load all entity and query type configurations
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            var typeConfigurations = assemblies.SelectMany(a => a.GetTypes()).Where(type =>
                (type.BaseType?.IsGenericType ?? false)
                && type.BaseType.GetGenericTypeDefinition() == typeof(BaseEntityTypeConfiguration<>));

            var mapList = new List<IMappingConfiguration?>();
            foreach (var typeConfiguration in typeConfigurations)
            {
                var configuration = Activator.CreateInstance(typeConfiguration) as IMappingConfiguration;
                mapList.Add(configuration);
            }
            foreach (var map in mapList.OrderBy(l => l?.Order))
            {
                map?.ApplyConfiguration(modelBuilder);
            }

            // make all decimals with a set precision
            foreach (var property in modelBuilder.Model.GetEntityTypes()
                .SelectMany(t => t.GetProperties())
                .Where(p => p.ClrType == typeof(decimal)
                            || p.ClrType == typeof(decimal?)))
            {
                property.SetColumnType("decimal(18, 2)");
            }

            // make all enums strings
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                foreach (var property in entityType.GetProperties())
                {
                    if (property.ClrType.BaseType == typeof(Enum))
                    {
                        var type = typeof(EnumToStringConverter<>).MakeGenericType(property.ClrType);
                        var converter = Activator.CreateInstance(type, new ConverterMappingHints()) as ValueConverter;
                        
                        property.SetValueConverter(converter);
                    }
                }
            }

            // make all ISoftDeletables hidden from query results
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                modelBuilder.Entity(entityType.ClrType, entityBuilder =>
                {
                    //Global Filters
                    var lambdaExp = ApplyEntityFilterTo(entityType.ClrType);
                    if (lambdaExp != null)
                        entityBuilder.HasQueryFilter(lambdaExp);
                });
            }

            base.OnModelCreating(modelBuilder);
        }

        protected virtual LambdaExpression? ApplyEntityFilterTo(Type entityClrType)
        {
            if (typeof(ISoftDeletable).IsAssignableFrom(entityClrType))
            {
                var parameter = Expression.Parameter(entityClrType, "entity");
                var member = Expression.Property(parameter, nameof(ISoftDeletable.IsDeleted));
                var body = Expression.Equal(member, Expression.Constant(false));
                return Expression.Lambda(body, parameter);
            }

            return null;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Creates a DbSet that can be used to query and save instances of entity.
        /// </summary>
        /// <typeparam name="TEntity">Entity type.</typeparam>
        /// <returns>A set for the given entity type.</returns>
        public new virtual DbSet<TEntity> Set<TEntity>() where TEntity : BaseEntity
        {
            return base.Set<TEntity>();
        }
        
        /// <summary>
        /// Detach an entity from the context.
        /// </summary>
        /// <typeparam name="TEntity">Entity type.</typeparam>
        /// <param name="entity">Entity.</param>
        public virtual void Detach<TEntity>(TEntity entity) where TEntity : BaseEntity
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            var entityEntry = Entry(entity);
            if (entityEntry == null)
                return;

            //set the entity is not being tracked by the context
            entityEntry.State = EntityState.Detached;
        }

        #endregion
    }
}