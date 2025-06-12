using System.Collections.Generic;
using System.Reflection;

using Client;

using Leopotam.EcsLite;

using UnityEngine;

[CreateAssetMenu(fileName = "NewUpgrade", menuName = "Business/NewUpgrade")]
public class UpgradeBase : ScriptableObject, ISerializationCallbackReceiver
{
    public string KEY_ID;
    public DescriptionComponent DescriptionComponent;
    public UpgradeComponent UpgradeComponent;
    public CostComponent CostComponent;

    public void InitEntity(EcsWorld world, int entity)
    {
        var components = new List<IComponent>();
        var fields = GetType().GetFields(BindingFlags.Public | BindingFlags.Instance);

        foreach (var field in fields)
        {
            if (field.GetValue(this) is IComponent value)
            {
                components.Add(value);
            }
        }

        foreach (var component in components)
        {
            component.Init(world, entity);
        }
    }

    public void OnAfterDeserialize()
    {

    }

    public void OnBeforeSerialize()
    {
        if (this != null)
        {
            KEY_ID = name;
        }
    }
}
