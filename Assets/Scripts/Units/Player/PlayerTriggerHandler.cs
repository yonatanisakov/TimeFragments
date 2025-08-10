using EventBusScripts;
using UnityEngine;
using Zenject;

public class PlayerTriggerHandler : MonoBehaviour
{
    private IPowerUpService _powerUpService;

    [Inject]
    public void Construct(IPowerUpService powerUpService)
    {
        _powerUpService = powerUpService;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Time Fragment"))
        {
            EventBus.Get<PlayerGetHitEvent>().Invoke();
        }
        else if (collision.gameObject.CompareTag("PowerUp"))
        {
            HandlePowerUpCollection(collision.gameObject);
        }
    }
    /// <summary>
    /// Handle power-up collection logic
    /// </summary>
    /// <param name="powerUpObject">The power-up GameObject that was touched</param>
    private void HandlePowerUpCollection(GameObject powerUpObject)
    {
        var powerUpCollectible = powerUpObject.GetComponent<PowerUpCollectible>();
        if (powerUpCollectible == null)
        {
            Debug.LogWarning("PowerUp tagged object doesn't have PowerUpCollectible component!");
            return;
        }

        // Create collection data
        var collectionData = new PowerUpCollectionData
        {
            powerUpType = powerUpCollectible.PowerUpType,
            collectionPosition = powerUpCollectible.transform.position,
            collectible = powerUpCollectible
        };

        // Fire the collection event (for UI, audio, etc.)
        EventBus.Get<PowerUpCollectedEvent>().Invoke(collectionData);

        // Activate the power-up effect through the service
        _powerUpService.ActivatePowerUp(powerUpCollectible.PowerUpType);

        // Handle the collection (deactivate the collectible)
        powerUpCollectible.Collect();

        Debug.Log($"Power-up collected and activated: {PowerUpTypeHelper.GetDisplayName(powerUpCollectible.PowerUpType)}");

    }
}
