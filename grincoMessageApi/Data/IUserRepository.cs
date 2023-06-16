using grincoAppModels;

namespace grincoMessageApi.Data
{
    public interface IUserRepository
{
    Task<IEnumerable<User>> GetAllUsers();
    Task<User> GetUserById(int id);
    Task<User> GetUserByLogin(string login);
    Task<int> CreateUser(User user);
    Task<int> UpdateUser(User user);
    Task<int> DeleteUser(int id);
}
}

