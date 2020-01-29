using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class SendMessage_ingame : MonoBehaviourPunCallbacks
{
    public Text[] ChatText;
    public InputField ingame_ChatInput;
    private static string[] myColor = new string[4] { "#FF453B" ,"#5393E0","#F8D700", "#9C00FF"};
    PhotonView PV;

    private void Awake()
    {
        PV = this.GetComponent<PhotonView>();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (ingame_ChatInput.text == "")
                ingame_ChatInput.ActivateInputField();
            else
                ingame_Send();
        }
    }
    
    #region 채팅
    public void ingame_Send()
    {
        SoundManager.instance.PlaysendMSGSound();
        string msg = PhotonNetwork.PlayerList[PhotonNetwork.LocalPlayer.ActorNumber -1].NickName;
        string msg2 = " : " + ingame_ChatInput.text;
        string merge_msg = msg + msg2;
        Debug.Log("<color=red>msg : </color>" + msg);
        PV.RPC("ingame_ChatRPC", RpcTarget.All, msg, msg2, NetworkRoundManager.public_Player_Id);
        ingame_ChatInput.text = "";
        ingame_ChatInput.ActivateInputField();
        Debug.Log("<color=red>playerid : </color>" + NetworkRoundManager.public_Player_Id);
    }

    [PunRPC] // RPC는 플레이어가 속해있는 방 모든 인원에게 전달한다
    void ingame_ChatRPC(string msg1, string msg2, int playerid)
    {
        bool isInput = false;
        //MonsterUI[7].text = " <color=#0000ffff>층</color>";
        for (int i = 0; i < ChatText.Length; i++)
            if (ChatText[i].text == "")
            {
                isInput = true;
                ChatText[i].text = "<color=" + myColor[playerid -1] + ">" + msg1 + "</color>" + msg2;
                break;
            }
        if (!isInput) // 꽉차면 한칸씩 위로 올림
        {
            for (int i = 1; i < ChatText.Length; i++) ChatText[i - 1].text = ChatText[i].text;
            ChatText[ChatText.Length - 1].text = "<color=" + myColor[playerid-1] + ">" + msg1 + "</color>" + msg2;
        }
    }
    #endregion
}
