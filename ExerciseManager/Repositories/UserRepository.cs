using ExerciseManager.Models;
using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Net;
using System.Text;
using System.Data;
using System.Windows;

namespace ExerciseManager.Repositories
{
    public class UserRepository : RepositoryBase, IUserRepository
    {
        public SQLResults Add(UserModel userModel)
        {
            if (GetByUsername(userModel.Username) != null)
            {
                return SQLResults.UserAlreadyExists;
            }
            else
            {

                int numberOfRowsAffected;
                using (var connection = GetConnection())
                using (var command = new SqlCommand())
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandText = "INSERT INTO dbo.Users (Nickname, Password) Values (@nickname, @password)";
                    command.Parameters.Add("@nickname", SqlDbType.NVarChar).Value = userModel.Username;
                    command.Parameters.Add("@password", SqlDbType.NVarChar).Value = userModel.Password;
                    numberOfRowsAffected = command.ExecuteNonQuery();
                }
                return numberOfRowsAffected > 0 ? SQLResults.Success : SQLResults.Failure;
            }
        }

        public bool AuthenticateUser(NetworkCredential credential)
        {
            bool validUser;
            using (var connection = GetConnection())
            using(var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "Select * from dbo.Users where Nickname=@nickname AND Password=@password";
                command.Parameters.Add("@nickname", SqlDbType.NVarChar).Value = credential.UserName;
                command.Parameters.Add("@password", SqlDbType.NVarChar).Value = credential.Password;
                validUser = command.ExecuteScalar() == null ? false: true ;
            }
            return validUser;
        }

        public void Edit(UserModel userModel)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<UserModel> GetByAll()
        {
            throw new NotImplementedException();
        }

        public UserModel GetById(int id)
        {
            throw new NotImplementedException();
        }

        public UserModel GetByUsername(string username)
        {
            UserModel userModel = null;
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "Select * from dbo.Users where Nickname=@nickname";
                command.Parameters.Add("@nickname", SqlDbType.NVarChar).Value = username;
                
                using (SqlDataReader reader = command.ExecuteReader()) 
                {
                    if (reader.HasRows == true)
                        userModel = new UserModel();
                    while(reader.Read())
                    {
                        userModel.Id = reader["Id_User"].ToString();
                        userModel.Username = reader["Nickname"].ToString();
                        userModel.Password = reader["Password"].ToString();

                    }
                    reader.Close();
                }
            }
            return userModel;
        }

        public void Remove(int id)
        {
            throw new NotImplementedException();
        }
    }
}
