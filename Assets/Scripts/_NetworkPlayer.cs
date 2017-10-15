using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _NetworkPlayer: Photon.MonoBehaviour {

	private Vector3 correctPlayerPos;
	private Quaternion correctPlayerRot;
    //public _PlayerMovement playerMovement;
    public Animator anim;
	// Use this for initialization
	void Start () {
		/*if (photonView.isMine) {
			playerMovement.enabled = true;
		}*/
	}

	// Update is called once per frame
	void Update () {
		if (!photonView.isMine)
		{
			transform.position = Vector3.Lerp(transform.position, this.correctPlayerPos, Time.deltaTime * 5);
			transform.rotation = Quaternion.Lerp(transform.rotation, this.correctPlayerRot, Time.deltaTime * 5);
		}
	}

	void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		Debug.Log("Something is happening to the stream...");
		if (stream.isWriting)
		{
			// We own this player: send the others our data
			stream.SendNext(transform.position);
			stream.SendNext(transform.rotation);
            stream.SendNext(anim.GetBool("Run"));
            stream.SendNext(anim.GetBool("Attack"));

        }
		else
		{
			// Network player, receive data
			this.correctPlayerPos = (Vector3)stream.ReceiveNext();
			this.correctPlayerRot = (Quaternion)stream.ReceiveNext();
            anim.SetBool("Run", (bool)stream.ReceiveNext());
            anim.SetBool("Attack", (bool)stream.ReceiveNext());
        }
	}
}
