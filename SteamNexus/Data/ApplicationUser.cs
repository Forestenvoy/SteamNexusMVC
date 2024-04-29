using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace SteamNexus.Data
{
    public class ApplicationUser : IdentityUser
    {
        [MaxLength(10)]
        public string Name { get; set; }

        public bool Gender { get; set; }  //使用 bool 來表示性別，true 表示男性，false 表示女性

        public DateTime Birthday { get; set; }

        public int Power { get; set; } = 1000;

        [MaxLength(200)]
        public string CPUId { get; set; }

        [MaxLength(200)]
        public string GPUId { get; set; }

        [MaxLength(50)]
        public int RAM { get; set; }

        [MaxLength(200)]
        public string Images { get; set; } = null;
    }
}
