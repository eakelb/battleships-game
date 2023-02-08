namespace GameOfBattleships.Domain;

internal class ShipPlacementService
{
    public Coordinates[] GetAvailableStartingCoordinates(BattleshipToCreate toCreate,
        ShipDirection shipDirection, GameFieldDimensions gameFieldDimensions,
        IReadOnlyCollection<Battleship> alreadyCreatedBattleships)
    {
        int numberOfAvailableColumns;
        int numberOfAvailableRows;
        if (shipDirection == ShipDirection.Horizontal)
        {
            numberOfAvailableColumns = gameFieldDimensions.X - (toCreate.Size - 1);
            numberOfAvailableRows = gameFieldDimensions.Y;
        }
        else
        {
            numberOfAvailableColumns = gameFieldDimensions.X;
            numberOfAvailableRows = gameFieldDimensions.Y - (toCreate.Size - 1);
        }

        var availableStartingCoordinates = Enumerable.Range(1, numberOfAvailableColumns).SelectMany(col =>
                Enumerable.Range(1, numberOfAvailableRows).Select(row =>
                    Coordinates.FromNumeric(col, row)))
            .ToList();

        var alreadyUsedCoordinates = alreadyCreatedBattleships.SelectMany(x => x.Coordinates.Coordinates);
        foreach (var alreadyUsedCoordinate in alreadyUsedCoordinates)
        {
            if (shipDirection == ShipDirection.Horizontal)
            {
                for (var i = 0; i < toCreate.Size; i++)
                {
                    var currentColumn = alreadyUsedCoordinate.GetXAsNumeric() - i; 
                    if (currentColumn < 1) break;

                    availableStartingCoordinates.Remove(Coordinates.FromNumeric(currentColumn, alreadyUsedCoordinate.Y));
                }
            }
            else
            {
                for (var i = 0; i < toCreate.Size; i++)
                {
                    var currentRow = alreadyUsedCoordinate.Y - i;
                    if (currentRow < 1) break;

                    availableStartingCoordinates.Remove(new Coordinates(alreadyUsedCoordinate.X, currentRow));
                }
            }
        }

        return availableStartingCoordinates.ToArray();
    }
}