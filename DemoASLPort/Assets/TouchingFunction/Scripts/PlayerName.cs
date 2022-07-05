using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ASL;

public class PlayerName : MonoBehaviour
{
    [SerializeField] int ASL_id;
    [SerializeField] TextMesh nameText;
    ASLObject aslObj;


    // Start is called before the first frame update
    void Start()
    {
        // StartCoroutine(SetNameCoroutine());
    }

    // IEnumerator SetNameCoroutine()
    // {
    //     while(!transform.parent.TryGetComponent<ASLObject>(out aslObj))
    //     {
    //         yield return null;
    //     }

    //     while(Mathf.Abs(Vector3.Distance(new Vector3(0.3f, 0.3f, 0.3f), transform.parent.localScale)) < float.Epsilon)
    //     {
    //         yield return null;
    //     }

    //     ASL_id = (int) transform.parent.localScale.x * 100;
    //     nameText.text = GameLiftManager.GetInstance().m_Players[ASL_id];

    //     transform.parent.localScale = new Vector3(0.3f, 0.3f, 0.3f);
    // }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.LookRotation(transform.position - Camera.main.transform.position, Vector3.up);
    }
}
