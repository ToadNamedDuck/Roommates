using Roommates.Models;
using Roommates.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Roommates
{
    class Program
    {
        //  This is the address of the database.
        //  We define it here as a constant since it will never change.
        private const string CONNECTION_STRING = @"server=localhost\SQLExpress;database=Roommates;integrated security=true;TrustServerCertificate=true;";

        static void Main(string[] args)
        {
            RoomRepository roomRepo = new RoomRepository(CONNECTION_STRING);
            ChoreRepository choreRepo = new ChoreRepository(CONNECTION_STRING);
            RoommateRepository rmRepo = new RoommateRepository(CONNECTION_STRING);
            bool runProgram = true;
            while (runProgram)
            {
                string selection = GetMenuSelection();

                switch (selection)
                {
                    case ("Show all rooms"):
                        List<Room> allRooms = roomRepo.GetAll();
                        foreach (Room r in allRooms)
                        {
                            Console.WriteLine($"{r.Name} has an Id of {r.Id} and a max occupancy of {r.MaxOccupancy}");
                        }
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case ("Search for room by ID"):
                        Console.WriteLine("Room Id: ");
                        int roomId = int.Parse(Console.ReadLine());
                        Room room = roomRepo.GetById(roomId);
                        Console.WriteLine($"{room.Id} - {room.Name} Max Occupancy({room.MaxOccupancy})");
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case ("Add a room"):
                        Console.Write("Room name: ");
                        string name = Console.ReadLine();

                        Console.Write("Max occupancy: ");
                        int max = int.Parse(Console.ReadLine());

                        Room roomToAdd = new Room()
                        {
                            Name = name,
                            MaxOccupancy = max
                        };

                        roomRepo.Insert(roomToAdd);

                        Console.WriteLine($"{roomToAdd.Name} has been added and assigned an Id of {roomToAdd.Id}");
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case ("Show all chores"):
                        List<Chore> allChores = choreRepo.GetAll();
                        foreach(Chore chore in allChores)
                        {
                            Console.WriteLine($"{chore.Name} has Id {chore.Id}");
                        }
                        Console.WriteLine("Press any key to continue...");
                        Console.ReadKey();
                        break;
                    case ("Search for chore by ID"):
                        Console.WriteLine("Enter the ID of the chore you would like to see.");
                        int choreId = int.Parse(Console.ReadLine());
                        Chore singleChore = choreRepo.GetById(choreId);
                        Console.WriteLine($"{singleChore.Name} has an ID of {singleChore.Id}");
                        Console.WriteLine("Press any key to continue...");
                        Console.ReadKey();
                        break;
                    case ("Add a chore"):
                        Console.WriteLine("Chore name: ");
                        string choreName = Console.ReadLine();
                        Chore choreToAdd = new Chore()
                        {
                            Name = choreName
                        };
                        choreRepo.Insert(choreToAdd);

                        Console.WriteLine($"{choreToAdd.Name} has been added with an ID of {choreToAdd.Id}");
                        Console.WriteLine("Press any key to continue...");
                        Console.ReadKey();
                        break;
                    case ("Select a Roommate by ID"):
                        Console.WriteLine("Roommate ID: ");
                        int rmId = int.Parse(Console.ReadLine());
                        Roommate foundRm = rmRepo.GetById(rmId);
                        Console.WriteLine($"{foundRm.FirstName} has a rent portion of {foundRm.RentPortion}% and resides in {foundRm.Room.Name}.");
                        Console.WriteLine("Press any key to continue...");
                        Console.ReadKey();
                        break;
                    case ("View unassigned chores"):
                        List<Chore> foundChores = choreRepo.GetUnassignedChores();
                        foreach ( Chore chore in foundChores )
                        {
                            Console.WriteLine($"{chore.Id}) {chore.Name}");
                        }
                        Console.WriteLine("Press any key to continue...");
                        Console.ReadKey();
                        break;
                    case ("Assign a chore"):
                        List<Chore> u_chores = choreRepo.GetUnassignedChores();
                        foreach( Chore chore in u_chores )
                        {
                            Console.WriteLine($"{chore.Id}) {chore.Name}");
                        }
                        Console.WriteLine("Select an unassigned chore: ");
                        int assignedChoreId = int.Parse(Console.ReadLine());
                        List<Roommate> rms = rmRepo.GetAll();
                        foreach(Roommate rm in rms )
                        {
                            Console.WriteLine($"{rm.Id}) {rm.FirstName} {rm.LastName}");
                        }
                        Console.WriteLine("Select a roommate");
                        int assignedRmId = int.Parse(Console.ReadLine());
                        choreRepo.AssignChore(assignedChoreId, assignedRmId);
                        Console.WriteLine("Press any key to continue...");
                        Console.ReadKey();
                        break;
                    case ("Exit"):
                        runProgram = false;
                        break;
                    case ("Update a room"):
                        List<Room> roomOptions = roomRepo.GetAll();
                        foreach (Room r in roomOptions)
                        {
                            Console.WriteLine($"{r.Id} - {r.Name} Max Occupancy({r.MaxOccupancy})");
                        }

                        Console.Write("Which room would you like to update? ");
                        int selectedRoomId = int.Parse(Console.ReadLine());
                        Room selectedRoom = roomOptions.FirstOrDefault(r => r.Id == selectedRoomId);

                        Console.Write("New Name: ");
                        selectedRoom.Name = Console.ReadLine();

                        Console.Write("New Max Occupancy: ");
                        selectedRoom.MaxOccupancy = int.Parse(Console.ReadLine());

                        roomRepo.Update(selectedRoom);

                        Console.WriteLine("Room has been successfully updated");
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case ("Delete a room"):
                        List<Room> rooms = roomRepo.GetAll();
                        Console.WriteLine("Which room would you like to delete?\nPlease enter the ID of the room.");
                        foreach(Room roomD in rooms)
                        {
                            Console.WriteLine($"{roomD.Id} - {roomD.Name}");
                        }
                        int roomDeleteId = int.Parse(Console.ReadLine());
                        roomRepo.Delete(roomDeleteId);
                        Console.WriteLine("Press any key to continue...");
                        Console.ReadKey();
                        break;
                }
            }

        }

        static string GetMenuSelection()
        {
            Console.Clear();

            List<string> options = new List<string>()
            {
                "Show all rooms",
                "Search for room by ID",
                "Add a room",
                "Show all chores",
                "Search for chore by ID",
                "Add a chore",
                "Exit",
                "Select a Roommate by ID",
                "View unassigned chores",
                "Assign a chore",
                "Update a room",
                "Delete a room"
            };

            for (int i = 0; i < options.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {options[i]}");
            }

            while (true)
            {
                try
                {
                    Console.WriteLine();
                    Console.Write("Select an option > ");

                    string input = Console.ReadLine();
                    int index = int.Parse(input) - 1;
                    return options[index];
                }
                catch (Exception)
                {

                    continue;
                }
            }
        }
    }
}