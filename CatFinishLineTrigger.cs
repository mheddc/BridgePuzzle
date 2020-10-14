using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Trigger für die Ziellinie der Katze
public class CatFinishLineTrigger : MonoBehaviour
{
	//Referenz auf RaceManager
    GameObject raceManager;

    // Start is called before the first frame update
    void Start()
    {
        raceManager = GameObject.Find("RaceManager");
    }

    private void OnTriggerEnter(Collider other)
    {	
		//Läuft das Rennen und betritt die Katze den Collider, so wird eine Runde hochgezählt. 
        if (other.tag == "Cat" && raceManager.GetComponent<RaceManager>().getIsRaceActive())
        {
			raceManager.GetComponent<RaceManager>().addCatRound();
        }
    }

}
