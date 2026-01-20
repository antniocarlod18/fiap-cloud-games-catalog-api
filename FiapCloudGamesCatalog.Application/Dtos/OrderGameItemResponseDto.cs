using FiapCloudGamesCatalog.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FiapCloudGamesCatalog.Application.Dtos
{
    public class OrderGameItemResponseDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Genre { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public string? Developer { get; set; }
        public string? Distributor { get; set; }
        public IList<string> GamePlatforms { get; set; } = [];
        public string? GameVersion { get; set; }
        public bool Available { get; set; }
        public DateTime DateCreated { get; set; }

        public static implicit operator OrderGameItemResponseDto?(OrderGameItem orderGameItem)
        {
            if (orderGameItem == null) return null;

            return new OrderGameItemResponseDto
            {
                Id = orderGameItem.Id,
                Title = orderGameItem.Game.Title,
                Genre = orderGameItem.Game.Genre,
                Description = orderGameItem.Game.Description,
                Price = orderGameItem.Price,
                Developer = orderGameItem.Game.Developer,
                Distributor = orderGameItem.Game.Distributor,
                GamePlatforms = orderGameItem.Game.GamePlatforms.Select(gp => gp.ToString()).ToList(),
                GameVersion = orderGameItem.Game.GameVersion,
                Available = orderGameItem.Game.Available,
                DateCreated = orderGameItem.DateCreated
            };
        }
    }
}
