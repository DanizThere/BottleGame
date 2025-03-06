using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PoolObject<T> where T : MonoBehaviour
{
    private T prefab;
    private List<T> objects;

    public PoolObject(T prefab, int defaultObjects)
    {
        this.prefab = prefab;
        objects = new List<T>();

        for(int i = 0; i < defaultObjects; i++)
        {
            var obj = GameObject.Instantiate(this.prefab);
            obj.gameObject.SetActive(false);
            objects.Add(obj);
        }
    }

    public T Get()
    {
        var obj = objects.FirstOrDefault(x => !x.isActiveAndEnabled);

        if (obj == null)
        {
            obj = Create();
        }

        obj.gameObject.SetActive(true);
        return obj;
    }

    public void Release(T obj)
    {
        obj.gameObject.SetActive(false);
    }

    public T Create()
    {
        var obj = GameObject.Instantiate(prefab);
        objects.Add(obj);
        return obj;
    }

    public int Count()
    {
        return objects.Count;
    }
}
