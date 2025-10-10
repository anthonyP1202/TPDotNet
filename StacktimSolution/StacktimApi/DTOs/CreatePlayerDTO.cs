using System.ComponentModel.DataAnnotations;

namespace StacktimApi.DTOs
{
    public class CreatePlayerDTO
    {
        [Required]
        [StringLength(40)]
        public string Pseudo { get; set; } = null!;

        [Required]
        [StringLength(100)]
        public string Email { get; set; } = null!;

        [StringLength(20)]
        public Ranks? Rank { get; set; }
    }
}
