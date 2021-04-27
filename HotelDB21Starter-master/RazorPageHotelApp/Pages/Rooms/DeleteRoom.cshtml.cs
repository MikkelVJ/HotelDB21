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
    public class DeleteRoomModel : PageModel
    {
        [BindProperty]
        public Room room { get; set; }

        private IRoomService hotelService;

        public DeleteRoomModel(IRoomService service)
        {
            this.hotelService = service;
        }
        public async Task OnGetAsync(int roomNumber, int hotelNumber)
        {
            room = await hotelService.GetRoomFromIdAsync(roomNumber, hotelNumber);
        }

        public async Task<IActionResult> OnPostAsync()
        {
            await hotelService.DeleteRoomAsync(room.RoomNr, room.HotelNr);
            return RedirectToPage("GetAllHotels");
        }
    }
}
