using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Data.SqlClient;

namespace ExerciseManager.Repositories
{
    public abstract class RepositoryBase
    {
        private readonly string connectionString;
        public RepositoryBase()
        {
            connectionString = "Server=localhost\\SQLEXPRESS;Database=muscleDB;Trusted_Connection=yes;TrustServerCertificate=True";
        }
        protected SqlConnection GetConnection()
        {
            return new SqlConnection(connectionString);
        }
    }
}
