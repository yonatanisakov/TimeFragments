public interface IScoreService
{
    int CurrentScore { get; }
    float CurrentMultiplier { get; }

    void Initialize();
    void AddFragmentScore(FragmentHitData hitData);
    void ResetScore();
}