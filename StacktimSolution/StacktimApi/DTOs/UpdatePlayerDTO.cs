using StacktimApi.Model;
using System.ComponentModel.DataAnnotations;

namespace StacktimApi.DTOs
{
    public class UpdatePlayerDTO
    {
        [Required]
        [StringLength(40)]
        public string Pseudo { get; set; } = null!;

        [Required]
        [StringLength(100)]
        public string Email { get; set; } = null!;

        [StringLength(20)]
        public Ranks? Rank { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "must be positif")]
        public int? TotalScore { get; set; }

        public virtual ICollection<Team> Teams { get; set; } = new List<Team>();
    }
}
