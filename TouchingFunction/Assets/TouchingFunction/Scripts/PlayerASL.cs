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
    public Transform playerBody;
    public Transform playerLeftArm;
    public Transform playerRightArm;
    public Transform playerLeftShoulder;
    public Transform playerRightShoulder;

    public void SetLocal()
    {
        isLocal = true;
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
        // aslObj._LocallySetPostDownloadFunction(DownloadAvatar);
    }

    // static public void DownloadAvatar(GameObject g, Texture2D texture)
    // {
    //     g.transform.Find("body").Find("head").GetComponent<MeshRenderer>().material.mainTexture = texture;
    // }

    public void SendTransform()
    {
        var f = new float[6];
        f[0] = 1; // Identifier
        f[1] = player.IsFlappingEnabled() ? 1 : -1;
        f[2] = player.IsWalkingEnabled() ? 1 : -1;
        f[3] = playerHead.transform.localRotation.eulerAngles.x;
        f[4] = transform.localRotation.eulerAngles.y;
        f[5] = transform.localScale.x;

        aslObj.SendAndSetClaim(() =>
        {
            aslObj.SendFloatArray(f);
        });
    }

    public void SendColor(Color color)
    {
        aslObj.SendAndSetClaim(() =>
        {
            aslObj.SendFloatArray(new float[] { 3, color.r, color.g, color.b, color.a });
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

                GetComponent<PlayerAnimation>().forceFlappingAnimation = _f[1] > 0;
                GetComponent<PlayerAnimation>().forceWalkingAnimation = _f[2] > 0;
                playerHead.transform.localRotation = Quaternion.Euler(_f[3], 0, 0);
                transform.localRotation = Quaternion.Euler(0, _f[4], 0);
                transform.localScale = new Vector3(_f[5], _f[5], _f[5]);
                break;

            case 3: // Set color
                Color color = new Color(_f[1], _f[2], _f[3], _f[4]);
                playerBody.GetComponent<MeshRenderer>().material.color = color;
                playerLeftArm.GetComponent<MeshRenderer>().material.color = color;
                playerRightArm.GetComponent<MeshRenderer>().material.color = color;
                playerRightShoulder.GetComponent<MeshRenderer>().material.color = color;
                playerLeftShoulder.GetComponent<MeshRenderer>().material.color = color;
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
