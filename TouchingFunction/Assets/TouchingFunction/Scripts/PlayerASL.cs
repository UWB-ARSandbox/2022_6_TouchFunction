using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ASL;

public class PlayerASL : MonoBehaviour
{
    private static readonly float UPDATES_PER_SECOND = 30.0f;
    bool isLocal = false; // Whether this player is controlled by this client
    public TextMesh nameText;
    ASLObject aslObj;
    public Player player;

    public Transform playerHead;
    /*public Transform playerBody;
    public Transform playerLeftArm;
    public Transform playerRightArm;
    public Transform playerLeftShoulder;
    public Transform playerRightShoulder;*/

    public Transform[] HairParts;
    public Transform[] ShirtParts;
    public Transform[] PantsParts;
    public Transform[] SkinParts;

    public Material[] HeadMat;
    public int PeerID;
    public bool recievedPeerID = false;

    public void SetLocal()
    {
        isLocal = true;
    }

    public void Quit()
    {
        if(IsLocal())
        {
            aslObj.SendAndSetClaim(()  =>
            {
                aslObj.DeleteObject();
            });
        }
    }
    
    public bool IsLocal()
    {
        return isLocal;
    }

    // Start is called before the first frame update
    void Start()
    {
        aslObj = GetComponent<ASLObject>();
        aslObj._LocallySetFloatCallback(Receive);
        //aslObj._LocallySetPostDownloadFunction(DownloadAvatar);
    }

    //static public void DownloadAvatar(GameObject g, Texture2D texture)
    //{
        //g.transform.Find("body").Find("head").GetComponent<MeshRenderer>().material.mainTexture = texture;
    //}

    public void SendTransform()
    {
        var f = new float[9];
        f[0] = 1; // Identifier
        f[1] = player.isFlying ? 1 : -1;
        f[2] = player.isMoving ? 1 : -1;
        f[3] = player.isSliding ? 1 : -1;
        f[4] = player.isFalling ? 1 : -1;
        f[5] = player.isThinking ? 1 : -1;
        f[6] = playerHead.transform.localRotation.eulerAngles.x;
        f[7] = transform.localRotation.eulerAngles.y;
        f[8] = transform.localScale.x;

        aslObj.SendAndSetClaim(() =>
        {
            aslObj.SendFloatArray(f);
        });
    }

    public void SendLook(float[] _f)
    {
        aslObj.SendAndSetClaim(() =>
        {
            aslObj.SendFloatArray(_f);
        });

    }

    public void Receive(string _id, float[] _f)
    {
        if (!string.Equals(aslObj.m_Id, _id)) return; // Not for this player, disregard

        switch (_f[0])
        {
            case 0: // Set ASL id / name
                if (!nameText.gameObject.activeInHierarchy) break;

                int ASL_id = (int)_f[1];
                nameText.text = GameLiftManager.GetInstance().m_Players[ASL_id];
                break;
            case 1: // Set transform
                if (isLocal) break;
                player.isFlying = _f[1] > 0;
                player.isMoving = _f[2] > 0;
                player.isSliding = _f[3] > 0;
                player.isFalling = _f[4] > 0;
                player.isThinking = _f[5] > 0;
                player.setAnimatorBool();
                
                playerHead.transform.localRotation = Quaternion.Euler(_f[6], 0, 0);
                transform.localRotation = Quaternion.Euler(0, _f[7], 0);
                transform.localScale = new Vector3(_f[8], _f[8], _f[8]);
                break;

            case 3: // Set color
                /*Color color = new Color(_f[1], _f[2], _f[3], _f[4]);
                playerBody.GetComponent<MeshRenderer>().material.color = color;
                playerLeftArm.GetComponent<MeshRenderer>().material.color = color;
                playerRightArm.GetComponent<MeshRenderer>().material.color = color;
                playerRightShoulder.GetComponent<MeshRenderer>().material.color = color;
                playerLeftShoulder.GetComponent<MeshRenderer>().material.color = color;*/
                playerHead.GetComponent<MeshRenderer>().material = HeadMat[(int)_f[1]];
                Color hairColor = new Color(_f[2], _f[3], _f[4], _f[5]);
                Color shirtColor = new Color(_f[6], _f[7], _f[8], _f[9]);
                Color pantsColor = new Color(_f[10], _f[11], _f[12], _f[13]);
                Color skinColor = new Color(_f[14], _f[15], _f[16], _f[17]);
                foreach (var part in HairParts)
                {
                    part.GetComponent<MeshRenderer>().material.color = hairColor;
                }
                foreach(var part in ShirtParts)
                {
                    part.GetComponent<MeshRenderer>().material.color = shirtColor;
                }
                foreach (var part in PantsParts)
                {
                    part.GetComponent<MeshRenderer>().material.color = pantsColor;
                }
                foreach (var part in SkinParts)
                {
                    part.GetComponent<MeshRenderer>().material.color = skinColor;
                }
                if(_f[18] > 0)
                {
                    HairParts[1].gameObject.SetActive(true);
                } else
                {
                    HairParts[1].gameObject.SetActive(false);
                }
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        nameText.transform.rotation = Quaternion.LookRotation(nameText.transform.position - Camera.main.transform.position, Vector3.up);
    }

    public void InitLocalPlayer()
    {
        StartCoroutine(NetworkedUpdate());
        FindObjectOfType<ChangeColor>().SetPlayer(this);
        FindObjectOfType<ChangeColor>().SetCamera(GetComponentInChildren<Camera>(), GetComponentInChildren<Player>());
        FindObjectOfType<MirrorCamera>().player = player.transform;
    }

    IEnumerator NetworkedUpdate()
    {
        while (aslObj == null)
        {
            yield return null;
        }

        aslObj.SendAndSetClaim(() =>
        {
            aslObj.SendFloatArray(new float[] { 0, GameLiftManager.GetInstance().m_PeerId });
        });

        while (true)
        {
            SendTransform();

            aslObj.SendAndSetClaim(() =>
            {
                aslObj.SendAndSetWorldPosition(transform.position);
            });

            yield return new WaitForSeconds(1 / UPDATES_PER_SECOND);
        }
    }
}
