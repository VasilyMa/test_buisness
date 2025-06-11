using UnityEngine;

[CreateAssetMenu(fileName = "InterfaceConfig", menuName = "Interface/InterfaceConfig")]
public class InterfaceConfig : ScriptableObject
{
    #region instance
    private static InterfaceConfig _instance;

    public static InterfaceConfig Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = Resources.Load<InterfaceConfig>("InterfaceConfig");
                if (_instance == null)
                    Debug.LogError("InterfaceConfig not found in Resources!");
            }
            return _instance;
        }
    }
    #endregion

    public BusinessSlotView BusinessSlotPref;
    public UpgradeSlotView UpgradeSlotPref;
}
