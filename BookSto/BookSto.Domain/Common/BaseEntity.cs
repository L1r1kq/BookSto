using SocialNetwork309.Domain.Common.Interfaces;

namespace SocialNetwork309.Domain.Common
{
    public abstract class BaseEntity : IEntity
    {
        public int Id { get; set; }

    }
}
