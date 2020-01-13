using Photon.Pun;
using UnityEngine;

public class startGame : MonoBehaviour
{
    public void OnClickStartSync()
    {
        Debug.Log("시작하기전");
        PhotonNetwork.LoadLevel(1);
    }
}
