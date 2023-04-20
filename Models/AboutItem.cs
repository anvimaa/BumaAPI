using System.ComponentModel.DataAnnotations;

namespace BumaAPI.Models;
public class AboutItem
{
    [Key]
    public int Id { get; set; }
    [Required]
    public string Title { get; set; } = string.Empty;
    [Required]
    public string PrimaryText { get; set; } = string.Empty;
    [Required]
    public string SecundaryText { get; set; } = string.Empty;
}