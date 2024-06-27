using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIMainMenu : MonoBehaviour
{
    [SerializeField ] List<Button> lsBtnShowPlayer=new List<Button>();
    [SerializeField] private List<GameObject> lsGOPlayer = new List<GameObject>();
    [SerializeField] private GameObject goOnline;
    [SerializeField] public Button btnOnline;
    [SerializeField] private List<GameObject> lsBtnForPlayer = new List<GameObject>();
    [SerializeField] private List<TextMeshProUGUI> lsTxtName = new List<TextMeshProUGUI>();
    [SerializeField] private List<GameObject> goPlayerList = new List<GameObject>();
    [SerializeField] public Button btnKickForHost;
    [SerializeField] private List<GameObject> goBorderMe = new List<GameObject>();
    public void OnSetUp()
    {
        
        lsBtnForPlayer[0].SetActive(true);
        lsBtnForPlayer[1].SetActive(false);
        goOnline.SetActive(false);
        lsGOPlayer[0].SetActive(false);
        lsGOPlayer[1].SetActive(false);
        goPlayerList[0].SetActive(false);
        goPlayerList[1].SetActive(false);
        btnKickForHost.gameObject.SetActive(true);
    }

    public void ShowPlayerBtn()
    {
        lsBtnForPlayer[1].SetActive(true);
        lsBtnForPlayer[0].SetActive(false);
    }

    public void HostChangeLobbyListName(Dictionary<string,Player> players)
    {

        for (int i = 0; i < players.Count; i++)
        {
            goPlayerList[i].SetActive(false);
        }
        int index = 0;
        foreach (var pair in players)
        {
            lsTxtName[index].text = pair.Value.name;
            goPlayerList[index].SetActive(true);
            btnKickForHost.gameObject.SetActive(true);
            
            index++;
        }
        goBorderMe[0].SetActive(true);
    }

    public void ChangeLobbyListName(Dictionary<string, Player> players)
    {
        for (int i = 0; i < players.Count; i++)
        {
            goPlayerList[i].SetActive(false);
        }
        int index = 0;
        foreach (var pair in players)
        {
            goPlayerList[index].SetActive(true);
            if (index != 0)
            {
                lsTxtName[index-1].text = pair.Value.name;
            }
            btnKickForHost.gameObject.SetActive(false);
            index++;
        }

        lsTxtName[index-1].text = players[Player_ID.MyPlayerID].name; 
        goBorderMe[1].SetActive(true);
    }
    public void AfterCreate()
    {
        btnOnline.gameObject.SetActive(false);
        lsBtnForPlayer[0].SetActive(true);
        lsBtnForPlayer[1].SetActive(false);
        lsGOPlayer[0].SetActive(true);
    }
    public void AfterCreateGuess()
    {
        btnOnline.gameObject.SetActive(false);
        lsBtnForPlayer[1].SetActive(true);
        lsBtnForPlayer[0].SetActive(false);
        lsGOPlayer[0].SetActive(true);
    }

    // public void JoinCall(int i)
    // {
    //     if (i == 0)
    //     {
    //         goBorderMe[i].SetActive(true);
    //         btnKickForHost.gameObject.SetActive(true);
    //     }
    //     else
    //     {
    //         goBorderMe[i].SetActive(true);
    //         btnKickForHost.gameObject.SetActive(false);
    //     }
    // }
    public void OnBtnClick(int index)
    {
        if (index == 0)
        {
            lsGOPlayer[1].SetActive(true);
            lsGOPlayer[0].SetActive(false);
        }
        else
        {
            lsGOPlayer[0].SetActive(true);
            lsGOPlayer[1].SetActive(false);
        }
    }

    public void OnOnline_Clicked()
    {
        goOnline.SetActive(true);
    }
    
}
