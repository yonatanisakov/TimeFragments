using UnityEngine;
using Zenject;

public class Player : MonoBehaviour
{
    private PlayerController playerController;

    [Inject]
    public void Construct(IPlayerInput playerInput)
    {
        playerController = new PlayerController(playerInput, transform);
    }
    // Update is called once per frame
    void Update()
    {
        playerController.Tick();
    }
}
