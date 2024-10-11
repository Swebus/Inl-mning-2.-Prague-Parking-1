

string[] parkingSpaces = new string[101];


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
            MoveVehicle();
            break;
        case "3":
            //GetVehicle(); eller RemoveVehicle();
            break;
        case "4":
            //SearchVehicle();
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
        Console.WriteLine("Press any for continue:");
        Console.ReadLine();
    }
}




//ParkVehicle();


void MoveVehicle()
{
    Console.WriteLine("Type Registration number for move vehicle:  ");
    string regNr = Console.ReadLine();

    int currentSpace = FindVehicle(regNr); // Ändra om till ParkVehicle input name hos seb.

    if (currentSpace == -1)
    {
        Console.WriteLine("Vehicle didn´t found. ");
        return;
    }

    Console.WriteLine($"The Vehicle is parking {currentSpace}");
    Console.WriteLine("Type the new parking spot (1-100:) ");

    if (int.TryParse(Console.ReadLine(), out int newSpace) && newSpace > 0 && newSpace <= 100)
    {

        if (parkingSpaces[newSpace] == null)
        {
            parkingSpaces[newSpace] = parkingSpaces[currentSpace];
            parkingSpaces[currentSpace] = null;
            Console.WriteLine($"Vehicle move to {newSpace}");
        }
        else
        {
            Console.WriteLine("The spot is occupied.");
        }
    }
    else
    {
        Console.WriteLine("Error parking spot.");
    }
}

//GetVehicle(); eller RemoveVehicle();

//SearchVecicle();

//ShowParkingSpaces();





// Sök fordon inom kodning 
int FindVehicle(string regNr) // ändra om regNr 
{
    for (int i = 1; i < parkingSpaces.Length; i++)
    {           // Contains() matcha inte 100% Men Equals gör.
        if (parkingSpaces[i] != null && parkingSpaces[i].Equals(regNr, StringComparison.Ordinal))
        {
            return i;
        }
    }
    return -1;

}

