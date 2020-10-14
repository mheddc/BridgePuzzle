using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MousePipeEnterTrigger : MonoBehaviour
{

    private GameStatus gameStatus;
    public Material newMaterialRef_transparent;

    // Start is called before the first frame update
    void Start()
    {
        //gameStatus - Instance holen
        gameStatus = GameStatus.GetInstance();
    }

    private void OnTriggerStay(Collider other)
    {
        //Handelt es sich um den Spieler? Dann mache die Pipe durchscheinend und verbiete alle Charaktere außer der Maus.
        if (other.tag == "Player")
        {
            //Golem, Wizard, Flying Squirrel verbieten, da nicht genügend Platz in der MousePipe und der Spieler die Pipe betreten hat.
            gameStatus.ForbidCharacter(Character.CharacterEnum.Golem);
            gameStatus.ForbidCharacter(Character.CharacterEnum.Wizard);
            gameStatus.ForbidCharacter(Character.CharacterEnum.FlyingSquirrel);
            transform.parent.gameObject.GetComponent<Renderer>().material = newMaterialRef_transparent;
        }
    }


}
