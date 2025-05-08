using EventBusScripts;
using System;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

public class TimeFragment : MonoBehaviour
{
    private IBoundsService boundsService;
    float fragmentWidth;
    float fragmentHeight;
    int splitDepth;
    float radius;
    [Inject]
    public void Construct(IBoundsService boundsService)
    {
        this.boundsService = boundsService;
    }
    public void Configure(LevelConfig.FragmentRecipe fragmentRecipe)
    {
        splitDepth = fragmentRecipe.splitDepth;
        radius = fragmentRecipe.radius;
        transform.localScale = new Vector3(radius, radius, 0);
        var sr = GetComponentInChildren<SpriteRenderer>();
        fragmentWidth = sr.bounds.size.x;
        fragmentHeight = sr.bounds.size.y;
        Debug.Log("fragment width = " + fragmentWidth + " Fragment Height = " + fragmentHeight);
    }

    void Pop()
    {
        // split logic later

        EventBus.Get<FragmentPoppedEvent>().Invoke(transform.position);
    }


    private void ResetPosition()
    {

        transform.position = new Vector3(Random.Range(boundsService.minX+ fragmentWidth, boundsService.maxX- fragmentWidth), boundsService.maxY+ fragmentHeight, 0);
    }
    
    public class Pool:MonoMemoryPool<TimeFragment>
    {
        protected override void Reinitialize(TimeFragment item)
        {
            item.ResetPosition();
        }
    }
}
