using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ASL;

public class RollerCoasterASL : MonoBehaviour
{
/*    public RollerCoasterControl RCControl;
    public ASL_AutonomousObject aslObj;
    private static readonly float UPDATES_PER_SECOND = 60f;
    public bool isLocal = false;
    public bool isDriving = false;

    //Start is called before the first frame update
    void Start()
    {
        //aslObj = GetComponent<ASLObject>();
        //aslObj._LocallySetFloatCallback(Receive);
       //UPDATES_PER_SECOND = 60f;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Receive(string _id, float[] _f)
    {
        i//f (!string.Equals(aslObj.m_Id, _id)) return;  
    } 

    IEnumerator NetworkUpdate()
    {
        while (aslObj == null)
        {
            //Debug.LogError("ASLObject is null");
            //aslObj = GetComponent<ASLObject>();
            yield return null;
        }

        while (true)
        {
            //SendTransform();
            //Debug.LogError("sending RC transform");
            aslObj.SendAndSetClaim(() =>
            {
                aslObj.SendAndSetWorldPosition(transform.position);
            });

            yield return new WaitForSeconds(1 / UPDATES_PER_SECOND);
        }
    }

    public void SendTransform()
    {

    }

    public void StartASL()
    {
        StartCoroutine(NetworkUpdate());
    }*/

/*    public void AttachASLObject()
    {
        //Debug.LogError("Attaching ASLOBJ");
        aslObj = GetComponent<ASLObject>();
    }*/

}
