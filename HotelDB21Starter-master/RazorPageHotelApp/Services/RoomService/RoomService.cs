using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using RazorPageHotelApp.Models;
using RazorPageHotelApp.Services.Interfaces;

namespace RazorPageHotelApp.Services.RoomService
{
    public class RoomService : Connection, IRoomService
    {
        private String queryString = "select * from Room where Hotel_No = @HNO";

        private String queryNoString = "select * from Room where Hotel_No = @HNO and Room_No = @RNO";

        //private String queryStringFromID = 
        private String insertSql = "insert into Room Values (@RNO, @HNO, @TYPE, @PRICE)";

        private String deleteSql = "delete from Room where Room_No = @RNO and Hotel_No = @HNO";
        //private String updateSql = 

        public RoomService(IConfiguration configuration) : base(configuration)
        {
        }

        public RoomService(string connectionString) : base(connectionString)
        {

        }

        public async Task<List<Room>> GetAllRoomsAsync(int hotelNr)
        {
            List<Room> rooms = new List<Room>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                command.Parameters.AddWithValue("@HNO", hotelNr);
                await command.Connection.OpenAsync();

                SqlDataReader reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    int roomNr = reader.GetInt32(0);
                    int hotelNumber = reader.GetInt32(1);
                    char roomType = reader.GetString(2)[0];
                    double roomPrice = reader.GetDouble(3);
                    Room room = new Room(roomNr, roomType, roomPrice, hotelNumber);
                    rooms.Add(room);

                }
            }

            return rooms;
        }

        //public async Task<List<Hotel>> GetAllHotelAsync()
        //{
        //    List<Hotel> hoteller = new List<Hotel>();
        //    using (SqlConnection connection = new SqlConnection(connectionString))
        //    {
        //        SqlCommand command = new SqlCommand(queryString, connection);
        //        await command.Connection.OpenAsync();

        //        SqlDataReader reader = await command.ExecuteReaderAsync();
        //        while (await reader.ReadAsync())
        //        {
        //            int hotelNr = reader.GetInt32(0);
        //            String hotelNavn = reader.GetString(1);
        //            String hotelAdr = reader.GetString(2);
        //            Hotel hotel = new Hotel(hotelNr, hotelNavn, hotelAdr);
        //            hoteller.Add(hotel);
        //        }
        //    }
        //    return hoteller;
        //}

        public async Task<Room> GetRoomFromIdAsync(int roomNr, int hotelNr)
        {
            Room room = new Room();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using SqlCommand command = new SqlCommand(queryNoString, connection);
                command.Parameters.AddWithValue("@RNO", roomNr);
                command.Parameters.AddWithValue("@HNO", hotelNr);

                await command.Connection.OpenAsync();
                SqlDataReader reader = await command.ExecuteReaderAsync();
                if (reader.Read())
                {
                    int roomNumberr = reader.GetInt32(0);
                    int hotelNumber = reader.GetInt32(1);
                    char roomType = reader.GetString(2)[0];
                    double roomPrice = reader.GetDouble(3);
                    room = new Room(roomNr, roomType, roomPrice, hotelNumber);

                    return room;
                }
            }

            return null;
        }

        public async Task<bool> CreateRoomAsync(Room room)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using SqlCommand command = new SqlCommand(insertSql, connection);
                command.Parameters.AddWithValue("@RNO", room.RoomNr);
                command.Parameters.AddWithValue("@HNO", room.HotelNr);
                command.Parameters.AddWithValue("@TYPE", room.Types);
                command.Parameters.AddWithValue("@PRICE", room.Pris);
                await command.Connection.OpenAsync();

                int noOfRows = await command.ExecuteNonQueryAsync(); //bruges  ved update, delete, insert
                if (noOfRows == 1)
                {
                    return true;
                }

                return false;
            }

        }

        public Task<bool> UpdateRoomAsync(Room room, int roomNr)
        {
            throw new NotImplementedException();
        }

        public async Task<Room> DeleteRoomAsync(int roomNr, int hotelNr)
        {
            Task<Room> room = GetRoomFromIdAsync(roomNr, hotelNr);
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using SqlCommand command = new SqlCommand(deleteSql, connection);
                command.Parameters.AddWithValue("@RNO", roomNr);
                command.Parameters.AddWithValue("@HNO", hotelNr);

                await command.Connection.OpenAsync();
                int noOfRows = await command.ExecuteNonQueryAsync();
                if (noOfRows != 1)
                    return null;
                return await room;
            }

        }

        public Task<List<Room>> GetRoomsByNameAsync(string name)
        {
            throw new NotImplementedException();
        }
    }
}
