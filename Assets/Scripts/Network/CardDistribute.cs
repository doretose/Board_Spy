using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class CardDistribute : MonoBehaviourPunCallbacks
{
    PhotonView pv;
    public GameObject CardToHand;
    public GameObject CardToHandAttack;

    // Start is called before the first frame update
    void Start()
    {
        pv = this.GetComponent<PhotonView>();
    }

    public void AllPlayerCardDistribute()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            pv.RPC("RPCCardDistribute", RpcTarget.AllBuffered);
        }
        else { return; }
    }

    [PunRPC]
    private void RPCCardDistribute() => StartCoroutine(startCardDistribute());

    IEnumerator startCardDistribute()
    {
        for (int i = 0; i <= 1; i++)
        {
            yield return new WaitForSeconds(0.5f);
            Instantiate(CardToHandAttack, transform.position, transform.rotation);
        }
        for (int i = 0; i <= 2; i++)
        {
            yield return new WaitForSeconds(0.5f);
            Instantiate(CardToHand, transform.position, transform.rotation);
        }
    }
}
