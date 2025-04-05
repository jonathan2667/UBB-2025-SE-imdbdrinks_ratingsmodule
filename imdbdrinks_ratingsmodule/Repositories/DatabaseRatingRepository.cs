using imdbdrinks_ratingsmodule.Domain;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace imdbdrinks_ratingsmodule.Repositories
{
    class DatabaseRatingRepository : IRatingRepository
    {
        private readonly string _connectionString;

        public DatabaseRatingRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void Delete(long ratingId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("DELETE FROM Ratings WHERE RatingId = @RatingId", connection))
                {
                    command.Parameters.AddWithValue("@RatingId", ratingId);
                    command.ExecuteNonQuery();
                }
            }
        }

        public IEnumerable<Rating> FindAll()
        {
            var ratings = new List<Rating>();

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("SELECT * FROM Ratings", connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ratings.Add(new Rating
                        {
                            RatingId = reader.GetInt32(reader.GetOrdinal("RatingId")),
                            ProductId = reader.GetInt32(reader.GetOrdinal("ProductId")),
                            UserId = reader.GetInt32(reader.GetOrdinal("UserId")),
                            RatingValue = reader.GetInt32(reader.GetOrdinal("RatingValue")),
                            RatingDate = reader.GetDateTime(reader.GetOrdinal("RatingDate")),
                            IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive"))
                        });
                    }
                }
            }

            return ratings;
        }

        public Rating FindById(long ratingId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("SELECT * FROM Ratings WHERE RatingId = @RatingId", connection))
                {
                    command.Parameters.AddWithValue("@RatingId", ratingId);
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Rating
                            {
                                RatingId = reader.GetInt32(reader.GetOrdinal("RatingId")),
                                ProductId = reader.GetInt32(reader.GetOrdinal("ProductId")),
                                UserId = reader.GetInt32(reader.GetOrdinal("UserId")),
                                RatingValue = reader.GetInt32(reader.GetOrdinal("RatingValue")),
                                RatingDate = reader.GetDateTime(reader.GetOrdinal("RatingDate")),
                                IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive"))
                            };
                        }
                    }
                }
            }

            return null;
        }

        public IEnumerable<Rating> FindByProductId(long productId)
        {
            var ratings = new List<Rating>();

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("SELECT * FROM Ratings WHERE ProductId = @ProductId", connection))
                {
                    command.Parameters.AddWithValue("@ProductId", productId);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ratings.Add(new Rating
                            {
                                RatingId = Convert.ToInt32(reader.GetInt32(reader.GetOrdinal("RatingId"))),
                                ProductId = Convert.ToInt32(reader.GetInt32(reader.GetOrdinal("ProductId"))),
                                UserId = Convert.ToInt32(reader.GetInt32(reader.GetOrdinal("UserId"))),
                                RatingValue = reader.GetDouble(reader.GetOrdinal("RatingValue")),
                                RatingDate = reader.GetDateTime(reader.GetOrdinal("RatingDate")),
                                IsActive = Convert.ToBoolean(reader["IsActive"])
                            });
                        }
                    }
                }
            }

            return ratings;
        }

        public Rating Save(Rating rating)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand())
                {
                    command.Connection = connection;

                    // Check existence
                    command.CommandText = "SELECT COUNT(*) FROM Ratings WHERE RatingId = @RatingId";
                    command.Parameters.AddWithValue("@RatingId", rating.RatingId);

                    var exists = Convert.ToInt32(command.ExecuteScalar()) > 0;
                    command.Parameters.Clear();

                    if (!exists)
                    {
                        command.CommandText = @"
                            INSERT INTO Ratings (ProductId, UserId, RatingValue, RatingDate, IsActive) 
                            OUTPUT INSERTED.RatingId 
                            VALUES (@ProductId, @UserId, @RatingValue, @RatingDate, @IsActive)";
                        command.Parameters.AddWithValue("@ProductId", rating.ProductId);
                        command.Parameters.AddWithValue("@UserId", rating.UserId);
                        command.Parameters.AddWithValue("@RatingValue", rating.RatingValue);
                        command.Parameters.AddWithValue("@RatingDate", rating.RatingDate);
                        command.Parameters.AddWithValue("@IsActive", rating.IsActive);
                        
                        rating.RatingId = (int)command.ExecuteScalar();
                    }
                    else
                    {
                        command.CommandText = @"
                            UPDATE Ratings 
                            SET ProductId = @ProductId, UserId = @UserId, RatingValue = @RatingValue, RatingDate = @RatingDate, IsActive = @IsActive 
                            WHERE RatingId = @RatingId";
                        command.Parameters.AddWithValue("@ProductId", rating.ProductId);
                        command.Parameters.AddWithValue("@UserId", rating.UserId);
                        command.Parameters.AddWithValue("@RatingValue", rating.RatingValue);
                        command.Parameters.AddWithValue("@RatingDate", rating.RatingDate);
                        command.Parameters.AddWithValue("@IsActive", rating.IsActive);
                        command.Parameters.AddWithValue("@RatingId", rating.RatingId);

                        command.ExecuteNonQuery();
                    }
                }
            }

            return rating;
        }
    }
}
