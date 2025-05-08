using Unity.VisualScripting;
using UnityEngine;
using Zenject;
using IInitializable = Zenject.IInitializable;
using EventBus = EventBusScripts.EventBus;
public class LevelManager : IInitializable,ITickable
{
    readonly LevelConfig levelConfig;
    readonly TimeFragment.Pool fragPool;

    int fragmentRemaining;
    float timeLeft;
    [Inject]
    public LevelManager(LevelConfig levelConfig,TimeFragment.Pool fragPool)
    {
        this.levelConfig = levelConfig;
        this.fragPool = fragPool;

    }
    public void Initialize()
    {
        timeLeft = levelConfig.timeLimit;
        if(levelConfig.background)
            Object.Instantiate(levelConfig.background,Vector3.zero,Quaternion.identity);
        if(levelConfig.music)
            AudioSource.PlayClipAtPoint(levelConfig.music,Vector3.zero);

        foreach (var wallConfig in levelConfig.staticWalls)
            SpawnAt(wallConfig.prefab, wallConfig.position, wallConfig.rotationScale);
        foreach(var hazardConfig in  levelConfig.hazards)
            SpawnAt(hazardConfig.prefab, hazardConfig.position,Vector3.one);
        foreach(var fragmentConfig in levelConfig.fragments)
        {
            var fragment = fragPool.Spawn();
            fragment.Configure(fragmentConfig.recipe);
            fragment.transform.position = fragmentConfig.position;
            fragmentRemaining++;
        }
        EventBus.Get<FragmentPoppedEvent>().Subscribe(OnFragmentPopped);
    }
    public void Tick()
    {
        if(levelConfig.timeLimit > 0)
        {
            timeLeft -= Time.deltaTime;
            if(timeLeft <= 0)
            {
                EventBus.Get<LevelFailEvent>().Invoke();
                CleanUp();
            }
        }
    }

    void OnFragmentPopped(Vector3 pos)
    {
        fragmentRemaining--;
        TryDropPowerUp(pos);

        if (fragmentRemaining == 0 && levelConfig.scoreToWin == 0)
        {
            EventBus.Get <LevelWinEvent>().Invoke();
        }
    }
    void TryDropPowerUp(Vector3 pos)
    {
        var loot = levelConfig.powerUpDrops;
        if (loot.prefab == null) return;
        if(Random.value <= loot.dropChanceEach)
            Object.Instantiate(loot.prefab,pos,Quaternion.identity);
    }
    void CleanUp()
    {
        EventBus.Get<FragmentPoppedEvent>().Unsubscribe(OnFragmentPopped);
    }
    static void SpawnAt(GameObject prefab,Vector3 pos, Vector3 rotScale)
    {
        if (prefab == null) return;
        var gameObject = Object.Instantiate(prefab,pos,Quaternion.identity);
        gameObject.transform.localScale = rotScale;
    }
}
