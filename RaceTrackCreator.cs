using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Berechnet eine Rennstrecke
public class RaceTrackCreator
{
    private BridgeGroupManager bridgeGroupManager;
    private List<AdjacencyListElement>[] adjacencyList;
    private GameObject[] raceTrack;

    private float raceTrackLengthMin;
    private float raceTrackLengthMax;
    private int startNode; //Rennstrecke wird von diesem Knoten ausgehend berechnet
    private int numBridgesMin;
    private int numBridgesMax;


    public RaceTrackCreator(BridgeGroupManager bridgeGroupManager, float raceTrackLengthMin, float raceTrackLengthMax, int numBridgesMin, int numBridgesMax)
    {
        this.bridgeGroupManager = bridgeGroupManager;

        //Randbedingungen der Rennstrecke
        this.raceTrackLengthMin=raceTrackLengthMin;
        this.raceTrackLengthMax = raceTrackLengthMax;

        this.numBridgesMin = numBridgesMin;
        this.numBridgesMax = numBridgesMax;
    }


    //Bestimmt eine Rennstrecke und gibt diese zurück.
    //Wird eine Rennstrecke bestimmt, die nicht den Randbedingungen entspricht, wird null zurückgegeben.
    public RaceTrack calcRaceTrack(int startNode)
    {
        //Start-Graphenknoten der Rennstrecke
        this.startNode = startNode;

        List<RaceTrackElement> raceTrackDummy = new List<RaceTrackElement>();
        RaceTrack raceTrack = new RaceTrack();

        //Liste aller Brücken
        BridgeManager[] bridges = bridgeGroupManager.getBridges();
    
        //Erzeugung der Adjazenzliste        
        createAdjacencyList(bridges);

        //Basis-Eulerkreis wird bestimmt und in die Rennstrecke eingefügt.
        calcCircle(startNode, startNode, raceTrackDummy);
        raceTrackDummy.Reverse();
        raceTrack.includeBridgeList(raceTrackDummy,0);

        //Falls die Rennstrecke noch nicht die Mndestlänge oder Mindestanzahl von Brücken hat, werden weitere Eulerkreise hinzugefügt
        if (raceTrack.getRaceLength() < raceTrackLengthMin || raceTrack.getNumberOfRaceTrackBridges() < numBridgesMin)
        {
            //Offset, damit der Unterkreis nicht immer beim erstmöglichen Graphenknoten beginnt
            int randomBridgeOffset = Random.Range(0, raceTrack.getNumberOfRaceTrackBridges()-1);
            
            //Gehe durch alle Brücken der Basis-Rennstrecke mit aufaddiertem Offset durch. 
            for (int ii = 0; ii < raceTrack.getNumberOfRaceTrackBridges(); ii++)
            {
                RaceTrackElement raceTrackElement = raceTrack.GetRaceTrackElement((randomBridgeOffset + ii) % (raceTrack.getNumberOfRaceTrackBridges()));
                if (raceTrackElement.isReversed())
                {
                    startNode = raceTrackElement.getBridge().getNodeA();
                } else
                {
                    startNode = raceTrackElement.getBridge().getNodeB();
                }

                raceTrackDummy = new List<RaceTrackElement>();

                //Kann ein Unterkreis bestimmt werden, so bricht die for-Schleife ab.
                if (calcCircle(startNode, startNode, raceTrackDummy))
                {
                    raceTrackDummy.Reverse();
                    raceTrack.includeBridgeList(raceTrackDummy, (randomBridgeOffset + ii +1) % raceTrack.getNumberOfRaceTrackBridges() );
                    break;
                }
            }
        }

        //Entspricht die Rennstrecke allen Randbedingungen?
        if (raceTrack.getRaceLength() > raceTrackLengthMin && raceTrack.getRaceLength() < raceTrackLengthMax && raceTrack.getNumberOfRaceTrackBridges() >= numBridgesMin && raceTrack.getNumberOfRaceTrackBridges()<=numBridgesMax)
        {
            return raceTrack;
        } else
        {
            return null;
        }

    }


    //Methode wird rekursiv aufgerufen. Tiefensuche nach einem Eulerkreis.
    //An jedem Graphenknoten wird zufällig eine Brücke ausgewählt und für den 
    //entsprechenden Folge-Graphenknoten die Methode erneut aufgerufen.
    //Output:   True: Falls Startknoten wieder erreicht wurde oder einer der Unteraufrufe den Startknoten erreicht hat
    //          False: Falls keiner der Unteraufrufe den Startknoten erreicht hat. 
    private bool calcCircle(int startNode, int node, List<RaceTrackElement> raceTrack)
    {
        //Zufälliger Offset, damit nicht immer die erste ausgehende Brücke ausgewählt wird. 
        int bridgeNum = Random.Range(0, adjacencyList[node].Count);

        //Es wird durch alle Brücken des Graphenknotens durchgegangen.
        //Dabei wird der zufällige Offset jeweils aufaddiert.
        for (int ii=0; ii < adjacencyList[node].Count; ii++)
        {
            int element = (ii + bridgeNum) % adjacencyList[node].Count;

            //Falls Brücke bereits Teil der Tiefensuche => versuche es mit der nächsten Brücke
            if (adjacencyList[node][element].isPartOfRaceTrack() == true)
            {
                continue;
            }
            else
            {
                adjacencyList[node][element].setPartOfRaceTrack(true);


                int jj = adjacencyList[node][element].getBridge().getNodeA();
                int kk = adjacencyList[node][element].getBridge().getNodeB();
                int nextNode;

                bool reversed;
                if (jj != node)
                {
                    nextNode = jj;
                    reversed = true;
                }
                else
                {
                    nextNode = kk;
                    reversed = false;
                }

                //Falls der nächste Knoten der Startknoten ist und sich der Kreis damit schließt oder falls ein rekursiver Unteraufruf erfolgreich war.
                if (nextNode == startNode || calcCircle(startNode, nextNode, raceTrack))
                {
                    //Brücke und Durchgangsrichtung werden dem RaceTrack hinzugefügt
                    raceTrack.Add(new RaceTrackElement(adjacencyList[node][element].getBridge(), reversed));
                    return true;
                }
                else
                {
                    //Falls keine Unteraufrufe der Brücke erfolgreich sind
                    adjacencyList[node][element].setPartOfRaceTrack(false);
                }
            }
        }

        //Falls keine Brücke von diesem Graphenknoten zum Startknoten führen
        return false;
    }

    //Input: Liste von Brücken, typischerweise einer Rennstrecke
    //Methode erzeugt einen Array mit Listen. Die Größe des Arrays entspricht der Anzahl aller Graphenknoten.
    //Die Listen enthalten die Brücken, die vom jeweiligen Graphenknoten abgehen.
    private void createAdjacencyList(BridgeManager[] bridges)
    {
        //Adjazenzliste wird initialisiert.
        adjacencyList = new List<AdjacencyListElement>[BridgeGroupManager.NUMBEROFNODES];

        //Erzeugt leere Listen für jeden Graphenknoten
        for (int ii = 0; ii < BridgeGroupManager.NUMBEROFNODES; ii++)
        {
            adjacencyList[ii] = new List<AdjacencyListElement>();
        }

        //Fügt alle Brücken den Listen der Graphenknoten zu.
        foreach (BridgeManager br in bridges)
        {
            int jj = br.GetComponent<BridgeManager>().getNodeA();
            int kk = br.GetComponent<BridgeManager>().getNodeB();
            AdjacencyListElement ale = new AdjacencyListElement(br, false);
            adjacencyList[jj].Add(ale);
            adjacencyList[kk].Add(ale);
        }

    }

}
