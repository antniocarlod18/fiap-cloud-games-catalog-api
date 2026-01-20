namespace FiapCloudGamesCatalog.Domain.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IOrderRepository OrdersRepo { get; }
        IGameRepository GamesRepo { get; }
        IAuditGameUserCollectionRepository AuditGameUsersRepo { get; }
        IAuditGamePriceRepository AuditGamePriceRepo { get; }
        IPromotionRepository PromotionsRepo { get; }
        ICartRepository CartsRepo { get; }
        ILibraryRepository LibrariesRepo { get; }
        Task Commit();
    }
}
