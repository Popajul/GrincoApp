namespace grincoMessageApi.CIModels
{
    public class PostCreateMessageModel
    {
        public string Content { get; set; }
        public int Senderid { get; set; }
        public int ConversationId { get; set; }
    }
}