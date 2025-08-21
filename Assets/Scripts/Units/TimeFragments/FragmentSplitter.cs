
using UnityEngine;
using Zenject;

public class FragmentSplitter : IFragmentSplitter
{
    private const int SPLIT_SIZE = 2;
    private const float RADIUS_REDUCTION_PER_SPLIT = 0.5f;

    private TimeFragment.Pool _fragmentPool;
    private readonly IFragmentPhysics _fragmentPhysics;

    [Inject]
    public FragmentSplitter(TimeFragment.Pool fragmentPool, IFragmentPhysics fragmentPhysics)
    {
        _fragmentPool = fragmentPool;
        _fragmentPhysics = fragmentPhysics;
    }


    public bool CanSplit(int splitDepth)
    {
        if (splitDepth <= 0) return false;
        return true;
    }

    public void SplitFragment(
        TimeFragment currentFragment,LevelConfig.FragmentRecipe fragmentRecipe,
        Vector3 position, float horizontalKick, float upwardKick
        )
    {
        _fragmentPool.Despawn(currentFragment);
        if (CanSplit(fragmentRecipe.splitDepth))
        {
            int direction = 1;
            for (int i = 0; i < SPLIT_SIZE; i++)
            {
                direction = -1 * direction;
                var childFragment = SpawnChild(currentFragment, fragmentRecipe, position);
                var childRb = childFragment.GetComponent<Rigidbody2D>();
                _fragmentPhysics.ApplySplitForce(childRb, direction, horizontalKick, upwardKick, fragmentRecipe.radius);
            }
        }
        
    }
    private TimeFragment SpawnChild(TimeFragment parent, LevelConfig.FragmentRecipe fragmentRecipe, Vector3 position)
    {
        var child = _fragmentPool.Spawn(new LevelConfig.FragmentRecipe
        {
            splitDepth = fragmentRecipe.splitDepth - 1,
            radius = fragmentRecipe.radius - RADIUS_REDUCTION_PER_SPLIT,
            prefab = parent.SkinPrefab ?? fragmentRecipe.prefab,
        });
        child.transform.position = position;
        return child;
    }
}