using System.Collections.Generic;
using UnityEngine;

public class ServiceLocator : IService
{
    private ServiceLocator()
    {
    }

    private readonly Dictionary<string, IService> services = new Dictionary<string, IService>();

    public static ServiceLocator Instance { get; private set; }

    public static void Initialize()
    {
        Instance = new ServiceLocator();
    }

    public T Get<T>() where T : IService
    {
        string key = typeof(T).Name;
        if (!services.ContainsKey(key))
        {
#if UNITY_EDITOR
            Debug.LogError($"{key} is not defined");
#endif
        }
        return (T)services[key];
    }

    public void Register<T>(T service) where T : IService
    {
        string key = typeof(T).Name;
        if (services.ContainsKey(key))
        {
#if UNITY_EDITOR
            Debug.LogError($"{key} is already exist");
#endif

            return;
        }
        services.Add(key, service);
    }

    public void Unregister<T>(T service) where T : IService
    {
        string key = typeof(T).Name;
        if (!services.ContainsKey(key))
        {
#if UNITY_EDITOR
            Debug.LogError($"{key} isnt exist");
#endif

            return;
        }

        services.Remove(key);
    }
}
