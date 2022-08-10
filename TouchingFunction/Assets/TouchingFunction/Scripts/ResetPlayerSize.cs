using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResetPlayerSize : MonoBehaviour
{
    Player player;
    public Button resetButton;
    
    // Start is called before the first frame update
    void Start()
    {
        resetButton.onClick.AddListener(ResetSizeOnClickHandler);
    }

    public void SetPlayer(Player player)
    {
        this.player = player;
    }

    private void ResetSizeOnClickHandler()
    {
        Debug.Log("Reset Click");
        player.ResetScale();
    }
    
}