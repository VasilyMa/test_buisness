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

            Value = Mathf.Min(Value, 0);

            ObserverEntity.ValueChange(Value);
        }
    }
}