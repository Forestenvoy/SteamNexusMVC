using SteamNexus_Server.Models;
using System.ComponentModel.DataAnnotations;

namespace SteamNexus_Server.ViewModels.Game
{
    public class DetailsViewModel
    {
        [Display(Name = "遊戲ID")]
        public int GameId { get; set; }

        //[Display(Name = "最低配備ID")]
        //public int? MinReqId { get; set; }

        //public virtual MinimumRequirement MinReq { get; set; }

        //[Display(Name = "最高配備ID")]
        //public int? RecReqId { get; set; }

        //public virtual RecommendedRequirement RecReq { get; set; }

        [Display(Name = "SteamID")]
        public int? AppId { get; set; }

        [Display(Name = "遊戲名稱")]
        public string? Name { get; set; }

        [Display(Name = "原始價格")]
        public int? OriginalPrice { get; set; }

        [Display(Name = "現在價格")]
        public int? CurrentPrice { get; set; }

        [Display(Name = "最低價格")]
        public int? LowestPrice { get; set; }

        [Display(Name = "遊戲分級")]
        public string? AgeRating { get; set; }

        [Display(Name = "遊戲評論")]
        public string? Comment { get; set; }

        [Display(Name = "評論數量")]
        [MaxLength(100)]
        public int? CommentNum { get; set; }

        [Display(Name = "上市日期")]
        public DateTime? ReleaseDate { get; set; }

        [Display(Name = "發行商")]
        [MaxLength(100)]
        public string? Publisher { get; set; }

        [Display(Name = "遊戲介紹")]
        [MaxLength(500)]
        public string? Description { get; set; }

        [Display(Name = "當前遊玩人數")]
        public int? Players { get; set; }

        [Display(Name = "24小時高峰人數")]
        public int? PeakPlayers { get; set; }

        [Display(Name = "遊戲圖片")]
        [MaxLength(300)]
        public string? ImagePath { get; set; }

        [Display(Name = "遊戲影片")]
        public string? VideoPath { get; set; }
    }
}
