using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class QuitScript : MonoBehaviour
{
    public void Quit() 
    {
<<<<<<< HEAD
=======
        
        FindObjectOfType<PlayerSpawn>().onQuit();
>>>>>>> main
        Application.Quit(); 
    }
}
