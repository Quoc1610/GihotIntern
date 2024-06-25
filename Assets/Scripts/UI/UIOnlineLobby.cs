using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIOnlineLobby : MonoBehaviour
{
    public Button btnCreateRoom;
    public Button btnGetRooms;
    [SerializeField] private GameObject prefabBtnRoom;
    public GameObject scrollViewContent;
    private List<GameObject> lsBtn = new List<GameObject>();
    [SerializeField] private GameObject popupCreate;
    [SerializeField] private Button btnPopUpCreate;
    [SerializeField] private GameObject inputRoomName;
    [SerializeField] private TMP_InputField inRoomName;
    [SerializeField] private TMP_Dropdown ddGameMode;
    [SerializeField] private Button btnCloseCreatePU;
    [System.Serializable]
    class MainMenuEvent 
    {
        public string event_name;
        public bool value;
        public string name;
        public string game_mode;
        public MainMenuEvent(string event_name, bool value, string name = "", string game_mode = "")
        {
            this.event_name = event_name;
            this.value = value;
            this.name = name;
            this.game_mode = game_mode;
        }
    }
    
    [System.Serializable]
    class SendData
    {
        public string player_id = Player_ID.MyPlayerID;
        public MainMenuEvent _event;
        public SendData(MainMenuEvent _event)
        {
            this._event = _event;
        }
    }

    public void OnPopUpCreate_Clicked()
    {
        popupCreate.SetActive(true);
    }
    public void OnSetUp()
    {
        popupCreate.SetActive(false);
        btnCloseCreatePU.onClick.AddListener(() =>
        {
            popupCreate.SetActive(false);
        });
    }

    public void OnGetRoom_Clicked()
    {
        SendData data = new SendData(new MainMenuEvent("get_rooms", true));
        
        SocketCommunication.GetInstance().Send(JsonUtility.ToJson(data));
        
    }
    public void OnClickCreateRoom_Clicked()
    {
        
        SendData data = new SendData(new MainMenuEvent("create_rooms", true, inRoomName.text, ddGameMode.options[ddGameMode.value].text));
        SocketCommunication.GetInstance().Send(JsonUtility.ToJson(data));
        
        //SocketCommunication.GetInstance().CreateRoom();
    }

    public void InitListRoom(Room[] rooms)
    {
        for (int i = lsBtn.Count-1; i >= 0; i--)
        {
            Destroy(lsBtn[i]);
            lsBtn.RemoveAt(i);
        }
        Debug.Log("Length"+lsBtn.Count);
        for (int i = 0; i < rooms.Length; i++)
        {
            //Debug.Log(rooms[i].game_mode+" "+rooms[i].name);
            GameObject btnContent = Instantiate(prefabBtnRoom, scrollViewContent.transform.position, Quaternion.identity);
            ItemLobby item = btnContent.GetComponent<ItemLobby>();
            item.OnGetLobby(rooms[i].name,rooms[i].game_mode);
            btnContent.transform.SetParent(scrollViewContent.transform);
            lsBtn.Add(btnContent);
        }
        Debug.Log("Length"+lsBtn.Count);
    }
}
