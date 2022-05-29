
Dictionary<string, (string name, int quantity, int size)> shipDistribution = new()
{
    { "PS", ("Porta-Aviões", 1, 5) },
    { "NT", ("Navio-Tanque", 2, 4) },
    { "DS", ("Destroyer", 3, 3) },
    { "SB", ("Submarinos", 4, 2) }
};


showGamePresentation();
gameModeSelection();


void showGamePresentation()
{
    Console.ForegroundColor = ConsoleColor.White;
    Console.WriteLine("-------------------------------------------------");
    Console.WriteLine("-----             Batalha Naval             -----");
    Console.WriteLine("-------------------------------------------------");
    Console.WriteLine();
    Console.WriteLine("Let's Code - Projeto Módulo 01");
    Console.WriteLine("Created by Luiza Motta Campello");
    Console.WriteLine();
    Console.WriteLine("-------------------------------------------------");
    Console.WriteLine();
    Console.WriteLine("Modos de Jogo");
    Console.WriteLine("1 - Um Jogador");
    Console.WriteLine("2 - Dois Jogadores");
    Console.WriteLine();

}

void gameModeSelection()
{
    int gameMode = inputGameMode();
    switch (gameMode)
    {
        case 1:
            Console.WriteLine();
            gamePlayer(gameMode);
            return;
        case 2:
            Console.WriteLine();
            gamePlayer(gameMode);
            return;
        default:
            Console.WriteLine();
            Console.WriteLine("Opção inválida");
            gameModeSelection();
            return;
    }

}

int inputGameMode()
{
    Console.Write("Insira o modo de jogo: ");
    string userInput = Console.ReadLine();
    int.TryParse(userInput, out int intMode);
    return intMode;
}

void gamePlayer(int gameMode)
{
    switch (gameMode)
    {
        case 1:
            gameModeOne();
            return;
        case 2:
            Console.WriteLine("Vamos Jogar?");
            Console.WriteLine("Digite qualquer valor para iniciar o jogo");
            Console.ReadLine();
            Console.Clear();
            gameModeTwo();
            return;
        default:
            return;
    }

}

void gameModeOne()
{
    Console.WriteLine("Disponivel somente na versão Pro");
    Console.WriteLine("Adquira já mandando um PIX para lumcampello@gmail.com");
    //Player playerOne = new Player(inputPlayerName(1), 1);
    //gameboardFiller(playerOne);

}

void gameModeTwo()
{
    Player playerOne = new Player(inputPlayerName(1), 1);
    Player playerTwo = new Player(inputPlayerName(2), 2);

    gameboardFiller(playerOne);
    gameboardFiller(playerTwo);

}

string inputPlayerName(int player)
{
    Console.Write($"Olá Jogador {player}, qual o seu nome? ");
    string playerName = Console.ReadLine();

    if (String.IsNullOrWhiteSpace(playerName))
    {
        Console.WriteLine("...");
        Console.WriteLine($"Vou te chamar de Player {player}!");
        Console.WriteLine();
        return $"Player {player}";
    }
    Console.WriteLine();
    return playerName;
}

void inputCoordinates(Player player)
{
    Console.WriteLine("Qual o tipo de embarcação?");
    string ship = validateShip(player);

    string placement = validateCoordinatesandShip(ship, player);
    Console.WriteLine(placement);

}

void gameboardFiller(Player player)
{
    Console.WriteLine($"{player.GetName()}, hora de posicionar os navios!");
    Console.WriteLine();
    Console.WriteLine(@"Os navios são identificados por siglas:
    - PS - Porta-Aviões (5 quadrantes)
    - NT - Navio-Tanque (4 quadrantes)
    - DS - Destroyers (3 quadrantes)
    - SB - Submarinos (2 quadrantes)");

    inputCoordinates(player);

    return;
}

string validateCoordinatesandShip(string ship, Player player)
{
    bool validInput = false;
    int shipSize = shipDistribution[ship].size;

    Console.WriteLine("Qual a sua posição? (Exemplo: A1A2)");
    while (!validInput)
    {
        string placement = Console.ReadLine().ToUpper();

        var stringValidation = validStringFormat(placement);        

        if (!stringValidation.valid)
        {
            Console.WriteLine("Formato inválido! Tente Novamente (Exemplo: A1A2)");
            // Thread.Sleep(2000);
            //Console.Clear();
        }
        else
        {
            validInput = validPlace(stringValidation.coordinates, shipSize, player);
            if (!validInput)
            {
                Console.WriteLine("Posição inválida! Tente Novamente");
            }
        }


    }

    return placement;

}
(bool, List<int>) validPlace(List<string> coordinates, int shipSize, Player player)
{
    bool validPosition = false;
    string[,] gameboard = player.GetDefenseGameboard();
    
    int firstLine = Convert.ToInt32(Char.GetNumericValue(coordinates[0], 0)) - 64;
    int lastLine = Convert.ToInt32(Char.GetNumericValue(coordinates[2], 0)) - 64;
    int distLines = lastLine - firstLine;

    int firstColumn = int.Parse(coordinates[1]);
    int lastColumn = int.Parse(coordinates[3]);
    int distColumns = lastColumn - firstColumn;

    List<int> indexShip = new List<int>()
    {
        firstLine,
        firstColumn,
        lastLine,
        lastColumn
    };

    if (distLines < 0 || distColumns < 0)
    {
        return (validPosition, indexShip);
    }

    if (distLines == 0 && distColumns == shipSize)
    {
        for (int i = 0; i < shipSize; i++)
        {
            if(gameboard[firstLine, firstColumn + i] != "")
            {
                validPosition = false;
                break;
            }
            else
            {
                validPosition = true;
            }
        }       
    }
    else if (distColumns == 0 && distLines == shipSize)
    {
        for (int i = 0; i < shipSize; i++)
        {
            if (gameboard[firstLine + i, firstColumn] != "")
            {
                validPosition = false;
                break;
            }
            else
            {
                validPosition = true;
            }
        }        
    }

    return (validPosition, indexShip);
}

(bool valid, List<string> coordinates) validStringFormat(string stringPosition)
{
    bool validString = false;
    CharType lastPositionType = CharType.number;
    List<string> coordinates = new List<string>();
    int stringLength = stringPosition.Length;

    if (stringLength < 4 | stringLength > 6)
    {
        return (false, coordinates);
    }

    for (int i = 0; i < stringLength; i++)
    {
        if (lastPositionType == CharType.number || lastPositionType == CharType.one)
        {
            if (stringPosition[i] >= 'A' && stringPosition[i] <= 'J')
            {
                lastPositionType = CharType.letter;
                coordinates.Append(stringPosition[i].ToString());
            }
            else if (lastPositionType == CharType.one && stringPosition[i] == '0')
            {
                lastPositionType = CharType.number;
                coordinates[i - 1] = "10";
            }
            else
            {
                validString = false;
                break;
            }
        }
        else
        {
            if (stringPosition[i] >= '1' && stringPosition[i] <= '9')
            {
                if (stringPosition[i] == '1')
                {
                    lastPositionType = CharType.one;
                    coordinates.Append(stringPosition[i].ToString());
                }
                else
                {
                    lastPositionType = CharType.number;
                    coordinates.Append(stringPosition[i].ToString());
                }
            }
            else
            {
                validString = false;
                break;
            }
        }
    }
    return (validString, coordinates);
}

string validateShip(Player player)
{
    string ship = "";
    bool validShip = false;

    do
    {
        ship = Console.ReadLine().ToUpper();
        if (shipDistribution.ContainsKey(ship))
        {
            int shipQuantity = shipDistribution[ship].quantity;
            int shipsPlaced = player.ShipsPlaced(ship);
            if (shipsPlaced < shipQuantity)
            {
                validShip = true;
            }
            else
            {
                Console.WriteLine($"Todas as embarcações do tipo {shipDistribution[ship].name} já foram posicionadas. Tente novamente!");
            }
        }
        else
        {
            Console.WriteLine("Embarcação inválida, tente novamente!");
        }
    }
    while (!validShip);
    return ship;
}





public class Player
{

    String name;
    int player;
    string[,] attackGameboard;
    string[,] defenseGameboard;
    String color;
    Dictionary<string, int> shipPlacement;


    public Player(String name, int player)
    {
        this.name = name;
        this.player = player;
        this.attackGameboard = new string[11, 11];        
        this.defenseGameboard = new string[11, 11];
        this.shipPlacement = new Dictionary<string, int>
        {
            { "PS", 0 },
            { "NT", 0 },
            { "DS", 0 },
            { "SB", 0 }
        };
        initGameboard(attackGameboard);
        initGameboard(defenseGameboard);

    }

    private void initGameboard(string[,] gameboard)
    {
        for (int i = 1; i < 11; i++)
        {
            gameboard[0, i] = i.ToString();
        }


        for (int i = 1; i < 11; i++)
        {
            int charInt = i + 64;
            char ch = (char)charInt;
            gameboard[i, 0] = ch.ToString();

            for (int j = 1; j < 11; j++)
            {
                gameboard[i, j] = "";
            }
        }
    }

    public String GetName()
    {
        return name;
    }

    public string[,] GetAttackGameboard()
    {
        return attackGameboard;
    }

    public string[,] GetDefenseGameboard()
    {
        return defenseGameboard;
    }

    public String GetColor()
    {
        return color;
    }

    public int GetPlacementStatus()
    {
        int shipsPlaced = shipPlacement.Sum(x => x.Value);
        return shipsPlaced;
    }

    public Dictionary<string, int> Place(string ship)
    {
        shipPlacement[ship] += 1;

        return shipPlacement;
    }


    public int ShipsPlaced(string ship)
    {
        int shipQuantityPlaced = shipPlacement[ship];

        return shipQuantityPlaced;
    }
}


enum CharType
{
    number,
    one,
    letter
}
