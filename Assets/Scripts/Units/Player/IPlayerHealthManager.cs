public interface IPlayerHealthManager
{
    int currentLives { get; }
    void OnPlayerHit();

}