using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YG;
using TMPro;

public class CheckConnectYG : MonoBehaviour
{
    private void OnEnable() => YandexGame.GetDataEvent += CheckSDK;
    private void OnDisable() => YandexGame.GetDataEvent -= CheckSDK;
    private TextMeshProUGUI scoreBest;
    public GameObject AchivList;
    // Start is called before the first frame update
    void Start(){   
        Debug.Log(YandexGame.SDKEnabled);
        if (YandexGame.SDKEnabled){
            CheckSDK();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CheckSDK(){
        if(YandexGame.auth){
            Debug.Log("User authorization OK");
        }
        else{
            Debug.Log("User NOT authorization");
            YandexGame.AuthDialog();
        }
        GameObject scoreBO = GameObject.Find("BestScore");
        scoreBest = scoreBO.GetComponent<TextMeshProUGUI>();
        scoreBest.text = "Best Score: " + YandexGame.savesData.bestScore.ToString();
        if ((YandexGame.savesData.achivment)[0] == null){

        }
        else{
            foreach(string value in YandexGame.savesData.achivment){
                AchivList.GetComponent<TextMeshProUGUI>().text = AchivList.GetComponent<TextMeshProUGUI>().text + value + "\n";
            }
        }
    }
}
