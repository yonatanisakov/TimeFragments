using System;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

public class TimeFragment : MonoBehaviour
{
    private IBoundsService boundsService;
    float fragmentWidth;
    float fragmentHeight;

    [Inject]
    public void Construct(IBoundsService boundsService)
    {
        this.boundsService = boundsService;

        var sr = GetComponentInChildren<SpriteRenderer>();
            fragmentWidth = sr.bounds.size.x;
            fragmentHeight = sr.bounds.size.y;
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
