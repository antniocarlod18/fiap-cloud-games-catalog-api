using FiapCloudGamesCatalog.Domain.Repositories;
using FiapCloudGamesCatalog.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace FiapCloudGamesCatalog.Infra.Data.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ContextDb _context;
        private IOrderRepository _orderRepository;
        private IGameRepository _gameRepository;
        private IAuditGameUserCollectionRepository _auditGameUserRepository;
        private IPromotionRepository _promotionRepository;
        private ICartRepository _cartRepository;
        private ILibraryRepository _libraryRepository;
        private IAuditGamePriceRepository _auditGamePriceRepository;

        public UnitOfWork(ContextDb contextDb)
        {
            _context = contextDb;
        }

        public IOrderRepository OrdersRepo
        {
            get
            {
                if (_orderRepository == null)
                {
                    _orderRepository = new OrderRepository(_context);
                }
                return _orderRepository;
            }
        }

        public IGameRepository GamesRepo
        {
            get
            {
                if (_gameRepository == null)
                {
                    _gameRepository = new GameRepository(_context);
                }
                return _gameRepository;
            }
        }

        public IAuditGameUserCollectionRepository AuditGameUsersRepo
        {
            get
            {
                if (_auditGameUserRepository == null)
                {
                    _auditGameUserRepository = new AuditGameUserCollectionRepository(_context);
                }
                return _auditGameUserRepository;
            }
        }

        public IPromotionRepository PromotionsRepo
        {
            get
            {
                if (_promotionRepository == null)
                {
                    _promotionRepository = new PromotionRepository(_context);
                }
                return _promotionRepository;
            }
        }

        public ICartRepository CartsRepo
        {
            get
            {
                if (_cartRepository == null)
                {
                    _cartRepository = new CartRepository(_context);
                }
                return _cartRepository;
            }
        }

        public ILibraryRepository LibrariesRepo
        {
            get
            {
                if (_libraryRepository == null)
                {
                    _libraryRepository = new LibraryRepository(_context);
                }
                return _libraryRepository;
            }
        }

        public IAuditGamePriceRepository AuditGamePriceRepo
        {
            get
            {
                if (_auditGamePriceRepository == null)
                {
                    _auditGamePriceRepository = new AuditGamePriceRepository(_context);
                }
                return _auditGamePriceRepository;
            }
        }

        public async Task Commit()
        {
            await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
