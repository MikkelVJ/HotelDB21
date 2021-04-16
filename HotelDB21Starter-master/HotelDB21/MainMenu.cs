using System;
using System.Collections.Generic;
using System.Text;
using HotelDBConsole21.Models;
using HotelDBConsole21.Services;

namespace HotelDBConsole21
{
    public static class MainMenu
    {
        //Lav selv flere menupunkter til at vælge funktioner for Rooms
        public static void showOptions()
        {
            Console.Clear();
            Console.WriteLine("Vælg et menupunkt");
            Console.WriteLine("1) List hoteller");
            Console.WriteLine("2) Opret nyt Hotel");
            Console.WriteLine("3) Fjern Hotel");
            Console.WriteLine("4) Søg efter hotel udfra hotelnr");
            Console.WriteLine("5) Opdater et hotel");
            Console.WriteLine("6) List alle værelser");
            Console.WriteLine("7) List alle værelser til et bestemt hotel");
            Console.WriteLine("8) Flere menupunkter kommer snart :) ");
            Console.WriteLine("9) Sæg efter hotel udfra navn");
            Console.WriteLine("Q) Afslut");
        }

        public static bool Menu()
        {
            showOptions();
            switch (Console.ReadLine())
            {
                case "1":
                    ShowHotels();
                    return true;
                case "2":
                    CreateHotel();
                    return true;
                case "3":

                    return true;
                case "4":
                    Console.WriteLine("Indtast HotelNr");
                    int hotelnr = int.Parse(Console.ReadLine());
                    GetHotelsFromId(hotelnr);
                    return true;
                case "5":
                    Console.WriteLine("Indtast HotelNr");
                    int hoteltoupdate = int.Parse(Console.ReadLine());
                    Console.WriteLine($"Hotel udvalgt:");
                    GetHotelsFromId(hoteltoupdate);
                    Console.WriteLine();
                    //Console.WriteLine("Indtast nyt HotelNr:");
                    int Nr = hoteltoupdate;
                    Console.WriteLine("Indtast Nyt Hotel navn:");
                    string nytNavn = Console.ReadLine();
                    Console.WriteLine("Indtast ny Hotel adresse:");
                    string nyAdresse = Console.ReadLine();
                    
                    Hotel NewHotel = new Hotel(Nr, nytNavn, nyAdresse);
                    UpdateHotel(NewHotel, hoteltoupdate);
                    return true;
                case "9":
                    Console.Clear();
                    Console.WriteLine("Indtast hotel navn:");
                    string hotelName = Console.ReadLine().ToLower().Trim();
                    FindHotelByName(hotelName);
                    return true;
                case "Q": 
                case "q": return false;
                default: return true;
            }

        }

        private static void CreateHotel()
        {
            //indlæs data
            Console.Clear();
            Console.WriteLine("Indlæs hotelnr");
            int hotelnr = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Indlæs hotelnavn");
            string hotelnavn = Console.ReadLine();
            Console.WriteLine("Indlæs hoteladresse");
            string hoteladrese = Console.ReadLine();


            //kalde hotelservice og vise resultat
            HotelService hs = new HotelService();
            bool ok = hs.CreateHotel(new Hotel(hotelnr, hotelnavn, hoteladrese));
            if (ok)
            {
                Console.WriteLine("Hotellet blev oprettet");
            }
            else
            {
                Console.WriteLine("Fejl: Hotellet blev ikke oprettet");
            }

        }

        private static void ShowHotels()
        {
            Console.Clear();
            HotelService hs = new HotelService();
            List<Hotel> hotels = hs.GetAllHotel();
            foreach (Hotel hotel in hotels)
            {
                Console.WriteLine($"HotelNr {hotel.HotelNr} Name {hotel.Navn} Address {hotel.Adresse}");
            }
        }

        private static void GetHotelsFromId(int HotelNr)
        {
            Console.Clear();
            HotelService hs = new HotelService();
            Hotel hotel = hs.GetHotelFromId(HotelNr);
            if (hotel != null)
            {
                Console.WriteLine(hotel.ToString());
            }
            else
            {
                Console.WriteLine("Fejl: Hotellet findes ikke.");
            }
        }

        private static void UpdateHotel(Hotel hotel, int HotelNr)
        {
            Console.Clear();
            HotelService hs = new HotelService();
            bool ok = hs.UpdateHotel(hotel, HotelNr);
            if (ok)
            {
                Console.WriteLine($"Hotel info opdateret: \nHotel Navn: {hotel.Navn} | Hotel #: {hotel.HotelNr} | Hotel Addresse: {hotel.Adresse}");
            }
            else
            {
                Console.WriteLine("Fejl: Kunne ikke opdatere");
            }

        }

        private static void DeleteHotel(int HotelNr)
        {
            Console.Clear();

        }

        private static void FindHotelByName(string hotelName)
        {
            Console.Clear();
            HotelService hs = new HotelService();
            List<Hotel> hotels = new List<Hotel>();

            hotels = hs.GetHotelsByName(hotelName);
            foreach (var h in hotels)
            {
                Console.WriteLine($"HotelNr {h.HotelNr} Name {h.Navn} Address {h.Adresse}");
            }

        }
    }
}
