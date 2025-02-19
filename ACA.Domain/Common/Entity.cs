namespace ACA.Domain.Common
{

    public abstract class Entity
    {
        #region Properties

        public Guid Id { get; set; }

        #endregion
        #region Constructors

        protected Entity() { }
        public Entity(Guid id)

        {
            Id = id;
        }
        #endregion
    }
}
