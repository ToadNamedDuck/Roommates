using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using Roommates.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Roommates.Repositories
{
    public class ChoreRepository : BaseRepository
    {
        // <summary>
        //On ChoreRepository intilization, pass in the connection string to the base repository class.
        //</summary>
        public ChoreRepository(string connectionString) : base(connectionString) { }

        public List<Chore> GetAll()
        {
            List<Chore> chores = new();
            using (SqlConnection connection = Connection)
            {
                connection.Open();//Open the connection to the sql server

                using (SqlCommand command = connection.CreateCommand())//The future Sql pieces will want a command object, so we set it up
                {
                    command.CommandText = "SELECT Id, Name FROM Chore";//Set up our command
                    using (SqlDataReader reader = command.ExecuteReader())//Actually reads SQL data.
                    {
                        while(reader.Read())
                        {
                            int IdColumn = reader.GetOrdinal("Id");//Returns the column for Id, which should be 0. In other database, could be anything.
                            int Id = reader.GetInt32(IdColumn);//Get the integer stored in the column.

                            //repeat for name
                            int NameColumn = reader.GetOrdinal("Name");
                            string Name = reader.GetString(NameColumn);

                            Chore chore = new Chore()
                            {
                                Id = Id,
                                Name = Name
                            };
                            chores.Add(chore);
                        }
                    }
                }

            }
            return chores;
        }
        public Chore GetById(int id)
        {
            using (SqlConnection connection = Connection)
            {
                connection.Open();
                using (SqlCommand cmd = connection.CreateCommand())//build command
                {
                    cmd.CommandText = "Select Id, Name from Chore where Id = @id";//@ means we are using a parameter in the command we are building
                    cmd.Parameters.AddWithValue("id", id);//We need to add a key/value pair to the parameter table in our command so it knows what we are talking about

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        Chore chore = null;
                        //Returning 1 object, don't need a loop.
                        if (reader.Read())
                        {
                            chore = new Chore()
                            {
                                Id = id,
                                Name = reader.GetString(reader.GetOrdinal("Name"))
                            };
                        }

                        return chore;
                    }
                }
            }
        }
        public void Insert(Chore chore)
        {
            using (SqlConnection connection = Connection)
            {
                connection.Open();
                using (SqlCommand cmd = connection.CreateCommand())
                {
                    cmd.CommandText = @"Insert into Chore (Name)
                                        OUTPUT INSERTED.Id
                                        Values (@name)";
                    cmd.Parameters.AddWithValue("@name", chore.Name);

                    int id = (int)cmd.ExecuteScalar();
                    chore.Id = id; //Assigning the objects Id???? Why when we return nothing?
                }
            }
        }
        public List<Chore> GetUnassignedChores()
        {
            List<Chore> chores = new List<Chore>();
            using (SqlConnection connection = Connection)
            {
                connection.Open();

                using (SqlCommand cmd = connection.CreateCommand())
                {
                    cmd.CommandText = "select ch.Name, ch.Id from Chore ch left join RoommateChore rmc on ch.Id = rmc.ChoreId where rmc.ChoreId is null";
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while(reader.Read())
                        {
                            Chore foundChore = new Chore()
                            {
                                Name = reader.GetString(reader.GetOrdinal("Name")),
                                Id = reader.GetInt32(reader.GetOrdinal("Id"))
                            };
                            chores.Add(foundChore);
                        }
                    }
                }

            }
            return chores;
        }
        public void AssignChore(int choreId, int roommateId)
        {
            using(SqlConnection connection = Connection)
            {
                connection.Open();
                using(SqlCommand cmd = connection.CreateCommand())
                {
                    cmd.CommandText = "insert into RoommateChore (RoommateId, ChoreId) values (@rmId, @chId)";
                    cmd.Parameters.AddWithValue("@rmId", roommateId);
                    cmd.Parameters.AddWithValue("@chId", choreId);

                    cmd.ExecuteNonQuery();
                    Console.WriteLine("Your request has been processed.");
                }
            }
        }
    }
}
