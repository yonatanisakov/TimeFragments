
using Zenject;

public class FragmentSpawner : IFragmentSpawner
{
    private readonly TimeFragment.Pool _fragmentPool;

    [Inject]
    public FragmentSpawner(TimeFragment.Pool fragmentPool)
    {
        _fragmentPool = fragmentPool;
    }
    public int CalculateTotalFragments(int splitDepth)
    {
        // Using the formula for sum of geometric series
        return ((1<<(splitDepth+1))-1);
    }

    public void SpawnFragment(LevelConfig.FragmentSpawn fragmentConfig)
    {
        var fragment = _fragmentPool.Spawn(fragmentConfig.recipe);
        fragment.transform.position = fragmentConfig.position;
    }
}