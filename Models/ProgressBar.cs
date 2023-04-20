using System.ComponentModel.DataAnnotations;

namespace BumaAPI.Models;

public class ProgressBar
{
    [Key]
    public int Id { get; set; }
    [Required]
    public string Title { get; set; } = string.Empty;
    [Required]
    public string Percent { get; set; } = string.Empty;
}
