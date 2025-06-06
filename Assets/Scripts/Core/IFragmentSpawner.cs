public interface IFragmentSpawner
{
    void SpawnFragment(LevelConfig.FragmentSpawn fragmentConfig);
    int CalculateTotalFragments(int splitDepth);

}   