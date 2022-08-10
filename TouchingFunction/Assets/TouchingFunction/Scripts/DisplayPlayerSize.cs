using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using TMPro;

public class DisplayPlayerSize : MonoBehaviour
{
    Player player;
    public TextMeshProUGUI sizeDisplay;

    public Canvas displayCanvas;
    
    // Start is called before the first frame update
    void Start()
    {
        displayCanvas.planeDistance = Camera.main.nearClipPlane + 0.01f;
    }

    public void SetPlayer(Player player)
    {
        this.player = player;
    }

    // Update is called once per frame
    void Update()
    {
        decimal size = Math.Round((decimal) player.GetScale(), 2);
        string sizeText = "Player Size: " + size.ToString();
        sizeDisplay.text = sizeText;
    }
}
