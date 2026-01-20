using FiapCloudGamesCatalog.Domain.Entities;

namespace FiapCloudGamesCatalog.Application.Dtos
{
    public class GameResponseDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Genre { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public decimal PriceWithDiscount { get; set; }
        public string? Developer { get; set; }
        public string? Distributor { get; set; }
        public IList<string> GamePlatforms { get; set; } = [];
        public string? GameVersion { get; set; }
        public bool Available { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }

        public static implicit operator GameResponseDto?(Game game)
        {
            if (game == null) return null;

            return new GameResponseDto
            {
                Id = game.Id,
                Title = game.Title,
                Genre = game.Genre,
                Description = game.Description,
                PriceWithDiscount = game.GetPriceWithDiscount(),
                Price = game.Price,
                Developer = game.Developer,
                Distributor = game.Distributor,
                GamePlatforms = game.GamePlatforms.Select(gp => gp.ToString()).ToList(),
                GameVersion = game.GameVersion,
                Available = game.Available,
                DateCreated = game.DateCreated,
                DateUpdated = game.DateUpdated
            };
        }
    }
}
