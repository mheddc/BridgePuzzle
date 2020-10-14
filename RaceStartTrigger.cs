using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Klasse händelt den Trigger, um das Rennen zu starten
public class RaceStartTrigger : MonoBehaviour
{
    private GameObject raceManager;
    private GameStatus gameStatus;
    private UIControl scriptUI;

    // Start is called before the first frame update
    void Start()
    {
        raceManager = GameObject.Find("RaceManager");
        
        gameStatus = GameStatus.GetInstance();

        //Objekt für UI beschaffen.
        GameObject temp = GameObject.Find("UI");
        scriptUI = temp.GetComponent<UIControl>();
    }

    private void OnTriggerEnter(Collider other)
    {
        //Rennen ist nicht aktiv und wurde bisher auch nicht gewonnen
        if (!raceManager.GetComponent<RaceManager>().getIsRaceActive() && !raceManager.GetComponent<RaceManager>().getWasRaceWon())
        {
            raceManager.GetComponent<RaceManager>().setUpRace();
            gameStatus.SetCrystalMax(2);
            scriptUI.SetPuzzleLine("0" + " / " + GameStatus.MAXPUZZLEPIECES);

            //Anzeige einblenden. Wird wieder ausgeblendet, wenn das Rennen gewonnen oder verloren wurde.
            scriptUI.brGViewDisplay.SetActive(true);
            scriptUI.brPViewDisplay.SetActive(true);

            //Anzahl der Brücken wird eingeblendet.
            scriptUI.SetBrigeCatLine("0" + " / " + raceManager.GetComponent<RaceManager>().getRaceTrackCat().getNumberOfRaceTrackBridges() + " / R" + raceManager.GetComponent<RaceManager>().getRoundsCompletedByCat());
            scriptUI.SetBrigePlayerLine("0" + " / " + raceManager.GetComponent<RaceManager>().getRaceTrackPlayer().getNumberOfRaceTrackBridges() + " / R" + raceManager.GetComponent<RaceManager>().getRoundsCompletedByPlayer());
        }
    }
}

