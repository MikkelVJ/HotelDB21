using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorPageHotelApp.Models;
using RazorPageHotelApp.Services.Interfaces;

namespace RazorPageHotelApp.Pages.Rooms
{
    public class GetAllRoomsModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public string FilterCriteria { get; set; }
        [BindProperty]
        public int hotelNumber { get; set; }

        public List<Room> Rooms { get; private set; }

        private IRoomService roomService;

        public GetAllRoomsModel(IRoomService RService)
        {
            this.roomService = RService;
        }

        public async Task OnGetMyRooms(int hotelNr)
        {
            hotelNumber = hotelNr;
            if (!String.IsNullOrEmpty(FilterCriteria))
            {
                Rooms = await roomService.GetRoomsByNameAsync(FilterCriteria);
            }
            else
                Rooms = await roomService.GetAllRoomsAsync(hotelNr);
        }
    }
}
