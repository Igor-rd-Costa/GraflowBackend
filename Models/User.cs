



using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

[Table("Users")]
[Index("NormalizedUsername", Name = "Users_NormalizedUsername_UN")]
[Index("NormalizedEmail", Name = "Users_NormalizedEmail_UN")]
public class User
{
    [JsonIgnore]
    public Guid Id { get; set; }
    public string Username { get; set; } = string.Empty;
    [JsonIgnore]
    public string NormalizedUsername { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    [JsonIgnore]
    public string NormalizedEmail { get; set; } = string.Empty;
    public bool IsEmailConfirmed { get; set; } = false;
    [JsonIgnore]
    public string Password { get; set; } = string.Empty;
}