using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MinusBTN : MonoBehaviour
{
    public Button button;
    public TextMeshProUGUI StatText;
    private int tempNum;

    private void Awake()
    {
        tempNum = 0;
    }


    public void DeceaseStat()
    {
        Debug.Log("MinusBTN");
        if (int.Parse(StatText.text) > 1)
        {
            tempNum = int.Parse(StatText.text);
            tempNum -= 1;
            StatText.text = tempNum.ToString();
        }
    }
}