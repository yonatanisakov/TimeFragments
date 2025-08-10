
using EventBusScripts;
using System;
using UnityEngine;
using Zenject;

public class FragmentCollisionHandler : IFragmentCollisionHandler
{
    private readonly IFragmentPhysics _fragmentPhysics;
    private readonly IFragmentSplitter _fragmentSplitter;
    private readonly Bullet.Pool _bulletPool;

    [Inject]
    public FragmentCollisionHandler(IFragmentPhysics fragmentPhysics, IFragmentSplitter fragmentSplitter,Bullet.Pool bulletPool)
    {
        _fragmentPhysics = fragmentPhysics;
        _fragmentSplitter = fragmentSplitter;
        _bulletPool = bulletPool;
    }
    public void OnFragmentCollision(TimeFragment fragment, GameObject collisionObject)
    {
        if(collisionObject.CompareTag("Bullet"))
        {
            HandleBulletCollision(fragment, collisionObject);
        }
        else if (collisionObject.CompareTag("Ground"))
        {
            HandleGroundCollision(fragment);
        }
        else if (collisionObject.CompareTag("Wall"))
        {
            HandleWallCollision(fragment);
        }
    }
    private void HandleBulletCollision(TimeFragment fragment, GameObject bulletObject)
    {
        Bullet bullet = bulletObject.GetComponent<Bullet>();
        if (bullet == null || !bullet.gameObject.activeInHierarchy)
            return;

        _bulletPool.Despawn(bullet);
        EventBus.Get<BulletHitFragmentEvent>().Invoke();

        var hitData = new FragmentHitData
        {
            position = fragment.transform.position,
            radius = fragment.Radius,
            splitDepth = fragment.SplitDepth,
            basePoints = CalculateBasePoints(fragment.Radius)
        };

        EventBus.Get<FragmentPoppedEvent>().Invoke(hitData);
        var recipe = new LevelConfig.FragmentRecipe
        {
            splitDepth = fragment.SplitDepth,
            radius = fragment.Radius
        };
                _fragmentSplitter.SplitFragment(fragment, recipe, fragment.transform.position,
            fragment.HorizontalKick, fragment.UpwardKick);
    }
    private void HandleGroundCollision(TimeFragment fragment)
    {
        _fragmentPhysics.ApplyGroundBounce(fragment);
    }
    private void HandleWallCollision(TimeFragment fragment)
    {
        _fragmentPhysics.ApplyWallBounce(fragment);
    }
    private int CalculateBasePoints(float radius)
    {
        if (radius >= 1.5f) return 100;      // Large fragments
        if (radius >= 1.0f) return 200;      // Medium fragments  
        if (radius >= 0.5f) return 400;      // Small fragments
        return 800;                          // Tiny fragments (0.25f)
    }


}
