using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Klasse kontrolliert die Ziellinie des Spielers.
/// </summary>
public class PlayerFinishLineTrigger : MonoBehaviour
{

    GameObject raceManager;

    // Start is called before the first frame update
    void Start()
    {
        raceManager = GameObject.Find("RaceManager");
    }

    private void OnTriggerEnter(Collider other)
    {
        //Handelt es sich um den Spieler? Dann erhöhe um eine Runde.
        if (other.tag == "Player")
        {
            if (raceManager.GetComponent<RaceManager>().getIsRaceActive() == true)
            {
                raceManager.GetComponent<RaceManager>().addPlayerRound();
            }
        }
    }

}
