using System.Linq.Expressions;

namespace grincoAppModels;
public class Conversation
{
 public int Id { get; set; }
 public string Description { get; set; } = "";
 public List<User> Participants {get;set;}  = new();
}
