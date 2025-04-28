
using Zenject;

public class GameManager : IInitializable
{
    readonly Player.Factory playerFactory;

    [Inject]
    public GameManager(Player.Factory playerFactory)
    {
        this.playerFactory = playerFactory;
    }
    public void Initialize()
    {
        playerFactory.Create();
      
    }
}
