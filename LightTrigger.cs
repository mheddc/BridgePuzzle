using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Klasse verwaltet den Lichtkollider, der sich auf jeder Brücke befindet.
/// </summary>
public class LightTrigger : MonoBehaviour
{
    private GameObject raceManager;

    void Start()
    {
        //Find reference to raceManager
        raceManager = GameObject.Find("RaceManager");
    }


    private void OnTriggerEnter(Collider other)
    {
        //Handelt es sich um den Spieler? Dann schalte das blaue Licht aus.
        if (other.tag == "Player")
        {
            if (raceManager.GetComponent<RaceManager>().getIsRaceActive() == true)
            {
                transform.parent.gameObject.GetComponent<BridgeManager>().switchBlueLightsOff();
            }

        }
        //Handelt es sich um die Katze? Dann schalte das orangene Licht aus.
        else if (other.tag == "Cat")
        {
            if (raceManager.GetComponent<RaceManager>().getIsRaceActive() == true)
            {
                transform.parent.gameObject.GetComponent<BridgeManager>().switchOrangeLightsOff();
            }
         
        }
    }
}
