using System.ComponentModel.DataAnnotations;

namespace blazorserverapp.Model;
public class Message
{
    public Message()
    {
        Sender = new User();
    }
    public int Id {get;set;}
    public string Content {get;set;} = "";
    [Required]
    public virtual User   Sender {get;set;}
    public DateTime TimeStamp {get;set;}
}