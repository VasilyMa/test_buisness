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
            ref var upgradeComp = ref world.GetPool<UpgradeComponent>().Get(entity);    
            
            if (upgradeComp.IsBuyed)
            {
                buyed = true;
                btnUpgrade.interactable = false;
                titleCost.text = "Куплено";
            }
            else
            {
                titleCost.text = $"Цена: \n${costComp.CostBase}";
                ObserverEntity.OnValueChange += onValueChange;
                btnUpgrade.onClick.AddListener(InvokeUpgrade);
            }
        }
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
        if (State.Instance.TryGetEntity("player", out int entityPlayer))
        {
            ref var playerComp = ref world.GetPool<PlayerComponent>().Get(entityPlayer);

            if (world.GetPool<CostComponent>().Has(upgradeEntity))
            {
                ref var upgradeComp = ref world.GetPool<UpgradeComponent>().Get(upgradeEntity);
                ref var costComp = ref world.GetPool<CostComponent>().Get(upgradeEntity);

                if (playerComp.Value >= costComp.Cost)
                {
                    playerComp.SubValue(costComp.Cost);

                    ref var upgradeEventComp = ref world.GetPool<ResolveUpgradeEvent>().Add(upgradeComp.BusinessEntity);
                    upgradeEventComp.Value = upgradeComp.UpgradeBonusValue;
                    upgradeEventComp.KEY_ID = upgradeComp.KEY_ID;

                    btnUpgrade.interactable = false;
                    titleCost.text = "Куплено";

                    buyed = true;

                    ObserverEntity.OnValueChange -= onValueChange;
                }
            }
        }
    }

    private void OnDestroy()
    {
        ObserverEntity.OnValueChange -= onValueChange;
        btnUpgrade.onClick.RemoveAllListeners();
    }
}
