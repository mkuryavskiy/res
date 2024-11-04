using System.Data;
using DAL.Models;
using Dapper;

namespace DAL.Data
{
    public class ReviewRepository
    {
        private readonly IDbConnection _connection;

        public ReviewRepository(IDbConnection connection)
        {
            _connection = connection;
        }

// Метод для створення відгуку
        public async Task<int> CreateAsync(Review review)
        {
            // Перевірка, чи існує користувач
            string checkUserSql = "SELECT COUNT(1) FROM Users WHERE Id = @UserId";
            var userExists = await _connection.ExecuteScalarAsync<bool>(checkUserSql, new { review.UserId });

            if (!userExists)
            {
                throw new Exception("User with the given UserId does not exist.");
            }

            // Якщо користувач існує, виконуємо операцію вставки
            string sql = @"
        INSERT INTO Reviews (UserId, Rating, Comment)
        VALUES (@UserId, @Rating, @Comment);
        SELECT SCOPE_IDENTITY();";

            return await _connection.ExecuteScalarAsync<int>(sql, review);
        }


        public async Task<IEnumerable<Review>> GetAllAsync()
        {
            string sql = @"
                SELECT r.*, u.*
                FROM Reviews r
                INNER JOIN Users u ON r.UserId = u.Id";

            var reviewDictionary = new Dictionary<int, Review>();

            return await _connection.QueryAsync<Review, User, Review>(
                sql,
                (review, user) => {
                    review.User = user;
                    return review;
                },
                splitOn: "Id"
            );
        }

        public async Task<Review?> GetAsync(int id)
        {
            string sql = @"
        SELECT r.*, u.*
        FROM Reviews r
        INNER JOIN Users u ON r.UserId = u.Id
        WHERE r.Id = @Id";

            var review = await Task.Run(() =>
                _connection.Query<Review, User, Review>(
                    sql,
                    (review, user) => {
                        review.User = user;
                        return review;
                    },
                    new { Id = id },
                    splitOn: "Id"
                ).SingleOrDefault()
            );

            return review;
        }


        public async Task DeleteAsync(int id)
        {
            string sql = "DELETE FROM Reviews WHERE Id = @Id";
            await _connection.ExecuteAsync(sql, new { Id = id });
        }
    }
}
