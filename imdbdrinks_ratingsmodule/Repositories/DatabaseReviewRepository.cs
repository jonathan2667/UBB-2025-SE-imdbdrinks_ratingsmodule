using imdbdrinks_ratingsmodule.Domain;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

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
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("DELETE FROM Reviews WHERE ReviewId = @ReviewId", connection))
                {
                    command.Parameters.AddWithValue("@ReviewId", reviewId);
                    command.ExecuteNonQuery();
                }
            }
        }

        public IEnumerable<Review> FindAll()
        {
            var reviews = new List<Review>();

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("SELECT * FROM Reviews", connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        reviews.Add(new Review
                        {
                            ReviewId = reader.GetInt64(reader.GetOrdinal("ReviewId")),
                            RatingId = reader.GetInt64(reader.GetOrdinal("RatingId")),
                            UserId = reader.GetInt64(reader.GetOrdinal("UserId")),
                            Content = reader.GetString(reader.GetOrdinal("Content")),
                            CreationDate = reader.GetDateTime(reader.GetOrdinal("CreationDate")),
                            IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive"))
                        });
                    }
                }
            }

            return reviews;
        }

        public Review FindById(long reviewId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("SELECT * FROM Reviews WHERE ReviewId = @ReviewId", connection))
                {
                    command.Parameters.AddWithValue("@ReviewId", reviewId);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Review
                            {
                                ReviewId = reader.GetInt64(reader.GetOrdinal("ReviewId")),
                                RatingId = reader.GetInt64(reader.GetOrdinal("RatingId")),
                                UserId = reader.GetInt64(reader.GetOrdinal("UserId")),
                                Content = reader.GetString(reader.GetOrdinal("Content")),
                                CreationDate = reader.GetDateTime(reader.GetOrdinal("CreationDate")),
                                IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive"))
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

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("SELECT * FROM Reviews WHERE RatingId = @RatingId", connection))
                {
                    command.Parameters.AddWithValue("@RatingId", ratingId);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            reviews.Add(new Review
                            {
                                ReviewId = reader.GetInt32(reader.GetOrdinal("ReviewId")),
                                RatingId = reader.GetInt32(reader.GetOrdinal("RatingId")),
                                UserId = reader.GetInt32(reader.GetOrdinal("UserId")),
                                Content = reader.GetString(reader.GetOrdinal("Content")),
                                CreationDate = reader.GetDateTime(reader.GetOrdinal("CreationDate")),
                                IsActive = Convert.ToBoolean(reader["IsActive"])
                            });
                        }
                    }
                }
            }

            return reviews;
        }

        public Review Save(Review review)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand())
                {
                    command.Connection = connection;

                    command.CommandText = "SELECT COUNT(*) FROM Reviews WHERE ReviewId = @ReviewId";
                    command.Parameters.AddWithValue("@ReviewId", review.ReviewId);
                    var exists = Convert.ToInt32(command.ExecuteScalar()) > 0;

                    command.Parameters.Clear();

                    if (!exists)
                    {
                        command.CommandText = @"
                            INSERT INTO Reviews (RatingId, UserId, Content, CreationDate, IsActive)
                            OUTPUT INSERTED.ReviewId
                            VALUES (@RatingId, @UserId, @Content, @CreationDate, @IsActive)";
                        command.Parameters.AddWithValue("@RatingId", review.RatingId);
                        command.Parameters.AddWithValue("@UserId", review.UserId);
                        command.Parameters.AddWithValue("@Content", review.Content);
                        command.Parameters.AddWithValue("@CreationDate", review.CreationDate);
                        command.Parameters.AddWithValue("@IsActive", review.IsActive);

                        review.ReviewId = (int)command.ExecuteScalar();
                    }
                    else
                    {
                        command.CommandText = @"
                            UPDATE Reviews
                            SET RatingId = @RatingId,
                                UserId = @UserId,
                                Content = @Content,
                                CreationDate = @CreationDate,
                                IsActive = @IsActive
                            WHERE ReviewId = @ReviewId";
                        command.Parameters.AddWithValue("@ReviewId", review.ReviewId);
                        command.Parameters.AddWithValue("@RatingId", review.RatingId);
                        command.Parameters.AddWithValue("@UserId", review.UserId);
                        command.Parameters.AddWithValue("@Content", review.Content);
                        command.Parameters.AddWithValue("@CreationDate", review.CreationDate);
                        command.Parameters.AddWithValue("@IsActive", review.IsActive);

                        command.ExecuteNonQuery();
                    }
                }
            }

            return review;
        }
    }
}
