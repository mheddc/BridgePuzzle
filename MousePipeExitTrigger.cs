using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Klasse kontrolliert den ExitTrigger beim Verlassen der MousePipes.
/// </summary>
public class MousePipeExitTrigger : MonoBehaviour
{
    private GameStatus gameStatus;
    public Material newMaterialRef_opaque;

    // Start is called before the first frame update
    void Start()
    {
        //gameStatus - Instance holen
        gameStatus = GameStatus.GetInstance();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            //Golem, Wizard, Flying Squirrel erlauben, da der Spieler die Pipe verlassen hat.
            gameStatus.RemoveForbiddenCharacter(Character.CharacterEnum.Golem);
            gameStatus.RemoveForbiddenCharacter(Character.CharacterEnum.Wizard);
            gameStatus.RemoveForbiddenCharacter(Character.CharacterEnum.FlyingSquirrel);
            //Material wird wieder opaque.
            transform.parent.gameObject.GetComponent<Renderer>().material = newMaterialRef_opaque;
        }
    }


}
