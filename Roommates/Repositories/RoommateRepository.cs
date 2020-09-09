using Microsoft.Data.SqlClient;
using Roommates.Models;
using System;
using System.Collections.Generic;

namespace Roommates.Repositories
{
    /// <summary>
    ///  This class is responsible for interacting with Roommate data.
    ///  It inherits from the BaseRepository class so that it can use the BaseRepository's Connection property
    /// </summary>
    public class RoommateRepository : BaseRepository
    {
        /// <summary>
        ///  When new RoommateRespository is instantiated, pass the connection string along to the BaseRepository
        /// </summary>
        public RoommateRepository(string connectionString) : base(connectionString) { }

        /// <summary>
        ///  Get a list of all Roommates in the database
        /// </summary>
        public List<Roommate> GetAll()
        {
            //  We must "use" the database connection.
            //  Because a database is a shared resource (other applications may be using it too) we must
            //  be careful about how we interact with it. Specifically, we Open() connections when we need to
            //  interact with the database and we Close() them when we're finished.
            //  In C#, a "using" block ensures we correctly disconnect from a resource even if there is an error.
            //  For database connections, this means the connection will be properly closed.
            using (SqlConnection conn = Connection)
            {
                // Note, we must Open() the connection, the "using" block doesn't do that for us.
                conn.Open();

                // We must "use" commands too.
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    // Here we setup the command with the SQL we want to execute before we execute it.
                    cmd.CommandText = "SELECT Id, FirstName, LastName, RentPortion, MoveInDate, RoomId FROM Roommate";

                    // Execute the SQL in the database and get a "reader" that will give us access to the data.
                    SqlDataReader reader = cmd.ExecuteReader();

                    // A list to hold the roommates we retrieve from the database.
                    List<Roommate> roommates = new List<Roommate>();

                    // Read() will return true if there's more data to read
                    while (reader.Read())
                    {
                        // The "ordinal" is the numeric position of the column in the query results.
                        //  For our query, "Id" has an ordinal value of 0 and "Name" is 1.
                        int idColumnPosition = reader.GetOrdinal("Id");

                        // We user the reader's GetXXX methods to get the value for a particular ordinal.
                        int idValue = reader.GetInt32(idColumnPosition);

                        int firstNameColumnPosition = reader.GetOrdinal("FirstName");
                        string firstNameValue = reader.GetString(firstNameColumnPosition);

                        int lastNameColumnPosition = reader.GetOrdinal("LastName");
                        string lastNameValue = reader.GetString(lastNameColumnPosition);

                        int rentPortionPosition = reader.GetOrdinal("RentPortion");
                        int rentPortionValue = reader.GetInt32(rentPortionPosition);

                        int moveInDatePosition = reader.GetOrdinal("MoveInDate");
                        DateTime moveInDateValue = reader.GetDateTime(moveInDatePosition);

                        int roomIdColumnPosition = reader.GetOrdinal("RoomId");
                        int roomIdValue = reader.GetInt32(roomIdColumnPosition);

                        // Now let's create a new roommate object using the data from the database.
                        Roommate roommate = new Roommate
                        {
                            Id = idValue,
                            FirstName = firstNameValue,
                            LastName = lastNameValue,
                            RentPortion = rentPortionValue,
                            MoveInDate = moveInDateValue,
                            Room = null
                        };

                        // ...and add that roommate object to our list.
                        roommates.Add(roommate);
                    }

                    // We should Close() the reader. Unfortunately, a "using" block won't work here.
                    reader.Close();

                    // Return the list of roommates who whomever called this method.
                    return roommates;
                }
            }
        }

        public List<Roommate> GetAllWithRoom()
        {
            //  We must "use" the database connection.
            //  Because a database is a shared resource (other applications may be using it too) we must
            //  be careful about how we interact with it. Specifically, we Open() connections when we need to
            //  interact with the database and we Close() them when we're finished.
            //  In C#, a "using" block ensures we correctly disconnect from a resource even if there is an error.
            //  For database connections, this means the connection will be properly closed.
            using (SqlConnection conn = Connection)
            {
                // Note, we must Open() the connection, the "using" block doesn't do that for us.
                conn.Open();

                // We must "use" commands too.
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    // Here we setup the command with the SQL we want to execute before we execute it.
                    cmd.CommandText = @"SELECT rm.Id, rm.FirstName, rm.LastName, rm.RentPortion, rm.MoveInDate, rm.RoomId, r.Name, r.MaxOccupancy 
                                        FROM Roommate rm
                                        LEFT JOIN Room r ON rm.RoomId = r.Id";

                    // Execute the SQL in the database and get a "reader" that will give us access to the data.
                    SqlDataReader reader = cmd.ExecuteReader();

                    // A list to hold the roommates we retrieve from the database.
                    List<Roommate> roommatesWithRoom = new List<Roommate>();

                    // Read() will return true if there's more data to read
                    while (reader.Read())
                    {
                        // The "ordinal" is the numeric position of the column in the query results.
                        //  For our query, "Id" has an ordinal value of 0 and "Name" is 1.
                        int idColumnPosition = reader.GetOrdinal("Id");

                        // We user the reader's GetXXX methods to get the value for a particular ordinal.
                        int idValue = reader.GetInt32(idColumnPosition);

                        int firstNameColumnPosition = reader.GetOrdinal("FirstName");
                        string firstNameValue = reader.GetString(firstNameColumnPosition);

                        int lastNameColumnPosition = reader.GetOrdinal("LastName");
                        string lastNameValue = reader.GetString(lastNameColumnPosition);

                        int rentPortionPosition = reader.GetOrdinal("RentPortion");
                        int rentPortionValue = reader.GetInt32(rentPortionPosition);

                        int moveInDatePosition = reader.GetOrdinal("MoveInDate");
                        DateTime moveInDateValue = reader.GetDateTime(moveInDatePosition);

                        int roomIdColumnPosition = reader.GetOrdinal("RoomId");
                        int roomIdValue = reader.GetInt32(roomIdColumnPosition);

                        // Now let's create a new roommate object using the data from the database.
                        Roommate roommateWithRoom = new Roommate
                        {
                            Id = idValue,
                            FirstName = firstNameValue,
                            LastName = lastNameValue,
                            RentPortion = rentPortionValue,
                            MoveInDate = moveInDateValue,

                            Room = new Room()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("RoomId")),
                                Name = reader.GetString(reader.GetOrdinal("Name")),
                                MaxOccupancy = reader.GetInt32(reader.GetOrdinal("MaxOccupancy"))

                            }
                        };

                        // ...and add that roommate object to our list.
                        roommatesWithRoom.Add(roommateWithRoom);
                    }

                    // We should Close() the reader. Unfortunately, a "using" block won't work here.
                    reader.Close();

                    // Return the list of roommates who whomever called this method.
                    return roommatesWithRoom;
                }
            }
        }

        /// <summary>
        ///  Returns a single roommate with the given id.
        /// </summary>
        public Roommate GetById(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT rm.Id, rm.FirstName, rm.LastName, rm.RentPortion, rm.MoveInDate, rm.RoomId, r.Name, r.MaxOccupancy 
                                        FROM Roommate rm
                                        LEFT JOIN Room r ON rm.RoomId = r.Id
                                        WHERE rm.Id = @id";
                    cmd.Parameters.AddWithValue("@id", id);
                    SqlDataReader reader = cmd.ExecuteReader();

                    Roommate roommate = null;

                    // If we only expect a single row back from the database, we don't need a while loop.
                    if (reader.Read())
                    {
                        roommate = new Roommate
                        {
                            Id = id,
                            FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                            LastName = reader.GetString(reader.GetOrdinal("LastName")),
                            RentPortion = reader.GetInt32(reader.GetOrdinal("RentPortion")),
                            MoveInDate = reader.GetDateTime(reader.GetOrdinal("MoveInDate")),

                            Room = new Room()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("RoomId")),
                                Name = reader.GetString(reader.GetOrdinal("Name")),
                                MaxOccupancy = reader.GetInt32(reader.GetOrdinal("MaxOccupancy"))

                            }
                        };
                    }

                    reader.Close();

                    return roommate;
                }
            }
        }

        /// <summary>
        ///  Add a new roommate to the database
        ///   NOTE: This method sends data to the database,
        ///   it does not get anything from the database, so there is nothing to return.
        /// </summary>
        public void Insert(Roommate roommate)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    // These SQL parameters are annoying. Why can't we use string interpolation?
                    // ... sql injection attacks!!!
                    cmd.CommandText = @"INSERT INTO Roommate (FirstName, LastName, RentPortion, MoveInDate, RoomId) 
                                         OUTPUT INSERTED.Id 
                                         VALUES (@firstName, @lastName, @rentPortion, @moveInDate, @roomId)";
                    cmd.Parameters.AddWithValue("@firstName", roommate.FirstName);
                    cmd.Parameters.AddWithValue("@lastName", roommate.LastName);
                    cmd.Parameters.AddWithValue("@rentPortion", roommate.RentPortion);
                    cmd.Parameters.AddWithValue("@moveInDate", roommate.MoveInDate);
                    cmd.Parameters.AddWithValue("@roomId", roommate.Room.Id);
                    int id = (int)cmd.ExecuteScalar();

                    roommate.Id = id;
                }
            }

            // When this method is finished we can look in the database and see the new roommate.
        }

        /// <summary>
        ///  Updates the roommate
        /// </summary>
        public void Update(Roommate roommate)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"UPDATE Roommate
                                    SET FirstName = @firstName,
                                        LastName = @lastName,
                                        RentPortion = @rentPortion,
                                        MoveInDate = @moveInDate,
                                        RoomId = @roomId
                                    WHERE Id = @id";
                    cmd.Parameters.AddWithValue("@firstName", roommate.FirstName);
                    cmd.Parameters.AddWithValue("@lastName", roommate.LastName);
                    cmd.Parameters.AddWithValue("@rentPortion", roommate.RentPortion);
                    cmd.Parameters.AddWithValue("@moveInDate", roommate.MoveInDate);
                    cmd.Parameters.AddWithValue("@roomId", roommate.Room.Id);
                    cmd.Parameters.AddWithValue("@id", roommate.Id);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        ///  Delete the roommate with the given id
        /// </summary>
        public void Delete(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "DELETE FROM Roommate WHERE Id = @id";
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}