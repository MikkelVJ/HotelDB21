using System;
using System.Collections.Generic;
using System.Text;
using HotelDBConsole21.Interfaces;
using HotelDBConsole21.Models;
using Microsoft.Data.SqlClient;

namespace HotelDBConsole21.Services
{
    public class HotelService : Connection, IHotelService
    {
        private string queryString = "select * from Hotel";
        private String queryStringFromID = "select * from Hotel where Hotel_No = @ID";
        private string queryStringFromName = "select * from Hotel where Name like %@NAME%";
        private string insertSql = "insert into Hotel Values(@ID, @Navn, @Adresse)";
        private string deleteSql = "delete from Hotel where Hotel_No = @ID";
        private string updateSql = "update Hotel set Name = @NAME, Address = @ADDRESS where Hotel_No = @ID";
        // lav selv sql strengene færdige og lav gerne yderligere sqlstrings


        public List<Hotel> GetAllHotel()
        {
            List<Hotel> hoteller = new List<Hotel>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                command.Connection.Open();
                SqlDataReader reader = command.ExecuteReader();
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

        public Hotel GetHotelFromId(int hotelNr)
        {
            Hotel hotel = new Hotel();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(queryStringFromID, connection);
                command.Parameters.AddWithValue("@ID", hotelNr);
                command.Connection.Open();
                SqlDataReader reader = command.ExecuteReader();
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

        public bool CreateHotel(Hotel hotel)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(insertSql, connection);
                command.Parameters.AddWithValue("@ID", hotel.HotelNr);
                command.Parameters.AddWithValue("@Navn", hotel.Navn);
                command.Parameters.AddWithValue("@Adresse", hotel.Adresse);
                command.Connection.Open();

               int noOfRows = command.ExecuteNonQuery(); //bruges  ved update, delete, insert
               if (noOfRows == 1)
               {
                   return true;
               }

               return false;
            }
        }

        public bool UpdateHotel(Hotel hotel, int hotelNr)
        {
            Hotel HotelUpdate = hotel;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(updateSql, connection);
                command.Parameters.AddWithValue("@ID", hotelNr);
                command.Parameters.AddWithValue("@NAME", HotelUpdate.Navn);
                command.Parameters.AddWithValue("@ADDRESS", HotelUpdate.Adresse);
                command.Connection.Open();

                int noOfRows = command.ExecuteNonQuery();
                if (noOfRows == 1)
                {
                    return true;
                }

                return false;
            }
        }

        public Hotel DeleteHotel(Hotel hotelNr)
        {
            throw new NotImplementedException();
        }

        public Hotel DeleteHotel(int hotelNr)
        {
            Hotel hotel = GetHotelFromId(hotelNr);
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(deleteSql, connection);
                command.Parameters.AddWithValue("@ID", hotelNr);
                command.Connection.Open();
                int noOfRows = command.ExecuteNonQuery();
                if (noOfRows != 1)
                    return null;
            }

            return hotel;
        }

        public List<Hotel> GetHotelsByName(string name)
        {
            List<Hotel> hotels = new List<Hotel>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(queryStringFromName, connection);
                command.Parameters.AddWithValue("@NAME", name);
                command.Connection.Open();
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
