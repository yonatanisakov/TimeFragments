using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Assets.Scripts.Utils
{
    public static class ObjectPooler
    {
        static Dictionary<string,Queue<Component>> poolDictionary = new Dictionary<string,Queue<Component>>();
        static Dictionary<string,Component> lookUpDictionary = new Dictionary<string,Component>();
        public static void SetUpPool<T>(T pooledPrefab,string className,int poolSize) where T : Component
        {
            poolDictionary.Add(className,new Queue<Component>());
            lookUpDictionary.Add(className,pooledPrefab);
            for (int i = 0; i < poolSize; i++)
            {
                T pooledInstance = Object.Instantiate(pooledPrefab);
                pooledInstance.gameObject.SetActive(false);
                poolDictionary[className].Enqueue(pooledInstance);
            }
        }

        public static void EnqueueObject<T>(T pooledPrefab,string className) where T:Component {
            if (!pooledPrefab.gameObject.activeSelf)
            {
                return;
            }
            pooledPrefab.gameObject.transform.position = Vector3.zero;
            pooledPrefab.gameObject.SetActive(false);
            poolDictionary[className].Enqueue(pooledPrefab);
        }
        public static T DequeueObject<T>(string className) where T : Component
        {
            if (poolDictionary[className].TryDequeue(out Component pooledItem))
                return (T)pooledItem;
            return (T)EnqueAdditionalObject(lookUpDictionary[className], className);
        }

        private static T EnqueAdditionalObject<T>(T pooledPrefab,string className) where T:Component
        {
            T pooledInstance = Object.Instantiate(pooledPrefab);
            pooledInstance.gameObject.SetActive(false);
            pooledInstance.gameObject.transform.position= Vector3.zero;
            poolDictionary[className].Enqueue(pooledInstance);
            return pooledInstance;
        }
    }
}
