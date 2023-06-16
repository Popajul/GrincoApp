using grincoAppModels;

namespace grincoMessageApi.Data
{
    public interface IMessageRepository
    {
        Task<IEnumerable<IGrouping<int,Message>>> GetMessagesByParticipantIdOrderByConversation(int participantId);
        Task<IEnumerable<Message>> GetMessagesByConversation(int conversationId);
        Task<int> CreateMessage(Message message);
        Task<int> DeleteMessageById(int id);
        Task<int> UpdateMessageById(int id, string content);
    }
}

