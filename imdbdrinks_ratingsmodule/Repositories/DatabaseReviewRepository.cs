using imdbdrinks_ratingsmodule.Domain;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;

namespace imdbdrinks_ratingsmodule.Repositories
{
    class DatabaseReviewRepository : IReviewRepository
    {
        private readonly string _connectionString;

        public DatabaseReviewRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void Delete(long reviewId)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new MySqlCommand("DELETE FROM Reviews WHERE ReviewId = @ReviewId", connection))
                {
                    command.Parameters.Add(new MySqlParameter("@ReviewId", reviewId));
                    command.ExecuteNonQuery();
                }
            }
        }

        public IEnumerable<Review> FindAll()
        {
            var reviews = new List<Review>();

            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new MySqlCommand("SELECT * FROM Reviews", connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            reviews.Add(new Review
                            {
                                ReviewId = reader.GetInt64("ReviewId"),
                                RatingId = reader.GetInt64("RatingId"),
                                UserId = reader.GetInt64("UserId"),
                                Content = reader.GetString("Content"),
                                CreationDate = reader.GetDateTime("CreationDate"),
                                IsActive = reader.GetBoolean("IsActive")
                            });
                        }
                    }
                }
            }

            return reviews;
        }

        public Review FindById(long reviewId)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new MySqlCommand("SELECT * FROM Reviews WHERE ReviewId = @ReviewId", connection))
                {
                    command.Parameters.Add(new MySqlParameter("@ReviewId", reviewId));

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Review
                            {
                                ReviewId = reader.GetInt64("ReviewId"),
                                RatingId = reader.GetInt64("RatingId"),
                                UserId = reader.GetInt64("UserId"),
                                Content = reader.GetString("Content"),
                                CreationDate = reader.GetDateTime("CreationDate"),
                                IsActive = reader.GetBoolean("IsActive")
                            };
                        }
                    }
                }
            }
            return null;
        }

        public IEnumerable<Review> FindByRatingId(long ratingId)
        {
            var reviews = new List<Review>();

            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new MySqlCommand("SELECT * FROM Reviews WHERE RatingId = @RatingId", connection))
                {
                    command.Parameters.Add(new MySqlParameter("@RatingId", ratingId));

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            reviews.Add(new Review
                            {
                                ReviewId = reader.GetInt64("ReviewId"),
                                RatingId = reader.GetInt64("RatingId"),
                                UserId = reader.GetInt64("UserId"),
                                Content = reader.GetString("Content"),
                                CreationDate = reader.GetDateTime("CreationDate"),
                                IsActive = reader.GetBoolean("IsActive")
                            });
                        }
                    }
                }
            }

            return reviews;
        }

        public Review Save(Review review)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new MySqlCommand())
                {
                    command.Connection = connection;

                    command.CommandText = "SELECT COUNT(*) FROM Reviews WHERE ReviewId = @ReviewId";
                    command.Parameters.Add(new MySqlParameter("@ReviewId", review.ReviewId));
                    var exists = Convert.ToInt32(command.ExecuteScalar()) > 0;


                    if (!exists)
                    {
                        command.Parameters.Clear();
                        command.CommandText = "INSERT INTO Reviews (RatingId, UserId, Content, CreationDate, IsActive) VALUES (@RatingId, @UserId, @Content, @CreationDate, @IsActive)";
                        command.Parameters.Add(new MySqlParameter("@RatingId", review.RatingId));
                        command.Parameters.Add(new MySqlParameter("@UserId", review.UserId));
                        command.Parameters.Add(new MySqlParameter("@Content", review.Content));
                        command.Parameters.Add(new MySqlParameter("@CreationDate", review.CreationDate));
                        command.Parameters.Add(new MySqlParameter("@IsActive", review.IsActive));
                        command.ExecuteNonQuery();
                        review.ReviewId = command.LastInsertedId;
                    }
                    else
                    {
                        command.Parameters.Clear();
                        command.CommandText = "UPDATE Reviews SET RatingId = @RatingId, UserId = @UserId, Content = @Content, CreationDate = @CreationDate, IsActive = @IsActive WHERE ReviewId = @ReviewId";
                        command.Parameters.Add(new MySqlParameter("@ReviewId", review.ReviewId));
                        command.Parameters.Add(new MySqlParameter("@RatingId", review.RatingId));
                        command.Parameters.Add(new MySqlParameter("@UserId", review.UserId));
                        command.Parameters.Add(new MySqlParameter("@Content", review.Content));
                        command.Parameters.Add(new MySqlParameter("@CreationDate", review.CreationDate));
                        command.Parameters.Add(new MySqlParameter("@IsActive", review.IsActive));
                        command.ExecuteNonQuery();
                    }
                }
            }

            return review;
        }
    }
}
