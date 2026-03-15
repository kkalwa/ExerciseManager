using ExerciseManager.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Text;

namespace ExerciseManager.Repositories
{
    public class DBRepository : RepositoryBase, IDBRepository
    {
        public List<string> GetDistinctIdSetsForUser(string id_user)
        {
            List<string> output = new();
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "SELECT DISTINCT Id_ExerciseSet\r\n" +
                    "FROM [dbo].[ExerciseSet]\r\n" +
                    "WHERE Id_User=@id_user\r\n" +
                    "ORDER BY Id_ExerciseSet ASC";
                command.Parameters.Add("@id_user", SqlDbType.Int).Value = Int32.Parse(id_user);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        output.Add(reader["Id_ExerciseSet"].ToString());
                    }
                    reader.Close();
                }
            }
            return output;
        }

        public List<ManageExercisesModel> GetExercisesBasedOnUserId(string Id)
        {
            List<ManageExercisesModel> output = new List<ManageExercisesModel>();
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "SELECT [dbo].[ExerciseSet].[Id_User],\r\n" +
                    "[dbo].[ExerciseSet].[Title],\r\n" +
                    "[dbo].[Exercises].[Id_ExerciseSet], \r\n" +
                    "[dbo].[Exercises].[Id_Exercise], \r\n" +
                    "[dbo].[Exercises].[Exercise_Name], \r\n" +
                    "[dbo].[Exercises].[Weight1],\r\n" +
                    "[dbo].[Exercises].[Weight2],\r\n" +
                    "[dbo].[Exercises].[Weight3],\r\n" +
                    "[dbo].[Exercises].[Weight4],\r\n" +
                    "[dbo].[Exercises].[Weight5]\r\n" +
                    "FROM [dbo].[Exercises], [dbo].[ExerciseSet]\r\n" +
                    "WHERE [dbo].[Exercises].[Id_ExerciseSet] = [dbo].[ExerciseSet].[Id_ExerciseSet] AND" +
                    "[dbo].[ExerciseSet].[Id_User] = @user_id\r\n" +
                    "ORDER By [dbo].[Exercises].[Id_ExerciseSet] ASC";

                command.Parameters.Add("@user_id", SqlDbType.Int).Value = Int32.Parse(Id);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ManageExercisesModel newEntry = new ManageExercisesModel()
                        {
                            IdUser = Id,
                            ExerciseSetTitle = reader["Title"].ToString(),
                            IdExerciseSet = reader["Id_ExerciseSet"].ToString(),
                            IdExercise = reader["Id_Exercise"].ToString(),
                            ExerciseName = reader["Exercise_Name"].ToString()
                        };

                        newEntry.ExerciseWeights[0] = reader["Weight1"].ToString();
                        newEntry.ExerciseWeights[1] = reader["Weight2"].ToString();
                        newEntry.ExerciseWeights[2] = reader["Weight3"].ToString();
                        newEntry.ExerciseWeights[3] = reader["Weight4"].ToString();
                        newEntry.ExerciseWeights[4] = reader["Weight5"].ToString();

                        output.Add(newEntry);
                    }
                    reader.Close();
                }

            }
            return output;
        }

        public List<ManageExercisesModel> GetExercisesByExerciseSetId(string IdExerciseSet)
        {
            List<ManageExercisesModel> output = new List<ManageExercisesModel>();
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "SELECT * FROM dbo.Exercises WHERE Id_ExerciseSet=@id_exercise_set";
                command.Parameters.Add("@id_exercise_set", SqlDbType.Int).Value = Int32.Parse(IdExerciseSet);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ManageExercisesModel newEntry = new ManageExercisesModel()
                        {
                            IdExercise = IdExerciseSet,
                            IdExerciseSet = reader["Id_ExerciseSet"].ToString(),
                            ExerciseName = reader["Exercise_Name"].ToString(),
                        };

                        newEntry.ExerciseWeights[0] = reader["Weight1"].ToString();
                        newEntry.ExerciseWeights[1] = reader["Weight2"].ToString();
                        newEntry.ExerciseWeights[2] = reader["Weight3"].ToString();
                        newEntry.ExerciseWeights[3] = reader["Weight4"].ToString();
                        newEntry.ExerciseWeights[4] = reader["Weight5"].ToString();

                        output.Add(newEntry);
                    }
                    reader.Close();
                }

            }
            return output;
        }


        public ExerciseSetModel GetExerciseSetById(string id)
        {
            throw new NotImplementedException();
        }
        public List<ExerciseSetModel> GetExerciseSetByUserId(string id)
        {
            List<ExerciseSetModel> output = new List<ExerciseSetModel>();
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "SELECT Id_ExerciseSet, Title FROM dbo.ExerciseSet WHERE Id_User=@id_user";
                command.Parameters.Add("@id_user", SqlDbType.Int).Value = Int32.Parse(id);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        output.Add(new ExerciseSetModel()
                        {
                            IdExerciseSet = reader["Id_ExerciseSet"].ToString(),
                            ExerciseSetTitle = reader["Title"].ToString(),
                            IdUser = id
                        }
                            );
                    }
                    reader.Close();
                }

            }
            return output;
        }

        public SQLResults InsertNewExerciseSet(ExerciseSetModel newSet)
        {
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "INSERT INTO dbo.ExerciseSet(Id_User, Title) VALUES(@id_user, @title)";
                command.Parameters.Add("@id_user", SqlDbType.Int).Value = Int32.Parse(newSet.IdUser);
                command.Parameters.Add("@title", SqlDbType.NVarChar).Value = newSet.ExerciseSetTitle;
                int rowsAffected = command.ExecuteNonQuery();
                if (rowsAffected < 1)
                {
                    return SQLResults.Failure;
                }
                
                command.CommandText = "SELECT Id_ExerciseSet FROM dbo.ExerciseSet WHERE Id_user=@id_user AND Title=@title";
                //command.Parameters.Add("@id_user", SqlDbType.Int).Value = Int32.Parse(newSet.ExerciseSetTitle);
                //command.Parameters.Add("@title", SqlDbType.NVarChar).Value = newSet.ExerciseSetTitle;
                Int32 exerciseSetId = (Int32)command.ExecuteScalar();
                if (exerciseSetId == null)
                {
                    return SQLResults.Failure;
                }
                command.Parameters.Clear();

                SqlTransaction transaction = connection.BeginTransaction();
                command.Transaction = transaction;
                command.CommandText = "INSERT INTO dbo.Exercises(Id_ExerciseSet, Exercise_Name, Weight1, Weight2, Weight3, Weight4, Weight5) " +
                    "VALUES(@id_exercise_set, @exercise_name, @weight1, @weight2, @weight3, @weight4, @weight5)";
                command.Parameters.Add("@id_exercise_set", SqlDbType.Int).Value = exerciseSetId;
                command.Parameters.Add("@exercise_name", SqlDbType.NVarChar);
                command.Parameters.Add("@weight1", SqlDbType.Real);
                command.Parameters.Add("@weight2", SqlDbType.Real);
                command.Parameters.Add("@weight3", SqlDbType.Real);
                command.Parameters.Add("@weight4", SqlDbType.Real);
                command.Parameters.Add("@weight5", SqlDbType.Real);
                CultureInfo usCulture = new CultureInfo("en-US");
                try
                {
                    foreach (var element in newSet.Exercises)
                    {
                        command.Parameters["@exercise_name"].Value = element.ExerciseName;
                        command.Parameters["@weight1"].Value = Double.Parse(element.ExerciseWeights[0].Value, usCulture);
                        command.Parameters["@weight2"].Value = Double.Parse(element.ExerciseWeights[1].Value, usCulture);
                        command.Parameters["@weight3"].Value = Double.Parse(element.ExerciseWeights[2].Value, usCulture);
                        command.Parameters["@weight4"].Value = Double.Parse(element.ExerciseWeights[3].Value, usCulture);
                        command.Parameters["@weight5"].Value = Double.Parse(element.ExerciseWeights[4].Value, usCulture);
                        
                        if(command.ExecuteNonQuery() == 0)
                        {
                            throw new InvalidProgramException();
                        }
                    }
                    transaction.Commit();
                    
                }catch(Exception e)
                {
                    transaction.Rollback(); 
                    return SQLResults.Failure;
                }
                connection.Close();
            }

            return SQLResults.Success;
        }
    }
}
