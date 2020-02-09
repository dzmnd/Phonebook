using Business.Models;
using Microsoft.Owin.Infrastructure;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace PopulateDataSet
{
    class Program
    {
        const string CONNECTION_STRING = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Projects\Phonebook\DataBase\PhonebookDb.mdf;Integrated Security=True;Connect Timeout=30";

        static async Task Main(string[] args)
        {
            try
            {
                RootObject rootObject = await GetUserData();
                CreateTableIfNoExist();
                SeedData(rootObject);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error generating person: {ex.Message}");
            }
        }

        private static async Task<RootObject> GetUserData() 
        {
            const string URL = "https://api.randomuser.me";
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("results", "1000");
            using (var client = new HttpClient())
            {
                try
                {
                    string fullUrl = WebUtilities.AddQueryString(URL, parameters);
                    HttpResponseMessage response = await client.GetAsync(fullUrl);
                    response.EnsureSuccessStatusCode();

                    string result = await response.Content.ReadAsStringAsync();
                    RootObject rootObject = JsonConvert.DeserializeObject<RootObject>(result);
                    return rootObject;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        private static void CreateTableIfNoExist()
        {
            const string QUERY = @"SELECT COUNT(*) FROM information_schema.tables WHERE table_type = 'BASE TABLE' and table_name='Users'";
            using (SqlConnection connection = new SqlConnection(CONNECTION_STRING))
            {
                connection.Open();
                bool isHaveTable = false;
                using (SqlCommand command = new SqlCommand(QUERY, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                isHaveTable = reader.GetInt32(0) == 1;
                            }
                        }
                    }
                }
                if (!isHaveTable)
                {
                    const string CREATE_TABLE_QUERY = @"CREATE TABLE [dbo].[Users]
                                                        (
                                                        	[Id] BIGINT IDENTITY (1, 1) NOT NULL PRIMARY KEY, 
                                                            [Title] NVARCHAR(50) NULL, 
                                                            [FirstName] NVARCHAR(50) NULL, 
                                                            [LastName] NVARCHAR(50) NULL, 
                                                            [Dob] NVARCHAR(50) NULL, 
                                                            [Photo] NVARCHAR(MAX) NULL,
                                                        )";
                    using (SqlCommand commandCreateTable = new SqlCommand(CREATE_TABLE_QUERY, connection))
                    {
                        commandCreateTable.ExecuteNonQuery();
                    }
                }
            }
        }

        private static void SeedData(RootObject rootObject)
        {
            using (SqlConnection connection = new SqlConnection(CONNECTION_STRING))
            {
                connection.Open();
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    using (SqlCommand command = connection.CreateCommand())
                    {
                        command.Transaction = transaction;
                        command.CommandType = CommandType.Text;
                        command.CommandText = "INSERT INTO [Users] ([Title], [FirstName], [LastName], [Dob], [Photo]) VALUES(@Title, @FirstName, @LastName, @Dob, @Photo);";
                        command.Parameters.Add(new SqlParameter("@Title", SqlDbType.NVarChar));
                        command.Parameters.Add(new SqlParameter("@FirstName", SqlDbType.NVarChar));
                        command.Parameters.Add(new SqlParameter("@LastName", SqlDbType.NVarChar));
                        command.Parameters.Add(new SqlParameter("@Dob", SqlDbType.NVarChar));
                        command.Parameters.Add(new SqlParameter("@Photo", SqlDbType.NVarChar));

                        try
                        {
                            foreach (var user in rootObject.results)
                            {
                                command.Parameters[0].Value = user.name.title;
                                command.Parameters[1].Value = user.name.first;
                                command.Parameters[2].Value = user.name.last;
                                command.Parameters[3].Value = user.dob.date;
                                command.Parameters[4].Value = user.picture.large;
                                if (command.ExecuteNonQuery() != 1)
                                {
                                    throw new InvalidProgramException();
                                }
                            }
                            transaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            throw ex;
                        }
                    }
                }
            }
        }
    }
}
