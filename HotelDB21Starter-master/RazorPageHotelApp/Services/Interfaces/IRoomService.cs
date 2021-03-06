using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RazorPageHotelApp.Models;

namespace RazorPageHotelApp.Services.Interfaces
{
    public interface IRoomService
    {
        Task<List<Room>> GetAllRoomsAsync(int hotelNr);

        Task<Room> GetRoomFromIdAsync(int roomNr, int hotelNr);

        Task<bool> CreateRoomAsync(Room room);

        Task<bool> UpdateRoomAsync(Room room, int roomNr);

        Task<Room> DeleteRoomAsync(int roomNr, int hotelNr);

        Task<List<Room>> GetRoomsByNameAsync(string name);

    }
}
