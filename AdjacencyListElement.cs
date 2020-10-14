using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Element der Adjacency-List. Speichert eine Brücke und ob diese Brücke Teil eines RaceTracks ist.
public class AdjacencyListElement
{
    private BridgeManager bridge;
    //Gehört die Brücke zum momentanen RaceTrack?
    private bool partOfRaceTrack;

    public AdjacencyListElement(BridgeManager bridge, bool partOfRaceTrack)
    {
        this.bridge = bridge;
        this.partOfRaceTrack = partOfRaceTrack;
    }

    public bool isPartOfRaceTrack()
    {
        return partOfRaceTrack;
    }

    public void setPartOfRaceTrack(bool partOfRaceTrack)
    {
        this.partOfRaceTrack = partOfRaceTrack;
    }

    public BridgeManager getBridge()
    {
        return bridge;
    }
}
