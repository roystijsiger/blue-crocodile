using PinkPanther.BlueCrocodile.Core.Models;
using PinkPanther.BlueCrocodile.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PinkPanther.BlueCrocodile.Infrastructure.Data
{
    public class RoomRepository : IRoomRepository
    {
        private readonly IEnumerable<Room> _rooms;

        public RoomRepository()
        {
            _rooms = GenerateRooms();
        }

        public Task<IEnumerable<Room>> GetAllAsync() => Task.FromResult(_rooms);

        public Task<Room> GetAsync(string id) => Task.FromResult(_rooms.ToList().SingleOrDefault(room => room.Number.ToString() == id));

        private IEnumerable<Room> GenerateRooms()
        {
            var r = new Random();

            var result = new List<Room>();

            for (var i = 0; i < 7; i++)
            {
                result.Add(new Room
                {
                    Number = i + 1,
                    Seats = GenerateSeats().ToList(),
                    WeelChairAccessable = i % 3 == 0
                });
            }

            return result;
        }
        private IEnumerable<Seat> GenerateSeats()
        {
            var result = new List<Seat>();

            for (var row = 0; row < 20; row++)
            {
                for (var seatNumber = 0; seatNumber < 20; seatNumber++)
                {
                    result.Add(new Seat
                    {
                        Location = string.Empty,
                        Row = row,
                        SeatNumber = seatNumber
                    });
                }
            }

            return result;
        }

        public Task<Room> InsertAsync(Room item) => throw new NotImplementedException();
        public Task InsertManyAsync(List<Room> orders) => throw new NotImplementedException();
        public Task<Room> UpdateAsync(Room item) => throw new NotImplementedException();
        public Task RemoveAllAsync() => throw new NotImplementedException();
        public Task RemoveAsync(string id) => throw new NotImplementedException();
    }
}
