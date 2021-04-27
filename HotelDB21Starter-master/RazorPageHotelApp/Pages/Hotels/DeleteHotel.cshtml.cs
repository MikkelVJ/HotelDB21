using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorPageHotelApp.Models;
using RazorPageHotelApp.Services.Interfaces;

namespace RazorPageHotelApp.Pages.Hotels
{
    public class DeleteHotelModel : PageModel
    {
        [BindProperty]
        public Hotel Hotel { get; set; }

        private IHotelService hotelService;

        public DeleteHotelModel(IHotelService service)
        {
            this.hotelService = service;
        }
        public async Task OnGetAsync(int id)
        {
            Hotel = await hotelService.GetHotelFromIdAsync(id);
        }

        public async Task<IActionResult> OnPostAsync()
        {
            await hotelService.DeleteHotelAsync(Hotel.HotelNr);
            return RedirectToPage("GetAllHotels");
        }
        //I made changes but i cant commit them???
    }
}
