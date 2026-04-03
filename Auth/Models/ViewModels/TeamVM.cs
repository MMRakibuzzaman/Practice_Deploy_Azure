using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Auth.Models.ViewModels;

public class TeamVM
{
    public int TeamId { get; set; }
    [Required,StringLength(50)]
    public string? TeamName { get; set; }
    [Column(TypeName = "date")]
    public DateTime Established { get; set; }
    [Column(TypeName = "money")]
    public decimal Revenue { get; set; }
    public string? TeamLogo { get; set; }
    public IFormFile? Image { get; set; }
    public bool IsActive { get; set; }
    public List<string> Players { get; set; } = new List<string>();
}