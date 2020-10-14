using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Klasse verwaltet Collider vor den Mausbrücken. Diese veranlassen die Katze zum springen.
/// </summary>
public class JumpCollider : MonoBehaviour
{
    GameObject cat;

    void Start()
    {
        //Referenz auf Katze
        cat = GameObject.Find("Cat");
    }

    private void OnTriggerEnter(Collider other)
    {
        //Handelt es sich um die Katze?
        if (other.tag == "Cat")
        {
            //Dann Katze spring!
            cat.GetComponent<CatController>().startJump();
        }
    }
}
