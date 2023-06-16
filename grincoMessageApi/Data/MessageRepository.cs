using grincoAppModels;
using MySqlConnector;
using Dapper;

namespace grincoMessageApi.Data
{
    public class MessageRepository : IMessageRepository
    {
        private readonly string  _connectionString;
        public MessageRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("Default");
        }
        public async Task<IEnumerable<IGrouping<int,Message>>> GetMessagesByParticipantIdOrderByConversation(int participantId)
        {
            using var connection = new MySqlConnection(_connectionString);
            const string sql = 
            @"WITH convids AS
            (
                SELECT *
                FROM conversation_participant
                WHERE participantid = @ParticipantId
            )
            SELECT * from message
            WHERE message.conversationId IN (SELECT conversationid FROM convids)";

            IEnumerable<Message> messages = (await connection.QueryAsync<Message>(sql, new {ParticipantId = participantId}))??Enumerable.Empty<Message>();
            return messages.GroupBy(message=>message.ConversationId);
        }

        public async Task<IEnumerable<Message>> GetMessagesByConversation(int conversationId)
        {
            using var connection = new MySqlConnection(_connectionString);
            const string sql = 
            @"
            SELECT * from message
            WHERE message.conversationId = @ConversationId";

            return (await connection.QueryAsync<Message>(sql, new {ConversationId = conversationId}))??Enumerable.Empty<Message>();
        }
        public async Task<int>  CreateMessage(Message message)
        {
            using var connection = new MySqlConnection(_connectionString);
            const string sql = @"INSERT INTO message (content, timestamp, senderid, conversationid) VALUES (@Content, @Timestamp, @SenderId, @ConversationId);
            SELECT LAST_INSERT_ID();";
            return await connection.ExecuteScalarAsync<int>(sql, new {message.Content, Timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), message.SenderId, message.ConversationId});
        }

        public async Task<int> DeleteMessageById(int id)
        {
            using var connection = new MySqlConnection(_connectionString);
            const string sql = "DELETE FROM message WHERE id = @Id";
            return await connection.ExecuteAsync(sql, new {Id = id});
        }

        public async Task<int> UpdateMessageById(int id, string content)
        {
            using var connection = new MySqlConnection(_connectionString);
            const string sql = "UPDATE message SET content = @Content WHERE id = @Id";
            return await connection.ExecuteAsync(sql, new {Id = id, Content = content});
        }
    }
}

