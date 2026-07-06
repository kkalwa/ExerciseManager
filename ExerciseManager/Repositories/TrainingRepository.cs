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
            int numberOfRowsAffected;
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                SqlTransaction transaction = connection.BeginTransaction();
                command.Transaction = transaction;
                command.CommandText = "INSERT INTO dbo.Training (Date, Id_User)\r\n" + 
                    "Values (@date, @id_user)\r\n" +
                    "INSERT INTO dbo.Training_Activities (Id_Training, Id_Exercise)\r\n" +
                    "VALUES(SCOPE_IDENTITY(), @id_exercise)";
                command.Parameters.Add("@date", SqlDbType.DateTime).Value = trainingModel.Date;
                command.Parameters.Add("@id_user", SqlDbType.Int).Value = Int32.Parse(trainingModel.IdUser);
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
                    transaction.Commit();
                } catch(Exception e)
                {
                    transaction.Rollback();
                    throw;
                }
                /*numberOfRowsAffected = command.ExecuteNonQuery();
                if (numberOfRowsAffected <= 0)
                {
                    throw new InvalidOperationException("Failed to add training.");
                }*/
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
