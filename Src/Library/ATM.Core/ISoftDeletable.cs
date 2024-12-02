namespace ATM.Core
{
    /// <summary>
    /// Represents an entity that cannot be hard deleted.
    /// </summary>
    public interface ISoftDeletable
    {
        /// <summary>
        /// Gets or sets a value indicating whether the entity is deleted.
        /// </summary>
        public bool IsDeleted { get; set; }
    }
}