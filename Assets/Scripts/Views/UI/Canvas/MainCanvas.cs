using UnityEngine;

public class MainCanvas : MonoBehaviour
{
    MainPanel mainPanel;

    public void Init()
    {
        mainPanel = GetComponentInChildren<MainPanel>().Init();
    }
}
