using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ASL;

public class PlayerASL : MonoBehaviour
{
    [SerializeField] int ASL_id;
    [SerializeField] TextMesh nameText;
    ASLObject aslObj;
    public Player player;

    public Transform playerBody;
    public Transform playerLeftArm;
    public Transform playerRightArm;


    // Start is called before the first frame update
    void Start()
    {
        aslObj = GetComponent<ASLObject>();
        aslObj._LocallySetFloatCallback(Receive);
        aslObj._LocallySetPostDownloadFunction(DownloadAvatar);
    }

    static public void DownloadAvatar(GameObject g, Texture2D texture)
    {
        g.transform.Find("body").Find("head").GetComponent<MeshRenderer>().material.mainTexture = texture;
    }

    public void SendArmBools()
    {
        aslObj.SendAndSetClaim(() =>
        {
            aslObj.SendFloatArray(new float[] {1, player.IsFlappingEnabled() ? 1 : -1, player.IsWalkingEnabled() ? 1 : -1});
        });
    }

    public void SendHeadRotation()
    {
        aslObj.SendAndSetClaim(() =>
        {
            aslObj.SendFloatArray(new float[] {2, player.head.transform.localRotation.eulerAngles.z});
        });
    }

    public void SendColor(Color color)
    {
        aslObj.SendAndSetClaim(() =>
        {
            aslObj.SendFloatArray(new float[] {3, color.r, color.g, color.b, color.a });
        });

    }

    public void SendScale()
    {
        aslObj.SendAndSetClaim(() =>
        {
            aslObj.SendAndSetLocalScale(aslObj.transform.localScale);
        });
    }

    public void Receive(string _id, float[] _f)
    {
        if(!string.Equals(aslObj.m_Id, _id)) return; // Not for this player, disregard

        switch(_f[0])
        {
            case 0: // Set ASL id / name
                ASL_id = (int)_f[1];
                nameText.text = GameLiftManager.GetInstance().m_Players[ASL_id];
                break;
            case 1: // Set arm movement bool
                // player.armFlappingUp = _f[1] > 0;
                // player.armMovingForward = _f[2] > 0;
                break;
            case 2: // Set head rotation
                player.head.transform.localRotation = Quaternion.Euler(
                    player.head.transform.localRotation.eulerAngles.x,
                    player.head.transform.localRotation.eulerAngles.y,
                    _f[1]
                );
                break;
            case 3: // Set color
                Color color = new Color(_f[1], _f[2], _f[3], _f[4]);
                playerBody.GetComponent<MeshRenderer>().material.color = color;
                playerLeftArm.GetComponent<MeshRenderer>().material.color = color;
                playerRightArm.GetComponent<MeshRenderer>().material.color = color;
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        nameText.transform.rotation = Quaternion.LookRotation(nameText.transform.position - Camera.main.transform.position, Vector3.up);
    }
}
