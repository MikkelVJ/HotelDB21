using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using HotelDBConsole21.Interfaces;
using HotelDBConsole21.Models;
using Microsoft.Data.SqlClient;

namespace HotelDBConsole21.Services
{
    public class HotelServiceAsync : Connection, IHotelServiceAsync
    {

        private String queryString = "select * from Hotel";
        private String queryStringFromID = "select * from Hotel where Hotel_No = @ID";
        private String insertSql = "insert into Hotel Values (@ID, @Navn, @Adresse)";
        private String deleteSql = "delete from Hotel where Hotel_No = @ID";

        private String updateSql = "update Hotel " +
                                   "set Hotel_No= @HotelID, Name=@Navn, Address=@Adresse " +
                                   "where Hotel_No = @ID";

        public async Task<List<Hotel>> GetAllHotelAsync()
        {
            List<Hotel> hoteller = new List<Hotel>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                await command.Connection.OpenAsync();
                Thread.Sleep(1000);
                SqlDataReader reader = await command.ExecuteReaderAsync();
                while (reader.Read())
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

        public Task<List<Hotel>> GetHotelsByNameAsync(string name)
        {
            throw new NotImplementedException();
        }
    }
}

