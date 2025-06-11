using System.Collections.Generic;

using UnityEngine;
using Leopotam.EcsLite;

public class State : MonoBehaviour
{
    static State instance;
    public static State Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<State>();
            }
            
            return instance;
        }
    }

    public EcsRunHandler EcsRunHandler;
    Dictionary<string, EcsPackedEntity> entities;


    private void Awake()
    {
        entities = new Dictionary<string, EcsPackedEntity>();
        EcsRunHandler = new EcsRunHandler();
    }

    void Start()
    {
        EcsRunHandler.Init();
    }

    void Update()
    {
        EcsRunHandler.Run();
    }

    private void OnDestroy()
    {
        EcsRunHandler.Dispose();
    }

    public bool TryAddEntity(string entityName, int entity)
    {
        if (entities.ContainsKey(entityName)) return false;

        entities[entityName] = EcsRunHandler.World.PackEntity(entity);

        return true;
    }

    public virtual bool TryGetEntity(string key, out EcsPackedEntity value)
    {
        if (entities.ContainsKey(key))
        {
            value = entities[key];
            return true;

        }

        value = default;
        return false;
    }

    public bool TryGetEntity(string entityName, out int value)
    {
        if (entities.ContainsKey(entityName))
        {
            if (entities[entityName].Unpack(EcsRunHandler.World, out int entity))
            {
                value = entity;
                return true;
            }

        }

        value = -1;
        return false;
    }
}
