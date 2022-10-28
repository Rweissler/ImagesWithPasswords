using ImagesWithPasswords.Data1;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace ImagesWithPasswords.Data
{
   public class ImageRepository
    {
        private string _connectionString;
        public ImageRepository(string connectionString)
        {
            connectionString = _connectionString;
        }

        public void AddImage(Image image)
        {
            using var connection = new SqlConnection(_connectionString);
            using var cmd = connection.CreateCommand();
            cmd.CommandText = @"INSERT INTO Images(@fileName, @password, @viewCount)SELECT SCOPE_IDENTITY()";
            cmd.Parameters.AddWithValue("@fileName", image.FileName);
            cmd.Parameters.AddWithValue("@password", image.Password);
            cmd.Parameters.AddWithValue("@viewCount", image.ViewCount);
            connection.Open();
            image.Id = (int)(decimal)cmd.ExecuteScalar();
        }

        public Image GetImage(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            using var cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT * FROM Images WHERE Id = @id";
            cmd.Parameters.AddWithValue("@id", id);
            connection.Open();
            var reader = cmd.ExecuteReader();
            if(!reader.Read())
            {
                return null;
            }
            return new Image
            {
                Id = (int)reader["Id"],
                FileName = (string)reader["FileName"],
                Password = (string)reader["Password"],
                ViewCount = (int)reader["ViewCount"]
            };

        }

        public string GetPassword(int Id)
        {
            var connection = new SqlConnection(_connectionString);
            var cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT * FROM Images WHERE Id = @id";
            cmd.Parameters.AddWithValue("@id", Id);
            connection.Open();
            return (string)cmd.ExecuteScalar();
        }
        public void IncrementImagesVIewCount(int Id)
        {
            var connection = new SqlConnection(_connectionString);
            var cmd = connection.CreateCommand();
            cmd.CommandText = "UPDATE Images SET ViewCount += 1 WHERE Id = @id";
            cmd.Parameters.AddWithValue("@id", Id);
            connection.Open();
            cmd.ExecuteNonQuery();
        }
    }
}
