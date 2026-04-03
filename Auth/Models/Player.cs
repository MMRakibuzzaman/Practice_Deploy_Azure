using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Auth.Models;

public class Player
{
    public int PlayerId { get; set; }
    [Required,StringLength(50)]
    public string? PlayerName { get; set; }
    [ForeignKey("TeamId")] 
    public int TeamId { get; set; }
    public virtual Team? Team { get; set; }
}