
using BookSto.Application.Common;
using BookSto.Domain.Interfaces;
using SocialNetwork309.Domain.Common;
using SocialNetwork309.Domain.Common.Interfaces;

namespace SocialNetwork309.Application.Common
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<T> Repository<T>() where T : BaseAuditableEntity;
        Task<int> Save(CancellationToken cancellationToken);
        Task<int> SaveAndRemoveCache(CancellationToken cancellationToken, params string[] cacheKeys);
        Task Rollback();
    }
}
