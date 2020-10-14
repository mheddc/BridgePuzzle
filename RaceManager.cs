using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Klasse verwaltet das Rennen.
public class RaceManager : MonoBehaviour
{
    //Zeigt an ob Rennen abgeschlossen ist und Puzzelteil damit gewonnen wurde.
    private bool wasRaceWon = false;

    //Zeigt an, ob das Rennen gerade läuft
    private bool isRaceActive = false;

    //Bool zur Steuerung des Pausierens des Spiels
    private bool gamePaused = false;
	
    //Referenz auf Brückenmanager
    private BridgeGroupManager bridgeGroupManager;

    //Referenzen für Rennstrecken
    private RaceTrack raceTracktPlayer;
    private RaceTrack raceTrackCat;

    //Referenzen für AudioManager
    private AudioManager audioManager;

    //Referenz auf GameStatus
    private GameStatus gameStatus;

    //Referenzen auf Puzzelteil
    private GameObject puzzlePiece;
    //Referenz auf UI
    private UIControl scriptUI;

    //Deklaration auf Spieler und Katze
    private GameObject player;
    private GameObject cat;

    //Rundenzähler
    private int roundsCompletedByCat = 0;
    private int roundsCompletedByPlayer = 0;
    private static int NUMOFTOTALROUNDS = 3;

    //Zähler für überquerte Brücken für Katze und Spieler
    private int bridgesCrossedByCat = 0;
    private int bridgesCrossedByPlayer = 0;
	
    //Positionierung und Ausrichtung von Spieler und Katze
    private Vector3 startingPointCat = new Vector3(14.5f, 1.7f, 24.1f);
    private Vector3 startOrientationCat = new Vector3(0, 0, -1);

    private Vector3 startingPointPlayer = new Vector3(9.8f, 1.7f, 16.4f);
    private Vector3 startOrientationPlayer = new Vector3(0, 0, -1);

    //Konstanten für die Startgraphenknoten
    private const int STARTNODEPLAYER = 4;
    private const int STARTNODECAT = 1;

    //Konstanten für die Bestimmung der Geschwindigkeit der Katze
    private const float MAXSPEEDWIZARD = 8f;
    private const float CATSPEEDRATIO = 0.55f;

    // Start is called before the first frame update
    void Start()
    {
        //Findet das Puzzleteil und inaktiviert es.
        puzzlePiece = GameObject.Find("Puzzle Piece 4");
        if (puzzlePiece != null) puzzlePiece.SetActive(false);

        //Objekt für UI beschaffen.
        GameObject temp = GameObject.Find("UI");
        scriptUI = temp.GetComponent<UIControl>();

        //Instanz des GameStatus beschaffen
        gameStatus = GameStatus.GetInstance();

        //Instanz des AudioManagers beschaffen
        audioManager = AudioManager.GetInstance();

        //Referenz auf BridgeGroupManager
        bridgeGroupManager = new BridgeGroupManager();
    }


    // Update is called once per frame
    void Update()
    {
		//Das Spiel pausiert.
        if (gamePaused && gameStatus.Paused==false)
        {
            gameStatus.Paused = true;
        }
		
		//Spiel geht weiter, falls der Spieler B drückt.
        if (Input.GetButton("Action") && gamePaused == true)
        {
            gamePaused = false;
            gameStatus.Paused = false;
        }
    }

	//Methode wird bei Überquerung einer Brücke durch die Katze aufgerufen
    public void increaseBridgesCrossedByCat()
    {
        bridgesCrossedByCat++;
		//Anzeige wird aktualisiert
        scriptUI.SetBrigeCatLine(bridgesCrossedByCat + " / " + raceTrackCat.getNumberOfRaceTrackBridges() + " / R" + roundsCompletedByCat);
    }

	//Methode wird bei Überquerung einer Brücke durch den Player aufgerufen
    public void increaseBridgesCrossedByPlayer()
    {
        bridgesCrossedByPlayer++;
		//Anzeige wird aktualisiert
        scriptUI.SetBrigePlayerLine(bridgesCrossedByPlayer + " / " + raceTracktPlayer.getNumberOfRaceTrackBridges() + " / R" + roundsCompletedByPlayer);
    }

    public void resetRace()
    {
        //Rennen wird gestoppt
        isRaceActive = false;

        //Brückenanzeigen im User-Interface werden deaktiviert.
        scriptUI.brGViewDisplay.SetActive(false);
        scriptUI.brPViewDisplay.SetActive(false);

        //Anzahl der überquerten Brücken wird zurückgesetzt
        bridgesCrossedByCat = 0;
        bridgesCrossedByPlayer = 0;

        //Katze Bisha wird an den Startpunkt zurückgesetzt
        cat.GetComponent<CatController>().setPosition(startingPointCat, startOrientationCat);

        //Referenz auf Renstrecke wird zurückgesetzt. Katze szoppt damit und kehrt in die Idle-Animation zurück.
        cat.GetComponent<CatController>().resetRaceTrack();
    }

	//Methode wird bei Abschluss einer Katzenrunde aufgerufen. Geschieht durch den Trigger am Startmast.
    public void addCatRound()
    {
		//Wird ausgeführt, falls das Rennen noch nicht zu Ende ist und alle Brücken überquert wurden.
        if (roundsCompletedByCat < NUMOFTOTALROUNDS - 1 && raceTrackCat.checkCatCrossedAllBridgesOnRaceTrack() && isRaceActive)
        {
			//Rundenanzahl wird hochgesetzt
            roundsCompletedByCat++;
			//Brückenanzahl und MovePoints werden zurückgesetzt.
            bridgesCrossedByCat = 0;
            cat.GetComponent<CatController>().resetCurrentMovePoint();
			//Lichter werden angestellt.
            raceTrackCat.resetCatRaceTrack();
			//Anzeige wird aktualisiert.
            scriptUI.SetBrigeCatLine(bridgesCrossedByCat + " / " + raceTrackCat.getNumberOfRaceTrackBridges() + " / R" + roundsCompletedByCat);

        }
		//Ausgeführt bei Abschluss des Rennens. Die Katze gewinnt!
        else if (roundsCompletedByCat == NUMOFTOTALROUNDS - 1 && raceTrackCat.checkCatCrossedAllBridgesOnRaceTrack() && isRaceActive)
        {
			//Rennen wird zurückgesetzt.
            resetRace();
            scriptUI.SetLine("\"Too bad. You have lost. Do you want to try again? Then go back to the ramps in front of my home.\" Press B to continue.", UIControl.Line.Dialogue, 5);
            audioManager.Play("LostBridgePuzzle");
            gamePaused = true;
        }
    }

	//Methode wird bei Abschluss einer Spieler-Runde aufgerufen. Geschieht durch den Trigger am Startmast.
    public void addPlayerRound()
    {
		//Wird ausgeführt, falls das Rennen noch nicht zu Ende ist und alle Brücken überquert wurden.
        if (roundsCompletedByPlayer < NUMOFTOTALROUNDS - 1 && raceTracktPlayer.checkPlayerCrossedAllBridgesOnRaceTrack() && isRaceActive)
        {
			//Rundenanzahl wird hochgesetzt
            roundsCompletedByPlayer++;
			//Brückenanzahl und MovePoints werden zurückgesetzt.
            bridgesCrossedByPlayer = 0;
			//Lichter werden angestellt.
            raceTracktPlayer.resetPlayerRaceTrack();
			//Anzeige wird aktualisiert.
            scriptUI.SetBrigePlayerLine(bridgesCrossedByPlayer + " / " + raceTracktPlayer.getNumberOfRaceTrackBridges() + " / R" + roundsCompletedByPlayer);
        }
		//Ausgeführt bei Abschluss des Rennens. Der Spieler gewinnt!
        else if (roundsCompletedByPlayer == NUMOFTOTALROUNDS - 1 && raceTracktPlayer.checkPlayerCrossedAllBridgesOnRaceTrack() && isRaceActive)
        {
            //Rennen wird zurückgesetzt.
			resetRace();
			scriptUI.SetLine("\"Well done. You are fast. Here I give you my puzzle piece. Good bye.\" Press B to continue.", UIControl.Line.Dialogue, 5);
            audioManager.Play("WonBridgePuzzle");
			
			//Rennen wird gewonnen und kann damit nicht mehr wiederholt werden.
			wasRaceWon = true;
            
			if (puzzlePiece != null) puzzlePiece.SetActive(true);
            gamePaused = true;
        }
    }

    public void setUpRace()
    {
        //Anzahl der Runden wird auf 0 gesetzt.
        roundsCompletedByCat = 0;
        roundsCompletedByPlayer = 0;

        //Betimmung der Rennstrecken
        raceTracktPlayer = new RaceTrack();
        raceTrackCat = new RaceTrack();

        RaceTrackCreator rbuildPlayer = new RaceTrackCreator(bridgeGroupManager, 80, 260, 12, 15);

        do
        {
            //Entspricht die Rennstrecke nicht den geforderten Randbedingungen, so wird null zurückgegeben.
            raceTracktPlayer = rbuildPlayer.calcRaceTrack(STARTNODEPLAYER);
        } while (raceTracktPlayer == null);

        RaceTrackCreator rbuildCat = new RaceTrackCreator(bridgeGroupManager, raceTracktPlayer.getRaceLength() * 0.85f, 260, 8, 14);

        do
        {
            //Entspricht die Rennstrecke nicht den geforderten Randbedingungen, so wird null zurückgegeben.
            raceTrackCat = rbuildCat.calcRaceTrack(STARTNODECAT);
        } while (raceTrackCat == null);

        //Referenz auf Spieler
        player = GameObject.FindWithTag("Player");
        //Spieler wird an Startposition gesetzt.
        player.GetComponent<PlayerController>().setPosition(startingPointPlayer, startOrientationPlayer);

        //Alle Brücken und Lichter werden aktiviert
        bridgeGroupManager.deactivateBridgesAndLights();

        //Rennstrecke der Katze wird aktiviert
        raceTrackCat.activateRaceTrack();
        raceTrackCat.activateOrangeLightsOnRaceTrack();
        raceTrackCat.resetCatRaceTrack();

        //Rennstrecke des Spielers wird aktiviert
        raceTracktPlayer.activateRaceTrack();
        raceTracktPlayer.activateBlueLightsOnRaceTrack();
        raceTracktPlayer.resetPlayerRaceTrack();

        //Referenz auf die Katze
        cat = GameObject.Find("Cat");

        //Die Rennstrecke wird der Katze zugewiesen.
        cat.GetComponent<CatController>().assignRaceTrack(raceTrackCat);
        //Geschwindigkeit der Katze wird in Abhängigkeit der Länge der Rennstrecke des Spielers festgesetzt.
        cat.GetComponent<CatController>().setSpeed((raceTrackCat.getRaceLength() / raceTracktPlayer.getRaceLength()) * MAXSPEEDWIZARD * CATSPEEDRATIO);

        //Kamera wird so gesetzt, dass Spieler und Katze zu sehen sind.
        Camera.main.gameObject.GetComponent<CameraController>().repositionCam(33, 30);

        //Text wird angezeigt.
        scriptUI.SetLine("\"Hello stranger. My name is Bisha the cat. I know that you are looking for puzzle pieces. I will give you one if you beat me in a race around my home. Cross all blue bridges and come back to the blue pole to complete one round. There will be three rounds. I will select the race track bridges so that you have a fair chance. If you want to give up, use the horn. Ready?\" Then press B to start.", UIControl.Line.Dialogue, 5);
        
        //Game is paused.
        gamePaused = true;

        //Rennen startet
        isRaceActive = true;
    }


    public bool getIsRaceActive()
    {
        return isRaceActive;
    }

    public bool getWasRaceWon()
    {
        return wasRaceWon;
    }

    public RaceTrack getRaceTrackPlayer()
    {
        return raceTracktPlayer;
    }

    public RaceTrack getRaceTrackCat()
    {
        return raceTrackCat;
    }

    public int getRoundsCompletedByCat()
    {
        return roundsCompletedByCat;
    }

    public int getRoundsCompletedByPlayer()
    {
        return roundsCompletedByPlayer;
    }
}