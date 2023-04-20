using System.ComponentModel.DataAnnotations;

namespace BumaAPI.Models;

public class PersonalInfo
{
    [Key]
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Carrer { get; set; } = string.Empty;
    public string Intro { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}
