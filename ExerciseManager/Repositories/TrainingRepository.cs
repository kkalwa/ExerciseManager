using ExerciseManager.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Text;

namespace ExerciseManager.Repositories
{
    internal class TrainingRepository : RepositoryBase, ITrainingRepository
    {
        public void AddTraining(TrainingModel trainingModel)
        {
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = 
                    "INSERT INTO dbo.Training (Date, Id_User)\r\n" +
                    "OUTPUT INSERTED.Id_Training\r\n" +
                    "VALUES (@date, @id_user)\r\n";
                command.Parameters.Add("@date", SqlDbType.DateTime).Value = trainingModel.Date;
                command.Parameters.Add("@id_user", SqlDbType.Int).Value = Int32.Parse(trainingModel.IdUser);
                int generatedKey = (Int32)command.ExecuteScalar();
                command.Transaction = connection.BeginTransaction();
                command.CommandText = 
                    "INSERT INTO dbo.Training_Activities (Id_Training, Id_Exercise)\r\n" +
                    "VALUES(@id_training, @id_exercise)";
                command.Parameters.Add("@id_training", SqlDbType.Int).Value = generatedKey;
                command.Parameters.Add("@id_exercise", SqlDbType.Int);
                
                foreach (var e in getListOfExerciseIds(trainingModel.ExerciseList))
                {
                    command.Parameters["@id_exercise"].Value = Int32.Parse(e);

                    if(command.ExecuteNonQuery() <= 0)
                    {
                        throw new InvalidOperationException("Failed to add exercise to training.");
                    }
                }
                try
                {
                    command.Transaction.Commit();
                } catch(Exception e)
                {
                    command.Transaction.Rollback();
                    throw;
                }
                connection.Close();
            }
        }

        private Collection<string> getListOfExerciseIds(Collection<ExerciseSetModel> exerciseSets)
        {
            Collection<string> output = [];
            foreach (var exerciseSet in exerciseSets)
            {
                foreach(var exercise in exerciseSet.Exercises)
                {
                    output.Add(exercise.IdExercise);
                }
            }
            return output;
        }
    }
}
