using grincoAppModels;

namespace grincoMessageApi.Data
{
    public interface IConversationRepository
    {
        Task<IEnumerable<Conversation>> GetConversationsByParticipantId(int participantId);
        Task<IEnumerable<User>> GetParticipants(int conversationId);
        Task<Conversation> GetConversationById(int id);
        Task<int> CreateConversation(Conversation conversation);
        Task<int> DeleteConversation(int id);
        Task<int> AddParticipants(int conversationId,List<int> participantIds);
        Task<int> RemoveParticipant(int conversationId, int participantId);
    }
}

