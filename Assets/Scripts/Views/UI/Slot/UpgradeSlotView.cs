using Leopotam.EcsLite;
using Client;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeSlotView : MonoBehaviour
{
    EcsWorld world;
    int upgradeEntity;
    [SerializeField] Text titleName;
    [SerializeField] Text titleDescription;
    [SerializeField] Text titleCost;
    [SerializeField] Button btnUpgrade;

    bool buyed;

    public void Init(int entity)
    {
        world = State.Instance.EcsRunHandler.World;
        upgradeEntity = entity;

        if (world.GetPool<DescriptionComponent>().Has(entity))
        {
            ref var descriptionComp = ref world.GetPool<DescriptionComponent>().Get(entity);
            titleName.text = descriptionComp.Name;
            titleDescription.text = descriptionComp.Description;
        }

        if (world.GetPool<CostComponent>().Has(entity))
        {
            ref var costComp = ref world.GetPool<CostComponent>().Get(entity);
            titleCost.text = $"Цена: \b${costComp.CostBase}";
        }

        ObserverEntity.OnValueChange += onValueChange;

        btnUpgrade.onClick.AddListener(InvokeUpgrade);
    }

    void onValueChange(int value)
    {
        if (buyed) return;

        if (world.GetPool<CostComponent>().Has(upgradeEntity)) 
        {
            ref var costComp = ref world.GetPool<CostComponent>().Get(upgradeEntity);

            if (value >= costComp.Cost)
            {
                btnUpgrade.interactable = true;
            }
            else
            {
                btnUpgrade.interactable = false;
            } 
        }
    }


    void InvokeUpgrade()
    {
        ref var upgradComp = ref world.GetPool<UpgradeComponent>().Get(upgradeEntity);

        world.GetPool<ResolveUpgradeEvent>().Add(upgradComp.BusinessEntity).Value = upgradComp.UpgradeBonusValue;

        btnUpgrade.interactable = false;
        titleCost.text = "Куплено";

        buyed = true;

        ObserverEntity.OnValueChange -= onValueChange;
    }

    private void OnDestroy()
    {
        ObserverEntity.OnValueChange -= onValueChange;
        btnUpgrade.onClick.RemoveAllListeners();
    }
}
