using System;
using UnityEngine;
using Zenject;
public class TimeFragmentSpawner:MonoBehaviour
{
    private TimeFragment.Pool pool;
    private float timeSpawner = 0f;
    [Inject]
    public void Construct(TimeFragment.Pool pool)
    {
        this.pool = pool;
    }
    private void Start()
    {
        pool.Spawn();
    }


    }
