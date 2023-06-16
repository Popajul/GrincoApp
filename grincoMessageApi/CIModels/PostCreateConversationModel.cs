namespace grincoMessageApi.CIModels
{
    public class PostCreateConversationModel
    {
        public string Desc { get; set; } = "";
        public List<int> ParticipantIds { get; set; }  = new();
    }
    
}