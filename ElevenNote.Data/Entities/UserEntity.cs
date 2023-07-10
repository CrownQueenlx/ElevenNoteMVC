using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace ElevenNote.Data.Entities;

public class UserEntity : IdentityUser<int>
{
    // Id is inherited

    // Email is inherited

    [Required]
    public string Username { get; set; } = string.Empty;
    public override string UserName => Username; //{ get; } forwarding to our property

    [Required]
    public string Password { get; set; } = string.Empty;

    [MaxLength(100)]
    public string? FirstName { get; set; }

    [MaxLength(100)]
    public string? LastName { get; set; }

    [Required]
    public DateTime DateCreated { get; set; }

    public List<NoteEntity> Notes { get; set; } = new();
}