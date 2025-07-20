using mike_and_conquer_simulation.main;
using Xunit;
using System;
using System.Reflection;

namespace mike_and_conquer_simulation.tests;

/// <summary>
/// Unit tests for the Minigunner class covering basic functionality, state management,
/// health/damage mechanics, movement parameters, and combat calculations.
/// Note: Some tests avoid calling methods that trigger event publishing due to SimulationMain dependency.
/// </summary>
public class MinigunnerTests
{
    [Fact]
    public void Constructor_InitializesCorrectDefaultValues()
    {
        // Arrange & Act
        var minigunner = new Minigunner();

        // Assert
        Assert.Equal(Minigunner.State.IDLE, minigunner.currentState);
        Assert.Equal(Minigunner.Mission.NONE, minigunner.CurrentMission);
        Assert.Equal(50, minigunner.Health);
        Assert.Equal(50, minigunner.MaxHealth);
        Assert.NotNull(minigunner.GameWorldLocation);
        Assert.Equal(0, minigunner.GameWorldLocation.X);
        Assert.Equal(0, minigunner.GameWorldLocation.Y);
        Assert.True(minigunner.UnitId > 0);
    }

    [Fact]
    public void Constructor_AssignsUniqueUnitIds()
    {
        // Arrange
        int initialGlobalId = SimulationMain.globalId;
        
        // Act
        var minigunner1 = new Minigunner();
        var minigunner2 = new Minigunner();

        // Assert
        Assert.NotEqual(minigunner1.UnitId, minigunner2.UnitId);
        Assert.True(minigunner2.UnitId > minigunner1.UnitId);
    }

    [Fact]
    public void Constructor_SetsCorrectMovementParameters()
    {
        // Arrange & Act
        var minigunner = new Minigunner();

        // Assert - Check that movement parameters are initialized
        // Using reflection to access private fields for verification
        var movementDistanceEpsilonField = typeof(Minigunner).GetField("movementDistanceEpsilon", BindingFlags.NonPublic | BindingFlags.Instance);
        var movementDeltaField = typeof(Minigunner).GetField("movementDelta", BindingFlags.NonPublic | BindingFlags.Instance);
        
        Assert.NotNull(movementDistanceEpsilonField);
        Assert.NotNull(movementDeltaField);
        
        var epsilon = (double)movementDistanceEpsilonField!.GetValue(minigunner)!;
        var delta = (float)movementDeltaField!.GetValue(minigunner)!;
        
        Assert.Equal(1.5, epsilon);
        Assert.True(delta > 0); // Movement delta should be positive
    }

    [Fact]
    public void Constructor_InitializesWeaponAsLoaded()
    {
        // Arrange & Act
        var minigunner = new Minigunner();

        // Assert - Check weapon state using reflection
        var weaponIsLoadedField = typeof(Minigunner).GetField("weaponIsLoaded", BindingFlags.NonPublic | BindingFlags.Instance);
        var reloadTimerField = typeof(Minigunner).GetField("reloadTimer", BindingFlags.NonPublic | BindingFlags.Instance);
        
        Assert.NotNull(weaponIsLoadedField);
        Assert.NotNull(reloadTimerField);
        
        var isLoaded = (bool)weaponIsLoadedField!.GetValue(minigunner)!;
        var timer = (int)reloadTimerField!.GetValue(minigunner)!;
        
        Assert.True(isLoaded);
        Assert.Equal(0, timer);
    }

    [Fact]
    public void State_EnumContainsExpectedValues()
    {
        // Arrange & Act - Test that the State enum has the expected values
        var expectedStates = new[] { "IDLE", "MOVING", "FIRING", "LANDING_AT_MAP_SQUARE" };
        var actualStateNames = Enum.GetNames(typeof(Minigunner.State));

        // Assert
        Assert.Equal(expectedStates.Length, actualStateNames.Length);
        foreach (var expectedState in expectedStates)
        {
            Assert.Contains(expectedState, actualStateNames);
        }
    }

    [Fact]
    public void Mission_EnumContainsExpectedValues()
    {
        // Arrange & Act - Test that the Mission enum has the expected values
        var expectedMissions = new[] { "NONE", "ATTACK_TARGET", "MOVE_TO_DESTINATION" };
        var actualMissionNames = Enum.GetNames(typeof(Minigunner.Mission));

        // Assert
        Assert.Equal(expectedMissions.Length, actualMissionNames.Length);
        foreach (var expectedMission in expectedMissions)
        {
            Assert.Contains(expectedMission, actualMissionNames);
        }
    }

    [Fact]
    public void CurrentMission_CanBeSetAndRead()
    {
        // Arrange
        var minigunner = new Minigunner();

        // Act
        minigunner.CurrentMission = Minigunner.Mission.ATTACK_TARGET;

        // Assert
        Assert.Equal(Minigunner.Mission.ATTACK_TARGET, minigunner.CurrentMission);
    }

    [Fact]
    public void CurrentState_CanBeSetAndRead()
    {
        // Arrange
        var minigunner = new Minigunner();

        // Act
        minigunner.currentState = Minigunner.State.MOVING;

        // Assert
        Assert.Equal(Minigunner.State.MOVING, minigunner.currentState);
    }

    [Fact]
    public void UnitId_IsUniqueAcrossInstances()
    {
        // Arrange & Act
        var minigunners = new Minigunner[5];
        for (int i = 0; i < 5; i++)
        {
            minigunners[i] = new Minigunner();
        }

        // Assert
        for (int i = 0; i < 5; i++)
        {
            for (int j = i + 1; j < 5; j++)
            {
                Assert.NotEqual(minigunners[i].UnitId, minigunners[j].UnitId);
            }
        }
    }

    [Fact]
    public void Health_MatchesMaxHealthInitially()
    {
        // Arrange & Act
        var minigunner = new Minigunner();

        // Assert
        Assert.Equal(minigunner.MaxHealth, minigunner.Health);
    }

    [Fact]
    public void Distance_CalculatesCorrectEuclideanDistance()
    {
        // Arrange
        var minigunner = new Minigunner();
        var distanceMethod = typeof(Minigunner).GetMethod("Distance", BindingFlags.NonPublic | BindingFlags.Instance);
        Assert.NotNull(distanceMethod);

        // Act & Assert
        // Test known distance calculations
        var distance1 = (double)distanceMethod!.Invoke(minigunner, new object[] { 0.0, 0.0, 3.0, 4.0 })!;
        Assert.Equal(5.0, distance1, 0.001); // 3-4-5 triangle

        var distance2 = (double)distanceMethod.Invoke(minigunner, new object[] { 0.0, 0.0, 0.0, 0.0 })!;
        Assert.Equal(0.0, distance2, 0.001); // Same point

        var distance3 = (double)distanceMethod.Invoke(minigunner, new object[] { 1.0, 1.0, 4.0, 5.0 })!;
        Assert.Equal(5.0, distance3, 0.001); // Another 3-4-5 triangle
    }

    [Fact]
    public void CalculateDistanceToTarget_UsesCorrectFormula()
    {
        // Arrange
        var minigunner = new Minigunner();
        
        // Create a mock target unit and set its position using the protected field
        var targetUnit = new Minigunner();
        var targetLocationField = typeof(Unit).GetField("gameWorldLocation", BindingFlags.NonPublic | BindingFlags.Instance);
        targetLocationField!.SetValue(targetUnit, GameWorldLocation.CreateFromWorldCoordinates(30, 40));
        
        // Set the currentAttackTarget field using reflection
        var targetField = typeof(Minigunner).GetField("currentAttackTarget", BindingFlags.NonPublic | BindingFlags.Instance);
        targetField!.SetValue(minigunner, targetUnit);
        
        // Get the private method
        var calculateDistanceMethod = typeof(Minigunner).GetMethod("CalculateDistanceToTarget", BindingFlags.NonPublic | BindingFlags.Instance);
        Assert.NotNull(calculateDistanceMethod);

        // Act
        var distance = (int)calculateDistanceMethod!.Invoke(minigunner, new object[] { })!;

        // Assert
        // Minigunner starts at (0,0), target is at (30,40), so distance should be 50
        Assert.Equal(50, distance);
    }

    [Fact]
    public void IsInAttackRange_ReturnsTrueForCloseTargets()
    {
        // Arrange
        var minigunner = new Minigunner();
        
        // Create a close target and set its position using the protected field
        var targetUnit = new Minigunner();
        var targetLocationField = typeof(Unit).GetField("gameWorldLocation", BindingFlags.NonPublic | BindingFlags.Instance);
        targetLocationField!.SetValue(targetUnit, GameWorldLocation.CreateFromWorldCoordinates(20, 25));
        
        // Set the currentAttackTarget field using reflection
        var targetField = typeof(Minigunner).GetField("currentAttackTarget", BindingFlags.NonPublic | BindingFlags.Instance);
        targetField!.SetValue(minigunner, targetUnit);
        
        // Get the private method
        var isInRangeMethod = typeof(Minigunner).GetMethod("IsInAttackRange", BindingFlags.NonPublic | BindingFlags.Instance);
        Assert.NotNull(isInRangeMethod);

        // Act
        var inRange = (bool)isInRangeMethod!.Invoke(minigunner, new object[] { })!;

        // Assert
        // Distance from (0,0) to (20,25) is approximately 32, which is < 35 (attack range)
        Assert.True(inRange);
    }

    [Fact]
    public void IsInAttackRange_ReturnsFalseForDistantTargets()
    {
        // Arrange
        var minigunner = new Minigunner();
        
        // Create a distant target and set its position using the protected field
        var targetUnit = new Minigunner();
        var targetLocationField = typeof(Unit).GetField("gameWorldLocation", BindingFlags.NonPublic | BindingFlags.Instance);
        targetLocationField!.SetValue(targetUnit, GameWorldLocation.CreateFromWorldCoordinates(100, 100));
        
        // Set the currentAttackTarget field using reflection
        var targetField = typeof(Minigunner).GetField("currentAttackTarget", BindingFlags.NonPublic | BindingFlags.Instance);
        targetField!.SetValue(minigunner, targetUnit);
        
        // Get the private method
        var isInRangeMethod = typeof(Minigunner).GetMethod("IsInAttackRange", BindingFlags.NonPublic | BindingFlags.Instance);
        Assert.NotNull(isInRangeMethod);

        // Act
        var inRange = (bool)isInRangeMethod!.Invoke(minigunner, new object[] { })!;

        // Assert
        // Distance from (0,0) to (100,100) is approximately 141, which is > 35 (attack range)
        Assert.False(inRange);
    }

    [Fact]
    public void GameWorldLocation_InitializesToZero()
    {
        // Arrange & Act
        var minigunner = new Minigunner();

        // Assert
        Assert.Equal(0, minigunner.GameWorldLocation.X);
        Assert.Equal(0, minigunner.GameWorldLocation.Y);
    }

    [Fact]
    public void MaxHealth_IsConstant()
    {
        // Arrange & Act
        var minigunner1 = new Minigunner();
        var minigunner2 = new Minigunner();

        // Assert
        Assert.Equal(50, minigunner1.MaxHealth);
        Assert.Equal(50, minigunner2.MaxHealth);
        Assert.Equal(minigunner1.MaxHealth, minigunner2.MaxHealth);
    }

    [Fact]
    public void MovementParameters_HaveCorrectValues()
    {
        // Arrange & Act
        var minigunner = new Minigunner();

        // Assert - Verify specific movement parameters using reflection
        var movementDistanceEpsilonField = typeof(Minigunner).GetField("movementDistanceEpsilon", BindingFlags.NonPublic | BindingFlags.Instance);
        var epsilon = (double)movementDistanceEpsilonField!.GetValue(minigunner)!;
        
        // The epsilon value controls how close a unit needs to be to its destination to consider it "arrived"
        Assert.Equal(1.5, epsilon);
    }

    [Fact]
    public void AttackRange_IsSetCorrectly()
    {
        // Arrange
        var minigunner = new Minigunner();
        
        // Create targets - attack range is 35, so test with 30 (close) and 40 (distant)
        var closeTarget = new Minigunner();
        var targetLocationField = typeof(Unit).GetField("gameWorldLocation", BindingFlags.NonPublic | BindingFlags.Instance);
        targetLocationField!.SetValue(closeTarget, GameWorldLocation.CreateFromWorldCoordinates(18, 24)); // Distance = 30 (in range)
        
        var distantTarget = new Minigunner();
        targetLocationField.SetValue(distantTarget, GameWorldLocation.CreateFromWorldCoordinates(24, 32)); // Distance = 40 (out of range)
        
        var targetField = typeof(Minigunner).GetField("currentAttackTarget", BindingFlags.NonPublic | BindingFlags.Instance);
        var isInRangeMethod = typeof(Minigunner).GetMethod("IsInAttackRange", BindingFlags.NonPublic | BindingFlags.Instance);
        
        // Act & Assert - Test close target
        targetField!.SetValue(minigunner, closeTarget);
        var closeInRange = (bool)isInRangeMethod!.Invoke(minigunner, new object[] { })!;
        Assert.True(closeInRange);
        
        // Act & Assert - Test distant target
        targetField.SetValue(minigunner, distantTarget);
        var distantInRange = (bool)isInRangeMethod.Invoke(minigunner, new object[] { })!;
        Assert.False(distantInRange);
    }

    [Fact]
    public void WeaponSystem_InitializesCorrectly()
    {
        // Arrange & Act
        var minigunner = new Minigunner();

        // Assert - Verify weapon state using reflection
        var weaponIsLoadedField = typeof(Minigunner).GetField("weaponIsLoaded", BindingFlags.NonPublic | BindingFlags.Instance);
        var reloadTimerField = typeof(Minigunner).GetField("reloadTimer", BindingFlags.NonPublic | BindingFlags.Instance);
        
        var isLoaded = (bool)weaponIsLoadedField!.GetValue(minigunner)!;
        var timer = (int)reloadTimerField!.GetValue(minigunner)!;
        
        Assert.True(isLoaded); // Weapon should start loaded
        Assert.Equal(0, timer); // No reload timer when loaded
    }

    [Fact]
    public void StateMissions_CanTransitionBetweenValues()
    {
        // Arrange
        var minigunner = new Minigunner();

        // Act & Assert - Test state transitions
        Assert.Equal(Minigunner.State.IDLE, minigunner.currentState);
        
        minigunner.currentState = Minigunner.State.MOVING;
        Assert.Equal(Minigunner.State.MOVING, minigunner.currentState);
        
        minigunner.currentState = Minigunner.State.FIRING;
        Assert.Equal(Minigunner.State.FIRING, minigunner.currentState);
        
        minigunner.currentState = Minigunner.State.LANDING_AT_MAP_SQUARE;
        Assert.Equal(Minigunner.State.LANDING_AT_MAP_SQUARE, minigunner.currentState);

        // Act & Assert - Test mission transitions
        Assert.Equal(Minigunner.Mission.NONE, minigunner.CurrentMission);
        
        minigunner.CurrentMission = Minigunner.Mission.MOVE_TO_DESTINATION;
        Assert.Equal(Minigunner.Mission.MOVE_TO_DESTINATION, minigunner.CurrentMission);
        
        minigunner.CurrentMission = Minigunner.Mission.ATTACK_TARGET;
        Assert.Equal(Minigunner.Mission.ATTACK_TARGET, minigunner.CurrentMission);
    }
}