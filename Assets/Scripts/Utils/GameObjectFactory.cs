
using UnityEngine;

public class GameObjectFactory : IGameObjectFactory
{
    public GameObject Create(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        return Object.Instantiate(prefab, position, rotation);
    }

    public GameObject Create(GameObject prefab, Vector3 position, Quaternion rotation, Vector3 scale)
    {
        var obj = Object.Instantiate(prefab, position, rotation);
        obj.transform.localScale = scale;
        return obj;
    }

    public GameObject Create(Sprite sprite, Vector3 position, Quaternion rotation)
    {
        var obj = new GameObject(sprite.name);
        var spriteRenderer = obj.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = sprite;
        obj.transform.position = position;
        obj.transform.rotation = rotation;
        return obj;
    }
}