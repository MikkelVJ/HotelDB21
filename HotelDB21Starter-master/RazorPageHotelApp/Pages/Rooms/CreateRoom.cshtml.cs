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
    public class CreateRoomModel : PageModel
    {
        [BindProperty]
        public Room Room { get; set; }
        [BindProperty]
        public int hotelNr { get; set; }

        IRoomService roomService;

        public CreateRoomModel(IRoomService service)
        {
            this.roomService = service;
        }

        public async Task OnGetAsync(int hotelNumber)
        {
            hotelNr = hotelNumber;
        }

        public async Task<IActionResult> OnPostAsync(Room room)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await roomService.CreateRoomAsync(room);
            return RedirectToPage("GetAllHotels");
        }
    }
}
