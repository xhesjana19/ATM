
using System;

namespace ATM.Core
{
    /// <summary>
    /// Represents an auditable entity.
    /// </summary>
    public interface IAuditable
    {
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
    }
}