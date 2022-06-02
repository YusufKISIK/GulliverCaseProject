using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIResChanger : MonoBehaviour
{
    public Text Reso;
    public Text Device;
    
    public void Iphone6s()
    {
        Screen.SetResolution(750, 1334, true);
        Device.text = "Iphone 6s";
        Reso.text = "750x1334";
    }
    public void Iphone11()
    {
        Screen.SetResolution(1792, 828, true);
        Device.text = "Iphone 11";
        Reso.text = "1792x828";
    }
    public void Ipad11()
    {
        Screen.SetResolution(640, 480, true);
        Device.text = "iPad Pro 11";
        Reso.text = "2388 x 1668";
    }
}
