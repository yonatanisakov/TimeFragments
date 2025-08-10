using UnityEngine;
using Zenject;

public class Player : MonoBehaviour
{
    private PlayerController playerController;
    [SerializeField] private float speed = 5;
    [SerializeField] private Transform muzzle;

    [Inject]
    public void Construct(IPlayerInput playerInput,IBoundsService bounds,Bullet.Pool bulletPool)
    {
        playerController = new PlayerController(playerInput, transform,speed, bounds,muzzle,bulletPool);
    }
    // Update is called once per frame
    void Update()
    {
        playerController.Tick();
    }
    public class Factory : PlaceholderFactory<Player> { }
}
