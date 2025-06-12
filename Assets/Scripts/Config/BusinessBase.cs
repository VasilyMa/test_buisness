using System.Collections.Generic;
using System.Reflection;

using Client;

using Leopotam.EcsLite;

using UnityEngine;

[CreateAssetMenu(fileName = "NewBusiness", menuName = "Business/NewBusiness")]
public class BusinessBase : ScriptableObject, ISerializationCallbackReceiver
{
    public string KEY_ID;
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

        ref var incomeComp = ref world.GetPool<IncomeComponent>().Get(entity);
        ref var businessComp = ref world.GetPool<BusinessComponent>().Get(entity);
        ref var costComp = ref world.GetPool<CostComponent>().Get(entity);

        businessComp.KEY_ID = KEY_ID;

        foreach (var upgrade in Upgrades)
        {
            var upgradeEntity = world.NewEntity();

            upgrade.InitEntity(world, upgradeEntity);

            ref var upgradeComp = ref world.GetPool<UpgradeComponent>().Get(upgradeEntity);
            upgradeComp.BusinessEntity = entity;
            upgradeComp.KEY_ID = upgrade.KEY_ID;

            businessComp.Upgrades.Add(upgradeEntity);

            var upgradeData = SaveModule.CurrentData.Upgrades.Find(x => x.KEY_ID == upgrade.KEY_ID);

            if (upgradeData != null)
            {
                if (upgradeData.IsBuyed)
                {
                    upgradeComp.IsBuyed = true;
                    incomeComp.AddModifier(upgradeComp.UpgradeBonusValue);
                }
            }
        }

        var businessData = SaveModule.CurrentData.BusinessList.Find(x => x.KEY_ID == KEY_ID);

        if (businessData != null)
        {
            incomeComp.Level = businessData.Level;
            incomeComp.Recalculate();

            costComp.Cost = costComp.CostBase * (1 + incomeComp.Level);
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
