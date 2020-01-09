using Photon.Pun;
using UnityEngine;

public class startGame : MonoBehaviour
{
    public void OnClickStartSync()
    {
        PhotonNetwork.LoadLevel(1);
    }
}
