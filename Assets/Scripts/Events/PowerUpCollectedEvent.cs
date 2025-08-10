using EventBusScripts;
using UnityEngine;

/// <summary>
/// Event fired when the player collects a power-up
/// Contains information about what was collected and where
/// </summary>
public struct PowerUpCollectionData
{
    public PowerUpType powerUpType;
    public Vector3 collectionPosition;
    public PowerUpCollectible collectible;
}

public class PowerUpCollectedEvent : Event<PowerUpCollectionData>
{
}