using grincoAppModels;
using MySqlConnector;
using Dapper;
using System.Globalization;
using grincoMessageApi.Controllers;

namespace grincoMessageApi.Data
{
    public class ConversationRepository : IConversationRepository
    {
        private readonly string  _connectionString;
        
        public ConversationRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("Default");
        }
        public async Task<IEnumerable<Conversation>> GetConversationsByParticipantId(int participantId)
        {
            using var connection = new MySqlConnection(_connectionString);
            const string sql = 
            @"WITH convids AS
            (
                SELECT *
                FROM conversation_participant
                WHERE participantid = @ParticipantId
            )
            SELECT * from conversation
            WHERE conversation.id IN (SELECT conversationid FROM convids)";

            IEnumerable<Conversation> conversations = (await connection.QueryAsync<Conversation>(sql, new {ParticipantId = participantId}))??Enumerable.Empty<Conversation>();
            return conversations;
        }

        public async Task<Conversation> GetConversationById(int id)
        {
            using var connection = new MySqlConnection(_connectionString);
            const string sql = 
            @"
            SELECT * from conversation
            WHERE conversation.id = @Id";

            Conversation conversation = await connection.QuerySingleOrDefaultAsync<Conversation>(sql, new {Id = id});
            return conversation;
        }

        public async Task<IEnumerable<User>> GetParticipants(int conversationId)
        {
            using var connection = new MySqlConnection(_connectionString);
            const string sql = 
            @"WITH participants AS
            (
                SELECT * from conversation_participant
                WHERE conversationid = @Id
            )
            SELECT * FROM user
            WHERE user.id IN (SELECT participantid FROM participants)
            ";

            List<User> participants = (await connection.QueryAsync<User>(sql, new {Id = conversationId})).ToList();
            return participants;
        }
        public async Task<int>  CreateConversation(Conversation conversation)
        {
            using var connection = new MySqlConnection(_connectionString);
            const string sql = "INSERT INTO conversation (description) VALUES (@Description);\nSELECT LAST_INSERT_ID();";
            return await connection.ExecuteScalarAsync<int>(sql, new {conversation.Description});
        }

        public async Task<int> DeleteConversation(int id)
        {
            using var connection = new MySqlConnection(_connectionString);
            var param = new {Id = id};
            const string sql1 = "DELETE FROM conversation_participant WHERE conversationid = @Id";
            const string sql2 = "DELETE FROM message WHERE conversationid = @Id";
            const string sql3 = "DELETE FROM conversation WHERE id = @Id";
            _ = await connection.ExecuteAsync(sql1, param);
            _ = await connection.ExecuteAsync(sql2, param);
            return await connection.ExecuteAsync(sql3, param);
        }

        public async Task<int> AddParticipants(int conversationId,List<int> participantIds)
        {
            using var connection = new MySqlConnection(_connectionString);
            var values = participantIds.Select(pId => new {ConversationId = conversationId, ParticipantId= pId} ).ToList();
            const string sql = "INSERT INTO conversation_participant (conversationid, participantid) VALUES (@ConversationId, @ParticipantId)";
            return await connection.ExecuteAsync(sql, values);
        }

        public async Task<int> RemoveParticipant(int conversationId, int participantId)
        {
            using var connection = new MySqlConnection(_connectionString);
            var value = new {ConversationId = conversationId, ParticipantId = participantId};
            const string sql = "DELETE FROM conversation_participant WHERE (conversationid, participantid) = (@ConversationId, @ParticipantId)";
            return await connection.ExecuteAsync(sql, value);
        }
    }
}

