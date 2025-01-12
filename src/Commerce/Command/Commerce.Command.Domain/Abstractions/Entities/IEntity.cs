namespace Commerce.Command.Domain.Abstractions.Entities
{
    /// <summary>
    /// Interface of domain entity
    /// </summary>
    public interface IEntity
    {
        /// <summary>
        /// Self validation of entity, throw exception if any business rule not match
        /// </summary>
        public void Validate();
    }
}