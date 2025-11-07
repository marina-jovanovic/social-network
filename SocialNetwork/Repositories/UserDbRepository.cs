using System;
using Microsoft.Data.Sqlite;
using System.Collections.Generic;
using SocialNetwork.Models;

namespace SocialNetwork.Repositories
{
    public class UserDbRepository
    {
        private readonly string _connectionString = "Data Source=database/database.db";

        public List<User> GetAll()
        {
            var users = new List<User>();

            try
            {
                using SqliteConnection connection = new SqliteConnection(_connectionString);
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
            catch (Exception ex)
            {
                throw new Exception("An error has occurred while retrieving users from the database.", ex);
            }
            return users;
        }

        public User GetById(int id)
        {
            using SqliteConnection connection = new SqliteConnection(_connectionString);
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
    }
}