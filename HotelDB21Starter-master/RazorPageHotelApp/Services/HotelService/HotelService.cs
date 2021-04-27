using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using RazorPageHotelApp.Models;
using RazorPageHotelApp.Services.Interfaces;

namespace RazorPageHotelApp.Services.HotelService
{
    public class HotelService : Connection, IHotelService
    {

        //SQL queries has been set here as strings, which will be used in the methods.
        //Variables like @ID (int) and @NAME (string) are placeholders for the actual variables in the SQL database
        private String queryString = "select * from Hotel";
        private String queryNameString = "select * from Hotel where  Name like @NAME";
        private String queryStringFromID = "select * from Hotel where Hotel_No = @ID";
        private String insertSql = "insert into Hotel Values (@ID, @Navn, @Adresse)";
        private String deleteSql = "delete from Hotel where Hotel_No = @ID";
        private String updateSql = "update Hotel " +
                                   "set Hotel_No= @HotelID, Name=@NAME, Address=@ADDRESS " +
                                   "where Hotel_No = @ID";

        //not much to say here, it's a constructor, it constructs
        public HotelService(IConfiguration configuration) : base(configuration)
        {

        }
        public HotelService(string connectionString) : base(connectionString)
        {

        }

        //This method will retrieve all of the Hotels from the Hotel table in the database
        //It creates an empty list of hotels, then it creates and opens an SQL connection to the database
        //Afterwards it will create and run an SQLDataReader object, which will retrieve the information in the database using a while loop
        //Which it stores in the Hotel list
        //When there is no more data in the Hotel table it will end the loop and return the finished Hotel List
        public async Task<List<Hotel>> GetAllHotelAsync()
        {
            List<Hotel> hoteller = new List<Hotel>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                await command.Connection.OpenAsync();

                SqlDataReader reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    int hotelNr = reader.GetInt32(0);
                    String hotelNavn = reader.GetString(1);
                    String hotelAdr = reader.GetString(2);
                    Hotel hotel = new Hotel(hotelNr, hotelNavn, hotelAdr);
                    hoteller.Add(hotel);
                }
            }
            return hoteller;
        }

        //This method is nearly identical to the one above except it uses an input of hotelNr to retrieve a single specific hotel
        //Otherwise it runs the same, returning just one specific hotel instead of a list of all hotels
        public async Task<Hotel> GetHotelFromIdAsync(int hotelNr)
        {
            Hotel hotel = new Hotel();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using SqlCommand command = new SqlCommand(queryStringFromID, connection);
                command.Parameters.AddWithValue("@ID", hotelNr);
                await command.Connection.OpenAsync();
                SqlDataReader reader = await command.ExecuteReaderAsync();
                if (reader.Read())
                {
                    hotel.HotelNr = reader.GetInt32(0);
                    hotel.Navn = reader.GetString(1);
                    hotel.Adresse = reader.GetString(2);
                    return hotel;
                }
            }

            return null;
        }
        //This method creates a new hotel object and adds it to the database
        //It takes a Hotel object as input and extracting the properties of the object one by one and
        //Adding it to the SQL query string, replacing the @ID, @Navn ad @Adresse placeholders with the proper values
        //Before running the SQL command and adding it to the database
        //It finishes by checking to see how many rows were affected (in this case we expect just 1)
        //If we get the right number of rows affected it returns true, otherwise something has gone wrong and it returns false instead
        public async Task<bool> CreateHotelAsync(Hotel hotel)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using SqlCommand command = new SqlCommand(insertSql, connection);
                command.Parameters.AddWithValue("@ID", hotel.HotelNr);
                command.Parameters.AddWithValue("@Navn", hotel.Navn);
                command.Parameters.AddWithValue("@Adresse", hotel.Adresse);
                await command.Connection.OpenAsync();

                int noOfRows = await command.ExecuteNonQueryAsync(); //bruges  ved update, delete, insert
                if (noOfRows == 1)
                {
                    return true;
                }

                return false;
            }
        }

        //This runs almost exactly like CreateHotelAsync, except it stores a copy of the Hotel that is being updated 
        //So that it can display the properties of the existing hotel properties, while it is being updated
        public async Task<bool> UpdateHotelAsync(Hotel hotel, int hotelNr)
        {
            Hotel HotelUpdate = hotel;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using SqlCommand command = new SqlCommand(updateSql, connection);
                command.Parameters.AddWithValue("@ID", hotelNr);
                command.Parameters.AddWithValue("@NAME", HotelUpdate.Navn);
                command.Parameters.AddWithValue("@ADDRESS", HotelUpdate.Adresse);
                await command.Connection.OpenAsync();

                int noOfRows = await command.ExecuteNonQueryAsync();
                if (noOfRows == 1)
                {
                    return true;
                }

                return false;
            }
        }

        //This runs the GetHotelFromIdAsync method to find the hotel that has been specified in the hotelNr input, and then runs a delete command
        public async Task<Hotel> DeleteHotelAsync(int hotelNr)
        {
            Task<Hotel> hotel = GetHotelFromIdAsync(hotelNr);
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using SqlCommand command = new SqlCommand(deleteSql, connection);
                command.Parameters.AddWithValue("@ID", hotelNr);
                await command.Connection.OpenAsync();
                int noOfRows = await command.ExecuteNonQueryAsync();
                if (noOfRows != 1)
                    return null;
                return await hotel;

            }
        }

        //This methods runs a check against all the names of hotels and compare them to the input string name, before returning any matches it finds
        //It creates a list so that in case more than 1 hotel matches the string it can show them all
        public async Task<List<Hotel>> GetHotelsByNameAsync(string name)
        {
            List<Hotel> hotels = new List<Hotel>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(queryNameString, connection);
                command.Parameters.AddWithValue("@NAME", name);
                await command.Connection.OpenAsync();
                SqlDataReader reader = command.ExecuteReader();
                string hotelName = reader.GetString(1);
                hotelName.ToLower();

                while (reader.Read())
                {
                    if (hotelName.Contains(name))
                    {
                        Hotel hotel = new Hotel(hotelNr: reader.GetInt32(0), navn: reader.GetString(1), adresse: reader.GetString(2));
                        hotels.Add(hotel);
                    }
                }
            }

            return hotels;
        }

    }

}
