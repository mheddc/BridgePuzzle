using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Klasse stellt die Rennstrecke dar
public class RaceTrack
{

    //Darstellung der Rennstrecke als Liste von Brücken und Liste von Movepoints
    private List<RaceTrackElement> raceTrackBridges = new List<RaceTrackElement>();
    private List<GameObject> movePoints = new List<GameObject>();

    private float raceLength = 0;
    private int numberOfMovePoints=0;


    //Input:    raceTrackDummy: Liste von Brücken, die in die gegenwärtige Rennstrecke eingefügt werden sollen
    //          position: Position an der die Liste von Brücken eingefügt werden soll.
    public void includeBridgeList(List<RaceTrackElement> raceTrackDummy, int position)
    {
        //Liste von Brücken wird in Rennstrecke eingefügt
        raceTrackBridges.InsertRange(position, raceTrackDummy);

        //Aus der Liste von Brücken wird eine Liste von MovePoints erstellt
        createListOfMovePoints();

        numberOfMovePoints = movePoints.Count;
        raceLength = calcRaceLength();
    }


    //Berechnet die Länge des Race Tracks auf Grundlage der MovePoints
    private float calcRaceLength()
    {
        float length = 0f;
        for (int ii = 0; ii < movePoints.Count-1; ii++)
        {
            length = length + (movePoints[ii + 1].transform.position - movePoints[ii].transform.position).magnitude;
        }
        return length;
    }

    //Methode listet alle Brücken des RaceTracks auf der Konsole auf.
    public void listBridges()
    {
        String bridgeNames="";
        foreach (RaceTrackElement br in raceTrackBridges)
        {
            bridgeNames=bridgeNames + " + " + br.getBridge().name;
        }
        Debug.Log(bridgeNames);
    }


    //Erstellt eine Liste von MovePoints, die abgelaufen werden können.
    private void createListOfMovePoints()
    {
        movePoints.Clear();

        for (int ii=0; ii<raceTrackBridges.Count; ii++)
        {
            //Falls Punkt B der nächste Graphenknoten ist, so können die positioningPoints der Brücke in die movePoints übernommen werden.
            //Falls nicht, so werden die positioningPoints erst umgekehrt, bevor sie den movePoints der Rennstrecke hinzugefügt werden.
            if (raceTrackBridges[ii].isReversed())
            {
                List<GameObject> toAdd = new List<GameObject>();
                toAdd.AddRange(raceTrackBridges[ii].getBridge().getPositioningPoints());
                toAdd.Reverse();
                movePoints.AddRange(toAdd);
            }
            else
            {
                movePoints.AddRange(raceTrackBridges[ii].getBridge().getPositioningPoints());
            }
        }
    }

    //Aktiviert alle Brücken der Rennstrecke.
    public void activateRaceTrack()
    {
        foreach (RaceTrackElement raceTrackElement in raceTrackBridges)
        {
            raceTrackElement.getBridge().setBridgeActive();
        }
    }

    //Aktiviert alle orangenen Lichter der Rennstrecke.
    public void activateOrangeLightsOnRaceTrack()
    {
        foreach (RaceTrackElement raceTrackElement in raceTrackBridges)
        {
            raceTrackElement.getBridge().setOrangeLightsActive(true);
        }
    }

    //Aktiviert alle blauen Lichter der Rennstrecke.
    public void activateBlueLightsOnRaceTrack()
    {
        foreach (RaceTrackElement raceTrackElement in raceTrackBridges)
        {
            raceTrackElement.getBridge().setBlueLightsActive(true);
        }
    }


    //Schaltet alle orangenen Lichter an und setzt CrossedByCat auf false.
    //Wird zu Anfang jeder Runde aufgerufen.
    public void resetCatRaceTrack()
    {
        foreach (RaceTrackElement raceTrackElement in raceTrackBridges)
        {
            raceTrackElement.getBridge().switchOrangeLightsOn();
            raceTrackElement.getBridge().setCrossedByCat(false);
        }
    }


    //Schaltet alle blauen Lichter an und setzt CrossedByCat auf false.
    //Wird zu Anfang jeder Runde aufgerufen.
    public void resetPlayerRaceTrack()
    {
        foreach (RaceTrackElement raceTrackElement in raceTrackBridges)
        {
            raceTrackElement.getBridge().switchBlueLightsOn();
            raceTrackElement.getBridge().setCrossedByPlayer(false);
        }
    }

    public bool checkPlayerCrossedAllBridgesOnRaceTrack()
    {
        foreach (RaceTrackElement raceTrackElement in raceTrackBridges)
        {
            if (!raceTrackElement.getBridge().getCrossedByPlayer())
            {
                return false;
            };

        }
        return true;
    }

    public bool checkCatCrossedAllBridgesOnRaceTrack()
    {
        foreach (RaceTrackElement raceTrackElement in raceTrackBridges)
        {

            if (!raceTrackElement.getBridge().getCrossedByCat())
            {
                return false;
            };

        }
        return true;
    }


    public int getNumberOfRaceTrackBridges()
    {
        return raceTrackBridges.Count;
    }

    public float getRaceLength()
    {
        return raceLength;
    }

    public int getNumberOfMovePoints()
    {
        return numberOfMovePoints;
    }

    //Gibt MovePoint an einer bestimmten Position zurück.
    public GameObject getMovePoint(int position)
    {
        return movePoints[position];
    }

    //Gibt RaceTrackElemet an ii-ter Position zurück.
    public RaceTrackElement GetRaceTrackElement(int ii)
    {
        return raceTrackBridges[ii];
    }

    public void listMovePoints()
    {
        string dummyMovePointList="";
        foreach (GameObject mp in movePoints)
        {
            dummyMovePointList=dummyMovePointList + " + " + mp.name;
        }
        Debug.Log(dummyMovePointList);
    }

}
