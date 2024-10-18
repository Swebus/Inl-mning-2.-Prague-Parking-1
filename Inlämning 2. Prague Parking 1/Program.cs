


using System.Text.RegularExpressions;



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
            parkVehicle();
            break;
        case "2":
            MoveVehicle();
            break;
        case "3":
            RemoveVehicle();
            break;
        case "4":
            SearchVehicle();
            break;
        case "5":
            ShowParkingSpaces();
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

DateTime checkIn = DateTime.Now;

void parkVehicle()
{
    string vehicleType = "";
    bool typeCheck = false;
    while (!typeCheck)
    {
        Console.Write("Enter 1 for Mc, 2 for Car or 3 to return to main menu: ");
        string vehicleTypeInput = Console.ReadLine();
        switch (vehicleTypeInput)
        {
            case "1":
                {
                    vehicleType = "mc"; 
                    typeCheck = true;
                    break;
                }
            case "2":
                {
                    vehicleType = "car";
                    typeCheck = true;
                    break;
                }
            case "3":
                {
                    return;
                }
            default:
                {
                    Console.WriteLine("Invalid choice, try again!");
                    break;
                }
        }
    }
    Console.Write("Enter vehicle registration number: ");
    string regNumber = Console.ReadLine();
    
    if ((regNumber.Length > 10) | (regNumber.Length < 1) | (ContainsSpecialCharacters(regNumber)))
    {
        Console.WriteLine("Invalid Regstration number, returning to main menu.");
    }
    else
    {
        int alreadyParked = alreadyRegistered(regNumber);
        if (alreadyParked != -1)
        {
            Console.WriteLine("Vehicle already registered, returning to main menu.");
            return;
        }

        string vehicleDesignation = vehicleType + "#" + regNumber, checkIn;
        string checkstring;
        for (int i = 1; i < parkingSpaces.Length; i++)
        {
            checkstring = parkingSpaces[i];
            if (vehicleDesignation.Contains("mc"))
            {
                if (checkstring == null)
                {
                    parkingSpaces[i] = vehicleDesignation;
                    Console.WriteLine("\nVehicle parked on parking spot number: {0}", i);
                    break;
                }
                else if ((!checkstring.Contains("car")) && (!checkstring.Contains("|")))
                {
                    parkingSpaces[i] = parkingSpaces[i] + "|" + vehicleDesignation;
                    Console.WriteLine("Vehicle parked on parking spot number: {0}", i);
                    break;
                }
            }
            else 
            {
                if (checkstring == null)
                {
                    parkingSpaces[i] = vehicleDesignation;
                    Console.WriteLine("Vehicle parked on parking spot number: {0}", i);
                    break;
                }
            }
        }
    }
}
static bool ContainsSpecialCharacters(string input)
{
    return Regex.IsMatch(input, @"[^\p{L}\p{N}]");
}

void MoveVehicle()
{
    Console.WriteLine($"VehicleTyp & registration number: {ColorExample()}");
    string regNumber = Console.ReadLine().ToLower();

    // hitta var fordonet är parkerat.
    int currentSpace = FindVehicle(regNumber); 

    if (currentSpace == -1)
    {
        Console.WriteLine("Vehicle not found. ");
        return;
    }
    
    //kolla om 2 Mc delar plats.
    if (parkingSpaces[currentSpace].Contains("|"))
    {
        string[] mcArray = parkingSpaces[currentSpace].Split( '|');

        if (mcArray[0].Contains(regNumber))
        {
            //flytta första Mc och lämna den andra.
            MoveSingleVehicle(currentSpace, regNumber, mcArray[1]);
        }
        else if (mcArray[1].Contains(regNumber))
        {
            // Flytta andra Mc och lämna den första.
            MoveSingleVehicle(currentSpace, regNumber, mcArray[0]);
        }

    }
    else
    {
        // 1 fordon i parking flytta.
        MoveSingleVehicle(currentSpace, regNumber,null);
    }
}
void MoveSingleVehicle(int currentSpace, string regNumber, string remainingVehicle)
{
    Console.WriteLine($"The Vehicle is parking {currentSpace}");
    Console.WriteLine("Type the new parking spot (1-100:) ");

    if (int.TryParse(Console.ReadLine(), out int newSpace) && newSpace > 0 && newSpace <= 100)
    {
        if (parkingSpaces[newSpace] == null)
        {
            //flytta fordon till nya platsen.
            parkingSpaces[newSpace] = regNumber;

            //update gamla platsen till null.
            if (remainingVehicle == null)
            {
                parkingSpaces[currentSpace] = null;
            }
            else
            {
                // lämna kvar den andra Mc på gamla platsen.
                parkingSpaces[currentSpace] = remainingVehicle;
            }
            Console.WriteLine($"Vehicle moved to spot {newSpace}");
        }
        else
        {
            Console.WriteLine("The New Parking spot is occupied.");
        }

    }
    else
    {
        Console.WriteLine("Invalid parking spot.");
    }
}

void RemoveVehicle()
{
    Console.WriteLine($"Enter Vehicle type & registration number to remove: {ColorExample()}");
    string regNumber = Console.ReadLine().ToLower();

    // Hitta var fordonet är parkerat
    int currentSpace = FindVehicle(regNumber);

    if (currentSpace == -1)
    {
        Console.WriteLine("Vehicle not found.");
        return;
    }

    // Kolla om 2 Mc delar plats.
    if (parkingSpaces[currentSpace].Contains("|")) 
    {
        string[] mcArray = parkingSpaces[currentSpace].Split('|'); 

        if (mcArray[0].Contains(regNumber)) 
        {
            // Ta bort första Mc och lämna den andra.
            RemoveSingleVehicle(currentSpace, regNumber, mcArray[1]);
        }
        else if (mcArray[1].Contains(regNumber)) 
        {
            // Ta bort andra Mc och lämna den första.
            RemoveSingleVehicle(currentSpace, regNumber, mcArray[0]);
        }
    }
    else
    {
        // 1 fordon på plats, ta bort det.
        RemoveSingleVehicle(currentSpace, regNumber, null);
    }
}
void RemoveSingleVehicle(int currentSpace, string regNumber, string remainingVehicle)
{
    // Bekräfta borttagningen
    string regNumb = RemoveVehicleType(regNumber);
    Console.WriteLine($"The vehicle {regNumb} is parked at spot {currentSpace}. Do you want to remove it? (y/n)");
    string confirmation = Console.ReadLine().ToLower();

    if (confirmation == "y")
    {
        // Om det finns ett annat Mc på platsen, lämna kvar det
        if (remainingVehicle == null)
        {
            // Rensa platsen om inget annat fordon finns kvar
            parkingSpaces[currentSpace] = null;
            Console.WriteLine($"Vehicle {regNumb} removed from spot {currentSpace}.");
        }
        else
        {
            // Om det finns ett annat Mc, uppdatera platsen till endast det återstående fordonet
            parkingSpaces[currentSpace] = remainingVehicle;
            Console.WriteLine($"Vehicle {regNumb} removed. {remainingVehicle} remains at spot {currentSpace}.");
        }
    }
    
}

//SearchVecicle();
void SearchVehicle()
{
    Console.WriteLine($"VehicleTyp & registration number: {ColorExample()}");
    string regNumber = Console.ReadLine().ToLower();

    int space = FindVehicle(regNumber);
    if (space == -1)
    {
        Console.WriteLine("Vehicle didn´t found.");
    }
    else
    {
        string foundVehicle = RemoveVehicleType(parkingSpaces[space]);
        Console.WriteLine($"Vehicle with {foundVehicle} are parking on {space}.");
    }
}

//ShowParkingSpaces();
void ShowParkingSpaces()
{
    Console.WriteLine("Parking Space and Vehicle: ");
    for (int i = 1; i < parkingSpaces.Length; i++)
    {
        if (parkingSpaces[i] != null)
        {
            string vehicleOnSpace = RemoveVehicleType(parkingSpaces[i]);
            Console.WriteLine($"Space: {i}: {vehicleOnSpace}");
        }
        else
        {
            Console.WriteLine($"Space: {i}: Empty");
        }
    }
}


int FindVehicle(string regNumber) 
{
    for (int i = 1; i < parkingSpaces.Length; i++)
    {           
        if (parkingSpaces[i] != null)
        {
            string[] vehicles = parkingSpaces[i].Split('|');

            foreach (var vehicle in vehicles)
            {
                if (vehicle.Equals(regNumber, StringComparison.OrdinalIgnoreCase))
                {
                    return i;
                }
            }
        }  
    }
    return -1;
}
int alreadyRegistered(string regNumberCheck) 
{
    for (int i = 1; i < parkingSpaces.Length; i++)
    {           
        if (parkingSpaces[i] != null)
        {
            string[] vehicles = parkingSpaces[i].Split('|');

            foreach (var vehicle in vehicles)
            {
                string[] number = vehicle.Split("#");
                foreach (var letterNumber in number)
                {
                    if (letterNumber.Equals(regNumberCheck, StringComparison.OrdinalIgnoreCase))
                    {
                        return i;
                    }
                }
            }
        }  
    }
    return -1;
}

// Ta bort mc#/car# så endast regnr som output.
static string RemoveVehicleType(string vehicle )
{
    return vehicle.Replace("car#", "", StringComparison.OrdinalIgnoreCase)
                                                     .Replace("mc#", "", StringComparison.OrdinalIgnoreCase);

}

// Färg för exmeple Input Fordon.
static string ColorExample()
{
    string color = "\n\u001b[90m(Mc#aaa111 or Car#aaa123)\u001b[0m";
    return color;
}


void CheckOut()
{
    
}
