using FluentAssertions;
using GameOfBattleships.Domain;
using NUnit.Framework;

namespace GameOfBattleships.Tests;

public class BattleshipTests
{
    [Test]
    public void CreateNew_ShouldCreateABattleship_WithoutAnyDamage()
    {
        var battleship = Battleship.CreateNew(BattleshipCoordinates);

        battleship.DestroyedCoordinates.Should().BeEmpty();
        battleship.IsDestroyed.Should().BeFalse();
    }
    
    [Test]
    public void ShipAttacked_ShouldThrow_WhenCoordinatesAreNotWithinTheShip()
    {
        var battleship = Battleship.CreateNew(BattleshipCoordinates);

        var act = () => battleship.ShipAttacked(new Coordinates('B', 10));

        act.Should().Throw<ArgumentException>();
    }
    
    
    [Test]
    public void ShipAttacked_ShouldReturnHit_WhenShipWasNotYetDestroyed()
    {
        var battleship = Battleship.CreateNew(BattleshipCoordinates);

        var result = battleship.ShipAttacked(new Coordinates('A', 1));

        result.Should().Be(Battleship.ShipAttackResult.Hit);
        battleship.IsDestroyed.Should().BeFalse();
    }
    
    [Test]
    public void ShipAttacked_ShouldReturnHit_WhenAttackingOnAlreadyDestroyedCoordinates()
    {
        var battleship = Battleship.CreateNew(BattleshipCoordinates);
        var coordinates = new Coordinates('A', 1);
        
        battleship.ShipAttacked(coordinates);
        var result = battleship.ShipAttacked(coordinates);

        result.Should().Be(Battleship.ShipAttackResult.Hit);
        battleship.IsDestroyed.Should().BeFalse();
    }
    
    [Test]
    public void ShipAttacked_ShouldReturnSunk_WhenAllFieldsAreDestroyed()
    {
        var battleship = Battleship.CreateNew(BattleshipCoordinates);
        
        battleship.ShipAttacked(new Coordinates('A', 1));
        var result = battleship.ShipAttacked(new Coordinates('A', 2));

        result.Should().Be(Battleship.ShipAttackResult.Sunk);
        battleship.IsDestroyed.Should().BeTrue();
    }

    private BattleshipCoordinates BattleshipCoordinates => new(new[]
    {
        new Coordinates('A', 1),
        new Coordinates('A', 2)
    });
}