using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gym
{
    internal static class MuscleDBScheme
    {
        
        public static readonly string DB_NAME = "muscleDB";
        public static readonly string STANDARD_CONNECTION_STRING = $"Server=localhost\\SQLEXPRESS;Database={DB_NAME};Trusted_Connection=yes;TrustServerCertificate=True";
        private static readonly string prefix = "dbo.";
        
        
        public static class UsersTable
        {
            private static string tableName = "Users";
            public static string TableName => prefix + tableName;

            public static class UsersColumns
            {
                public static readonly string IdUser = "Id_User";
                public static readonly string Nickname = "Nickname";
                public static readonly string Password = "Password";
            }
        }
    }
}
