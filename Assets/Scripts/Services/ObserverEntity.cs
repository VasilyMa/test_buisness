using System;

public static class ObserverEntity
{
    public static Action<int> OnValueChange;
    public static Action<int, float> OnProgressChange;
    public static Action<int> OnBusinessChange;

    public static void ValueChange(int value)
    {
        OnValueChange?.Invoke(value);
    }
    public static void ProgressChange(int entity, float value)
    {
        OnProgressChange?.Invoke(entity, value);
    }
    public static void BusinessChange(int entity) 
    {
        OnBusinessChange?.Invoke(entity);
    }
}
