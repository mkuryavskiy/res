using System.Data;
using DAL.Models;
using Dapper;

namespace DAL.Data
{
    public class UserRepository
    {
        private readonly IDbConnection _connection;

        public UserRepository(IDbConnection connection)
        {
            _connection = connection;
        }

        public async Task<int> CreateAsync(User user)
        {
            string sql = @"
                INSERT INTO Users (Name)
                VALUES (@Name)
                SELECT SCOPE_IDENTITY();";

            return await _connection.ExecuteScalarAsync<int>(sql, user);
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            string sql = "SELECT * FROM Users";
            return await _connection.QueryAsync<User>(sql);
        }

        public async Task<User?> GetAsync(int id)
        {
            string sql = "SELECT * FROM Users WHERE Id = @Id";
            return await _connection.QuerySingleOrDefaultAsync<User>(sql, new { Id = id });
        }

        public async Task DeleteAsync(int id)
        {
            string sql = "DELETE FROM Users WHERE Id = @Id";
            await _connection.ExecuteAsync(sql, new { Id = id });
        }
    }
}
