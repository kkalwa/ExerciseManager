using ExerciseManager.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Text;

namespace ExerciseManager.Repositories
{
    public class TrainingRepository : RepositoryBase, ITrainingRepository
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

        public List<TrainingHistoryModel> GetTrainingHistoryByUserId(string userId)
        {
           using (var connection = GetConnection())
            {
                connection.Open();

                using (var command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = "SELECT * FROM dbo.Training, dbo.Training_Activities, dbo.Exercises WHERE " +
                        "dbo.Training.Id_Training = dbo.Training_Activities.Id_Training AND " +
                        "dbo.Training_Activities.Id_Exercise = dbo.Exercises.Id_Exercise AND " +
                        "dbo.Training.Id_User = @id_user";
                    command.Parameters.Add("@id_user", SqlDbType.Int).Value = Int32.Parse(userId);
                    var reader = command.ExecuteReader();
                    List<TrainingHistoryModel> trainings = [];
                    TrainingHistoryModel training;
                    
                    while (reader.Read())
                    { 
                        if(!isTrainingAlreadyInTheList(trainings, reader.GetInt32("Id_Training")))
                        {
                            training = createNewTrainingObject(reader);
                            trainings.Add(training);
                        }

                        AddExercisesToTraining(reader, trainings.Last());
                        AddWeightsToExercise(reader, trainings.Last());
                    }
                    return trainings;
                }
            }
        }

        private void AddWeightsToExercise(SqlDataReader reader, TrainingHistoryModel trainingHistoryModel)
        {
            for (int i = 1; i <= 5; i++)
            {
                if (!reader.IsDBNull(reader.GetOrdinal($"Weight{i}")))
                {
                    string weight = reader.GetFloat($"Weight{i}").ToString();
                    trainingHistoryModel.Exercises.Last().ExerciseWeights.Add(new MutableStringValue(weight));
                }
            }
            if (!reader.IsDBNull(reader.GetOrdinal("Description")))
            {
                trainingHistoryModel.Exercises.Last().Description.Value = reader.GetString("Description");
            }
        }

        private void AddExercisesToTraining(SqlDataReader reader, TrainingHistoryModel trainingHistoryModel)
        {
            ExerciseModel entry = new()
            {
                IdExercise = reader.GetInt32("Id_Exercise").ToString(),
                IdExerciseSet = reader.GetInt32("Id_ExerciseSet").ToString(),
                ExerciseName = reader.GetString("Exercise_Name")
            };
            trainingHistoryModel.Exercises.Add(entry);
        }

        private TrainingHistoryModel createNewTrainingObject(SqlDataReader reader)
        {
            return new TrainingHistoryModel
            {
                IdTraining = reader.GetInt32("Id_Training"),
                Date = reader.GetDateTime("Date"),
                IdUser = reader.GetInt32("Id_User"),
                Exercises = []
            };
        }

        private bool isTrainingAlreadyInTheList(List<TrainingHistoryModel> trainings, int v)
        {
            if(trainings != null)
            {
                return trainings.Any(t => t.IdTraining == v);
            }
            return false;
        }

        public ObservableCollection<TrainingHistoryModel> GetTrainingsByUserId(string userId)
        {
            using (var connection = GetConnection())
            {
                connection.Open();
               
                using (var command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = "SELECT * FROM dbo.Training, dbo.Training_Activities, dbo.Exercises WHERE " +
                        "dbo.Training.Id_Training = dbo.Training_Activities.Id_Training AND " +
                        "dbo.Training_Activities.Id_Exercise = dbo.Exercises.Id_Exercise AND " +
                        "dbo.Training.Id_User = @id_user";
                    command.Parameters.Add("@id_user", SqlDbType.Int).Value = Int32.Parse(userId);
                    var reader = command.ExecuteReader();
                    ObservableCollection<TrainingHistoryModel> trainings = [];
                    while (reader.Read())
                    {
                        /* Jeśli lista jest pusta to należy dodać nowy obiekt TrainingHistoryModel
                         * jeśli lista nie jest pusta to należy sprawdzić czy istnieje już obiekt o danym Id_Training
                         * jeśli istnieje to należy dodać do niego ćwiczenie, jeśli nie istnieje to należy dodać nowy obiekt TrainingHistoryModel
                         */
                        TrainingHistoryModel training = null;
                        if (isTrainingInListOrListEmpty(trainings, reader.GetInt32("Id_Training")))
                        {
                            training = new()
                            {
                                IdTraining = reader.GetInt32("Id_Training"),
                                Date = reader.GetDateTime("Date"),
                                IdUser = reader.GetInt32("Id_User")
                            };
                            trainings.Add(training);
                        }
                        ObservableCollection<ExerciseModel> exercises = [];
                        ExerciseModel entry = new()
                        {
                            IdExercise = reader.GetInt32("Id_Exercise").ToString(),
                            IdExerciseSet = reader.GetInt32("Id_ExerciseSet").ToString(),
                            ExerciseName = reader.GetString("Exercise_Name")
                        };
                        
                        for (int i=1; i<=5;i++)
                        {
                            if(!reader.IsDBNull(reader.GetOrdinal($"Weight{i}")))
                            {
                                string weight = reader.GetFloat($"Weight{i}").ToString();
                                entry.ExerciseWeights.Add(new MutableStringValue(weight));
                            }
                        }
                        if(!reader.IsDBNull(reader.GetOrdinal("Description")))
                        {
                            entry.Description.Value = reader.GetString("Description");
                        }
                        exercises.Add(entry);
                        training.Exercises = new List<ExerciseModel> { entry };
                    }
                    return trainings;
                }
                    
            }
        }

        
        
        private bool isTrainingInListOrListEmpty(ObservableCollection<TrainingHistoryModel> trainings, int idTraining)
        {
            if ((trainings.Count == 0))
            {
                return true;
            }
                foreach (var t in trainings)
                {
                    if (t.IdTraining == idTraining)
                    {
                        return true;
                    }
                }
            
            return false;
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
