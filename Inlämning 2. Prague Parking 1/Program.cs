// Vårt spar plats
string[] parkingSpaces = new string[100];

// App system
bool exit = false;
while (!exit) // True nu
{
    Console.WriteLine("Welcome to Prague parking");
    Console.WriteLine("1. Park Vechicle.");
    Console.WriteLine("2. Move Vechicle.");
    Console.WriteLine("3. Get Vechicle.");
    Console.WriteLine("4. Search Vechicle.");
    Console.WriteLine("5. Show Vechicle.");
    Console.WriteLine("6. Exit.");

    string choice = Console.ReadLine();

    switch (choice)
    {
        case "1":
            //ParkVehicle();
            break;
        case "2":
            //MoveVehicle();
            break;
        case "3":
            //GetVehicle(); eller RemoveVehicle();
            break;
        case "4":
            //SearchVecicle();
            break;
        case "5":
            //ShowParkingSpaces();
            break;
        case "6":
            exit = true; // ändra till false
            break;
        default:
            Console.WriteLine("Invalid choice, try again.");
            break;
    }
    if (!exit)
    {
        Console.WriteLine("Press any key to continue: ");
        Console.ReadLine();
    }
}


// Våra metoder vad dom ska utföra.

//ParkVehicle();

//MoveVehicle();

//GetVehicle(); eller RemoveVehicle();

//SearchVecicle();

//ShowParkingSpaces();