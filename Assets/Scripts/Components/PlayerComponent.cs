using UnityEngine;

namespace Client 
{
    struct PlayerComponent 
    {
        public int Value;

        public void AddValue(int value)
        {
            Value += value;

            ObserverEntity.ValueChange(Value);
        }

        public void SubValue(int value)
        {
            Value -= value;

            Value = Mathf.Clamp(Value, 0, int.MaxValue);

            ObserverEntity.ValueChange(Value);
        }
    }
}