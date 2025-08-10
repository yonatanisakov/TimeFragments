using UnityEngine;

[CreateAssetMenu(fileName = "PowerUpConfig", menuName = "Time Fragments Game/PowerUp Config")]
public class PowerUpConfig : ScriptableObject
{
    [System.Serializable]
    public struct PowerUpVisualData
    {
        public PowerUpType powerUpType;
        public Sprite icon;
        public string displayName;
        public string description;
    }

    [SerializeField] private PowerUpVisualData[] _powerUpData;

    public Sprite GetIcon(PowerUpType powerUpType)
    {
        foreach (var data in _powerUpData)
        {
            if (data.powerUpType == powerUpType)
                return data.icon;
        }
        return null;
    }

    public string GetDisplayName(PowerUpType powerUpType)
    {
        foreach (var data in _powerUpData)
        {
            if (data.powerUpType == powerUpType)
                return data.displayName;
        }
        return PowerUpTypeHelper.GetDisplayName(powerUpType);
    }
    /// <summary>
    /// Get all power-up types that have been configured (have icons assigned)
    /// </summary>
    /// <returns>Array of available power-up types</returns>
    public PowerUpType[] GetAvailablePowerUpTypes()
    {
        var availableTypes = new System.Collections.Generic.List<PowerUpType>();

        foreach (var data in _powerUpData)
        {
            if (data.icon != null) // Only include power-ups with icons assigned
            {
                availableTypes.Add(data.powerUpType);
            }
        }

        return availableTypes.ToArray();
    }
    /// <summary>
    /// Get a random power-up type from configured types
    /// </summary>
    /// <returns>Random available power-up type</returns>
    public PowerUpType GetRandomPowerUpType()
    {
        var availableTypes = GetAvailablePowerUpTypes();

        if (availableTypes.Length == 0)
        {
            Debug.LogWarning("No power-up types configured in PowerUpConfig!");
            return PowerUpType.Shield; // Fallback
        }

        return availableTypes[UnityEngine.Random.Range(0, availableTypes.Length)];
    }
}