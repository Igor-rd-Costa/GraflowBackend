
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

[Table("Projects")]
[PrimaryKey("Id")]
public class Project
{
    public Guid Id { get; set; }
    public Guid Owner { get; set; }
    [MaxLength(300)]
    [MinLength(3)]
    public string Name { get; set; } = string.Empty;
    public DateTime CreationDate { get; set; }
    public DateTime LastAccessDate { get; set; }


    [JsonIgnore]
    [ForeignKey(nameof(Owner))]
    public User User { get; set; } = default!;
}