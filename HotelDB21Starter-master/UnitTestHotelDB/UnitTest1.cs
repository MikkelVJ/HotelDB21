using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RazorPageHotelApp.Models;
using RazorPageHotelApp.Services.HotelService;
using RazorPageHotelApp.Services.Interfaces;
using RazorPageHotelApp.Services.RoomService;

namespace UnitTestHotelDB
{
    [TestClass]
    public class HotelTest
    {
        private IHotelService _hotelService;

        private string connectionString =
            "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=HotelDbtest2;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        public HotelTest()
        {
            _hotelService = null;
        }

        private void Arrange()
        {
            _hotelService = new HotelService(connectionString);
        }

        private Hotel hotel = new Hotel(21, "UnitTestHotel", "UnitAddress");
        private List<Hotel> hotelList;
        private void ArrangeGetAllHotelAsync()
        {
            Arrange(); 
            //hotelList = await Task.Run(() => _hotelService.GetAllHotelAsync());
            hotelList = _hotelService.GetAllHotelAsync().Result;
        }

        [TestMethod]
        public void TestGetAllHotel()
        {
            //Arrange
            Arrange();
            ArrangeGetAllHotelAsync();
            
            //Act


            //Assert
            Assert.IsFalse(0 == hotelList.Count);
        }

    }

    [TestClass]
    public class RoomTest
    {
        private string connectionString =
            "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=HotelDbtest2;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        private IRoomService _roomService;

        public RoomTest()
        {
            _roomService = null;
        }
        public Room room = new Room(621, 'D', 351.25, 1);

        private void Arrenge()
        {
            _roomService = new RoomService(connectionString);
        }

        [TestMethod]
        public void TestCreateRoomAsync()
        {
            Arrenge();
            _roomService.CreateRoomAsync(room);
            Assert.IsNotNull(_roomService.GetRoomFromIdAsync(621, 1));
        }



    }
}
