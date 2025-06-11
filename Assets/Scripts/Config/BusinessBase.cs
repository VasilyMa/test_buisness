using System.Collections.Generic;
using System.Reflection;

using Client;

using Leopotam.EcsLite;

using UnityEngine;

[CreateAssetMenu(fileName = "NewBusiness", menuName = "Business/NewBusiness")]
public class BusinessBase : ScriptableObject
{
    public DescriptionComponent DescriptionComponent;
    public BusinessComponent BusinessComponent;
    public CostComponent CostComponent;
    public IncomeComponent IncomeComponent;
    public IncomeDelayComponent DelayComponent;

    public List<UpgradeBase> Upgrades;

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

        ref var businessComp = ref world.GetPool<BusinessComponent>().Get(entity);

        foreach (var upgrade in Upgrades)
        {
            var upgradeEntity = world.NewEntity();

            upgrade.InitEntity(world, upgradeEntity);

            world.GetPool<UpgradeComponent>().Get(upgradeEntity).BusinessEntity = entity;

            businessComp.Upgrades.Add(upgradeEntity);
        }
    }
}
