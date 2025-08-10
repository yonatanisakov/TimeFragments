
using UnityEngine;

public interface IGameObjectFactory
{
    GameObject Create(GameObject prefab,Vector3 position,Quaternion rotation);
    GameObject Create(GameObject prefab, Vector3 position, Quaternion rotation, Vector3 scale);
    GameObject Create(Sprite sprite, Vector3 position, Quaternion rotation);
}