using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Klasse verwaltet alle Brücken des Brückenrätsels. Es fokussiert also nicht nur auf die Brücken einer Rennstrecke.
public class BridgeGroupManager
{
    //Gesamtanzahl aller Grapehnknoten
    public const int NUMBEROFNODES = 13;

    //Liste aller Brücken des Brückenrätsels
    private BridgeManager[] bridges;

    public BridgeGroupManager()
    {
        //Alle Brücken werden dem bridges-Array hinzugefügt
        if (bridges == null)
        {
            GameObject[] GameObjectBridges = GameObject.FindGameObjectsWithTag("BridgeContainer");
            bridges = new BridgeManager[GameObjectBridges.Length];

            for (int ii = 0; ii < GameObjectBridges.Length; ii++)
            {
                bridges[ii] = GameObjectBridges[ii].GetComponent<BridgeManager>();
            }
        }
    }


    //Getter für das Array bridges
    public BridgeManager[] getBridges()
    {
        return bridges;
    }


    //Deaktiviert alle Brücken und Lichter
    public void deactivateBridgesAndLights()
    {
        foreach (BridgeManager br in bridges)
        {
            br.GetComponent<BridgeManager>().setBridgeInactive();
            br.GetComponent<BridgeManager>().setOrangeLightsActive(false);
            br.GetComponent<BridgeManager>().setBlueLightsActive(false);
        }
    }

}
