using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BusinessConfig", menuName = "Business/Config")]
public class BusinessConfig : ScriptableObject
{
    #region instance
    private static BusinessConfig _instance;

    public static BusinessConfig Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = Resources.Load<BusinessConfig>("BusinessConfig");
                if (_instance == null)
                    Debug.LogError("BusinessConfig not found in Resources!");
            }
            return _instance;
        }
    }
    #endregion

    public List<BusinessBase> BusinessListData;
}
