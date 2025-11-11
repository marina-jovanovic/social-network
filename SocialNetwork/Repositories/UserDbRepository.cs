using System;
using Microsoft.Data.Sqlite;
using System.Collections.Generic;
using SocialNetwork.Models;

namespace SocialNetwork.Repositories
{
    public class UserDbRepository
    {
        private readonly string connectionString;
        public UserDbRepository(IConfiguration configuration)
        {
            connectionString = configuration["ConnectionString:SQLiteConnection"];
        }

        public List<User> GetAll()
        {
            var users = new List<User>();

            try
            {
                using SqliteConnection connection = new SqliteConnection(connectionString);
                connection.Open();

                string query = "SELECT Id, Username, Name, Surname, Birthday FROM Users";

                using SqliteCommand command = new SqliteCommand(query, connection);
                using SqliteDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    users.Add(new User
                    {
                        Id = reader.GetInt32(0),
                        Name = reader.GetString(2),
                        Surname = reader.GetString(3),
                        DateOfBirth = DateTime.Parse(reader.GetString(4))
                    });
                }
            }

            catch (SqliteException ex)
            {
                throw new Exception("A database error has occurred while retrieving users.", ex);
            }

            catch (FormatException ex)
            {
                throw new Exception("Data format error while processing user records.", ex);
            }

            catch (InvalidOperationException ex)
            {
                throw new Exception("An invalid operation occurred while accessing the database.", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("An error has occurred while retrieving users from the database.", ex);
            }
            return users;
        }

        public User GetById(int id)
        {
            try
            {
                using SqliteConnection connection = new SqliteConnection(connectionString);
                connection.Open();

                string query = "SELECT Id, Username, Name, Surname, Birthday FROM Users WHERE Id = @id";

                using SqliteCommand command = new SqliteCommand(query, connection);
                command.Parameters.AddWithValue("@id", id);

                using SqliteDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    return new User
                    {
                        Id = reader.GetInt32(0),
                        Name = reader.GetString(2),
                        Surname = reader.GetString(3),
                        DateOfBirth = DateTime.Parse(reader.GetString(4))
                    };
                }
                else
                {
                    return null;
                }
            }
            catch (SqliteException ex)
            {
                throw new Exception("A database error has occurred while retrieving users.", ex);
            }

            catch (FormatException ex)
            {
                throw new Exception("Data format error while processing user records.", ex);
            }

            catch (InvalidOperationException ex)
            {
                throw new Exception("An invalid operation occurred while accessing the database.", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("An error has occurred while retrieving users from the database.", ex);
            }
        }

        public User Create(User user)
        {
            try
            {
                using SqliteConnection connection = new SqliteConnection(connectionString);
                connection.Open();

                string query = @"INSERT INTO Users (Username, Name, Surname, Birthday)
                            VALUES (@username, @name, @surname, @birthday);
                            SELECT LAST_INSERT_ROWID()";

                using SqliteCommand command = new SqliteCommand(query, connection);

                command.Parameters.AddWithValue("@username", $"{user.Name.ToLower()}_{user.Surname.ToLower()}");
                command.Parameters.AddWithValue("@name", user.Name);
                command.Parameters.AddWithValue("@surname", user.Surname);
                command.Parameters.AddWithValue("@birthday", user.DateOfBirth.ToString("yyyy-MM-dd"));

                object result = command.ExecuteScalar();

                if (result != null)
                {
                    user.Id = Convert.ToInt32(result);
                }

                return user;
            }
            catch (SqliteException ex)
            {
                throw new Exception("A database error has occurred while retrieving users.", ex);
            }

            catch (FormatException ex)
            {
                throw new Exception("Data format error while processing user records.", ex);
            }

            catch (InvalidOperationException ex)
            {
                throw new Exception("An invalid operation occurred while accessing the database.", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("An error has occurred while retrieving users from the database.", ex);
            }
        }

        public bool Update(User user)
        {
            try
            {
                using SqliteConnection connection = new SqliteConnection(connectionString);
                connection.Open();

                string query = @"UPDATE Users 
                         SET Name = @name, Surname = @surname, Birthday = @birthday 
                         WHERE Id = @id";

                using SqliteCommand command = new SqliteCommand(query, connection);

                command.Parameters.AddWithValue("@name", user.Name);
                command.Parameters.AddWithValue("@surname", user.Surname);
                command.Parameters.AddWithValue("@birthday", user.DateOfBirth.ToString("yyyy-MM-dd"));
                command.Parameters.AddWithValue("@id", user.Id);

                int rowsAffected = command.ExecuteNonQuery();

                return rowsAffected > 0;
            }
            catch (SqliteException ex)
            {
                throw new Exception("A database error has occurred while retrieving users.", ex);
            }

            catch (InvalidOperationException ex)
            {
                throw new Exception("An invalid operation occurred while accessing the database.", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("An error has occurred while retrieving users from the database.", ex);
            }
        }

        public bool Delete(int id)
        {
            try
            {
                using SqliteConnection connection = new SqliteConnection(connectionString);
                connection.Open();

                string query = "DELETE FROM Users WHERE Id = @id";

                using SqliteCommand command = new SqliteCommand(query, connection);

                command.Parameters.AddWithValue("@id", id);

                int rowsAffected = command.ExecuteNonQuery();

                return rowsAffected > 0;
            }
            catch (SqliteException ex)
            {
                throw new Exception("A database error has occurred while retrieving users.", ex);
            }

            catch (InvalidOperationException ex)
            {
                throw new Exception("An invalid operation occurred while accessing the database.", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("An error has occurred while retrieving users from the database.", ex);
            }
        }
    }
}