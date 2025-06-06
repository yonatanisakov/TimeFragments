
using UnityEngine;

public interface IFragmentSplitter
{
    void SplitFragment(TimeFragment currentFragment,LevelConfig.FragmentRecipe fragmentRecipe, Vector3 position, float horizontalKick, float upwardKick);
    bool CanSplit(int splitDepth);
}