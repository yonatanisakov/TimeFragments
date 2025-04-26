using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float speed = 5;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.forward * speed*Time.deltaTime);
    }
}
