using System.ComponentModel.DataAnnotations;

namespace SteamNexus_Server.ViewModels.Game
{
    public class EditViewModel
    {
        [Key]
        [Required]
        public int GameId { get; set; }

        [Required(ErrorMessage = "SteamID為必填欄位")]
        [Display(Name = "SteamID")]
        public int? AppId { get; set; }

        [Required(ErrorMessage = "遊戲名稱為必填欄位")]
        [MaxLength(100)]
        [Display(Name = "遊戲名稱")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "原始價格為必填欄位")]
        [Display(Name = "原始價格")]
        public int? OriginalPrice { get; set; }

        [Required(ErrorMessage = "遊戲分級為必填欄位")]
        [Display(Name = "遊戲分級")]
        [MaxLength(100)]
        public string? AgeRating { get; set; }

        [Display(Name = "上市日期")]
        //[MaxLength(100)]
        public DateTime? ReleaseDate { get; set; }

        [Display(Name = "發行商")]
        [MaxLength(100)]
        public string? Publisher { get; set; }

        [Display(Name = "遊戲介紹")]
        [MaxLength(500)]
        public string? Description { get; set; }

        [Display(Name = "遊戲圖片")]
        [MaxLength(300)]
        public string? ImagePath { get; set; }

        [Display(Name = "遊戲影片")]
        [MaxLength(300)]
        public string? VideoPath { get; set; }
    }
}
