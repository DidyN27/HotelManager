using Hotel.Models;

namespace Hotel.Services
{
    public interface IRoomService
    {
        // Всички потребители могат да разглеждат стаите 
        IEnumerable<Room> GetAllRooms();

        // Филтриране по капацитет, тип и заетост 
        IEnumerable<Room> GetFilteredRooms(int? capacity, string type, bool? isFree);
        // Само администраторът може да добавя, редактира и трие 
        void AddRoom(Room room);
        void UpdateRoom(Room room);
        void DeleteRoom(int id);
        Room GetRoomById(int id);
    }
}