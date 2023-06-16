using Dapper;
using grincoAppModels;
using Microsoft.AspNetCore.Identity;
using MySqlConnector;

namespace grincoMessageApi.Data
{
    public class UserRepository : IUserRepository
    {
        private readonly string _connectionString;

        public UserRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("Default");
        }

        public async Task<IEnumerable<User>> GetAllUsers()
        {
            using var connection = new MySqlConnection(_connectionString);
            const string sql = "SELECT * FROM grincochatbdd.user";
            return await connection.QueryAsync<User>(sql);
        }

        public async Task<User> GetUserById(int id)
        {
                using var connection = new MySqlConnection(_connectionString);
                const string sql = "SELECT * FROM user WHERE id = @Id";
                return await connection.QueryFirstOrDefaultAsync<User>(sql, new { Id = id });
        }

        public async Task<User> GetUserByLogin(string login)
        {
                using var connection = new MySqlConnection(_connectionString);
                const string sql = "SELECT * FROM user WHERE login = @Login";
                return await connection.QueryFirstOrDefaultAsync<User>(sql, new { Login = login });
        }

        public async Task<int> CreateUser(User user)
        {
                using var connection = new MySqlConnection(_connectionString);
                const string sql = "INSERT INTO user (login, password) VALUES (@Login, @Password)";
                return await connection.ExecuteAsync(sql, new { user.Login, user.Password});
        }

        public async Task<int> UpdateUser(User user)
        {
                using var connection = new MySqlConnection(_connectionString);
                const string sql = "UPDATE user SET login = @Login, password = @Password WHERE id = @Id";
                return await connection.ExecuteAsync(sql, new { user.Login, user.Password});
        }

        public async Task<int> DeleteUser(int id)
        {
                using var connection = new MySqlConnection(_connectionString);
                const string sql = "DELETE FROM user WHERE id = @Id";
                return await connection.ExecuteAsync(sql, new { Id = id });
        }
    }
}