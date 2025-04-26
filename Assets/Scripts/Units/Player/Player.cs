using UnityEngine;
using Zenject;

public class Player : MonoBehaviour
{
    private PlayerController playerController;
    [SerializeField] private float speed = 5;

    [Inject]
    public void Construct(IPlayerInput playerInput)
    {
        playerController = new PlayerController(playerInput, transform,speed);
    }
    // Update is called once per frame
    void Update()
    {
        playerController.Tick();
    }
}
