using System.Text.RegularExpressions;
string[] parkingSpaces = new string[101];
bool exit = false;
string typeHolder;
while (!exit)
{
    Console.WriteLine("Welcome to Prague parking");
    Console.WriteLine("1. Park Vechicle.");
    Console.WriteLine("2. Move Vechicle.");
    Console.WriteLine("3. Get Vechicle.");
    Console.WriteLine("4. Search Vechicle.");
    Console.WriteLine("5. Show Parkingspaces.");
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
            exit = true;
            break;
        default:
            Console.WriteLine("Invalid choice, try again.");
            break;
    }
    if (!exit)
    {
        Console.WriteLine("\n\nPress any key to continue: ");
        Console.ReadLine();
    }
    PairSingleMcs();           //Körs efter varje loop av huvudmenyn för att se om det finns 2 ensamma Mc
}
void parkVehicle()     // Metod för att registrera ett fordon på en parkeringsplats
{
    string vehicleType = "";                 //För att vehicleType inte kan vara null
    bool typeExit = false;
    while (!typeExit)                        // Loop för val av fordonstyp
    {
        Console.Write("Enter 1 for Mc, 2 for Car or 3 to return to main menu: ");
        string vehicleTypeInput = Console.ReadLine();
        switch (vehicleTypeInput)
        {
            case "1":
                {
                    vehicleType = "mc";
                    typeExit = true;
                    break;
                }
            case "2":
                {
                    vehicleType = "car";
                    typeExit = true;
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

    //vvv Kollar ifall inputen är för lång, för kort, eller har otillåtna tecken vvv\\
    if ((regNumber.Length > 10) | (regNumber.Length < 1) | (ContainsSpecialCharacters(regNumber)))
    {
        Console.WriteLine("Invalid Regstration number, returning to main menu.");
    }
    else
    {
        //vvv skickar input för att kolla om den redan finns vvv\\
        int alreadyParked = FindVehicle(regNumber);
        if (alreadyParked != -1)
        {
            Console.WriteLine("Vehicle already registered, returning to main menu.");
            return;
        }
        //vvv Lägger ihop fordonstyp och regnummer vvv\\
        string vehicleDesignation = vehicleType + "#" + regNumber;
        string checkstring;
        for (int i = 1; i < parkingSpaces.Length; i++)
        {
            checkstring = parkingSpaces[i];
            if (vehicleDesignation.Contains("mc"))    //Om det är en Mc man vill parkera
            {
                if (checkstring == null)   //Är platsen tom? 
                {
                    parkingSpaces[i] = vehicleDesignation;
                    Console.WriteLine("\nVehicle registered to parking spot number: {0}", i);
                    break;
                }
                //vvv Om det inte står en bil och inte två Mc vvv\\
                else if ((!checkstring.Contains("car")) && (!checkstring.Contains("|")))  
                {
                    parkingSpaces[i] = parkingSpaces[i] + "|" + vehicleDesignation;
                    Console.WriteLine("Vehicle registered to parking spot number: {0}", i);
                    break;
                }
            }
            else       //Om det är en bil man vill parkera
            {
                if (checkstring == null)
                {
                    parkingSpaces[i] = vehicleDesignation;
                    Console.WriteLine("Vehicle registered to parking spot number: {0}", i);
                    break;
                }
            }
        }
    }
}
bool ContainsSpecialCharacters(string input)
{
    return Regex.IsMatch(input, @"[^\p{L}\p{N}]");  //\p{L} representerar alla bokstäver och \p{N} alla siffror
}
void MoveVehicle()
{
    typeHolder = "";
    Console.WriteLine("Registration number to move: ");
    string regNumber = Console.ReadLine().ToLower();

    // hitta var fordonet är parkerat.
    int currentSpace = FindVehicle(regNumber); 

    if (currentSpace == -1)
    {
        Console.WriteLine("Vehicle not found. ");
        return;
    }
    string moveFinal = typeHolder + "#" + regNumber;
    //kolla om 2 Mc delar plats.
    if (parkingSpaces[currentSpace].Contains("|"))
    {
        string[] mcArray = parkingSpaces[currentSpace].Split( '|');

        if (mcArray[0].Contains(moveFinal))
        {
            //flytta första Mc och lämna den andra.
            MoveSingleVehicle(currentSpace, moveFinal, mcArray[1]);
        }
        else if (mcArray[1].Contains(moveFinal))
        {
            // Flytta andra Mc och lämna den första.
            MoveSingleVehicle(currentSpace, moveFinal, mcArray[0]);
        }

    }
    else
    {
        // 1 fordon i parking flytta.
        MoveSingleVehicle(currentSpace, moveFinal,null);
    }
}
void MoveSingleVehicle(int currentSpace, string moveFinal, string remainingVehicle)
{
   
    Console.WriteLine($"The Vehicle is parked at spot {currentSpace}");
    Console.Write("Type the new parking spot (1-100):  ");

    if (int.TryParse(Console.ReadLine(), out int newSpace) && newSpace > 0 && newSpace <= 100)
    {
        string newSpaceCheck = parkingSpaces[newSpace];
        if (parkingSpaces[newSpace] == null) 
        {
            //flytta fordon till nya platsen.
            parkingSpaces[newSpace] = moveFinal;
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
        else if (parkingSpaces[newSpace].Contains("|"))
        {
            Console.WriteLine("The New Parking spot is occupied.");
        }
        else if (parkingSpaces[newSpace].Contains("mc"))
        {
                parkingSpaces[newSpace] = parkingSpaces[newSpace] + "|" + moveFinal;
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
    Console.Write("Enter Vehicle registration number to retrieve: ");
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
    Console.Write($"The vehicle {regNumber} is parked at spot {currentSpace}. Do you want to remove it? (y/n): ");
    string confirmation = Console.ReadLine().ToLower();

    if (confirmation == "y")
    {
        // Om det finns ett annat Mc på platsen, lämna kvar det
        if (remainingVehicle == null)
        {
            // Rensa platsen om inget annat fordon finns kvar
            parkingSpaces[currentSpace] = null;
            Console.WriteLine($"Vehicle {regNumber} removed from spot {currentSpace}.");
        }
        else
        {
            // Om det finns ett annat Mc, uppdatera platsen till endast det återstående fordonet
            parkingSpaces[currentSpace] = remainingVehicle;
            Console.WriteLine($"Vehicle {regNumber} removed. {remainingVehicle} remains at spot {currentSpace}.");
        }
    }
    else
    {
        Console.WriteLine("Vehicle remains parked.");
    }
}
void SearchVehicle()

{
    Console.WriteLine("Enter the vehicle registration number to search:");
    string regNumber = Console.ReadLine();

    int vehiclePosition = FindVehicle(regNumber);
    if (vehiclePosition != -1)
    {
        Console.WriteLine($"Vehicle is parked at parking spot number: {vehiclePosition}.");
    }
    else
    {
        Console.WriteLine("Vehicle not found, returning to main menu.");
    }
}
void ShowParkingSpaces()
{
    int rowCount = 0;
    Console.WriteLine("Current parkinglot status: \n");
    for (int i = 1; i < parkingSpaces.Length; i++)         //Loopar igenom alla platser
    {
        if ((rowCount % 5 == 0) && (rowCount != 0))     //För att byta rad var 5e plats 
        {
            Console.WriteLine("--");
        }
        Console.Write("--");
        if (parkingSpaces[i] == null)                  
        {
            Console.Write($" Spot {i}: Empty ");
        }
        else
        {
            Console.Write($" Spot {i}: {parkingSpaces[i]} ");
        }
        rowCount++;
    }
}
int FindVehicle(string regNumber)
{
    for (int i = 1; i < parkingSpaces.Length; i++)
    {
        if (parkingSpaces[i] != null)
        {
            string[] vehicles = parkingSpaces[i].Split('|');    //separerar fordonen om det finns mer än ett 
            foreach (var vehicle in vehicles)
            {
                string[] number = vehicle.Split("#");        //Separerar fordonstyp från regnummer
                typeHolder = number[0];                      //Sparar fordonstypen för att kunna stoppa tillbaka
                                                             //den när MoveVehicle() körs
                foreach (var letterNumber in number)
                {
                    int j = 1;
                    if (letterNumber.Equals(regNumber, StringComparison.OrdinalIgnoreCase))
                    {
                        return i;         //returnerar indexet om fordonet hittas
                    }
                }
            }
        }
    }
    return -1;   //returnerar -1 om det inte hittas
}
void PairSingleMcs()
{
    for (int i = 1; i < parkingSpaces.Length; i++)     //Loop för att hitta första ensamma Mc
    {
        if (parkingSpaces[i] != null)
        {
            string singleCheck = parkingSpaces[i];
            if (!singleCheck.Contains("|") && (singleCheck.Contains("mc")))
            {
                for (int j = i + 2; j < parkingSpaces.Length; j++)       //Loop för att hitta andra ensamma Mc 
                {
                    if (parkingSpaces[j] != null)
                    {
                        string singelCheck2 = parkingSpaces[j];
                        if ((!parkingSpaces[j].Contains("|")) && (parkingSpaces[j].Contains("mc")))
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("There are singel Motorcycles on parking spots {0} and {1}.", i, j);
                            Console.WriteLine("Please move the one on number {0} to number {1}!", j, i);
                            Console.ResetColor();
                            Console.WriteLine("\n\n\n");
                            parkingSpaces[i] = parkingSpaces[i] + "|" + parkingSpaces[j];   //parar ihop dem
                            parkingSpaces[j] = null;                         //Och tömmer den andra platsen
                        }
                    }
                }
            }
        }
    }
}