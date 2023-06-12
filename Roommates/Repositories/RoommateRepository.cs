using Microsoft.Data.SqlClient;
using Roommates.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Roommates.Repositories
{
    public class RoommateRepository : BaseRepository
    {
        public RoommateRepository(string connectionString) : base(connectionString) { }

        public Roommate GetById(int id)
        {
            using (SqlConnection connection = Connection)
            {
                connection.Open();
                using(SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = "select rm.FirstName, rm.RentPortion, ro.Name from Roommate rm join Room ro on rm.RoomId = ro.Id and rm.Id = @id";
                    command.Parameters.AddWithValue("id", id);

                    using(SqlDataReader reader = command.ExecuteReader())
                    {
                        Roommate foundRM = null;
                        if(reader.Read())
                        {
                            Room rmRoom = new Room()
                            {
                                Name = reader.GetString(reader.GetOrdinal("Name"))
                            };
                            foundRM = new Roommate()
                            {
                                Id = id,
                                FirstName = reader.GetString((reader.GetOrdinal("FirstName"))),
                                Room = rmRoom,
                                RentPortion = reader.GetInt32(reader.GetOrdinal("RentPortion"))
                            };
                        }
                        return foundRM;
                    }
                }
            }
        }

        public List<Roommate> GetAll()
        {
            List<Roommate> roommates = new List<Roommate>();
            using (SqlConnection connection = Connection)
            {
                connection.Open();

                using(SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = "Select Id, FirstName, LastName from Roommate";

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Roommate rm = new();
                            rm.FirstName = reader.GetString(reader.GetOrdinal("FirstName"));
                            rm.LastName = reader.GetString(reader.GetOrdinal("LastName"));
                            rm.Id = reader.GetInt32(reader.GetOrdinal("Id"));
                            roommates.Add(rm);
                        }
                    }
                }
            }
            return roommates;
        }
    }
}
