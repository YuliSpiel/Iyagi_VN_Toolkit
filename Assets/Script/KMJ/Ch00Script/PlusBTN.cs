using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlusBTN : MonoBehaviour
{
    public Button button;
    public TextMeshProUGUI StatText;
    private int tempNum;

    private void Awake()
    {
        tempNum = 0;
    }

    public void IncreaseStat()
    {
        Debug.Log("PlusBTN");
        if (int.Parse(StatText.text) < 4)
        {
            tempNum = int.Parse(StatText.text);
            tempNum += 1;
            StatText.text = tempNum.ToString();
        }
    }
}