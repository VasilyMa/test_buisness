using Leopotam.EcsLite;
using UnityEngine;
namespace Client
{
    [System.Serializable]
    public struct CostComponent : IComponent
    {
        public int CostBase;
        [HideInInspector] public int Cost;
        

        public void Init(EcsWorld world, int entity)
        {
            ref var costComp = ref world.GetPool<CostComponent>().Add(entity);
            costComp.CostBase = CostBase;
            costComp.Cost = CostBase;
        }
    }
}