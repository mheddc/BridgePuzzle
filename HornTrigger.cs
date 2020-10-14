using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HornTrigger : MonoBehaviour
{
    private GameStatus gameStatus;
    private AudioManager audioManager;
    private UIControl scriptUI;

    private GameObject raceManager;

    // Start is called before the first frame update
    void Start()
    {
        //Referenzen
        gameStatus = GameStatus.GetInstance();
        audioManager = AudioManager.GetInstance();
        //Objekt des UI beschaffen.
        GameObject temp = GameObject.Find("UI");
        scriptUI = temp.GetComponent<UIControl>();

        //Referenz auf den RaceManager
        raceManager = GameObject.Find("RaceManager");
    }

    //Bei Verlassen des Colliders wird die ActionLine zurückgesetzt
    private void OnTriggerExit(Collider other)
    {
            scriptUI.SetLine("", UIControl.Line.Action, 0);
    }

    //Befinde sich der Zauberer!! im Collider wird der Reset-Text angezeigt.
    private void OnTriggerStay(Collider other)
    {
        //ActionLine anzeigen, wenn es der Zauberer ist und Rennen läuft.
        if (gameStatus.GetCurrentCharacter() == Character.CharacterEnum.Wizard && raceManager.GetComponent<RaceManager>().getIsRaceActive() == true && other.gameObject.tag == "Player")
        {
            scriptUI.SetLine("Press B to reset the race.", UIControl.Line.Action, 0.1f);
            //Ja! Drückt er die Actionstaste? Dann gibt er auf
            if (Input.GetButtonDown("Action"))
            {
                //Neuer Text wird gesetzt.
                scriptUI.SetLine("You gave up! Go back to the ramps to restart the race.", UIControl.Line.Info, 3);
                //Rennen wird zurückgesetzt.
                raceManager.GetComponent<RaceManager>().resetRace();
                audioManager.Play("HornBridgePuzzle");
            }
        }
    }
}
