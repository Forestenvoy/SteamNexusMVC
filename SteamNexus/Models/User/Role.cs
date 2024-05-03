using System.ComponentModel.DataAnnotations;

namespace SteamNexus.Models.User
{
    public partial class Role
    {
        [Key]
        [Required]
        public string Rolesid { get; set; }

        [Required]
        public string Rolename { get; set; }

        public virtual Users Users { get; set; }

    }
}
