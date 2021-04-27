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
    public class CreateHotelModel : PageModel
    {
        [BindProperty]
        public Hotel Hotel { get; set; }

        IHotelService hotelService;

        public CreateHotelModel(IHotelService service)
        {
            this.hotelService = service;
        }

        public async Task OnGetAsync()
        {

        }

        public async Task<IActionResult> OnPostAsync(Hotel hotel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await hotelService.CreateHotelAsync(hotel);
            return RedirectToPage("GetAllHotels");
        }
    }
}
