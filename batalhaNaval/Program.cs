
Dictionary<string, (string name, int quantity, int size)> shipDistribution = new()
{
    { "PS", ("Porta-Aviões", 1, 5) },
    { "NT", ("Navio-Tanque", 2, 4) },
    { "DS", ("Destroyer", 3, 3) },
    { "SB", ("Submarinos", 4, 2) }
};

Dictionary<string, string> shotMessages = new()
{
    { "Miss1", "Splash! Você acertou a água!" },
    { "Miss2", "Não tem nada aqui" },
    { "Miss3", "Não foi dessa vez" },
    { "Hit1", "BOOM! Tiro certeiro!" },
    { "Hit2", "Acertou!" },
    { "Hit3", "Bom tiro!" }
};

beginGame();

void beginGame()
{
    showGamePresentation();
    gameModeSelection();
}
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
            gameModeOne();
            return;
        case 2:
            Console.WriteLine();
            gameModeTwo();
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

    Console.WriteLine("Vamos Jogar?");
    Console.WriteLine("Digite qualquer valor para iniciar o jogo");
    Console.ReadLine();
    Console.Clear();
    
    gameboardFiller(playerOne);
    playerChanger(playerOne, playerTwo);    
    gameboardFiller(playerTwo);

    Player actual = playerTwo;
    Player next = playerOne;
    bool isWinner = false;

    while (!isWinner)
    {
        (actual, next) = (next, actual);
        playerChanger(actual, next);
        headerGameboard(actual);
        jogada(actual, next);
        isWinner = actual.IsWinner();        
    }

    PlayAgain();
}

void headerGameboard(Player player)
{
    var defenseGameboard = player.GetDefenseGameboard();
    var attackGameboard = player.GetAttackGameboard();
    Console.WriteLine("Meu tabuleiro                                     Tabuleiro de ataque");
    for (int i = 0; i < 11; i++)
    {
        for (int j = 0; j < 11; j++)
        {
            Console.Write($"{defenseGameboard[i, j],-2} ");
        }
        Console.Write("      ");
        for (int j = 0; j < 11; j++)
        {
            Console.Write($"{attackGameboard[i, j],-2} ");
        }
        Console.Write(Environment.NewLine);
    }
    Console.WriteLine($"{player.PerfectHits()}/30");
}

void jogada(Player actual, Player next)
{
    string[,] attackGameboard = actual.GetAttackGameboard();
    string[,] enemyGameboard = next.GetDefenseGameboard();
    var target = acquiredTarget(attackGameboard);
    shotTarget(target.line, target.column, enemyGameboard, attackGameboard, actual);

}

(int line, int column) acquiredTarget(string[,] gameboard)
{
    Console.WriteLine("Vamos derrubar alguns navios. Onde deseja atirar? Exemplo(B10)");
    string input = Console.ReadLine();

    if (string.IsNullOrWhiteSpace(input))
    {
        Console.WriteLine("Alvo inválido.Tente Novamente");
        acquiredTarget(gameboard);
    }

    int stringSize = input.Length;
    if (stringSize < 2 || stringSize > 3)
    {
        Console.WriteLine("Alvo inválido.Tente Novamente");
        acquiredTarget(gameboard);
    }
    
    if (input[0] >= 'A' && input[0] <= 'J')
    {        
        int columnIndex;
        if(int.TryParse(input.Substring(1), out columnIndex))
        {
            if (columnIndex >= 1 || columnIndex <= 10)
            {
                int lineIndex = (input[0]) - 64;
                if (gameboard[lineIndex,columnIndex] == "")
                {
                    return (lineIndex, columnIndex);
                }
                else
                {
                    Console.WriteLine("Você já atacou esse alvo.Tente outra posição");
                    acquiredTarget(gameboard);
                }
            }            
        }
    }    
    Console.WriteLine("Alvo inválido.Tente Novamente");
    acquiredTarget(gameboard);
    return (0, 0);
}

void shotTarget(int targetLine, int targetColumn, string[,] enemyGameboard, string[,] attackGameboard, Player player) 
{
    Random rnd = new Random();
    int random = rnd.Next(1, 3);
    if (enemyGameboard[targetLine, targetColumn] == "")
    {
        attackGameboard[targetLine, targetColumn] = "A";
        enemyGameboard[targetLine, targetColumn] = "~";
        string message = $"Miss{random}";
        Console.WriteLine(shotMessages[message]);
    }
    else
    {
        attackGameboard[targetLine, targetColumn] = "X";
        enemyGameboard[targetLine, targetColumn] = "X";
        player.Hits();
        string message = $"Hit{random}";
        Console.WriteLine(shotMessages[message]);
        
    }
} 

void PlayAgain()
{
    Console.Write("Gostaria de jogar novamente? S/N");
    string input = Console.ReadLine();
    switch (input)
    {
        case "S":
            beginGame();
            return;
        case "N":
            Console.WriteLine("É isso então. Muito obrigada por jogar e até mais!");
            return;
        default:
            Console.WriteLine("Não entendi...");
            PlayAgain();
            return;
    }
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

void gameboardFiller(Player player)
{
    Console.WriteLine($"{player.GetName()}, hora de posicionar os navios!");
    Console.WriteLine();
    
    while (!player.PlacementStatus())
    {
        Console.WriteLine($"Os navios são identificados por siglas:");
        Console.WriteLine($"- PS - Porta-Aviões (5 quadrantes) - {player.ShipsPlaced("PS")}/{shipDistribution["PS"].quantity}");
        Console.WriteLine($"- NT - Navio-Tanque (4 quadrantes) - {player.ShipsPlaced("NT")}/{shipDistribution["NT"].quantity}");
        Console.WriteLine($"- DS - Destroyers (3 quadrantes)  - {player.ShipsPlaced("DS")}/{shipDistribution["DS"].quantity}");
        Console.WriteLine($"- SB - Submarinos (2 quadrantes)  - {player.ShipsPlaced("SB")}/{shipDistribution["SB"].quantity}");

        gameboardPrinter(player);
        inputCoordinates(player);
        Console.Clear();
    }
    Console.WriteLine("Todos os navios foram posicionados");
    gameboardPrinter(player);
   
    return;
}

void gameboardPrinter(Player player)
{
    var gameboard = player.GetDefenseGameboard();

    for (int i = 0; i < 11; i++)
    {
        for (int j = 0; j < 11; j++)
        {
            Console.Write($"{gameboard[i, j], -2} ");
                        
        }
        Console.Write(Environment.NewLine);
    }
}

void inputCoordinates(Player player)
{

    string ship = validateShip(player);
    var placement = validateInputPlacement(ship, player);
    dropShip(player, placement, ship);
}

void dropShip(Player player, List<int> placement, string ship)
{
    var gameboard = player.GetDefenseGameboard();
    int firstLine = placement[0];
    int firstColumn = placement[1];
    int isVertical = placement[4];

    if (isVertical == 1)
    {
        for (int i = 0; i < shipDistribution[ship].size; i++)
        {
            gameboard[firstLine + i, firstColumn] = ship;
        }
    }
    else
    {
        for (int i = 0; i < shipDistribution[ship].size; i++)
        {
            gameboard[firstLine, firstColumn + i] = ship;
        }
    }
    player.Drop(ship);

}

List<int> validateInputPlacement(string ship, Player player)
{
    bool validInput = false;
    int shipSize = shipDistribution[ship].size;
    List<int> coordinates = new List<int>();

    Console.WriteLine("Qual a sua posição? (Exemplo: A1A2)");
    while (!validInput)
    {
        string placement = Console.ReadLine().ToUpper();

        var stringValidation = validStringFormat(placement);        

        if (!stringValidation.valid)
        {
            Console.WriteLine("Formato inválido! Tente Novamente (Exemplo: A1A2)");
        }
        else
        {
            var positionValidation = validPlaceGameboard(stringValidation.coordinates, shipSize, player, placement);
            if (!positionValidation.valid)
            {
                Console.WriteLine("Posição inválida! Tente Novamente");
            }
            else
            {
                validInput = true;
                coordinates = positionValidation.coordinates;
            }
        }
    } 
    return coordinates;
}

(bool valid, List<int> coordinates) validPlaceGameboard(List<string> coordinates, int shipSize, Player player, string placement)
{
    bool validPosition = false;
    string[,] gameboard = player.GetDefenseGameboard();
    int firstLine = (char.Parse(coordinates[0])) - 64;
    int lastLine = (char.Parse(coordinates[2])) - 64;
    int distLines = lastLine - firstLine;

    int firstColumn = int.Parse(coordinates[1]);
    int lastColumn = int.Parse(coordinates[3]);
    int distColumns = lastColumn - firstColumn;

    int isVertical = 0;
    List<int> indexShip = new List<int>()
    {
        firstLine,
        firstColumn,
        lastLine,
        lastColumn,
        isVertical
    };

    if (distLines < 0 || distColumns < 0)
    {
        return (validPosition, indexShip);
    }

    if (distLines == 0 && distColumns + 1 == shipSize)
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
    else if (distColumns == 0 && distLines + 1 == shipSize)
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
                indexShip[4] = 1;
            }
        }        
    }

    return (validPosition, indexShip);
}

(bool valid, List<string> coordinates) validStringFormat(string stringPosition)
{
    bool validString = true;
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
                coordinates.Add(stringPosition[i].ToString());
            }
            else if (lastPositionType == CharType.one && stringPosition[i] == '0')
            {
                lastPositionType = CharType.number;
                coordinates[coordinates.Count - 1] = "10";
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
                    coordinates.Add(stringPosition[i].ToString());
                }
                else
                {
                    lastPositionType = CharType.number;
                    coordinates.Add(stringPosition[i].ToString());
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
    Console.WriteLine("Qual tipo de embarcação deseja posicionar?");
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

void playerChanger(Player player, Player next)
{
    Console.WriteLine($"Digite qualquer tecla para encerrar a jogada de {player.GetName()}");
    Console.ReadLine();
    Console.Clear();
    if (next.GetPlayer() == 2)
    {
        Console.BackgroundColor = ConsoleColor.DarkBlue;
    }
    else
    {
        Console.ResetColor();
    }

    Console.WriteLine($"Insira qualquer valor para iniciar a jogada de {next.GetName()}");
    Console.ReadLine();
    Console.Clear();
}



public class Player
{

    String name;
    int player;
    int perfectHits;
    string[,] attackGameboard;
    string[,] defenseGameboard;
    Dictionary<string, int> shipPlacement;


    public Player(String name, int player)
    {
        this.name = name;
        this.player = player;
        this.perfectHits = 0;
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

    public int GetPlayer()
    {
        return player;
    }

    public string[,] GetAttackGameboard()
    {
        return attackGameboard;
    }

    public string[,] GetDefenseGameboard()
    {
        return defenseGameboard;
    }

    public bool PlacementStatus()
    {
        int shipsPlaced = shipPlacement.Sum(x => x.Value);
        return shipsPlaced == 10;
    }

    public Dictionary<string, int> Drop(string ship)
    {
        shipPlacement[ship] += 1;

        return shipPlacement;
    }

    public int ShipsPlaced(string ship)
    {
        int shipQuantityPlaced = shipPlacement[ship];

        return shipQuantityPlaced;
    }

    public void Hits()
    {
        perfectHits++;
    }

    public bool IsWinner()
    {
        return perfectHits == 30;
    }

    public int PerfectHits()
    {
        return perfectHits;
    }
}


enum CharType
{
    number,
    one,
    letter
}
