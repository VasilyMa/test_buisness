using System.Collections.Generic;

using Leopotam.EcsLite;
using UnityEngine;

namespace Client 
{
    public struct BusinessComponent : IComponent
    {
        public List<int> Upgrades;

        public void Init(EcsWorld world, int entity)
        {
            world.GetPool<BusinessComponent>().Add(entity).Upgrades = new List<int>(); 
        }
    }

    [System.Serializable]
    public struct IncomeComponent : IComponent 
    {
        public int BaseValue;
        [HideInInspector] public int Level;
        [HideInInspector] public int Value;
        private List<float> _modifiers;


        public void Init(EcsWorld world, int entity)
        {
            ref var incomeComp = ref world.GetPool<IncomeComponent>().Add(entity);
            incomeComp.BaseValue = BaseValue;
            incomeComp.Value = BaseValue;
            incomeComp.Level = 1;
            incomeComp._modifiers = new List<float>();

        }

        public void LevelUp()
        {
            Level++;
            Recalculate();
        }

        public int AddModifier(float modifier)
        {
            _modifiers.Add(modifier);
            Recalculate();
            return Value;
        }

        void Recalculate()
        {
            float totalModifiers = 1f;

            _modifiers.ForEach(x => totalModifiers += x);

            Value = Mathf.RoundToInt(Level * BaseValue * totalModifiers);
        }
    }

    [System.Serializable]
    public struct IncomeDelayComponent : IComponent
    {
        [HideInInspector] public float RemainningTime;
        public float DelayTime;

        public void Init(EcsWorld world, int entity)
        {
            ref var delayComp = ref world.GetPool<IncomeDelayComponent>().Add(entity);
            delayComp.DelayTime = DelayTime;
        }
    }
}