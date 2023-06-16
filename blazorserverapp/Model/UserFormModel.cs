using System.ComponentModel.DataAnnotations;

namespace blazorserverapp.Model;
public class User
{
 
    public int Id {get;set;}
    public string Login {get; set;}
    public string Password {get;set;}
    // public virtual ICollection<Conversation>? Conversations {get;set;}
}

public class UserFormModel
{
    [Required]
    [StringLength(10,ErrorMessage = "Le login saisi ne doit pas dépasser 10 caractères")]
    public string? Login {get; set;}
    [Required]
    public string? Password {get;set;}
}