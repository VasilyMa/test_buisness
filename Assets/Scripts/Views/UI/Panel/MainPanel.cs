
using UnityEngine;

public class MainPanel : MonoBehaviour
{
    private BusinessLayout layout;


    public MainPanel Init()
    {
        layout = GetComponentInChildren<BusinessLayout>().Init();
        return this;
    }
}
