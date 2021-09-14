using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class StatBar : MonoBehaviour
{
    public Image fillBar;
    public TMP_Text text;
    public void UpdateBar(float min, float max)
    {
        text.text = min + "/" + max;
        fillBar.fillAmount = min/  max;
    }
}
