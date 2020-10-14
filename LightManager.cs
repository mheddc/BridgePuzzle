using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Klasse verwaltet ein einzelnes Licht einer Brücke und setzt die Farbe auf Grau oder Blau/Orange
public class LightManager : MonoBehaviour
{
    //Materials werden im Inspektor zugeordnet. 
    public Material newMaterialRef_colored; //Kann Material für Orange oder Blau sein
    public Material newMaterialRef_grey;    //Material mit grauer Farbe.

    public void SwitchLightOff()
    { 
            GetComponent<Renderer>().material = newMaterialRef_grey;
    }

    public void SwitchLightOn()
    {
        GetComponent<Renderer>().material = newMaterialRef_colored;
    }

}
