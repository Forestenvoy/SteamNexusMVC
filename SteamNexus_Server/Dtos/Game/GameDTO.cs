using System.ComponentModel.DataAnnotations;

namespace SteamNexus_Server.Dtos.Game
{
    public class GameDTO
    {
        public int GameId { get; set; }

        public int? AppId { get; set; }

        public string? Name { get; set; }

        public int? OriginalPrice { get; set; }

        public int? CurrentPrice { get; set; }

        public int? LowestPrice { get; set; }

        public string? AgeRating { get; set; }

        public string? Comment { get; set; }

        public int? CommentNum { get; set; }

        public DateTime? ReleaseDate { get; set; }

        public string? Publisher { get; set; }

        public string? Description { get; set; }

        public int? Players { get; set; }

        public int? PeakPlayers { get; set; }

        public string? ImagePath { get; set; }

        public string? VideoPath { get; set; }

        public string? LanguageName { get; set; }

        public string? LanguageSupport { get; set; }

        public List<string>? TagsName { get; set; }
    }
}
