using UnityEngine;

public class KeepAlive : MonoBehaviour
{
    private static KeepAlive _instance;

    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this;

        DontDestroyOnLoad(gameObject); 
    }
}
