using Leopotam.EcsLite;

using UnityEngine;
namespace Client 
{
    [System.Serializable]
    public struct UpgradeComponent : IComponent
    {
        [HideInInspector] public int BusinessEntity;
        public float UpgradeBonusValue;

        public void Init(EcsWorld world, int entity)
        {
            world.GetPool<UpgradeComponent>().Add(entity).UpgradeBonusValue = UpgradeBonusValue;
        }
    }
}