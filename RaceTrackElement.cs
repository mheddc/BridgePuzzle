using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Element der RaceTrackbridges-Liste.
public class RaceTrackElement
{

    private BridgeManager bridge;
    //Bool-Wert der angibt, ob die Laufpunkte der Brücke in normaler oder umgekehrter Reihenfolge durchlaufen werden.
    private bool reversedDirection = false;

    public RaceTrackElement(BridgeManager bridge, bool reversedDirection)
    {
        this.bridge = bridge;
        this.reversedDirection = reversedDirection;
    }

    public bool isReversed()
    {
        return reversedDirection;
    }

    public BridgeManager getBridge()
    {
        return bridge;
    }
}
