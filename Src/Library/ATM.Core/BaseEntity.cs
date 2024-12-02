using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace ATM.Core
{
    /// <summary>
    /// Base class for entities.    
    /// </summary>
    public abstract class BaseEntity
    {
        /// <summary>
        /// Gets or sets the entity identifier.
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual Guid Id { get; set; }

        public virtual object GetId()
        {
            return $"({Id})";
        }
    }

    public abstract class AuditableEntity : BaseEntity, IAuditable, ISoftDeletable
    {
        #region Implementation of IAuditable

        /// <summary>
        /// Gets or sets the UTC date time when the entity was first created.
        /// </summary>
        public DateTime CreatedOnUtc { get; set; }

        /// <summary>
        /// Gets or sets the UTC date time when the entity was last updated.
        /// </summary>
        public DateTime? UpdatedOnUtc { get; set; }

        /// <summary>
        /// Gets or sets the Id of the user that created this entity.
        /// </summary>
        public Guid CreatedById { get; set; }

        /// <summary>
        /// Gets or sets the Id of the user that last updated this entity.
        /// </summary>
        public Guid? UpdatedById { get; set; }

        /// <summary>
        /// Gets or sets the user that last updated this entity.
        /// </summary>
        //public virtual User? UpdatedBy { get; set; }

        #endregion

        #region Implementation of ISoftDeletable

        /// <summary>
        /// Gets or sets a value indicating whether the entity is deleted.
        /// </summary>
        public bool IsDeleted { get; set; }

        #endregion
    }
}