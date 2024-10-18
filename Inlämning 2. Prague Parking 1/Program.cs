


using System.Runtime.CompilerServices;
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
            //getVehicle();
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

        Console.WriteLine("Press any key to continue: ");

        Console.ReadLine();
    }
}





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

        string vehicleDesignation = vehicleType + "#" + regNumber;
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
    Console.WriteLine("VehicleTyp & registration number: \n\u001b[90m(Car#aaa111 or Mc#aaa123)\u001b[0m");
    string regNumber = Console.ReadLine().ToLower();

    // hitta var fordonet är parkerat.
    int currentSpace = FindVehicle(regNumber); 

    if (currentSpace == -1)
    {
        Console.WriteLine("Vehicle didn´t found. ");
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


void getVehicle()
{
    //void GetVehicle ()
    //Get regNr input- får vara max 10 tecken

    int choice = 0; //användbar för användaren park/remove

    Console.WriteLine("Enter vehicle(s) registration number");
    string regNumber = Console.ReadLine();
    //fråga efter fordonstyp
    //spara enskilt då två mc kan vara på samma plats

    if (regNumber.Length >= 10 || regNumber == null || regNumber == " ")
    {
        Console.WriteLine("Invalid registration number");
    }
    bool vehicleFound = false;
    //leta igenom alla platser med en forloop om regnr är giltigt (100 platser)
    for (int i = 0; i < parkingSpaces.Length; i++)
    {
        //kontrollera att fordonet finns på en plats
        if (parkingSpaces[i] != null && parkingSpaces[i] == regNumber)
        {
            vehicleFound = true;
            Console.WriteLine($"Vehicle {regNumber} has been removed from space {i + 1}.");
            break;// varför +1? för att bli mer användarvänligt då arrays första element på index är 0
        }

    }
    // var vehicleFound = 0; för att hitta vehicle i search var för okänd variabeltyp
    //Ta bort fordonet
    parkingSpaces = null; //kan vara ett = bara
                          //fordonet finns inte
    if (!vehicleFound)
    {
        Console.WriteLine("Vehicle not found");

    }
    //hitta input i parkingSpaces
    //ta bort det från parkingSpaces

}





void SearchVehicle()

{
    Console.WriteLine("Enter the vehicle registration number to search:");
string regNumber = Console.ReadLine();

int vehiclePosition = FindVehicle(regNumber);
if (vehiclePosition != -1)
{
    Console.WriteLine($"Vehicle found at parking spot number: {vehiclePosition}");
}
else
{
    Console.WriteLine("Vehicle not found.");
}

   
}
6
void ShowParkingSpaces()

{
    Console.WriteLine("Current parking spaces status:");
    for (int i = 1; i < parkingSpaces.Length; i++)
    {
        if (parkingSpaces[i] == null)
        {
            Console.WriteLine($"Spot {i}: Empty");
        }
        else
        {
            Console.WriteLine($"Spot {i}: {parkingSpaces[i]}");
        }
    }
}



// Sök fordon inom kodning 
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

//ShowParkingSpaces();

