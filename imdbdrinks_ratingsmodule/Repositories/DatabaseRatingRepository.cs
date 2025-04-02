using imdbdrinks_ratingsmodule.Domain;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;

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
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new MySqlCommand("DELETE FROM Ratings WHERE RatingId = @RatingId", connection))
                {
                    command.Parameters.Add(new MySqlParameter("@RatingId", ratingId));
                    command.ExecuteNonQuery();
                }
            }
        }

        public IEnumerable<Rating> FindAll()
        {
            var ratings = new List<Rating>();

            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new MySqlCommand("SELECT * FROM Ratings", connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ratings.Add(new Rating
                            {
                                RatingId = reader.GetInt64("RatingId"),
                                ProductId = reader.GetInt64("ProductId"),
                                UserId = reader.GetInt64("UserId"),
                                RatingValue = reader.GetInt32("RatingValue"),
                                RatingDate = reader.GetDateTime("RatingDate"),
                                IsActive = reader.GetBoolean("IsActive")
                            });
                        }
                    }
                }
            }

            return ratings;
        }

        public Rating FindById(long ratingId)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new MySqlCommand("SELECT * FROM Ratings WHERE RatingId = @RatingId", connection))
                {
                    command.Parameters.Add(new MySqlParameter("@RatingId", ratingId));

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Rating
                            {
                                RatingId = reader.GetInt64("RatingId"),
                                ProductId = reader.GetInt64("ProductId"),
                                UserId = reader.GetInt64("UserId"),
                                RatingValue = reader.GetInt32("RatingValue"),
                                RatingDate = reader.GetDateTime("RatingDate"),
                                IsActive = reader.GetBoolean("IsActive")
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

            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new MySqlCommand("SELECT * FROM Ratings WHERE ProductId = @ProductId", connection))
                {
                    command.Parameters.Add(new MySqlParameter("@ProductId", productId));

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ratings.Add(new Rating
                            {
                                RatingId = reader.GetInt64("RatingId"),
                                ProductId = reader.GetInt64("ProductId"),
                                UserId = reader.GetInt64("UserId"),
                                RatingValue = reader.GetInt32("RatingValue"),
                                RatingDate = reader.GetDateTime("RatingDate"),
                                IsActive = reader.GetBoolean("IsActive")
                            });
                        }
                    }
                }
            }

            return ratings;
        }

        public Rating Save(Rating rating)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new MySqlCommand())
                {
                    command.Connection = connection;

                    // Check if the rating already exists
                    command.CommandText = "SELECT COUNT(*) FROM Ratings WHERE RatingId = @RatingId";
                    command.Parameters.Add(new MySqlParameter("@RatingId", rating.RatingId));
                    var exists = Convert.ToInt32(command.ExecuteScalar()) > 0;

                    if (!exists)
                    {
                        // Insert new rating
                        command.CommandText = "INSERT INTO Ratings (ProductId, UserId, RatingValue, RatingDate, IsActive) VALUES (@ProductId, @UserId, @RatingValue, @RatingDate, @IsActive)";
                        command.Parameters.Add(new MySqlParameter("@ProductId", rating.ProductId));
                        command.Parameters.Add(new MySqlParameter("@UserId", rating.UserId));
                        command.Parameters.Add(new MySqlParameter("@RatingValue", rating.RatingValue));
                        command.Parameters.Add(new MySqlParameter("@RatingDate", rating.RatingDate));
                        command.Parameters.Add(new MySqlParameter("@IsActive", rating.IsActive));
                        command.ExecuteNonQuery();
                        rating.RatingId = command.LastInsertedId;
                    }
                    else
                    {
                        // Update existing rating
                        command.CommandText = "UPDATE Ratings SET ProductId = @ProductId, UserId = @UserId, RatingValue = @RatingValue, RatingDate = @RatingDate, IsActive = @IsActive WHERE RatingId = @RatingId";
                        command.Parameters.Add(new MySqlParameter("@ProductId", rating.ProductId));
                        command.Parameters.Add(new MySqlParameter("@UserId", rating.UserId));
                        command.Parameters.Add(new MySqlParameter("@RatingValue", rating.RatingValue));
                        command.Parameters.Add(new MySqlParameter("@RatingDate", rating.RatingDate));
                        command.Parameters.Add(new MySqlParameter("@IsActive", rating.IsActive));
                        command.ExecuteNonQuery();
                    }
                }
            }

            return rating;
        }
    }
}
