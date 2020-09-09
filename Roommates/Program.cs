using System;
using System.Collections.Generic;
using Roommates.Models;
using Roommates.Repositories;

namespace Roommates
{
    class Program
    {
        /// <summary>
        ///  This is the address of the database.
        ///  We define it here as a constant since it will never change.
        /// </summary>
        private const string CONNECTION_STRING = @"server=localhost\SQLExpress;database=Roommates;integrated security=true";

        static void Main(string[] args)
        {
            RoomRepository roomRepo = new RoomRepository(CONNECTION_STRING);

            //Create
            //Room bathroom = new Room
            //{
            //    Name = "Bathroom",
            //    MaxOccupancy = 1
            //};

            //roomRepo.Insert(bathroom);

            //Console.WriteLine("-------------------------------");
            //Console.WriteLine($"Added the new Room with id {bathroom.Id}");

            //Update
            //Room bathroomUpdate = roomRepo.GetById(7);
            //bathroomUpdate.MaxOccupancy = 2;

            //roomRepo.Update(bathroomUpdate);

            //Delete
            //roomRepo.Delete(8);

            Console.WriteLine("Getting All Rooms:");
            Console.WriteLine();

            List<Room> allRooms = roomRepo.GetAll();

            foreach (Room room in allRooms)
            {
                Console.WriteLine($"{room.Id} {room.Name} {room.MaxOccupancy}");
            }

            Console.WriteLine("----------------------------");
            Console.WriteLine("Getting Room with Id 1");

            Room singleRoom = roomRepo.GetById(1);
            Console.WriteLine($"{singleRoom.Id} {singleRoom.Name} {singleRoom.MaxOccupancy}");


            //ROOMMATE


            RoommateRepository roommateRepo = new RoommateRepository(CONNECTION_STRING);

            //Roommate test = new Roommate()
            //{
            //    FirstName = "Chris",
            //    LastName = "Test",
            //    RentPortion = 100,
            //    MoveInDate = DateTime.Now,
            //    Room = singleRoom
            //};

            //roommateRepo.Insert(test);


            Console.WriteLine("----------------------------");
            Console.WriteLine("Getting Roommate with Id 1");

            Roommate singleRoommate = roommateRepo.GetById(1);
            Console.WriteLine($"{singleRoommate.Id} {singleRoommate.FirstName} {singleRoommate.LastName} {singleRoommate.RentPortion} {singleRoommate.MoveInDate}");

            singleRoommate.LastName = "Test3";
            Console.WriteLine($"{singleRoommate.LastName} {singleRoommate.Room.Id}");
            //roommateRepo.Update(singleRoommate);

            Console.WriteLine("----------------------------");
            Console.WriteLine("Getting All Roommates:");
            Console.WriteLine();

            List<Roommate> allRoommates = roommateRepo.GetAllWithRoom();

            foreach (Roommate roommate in allRoommates)
            {
                Console.WriteLine($"{roommate.Id} {roommate.FirstName} {roommate.LastName} {roommate.RentPortion} {roommate.MoveInDate} {roommate.Room.Id} {roommate.Room.Name}");
            }

            //roommateRepo.Delete(4);
        }
    }
}