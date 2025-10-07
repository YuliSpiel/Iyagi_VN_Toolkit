using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MoveCount : MonoBehaviour
{
    public int movecountPublic;
    public StatManager statManager;
    private MoveCount Instance = null;

    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
        statManager = GameObject.Find("StatDataManager").GetComponent<StatManager>();
    }

    private void Start()
    {
        //1 이하일 때 폭주 게임오버, 66 이상일 때 자살 게임오버 추가
        if(statManager.statJsonList.Items[0].Sanity == 0) SceneManager.LoadScene("BadEnding 2");
        else if (statManager.statJsonList.Items[0].Sanity < 12 && statManager.statJsonList.Items[0].Sanity > 0) //0~11
        {
            movecountPublic = 1;
        }
        else if (statManager.statJsonList.Items[0].Sanity > 11 && statManager.statJsonList.Items[0].Sanity < 23) //12~22
        {
            movecountPublic = 2;
        }
        else if (statManager.statJsonList.Items[0].Sanity > 22 && statManager.statJsonList.Items[0].Sanity < 34) //23~33
        {
            movecountPublic = 3;
        }
        else if (statManager.statJsonList.Items[0].Sanity > 33 && statManager.statJsonList.Items[0].Sanity < 45) //34~44
        {
            movecountPublic = 4;
        }
        else if (statManager.statJsonList.Items[0].Sanity > 44 && statManager.statJsonList.Items[0].Sanity < 56) //45~55
        {
            movecountPublic = 5;
        }
        else if (statManager.statJsonList.Items[0].Sanity > 55 && statManager.statJsonList.Items[0].Sanity < 66) //55~66
        {
            movecountPublic = 6;
        }
        else if (statManager.statJsonList.Items[0].Sanity >= 66) SceneManager.LoadScene("BadEnding 1");
    }
}
