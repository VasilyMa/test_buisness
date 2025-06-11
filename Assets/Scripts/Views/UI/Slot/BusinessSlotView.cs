using Client;

using Leopotam.EcsLite;

using UnityEngine;
using UnityEngine.UI;

public class BusinessSlotView : MonoBehaviour
{
    int businessEntity;
    EcsWorld world;
    [SerializeField] Text titleName;
    [SerializeField] Image progressFill;
    [SerializeField] Text titleBusinessInfo;
    [SerializeField] Text titleIncomeInfo;
    [SerializeField] Text titleCostLevelUpInfo;

    [SerializeField] Button btnLevelUp;

    [SerializeField] Transform parentSlotsButtonUpgrade;

    public void Init(int entity)
    {
        world = State.Instance.EcsRunHandler.World;
        businessEntity = entity;

        btnLevelUp.onClick.AddListener(InvokLevelUp);

        if (State.Instance.TryGetEntity("player", out int playerEntity))
        {
            ref var playerComp = ref world.GetPool<PlayerComponent>().Get(playerEntity);
            onValueChange(playerComp.Value);
        }

        onBusinessChange(entity);
        InitUpgrades();

        ObserverEntity.OnProgressChange += onProgressUpdate;
        ObserverEntity.OnBusinessChange += onBusinessChange;
        ObserverEntity.OnValueChange += onValueChange;
    }

    void InitUpgrades()
    {
        var interfaceConfig = InterfaceConfig.Instance;

        ref var businessComp = ref world.GetPool<BusinessComponent>().Get(businessEntity);

        foreach (var entityUpgrade in businessComp.Upgrades)
        {
            var upgradeSlot = GameObject.Instantiate(interfaceConfig.UpgradeSlotPref, parentSlotsButtonUpgrade);

            upgradeSlot.Init(entityUpgrade);
        }
    }

    void onProgressUpdate(int entity, float value)
    {
        if (entity == businessEntity)
        {
            progressFill.fillAmount = value;
        }
    }

    void onBusinessChange(int entity)
    {
        if (businessEntity == entity)
        {
            if (world.GetPool<DescriptionComponent>().Has(entity))
            {
                ref var descriptionComp = ref world.GetPool<DescriptionComponent>().Get(entity);
                titleName.text = descriptionComp.Name;
            }

            if (world.GetPool<IncomeComponent>().Has(entity))
            {
                ref var incomeComp = ref world.GetPool<IncomeComponent>().Get(entity);
                titleBusinessInfo.text = $"LVL\b{incomeComp.Level}";
                titleIncomeInfo.text = $"Доход\b${incomeComp.Value}";

                if (world.GetPool<CostComponent>().Has(entity))
                {
                    ref var costComp = ref world.GetPool<CostComponent>().Get(entity);
                    titleCostLevelUpInfo.text = $"Цена: \b${costComp.CostBase * (1 + incomeComp.Level)}";
                }
            }
        }
    }

    void onValueChange(int value)
    {
        if (world.GetPool<CostComponent>().Has(businessEntity))
        {
            ref var costComp = ref world.GetPool<CostComponent>().Get(businessEntity);

            if (value >= costComp.Cost)
            {
                btnLevelUp.interactable = true;
            }
            else
            {
                btnLevelUp.interactable = false;
            }
        }
    }

    void InvokLevelUp()
    {
        world.GetPool<ResolveLevelUpEvent>().Add(businessEntity);
    }

    private void OnDestroy()
    {
        ObserverEntity.OnProgressChange -= onProgressUpdate;
        ObserverEntity.OnBusinessChange -= onBusinessChange;
        ObserverEntity.OnValueChange -= onValueChange;
        btnLevelUp.onClick.RemoveAllListeners();
    }
}
