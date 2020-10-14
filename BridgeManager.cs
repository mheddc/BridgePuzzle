using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Klasse managed die Brücke. Dabei beinhaltet es Referenzen auf die Lichter, MovePoints und auch die Brückenstruktur an sich.
public class BridgeManager : MonoBehaviour
{

    //MovePoints sind die Laufpunkte, die von der Katze abgelaufen werden. Es handelt sich um Sphere-Gameobjects deren Koordinaten verwendet zur Bewegung der Katze verwendet werden.
    private List<GameObject> positioningPoints=new List<GameObject>();

    //Lichter werden als Liste verwaltet.
    private List<GameObject> blueLights = new List<GameObject>();
    private List<GameObject> orangeLights = new List<GameObject>();

    //Referenz auf die Brückenstruktur, die ein Child-Objekt des Brücken-GameObjects ist.
    private GameObject bridge;

    //Knoten der Graphenstruktur, die durch die Brücke verbunden werden.
    //Werden aus der Benennung des letzten und ersten MovePoints extrahiert.
    private int nodeA;
    private int nodeB;

    //Zeigt an, ob die Brücke bereits von der Katze oder dem Spieler überquert wurden.
    private bool crossedByPlayer = false;
    private bool crossedByCat = false;

    //Referenz auf den RaceManager. Diesem wird eine NAchricht geschickt, falls die Brücke überquert wird.
    private GameObject raceManager;



    // Start is called before the first frame update
    void Start()
    {

        raceManager = GameObject.Find("RaceManager");


        //Alle Child-GameObjects werden zugeordnet: Bridge, Lichter, MovePoints
        foreach (Transform child in transform)
        {

            if (child.gameObject.name.Contains("Bridge"))
            {
                bridge = child.gameObject;
            }
            else if (child.gameObject.name.Contains("BLight"))
            {
                blueLights.Add(child.gameObject);
            }
            else if (child.gameObject.name.Contains("OLight"))
            {
                orangeLights.Add(child.gameObject);
            }
            else if (child.gameObject.name.Contains("Sphere"))
            {
                positioningPoints.Add(child.gameObject);
            }
        }

        //Graphen-Knoten werden aus der Benennung der MovePoints extrahiert.
        nodeA = int.Parse(positioningPoints[0].name.Substring(7, 2));
        nodeB = int.Parse(positioningPoints[positioningPoints.Count - 1].name.Substring(7, 2));

        //MovePoints werden ausgeblendet/inaktiv gemacht.
        makeMovePointsInactive();

    }

    public int getNodeA()
    {
        return nodeA;
    }

    public int getNodeB()
    {
        return nodeB;
    }

    public bool getCrossedByPlayer()
    {
        return crossedByPlayer;
    }

    public bool getCrossedByCat()
    {
        return crossedByCat;
    }


    //Methode, die mit true aufgerufen wird, falls der Spieler die Brücke überquert.
    //Dann wird eine Nachricht an den RaceManager geschickt.
    //Kann auch mit false zurückgesetzt werden.
    public void setCrossedByPlayer(bool crossedByPlayerDummy)
    {
        if (!crossedByPlayer && crossedByPlayerDummy==true)
        {
            crossedByPlayer = true;
            raceManager.GetComponent<RaceManager>().increaseBridgesCrossedByPlayer();
        } else if (crossedByPlayerDummy == false)
        {
            crossedByPlayer = false;
        }
    }

    //Methode, die mit true aufgerufen wird, falls die Katze die Brücke überquert.
    //Dann wird eine Nachricht an den RaceManager geschickt.
    //Kann auch mit false zurückgesetzt werden.
    public void setCrossedByCat(bool crossedByCatDummy)
    {
        if (!crossedByCat && crossedByCatDummy==true)
        {
            crossedByCat = true;
            raceManager.GetComponent<RaceManager>().increaseBridgesCrossedByCat();
        }else if (crossedByCatDummy == false)
        {
            crossedByCat = false;
        }
    }

    //Brücke wird eingelendet.
    public void setBridgeActive()
    {
        bridge.SetActive(true);
    } 

    //Brücke wird ausgeblendet.
    public void setBridgeInactive()
    {
        bridge.SetActive(false);
    }

    //Blendet alle movePoints aus
    private void makeMovePointsInactive()
    {
        foreach (GameObject mp in positioningPoints)
        {
            mp.SetActive(false);
        }
    }

    public void switchBlueLightsOff()
    {
        foreach(GameObject bl in blueLights)
        {
            if (bl.activeSelf)
            {
                bl.GetComponent<LightManager>().SwitchLightOff();
                setCrossedByPlayer(true);
            }
        }
    }

    public void switchOrangeLightsOff()
    {
        foreach (GameObject ol in orangeLights)
        {
            if (ol.activeSelf)
            {
                ol.GetComponent<LightManager>().SwitchLightOff();
                setCrossedByCat(true);
            }
        }
    }

    public void switchBlueLightsOn()
    {
        foreach (GameObject bl in blueLights)
        {
            bl.GetComponent<LightManager>().SwitchLightOn();
            
        }
    }

    public void switchOrangeLightsOn()
    {
        foreach (GameObject ol in orangeLights)
        {
            ol.GetComponent<LightManager>().SwitchLightOn();
        }
    }

    //Methode zum in/-aktivieren aller blauen Lichter, falls Brücke nicht Teil des Spieler-RaceTracks ist.
    public void setBlueLightsActive(bool isActive)
    {
        foreach (GameObject bl in blueLights)
        {
            bl.SetActive(isActive);
        }
    }


    //Methode zum in-/aktivieren aller orangenen Lichter, falls Brücke nicht Teil des Cat-RaceTracks ist.
    public void setOrangeLightsActive(bool isActive)
    {
        foreach (GameObject ol in orangeLights)
        {
            ol.SetActive(isActive);
        }
    }

    public List<GameObject> getPositioningPoints()
    {
        return positioningPoints;
    }

}
