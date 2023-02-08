using GameOfBattleships;
using GameOfBattleships.Domain;

Console.WriteLine("Welcome to the battleship game! Type in where you want to attack, e.g. B5.");


var dimensions = new GameFieldDimensions(10, 10);
var battleshipsToCreate = new[]
{
    new BattleshipToCreate(Size: 5),
    new BattleshipToCreate(Size: 4),
    new BattleshipToCreate(Size: 4)
};

var gameField = GameField.CreateNew(dimensions, battleshipsToCreate);

while (!gameField.GameWon)
{
    var input = Console.ReadLine();
    if (string.IsNullOrEmpty(input)) continue;

    if (!CoordinatesFactory.TryCreateFromUserInput(input, out var coordinates))
    {
        Console.WriteLine("Can't parse your input, could you please try again?");
        continue;
    }

    var attackResult = gameField.AttackAt(coordinates);

    if (attackResult == GameField.AttackResult.Hit)
        Console.WriteLine("It's a hit!");
    else if (attackResult == GameField.AttackResult.Miss)
        Console.WriteLine("No ship there, it's a miss");
    else if (attackResult == GameField.AttackResult.Sunk)
        Console.WriteLine("Whoa, a whole ship sank!");
    else if (attackResult == GameField.AttackResult.Win)
        Console.WriteLine("Congrats, you won!");
    else if (attackResult == GameField.AttackResult.AttackOutsideOfGameField)
        Console.WriteLine($"Oops, you missed the game field (it's {gameField.Dimensions.X}x{gameField.Dimensions.Y})");
}

Console.ReadLine();