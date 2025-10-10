using StacktimApi.Model;
using System.ComponentModel.DataAnnotations;

namespace StacktimApi.DTOs
{
    public class PlayerDTO
    {
        public int Id { get; set; }

        [Required]
        [StringLength(40)]
        public string Pseudo { get; set; } = null!;

        [Required]
        [StringLength(100)]
        public string Email { get; set; } = null!;

        [StringLength(20)]
        public Ranks? Rank { get; set; }

        [Range(0,int.MaxValue, ErrorMessage = "must be positif")]
        public int? TotalScore { get; set; }

        public DateTime? RegistrationDate { get; set; }

        public virtual ICollection<Team> Teams { get; set; } = new List<Team>();
    }

    public enum Ranks
    {
        Bronze = 0, 
        Silver = 1, 
        Gold = 2, 
        Platinum = 3, 
        Diamond = 4, 
        Master = 5
    }
}
