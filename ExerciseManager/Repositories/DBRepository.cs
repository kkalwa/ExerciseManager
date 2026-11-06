using ExerciseManager.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Globalization;
using System.Reflection;
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
                    "[dbo].[Exercises].[Description], \r\n" +
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
                        if (!reader.IsDBNull(reader.GetOrdinal("Description")))
                        {
                            newEntry.Description = new MutableStringValue(reader["Description"].ToString());
                        }
                        else
                        {
                            newEntry.Description = MutableStringValue.Empty;
                        }
                        newEntry.ExerciseWeights[0] = new MutableStringValue(reader["Weight1"].ToString());
                        newEntry.ExerciseWeights[1] = new MutableStringValue(reader["Weight2"].ToString());
                        newEntry.ExerciseWeights[2] = new MutableStringValue(reader["Weight3"].ToString());
                        newEntry.ExerciseWeights[3] = new MutableStringValue(reader["Weight4"].ToString());
                        newEntry.ExerciseWeights[4] = new MutableStringValue(reader["Weight5"].ToString());

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

                        newEntry.ExerciseWeights[0] = new MutableStringValue(reader["Weight1"].ToString());
                        newEntry.ExerciseWeights[1] = new MutableStringValue(reader["Weight2"].ToString());
                        newEntry.ExerciseWeights[2] = new MutableStringValue(reader["Weight3"].ToString());
                        newEntry.ExerciseWeights[3] = new MutableStringValue(reader["Weight4"].ToString());
                        newEntry.ExerciseWeights[4] = new MutableStringValue(reader["Weight5"].ToString());

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

                        if (command.ExecuteNonQuery() == 0)
                        {
                            throw new InvalidProgramException();
                        }
                    }
                    transaction.Commit();

                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    return SQLResults.Failure;
                }
                connection.Close();
            }

            return SQLResults.Success;
        }

        public ExerciseModel GetExerciseById(string ExerciseId)
        {
            ExerciseModel exercise = null;
            using (var connection = GetConnection())
            {
                connection.Open();
                using (var command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = "SELECT * FROM dbo.Exercises WHERE Id_Exercise = @id_exercise";
                    command.Parameters.Add("@id_exercise", SqlDbType.Int).Value = Int32.Parse(ExerciseId);
                    var reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        exercise = new ExerciseModel
                        {
                            IdExercise = reader["Id_Exercise"].ToString(),
                            ExerciseName = reader["Exercise_Name"].ToString()

                        };
                        exercise.Description.Value = reader["Description"].ToString();
                        exercise.exerciseWeights[0].Value = reader["Weight1"].ToString();
                        exercise.exerciseWeights[1].Value = reader["Weight2"].ToString();
                        exercise.exerciseWeights[2].Value = reader["Weight3"].ToString();
                        exercise.exerciseWeights[3].Value = reader["Weight4"].ToString();
                        exercise.exerciseWeights[4].Value = reader["Weight5"].ToString();
                    }
                }
            }
            return exercise ?? throw new NullReferenceException("Exercise not found");
        }

        public void RemoveExerciseSetById(string id)
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                using (var command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = "DELETE FROM dbo.ExerciseSet WHERE Id_ExerciseSet = @id_exercise_set";
                    command.Parameters.Add("@id_exercise_set", SqlDbType.Int).Value = Int32.Parse(id);
                    command.ExecuteNonQuery();
                }
            }
        }

        public void UpdateExerciseSet(EditSetInfo editSetInfo)
        {
            if (editSetInfo.HasChanges())
            {
                string exerciseSetId = editSetInfo.IdExerciseSet ?? throw new InvalidOperationException("Invalid exercise set ID");
                string exerciseSetName = editSetInfo.ExerciseSetName ?? throw new InvalidOperationException("Invalid exercise set name");
                using (var connection = GetConnection())
                {
                    connection.Open();
                    using (var transaction = connection.BeginTransaction())
                    {
                        string sqlUpdateExerciseSetName = "UPDATE dbo.ExerciseSet SET Title = @exercise_set_name WHERE Id_ExerciseSet = @id_exercise_set";
                        using (var updateExerciseSetNameCommand = new SqlCommand(sqlUpdateExerciseSetName, connection, transaction))
                        {
                            updateExerciseSetNameCommand.Parameters.Add("@id_exercise_set", SqlDbType.Int).Value = Int32.Parse(exerciseSetId);
                            updateExerciseSetNameCommand.Parameters.Add("@exercise_set_name", SqlDbType.VarChar).Value = exerciseSetName;
                            updateExerciseSetNameCommand.ExecuteNonQuery();
                        }
                        string sqlUpdateExercise = "UPDATE dbo.Exercises SET Exercise_Name = @exercise_name, " +
                            "Weight1 = @weight1, Weight2 = @weight2, Weight3 = @weight3, Weight4 = @weight4, Weight5 = @weight5 " +
                            "WHERE Id_Exercise = @id_exercise";
                        using (var updateExerciseCommand = new SqlCommand(sqlUpdateExercise, connection, transaction))
                        {
                            updateExerciseCommand.Parameters.Add("@id_exercise", SqlDbType.Int);
                            updateExerciseCommand.Parameters.Add("@exercise_name", SqlDbType.VarChar);
                            updateExerciseCommand.Parameters.Add("@weight1", SqlDbType.Real);
                            updateExerciseCommand.Parameters.Add("@weight2", SqlDbType.Real);
                            updateExerciseCommand.Parameters.Add("@weight3", SqlDbType.Real);
                            updateExerciseCommand.Parameters.Add("@weight4", SqlDbType.Real);
                            updateExerciseCommand.Parameters.Add("@weight5", SqlDbType.Real);
                            foreach (var exercise in editSetInfo.ExercisesToUpdate)
                            {
                                updateExerciseCommand.Parameters["@id_exercise"].Value = Int32.Parse(exercise.IdExercise ?? throw new InvalidOperationException("Invalid exercise ID"));
                                updateExerciseCommand.Parameters["@exercise_name"].Value = exercise.ExerciseName ?? throw new InvalidOperationException("Invalid exercise name");
                                updateExerciseCommand.Parameters["@weight1"].Value = Double.Parse(exercise.exerciseWeights[0].Value, CultureInfo.InvariantCulture);
                                updateExerciseCommand.Parameters["@weight2"].Value = Double.Parse(exercise.exerciseWeights[1].Value, CultureInfo.InvariantCulture);
                                updateExerciseCommand.Parameters["@weight3"].Value = Double.Parse(exercise.exerciseWeights[2].Value, CultureInfo.InvariantCulture);
                                updateExerciseCommand.Parameters["@weight4"].Value = Double.Parse(exercise.exerciseWeights[3].Value, CultureInfo.InvariantCulture);
                                updateExerciseCommand.Parameters["@weight5"].Value = Double.Parse(exercise.exerciseWeights[4].Value, CultureInfo.InvariantCulture);
                                updateExerciseCommand.ExecuteNonQuery();
                            }
                        }
                        string sqlDeleteExercise = "DELETE FROM dbo.Exercises WHERE Id_Exercise = @id_exercise";
                        using (var deleteExerciseCommand = new SqlCommand(sqlDeleteExercise, connection, transaction))
                        {
                            deleteExerciseCommand.Parameters.Add("@id_exercise", SqlDbType.Int);
                            foreach (var exerciseId in editSetInfo.ExerciseIdsToDelete)
                            {
                                deleteExerciseCommand.Parameters["@id_exercise"].Value = Int32.Parse(exerciseId ?? throw new InvalidOperationException("Invalid exercise ID"));
                                deleteExerciseCommand.ExecuteNonQuery();
                            }
                        }
                        string sqlInsertNewExercises = "INSERT INTO dbo.Exercises (Id_ExerciseSet, Exercise_Name, Weight1, Weight2, Weight3, Weight4, Weight5) " +
                            "VALUES (@id_exercise_set, @exercise_name, @weight1, @weight2, @weight3, @weight4, @weight5)";
                        using (var insertNewExercisesCommand = new SqlCommand(sqlInsertNewExercises, connection, transaction))
                        {
                            insertNewExercisesCommand.Parameters.Add("@id_exercise_set", SqlDbType.Int);
                            insertNewExercisesCommand.Parameters.Add("@exercise_name", SqlDbType.VarChar);
                            insertNewExercisesCommand.Parameters.Add("@weight1", SqlDbType.Float);
                            insertNewExercisesCommand.Parameters.Add("@weight2", SqlDbType.Float);
                            insertNewExercisesCommand.Parameters.Add("@weight3", SqlDbType.Float);
                            insertNewExercisesCommand.Parameters.Add("@weight4", SqlDbType.Float);
                            insertNewExercisesCommand.Parameters.Add("@weight5", SqlDbType.Float);
                            foreach (var exercise in editSetInfo.ExercisesToAdd)
                            {
                                insertNewExercisesCommand.Parameters["@id_exercise_set"].Value = Int32.Parse(exercise.IdExerciseSet ?? throw new InvalidOperationException("Invalid exercise set ID"));
                                insertNewExercisesCommand.Parameters["@exercise_name"].Value = exercise.ExerciseName ?? throw new InvalidOperationException("Invalid exercise name");
                                insertNewExercisesCommand.Parameters["@weight1"].Value = Double.Parse(exercise.exerciseWeights[0].Value, CultureInfo.InvariantCulture);
                                insertNewExercisesCommand.Parameters["@weight2"].Value = Double.Parse(exercise.exerciseWeights[1].Value, CultureInfo.InvariantCulture);
                                insertNewExercisesCommand.Parameters["@weight3"].Value = Double.Parse(exercise.exerciseWeights[2].Value, CultureInfo.InvariantCulture);
                                insertNewExercisesCommand.Parameters["@weight4"].Value = Double.Parse(exercise.exerciseWeights[3].Value, CultureInfo.InvariantCulture);
                                insertNewExercisesCommand.Parameters["@weight5"].Value = Double.Parse(exercise.exerciseWeights[4].Value, CultureInfo.InvariantCulture);
                                insertNewExercisesCommand.ExecuteNonQuery();
                            }
                        }
                        try
                        {
                            transaction.Commit();
                        }
                        catch (Exception ex)
                            {
                                transaction.Rollback();
                                throw new InvalidOperationException("Failed to update exercise set", ex);
                            }
                    }
                }
            }
        }
    }
}
