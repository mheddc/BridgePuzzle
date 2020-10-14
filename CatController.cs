using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Steuerung der Katze
/// </summary>
public class CatController : MonoBehaviour
{
    private GameStatus gameStatus;
    private Animator animator;
	private CharacterController charController;
	
	//Referenz auf die Rennstrecke
    private RaceTrack raceTrack;
    
    private int currentMovePoint = 0;
    private Vector3 velocity;

	//Geschwindigkeit der Katze
	private float speed=5f;

	//Sprungsteuerung
	private bool jumping = false;
    private float jumpHeight = 1.5f;
    
	//Variablen zur Repositionierung der Katze
    private bool reposition=false;
    private Vector3 newPosition;
    private Vector3 lookAtVector;
	
    // Start is called before the first frame update
    void Start()
    {
       gameStatus = GameStatus.GetInstance();
       charController = gameObject.GetComponent<CharacterController>();
       animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
		//Falls Katze repositioniert wird
        if (reposition)
        {
            reposition = false;
            charController.enabled = false;
            charController.transform.position = newPosition;
            charController.transform.rotation = Quaternion.LookRotation(lookAtVector);
            charController.enabled = true;
        }
        //Wenn das Spiel gerade pausiert wird, einfach zurück kehren.
        if (gameStatus.Paused) return;
        //Falls keine Rennstrecke existiert, die die Katze ablaufen könnte, einfach zurückkehren
        if (raceTrack == null) return;

        //Katze bewegt sich
        MoveCat();
    }

	//Katze wird repositioniert und ausgerichtet.
    public void setPosition(Vector3 vector3pos, Vector3 vector3look)
    {
        newPosition = vector3pos;
        lookAtVector = vector3look;
        reposition = true;
    }
    
	//Katze springt
    public void startJump()
    {
        jumping = true;
    }
	
	//Geschwindikeit der Katze wird definiert
    public void setSpeed(float speed)
    {
        this.speed = speed;
    }

	//Katze wird entlang der Rennstrecke bewegt
    private void MoveCat()
    {
		//Bewegungsvektor wird bestimmt.
        Vector3 moveVector = GetMoveVector();
        
		//Animation
        animator.SetBool("Walking",true);
        animator.speed = speed;
		
        //Rotation der Spielfigur in Bewegungsrichtung; hierfür wird der Bewegungsvektor ohne Anpassung verwendet, damit der Spieler in Richtung der Joystickbewegung schaut
        if (moveVector != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(moveVector);
        }

		//Katze bewegt sich.
        charController.Move(moveVector.normalized * speed * Time.deltaTime);
        
        // Berechnung der Gravitation (velocity.y) die auf den Spieler wirkt
        velocity.y += GetGravity();

		//Springt die Katze, 
        if (jumping)
        {
            //Sprunganimation mit geringerer Geschwindigkeit wird ausgeführt.
			animator.speed = 0.5f;
            animator.SetTrigger("Jumping");
			
            jumping = false;
            //Vertikale Geschwindigkeit wird angepasst.
			velocity.y += Mathf.Sqrt(jumpHeight * -3f * Physics.gravity.y);   
        }

		//Katze wird vertikal bewegt
        charController.Move(velocity*Time.deltaTime);
		
		//Kommt die Katze am Boden auf, so wird die vertikale Geschwindigkeit auf null gesetzt.
        if (IsGrounded() && (velocity.y < 0))
        {
            velocity.y = 0f;
        }
    }

	//Methode bestimmt, ob sich die Katze am Boden befindet.
    private bool IsGrounded()
    {
        //Wenn gerade ein Sprung ausgeführt wurde, dann ist er nicht am Boden.
        if (velocity.y > 0)
        {
            return false;
        }

        //Variable für den eventuellen Treffer.
        RaycastHit hit;
		
        //Nimmt Default Layer und Ignore Raycast in die Layermask. Diese gelten als Untergrund.
        int layermask = (1 << 2)|(1<<0);


        //Schaue nach, ob etwas bis zu einer Länge von 0.05f unter (new Vector3(0,-1,0)) dem Character ist. Der Transform ist am Fuß  der Katze.
        bool meet = Physics.Raycast(transform.position + new Vector3(0,0.05f,0), new Vector3(0, -1, 0), out hit, 1f, layermask);
		
        //War der Abstand zwischen 0 und 0.15f?
        if (meet && hit.distance < 0.15f && hit.distance > 0)
        {
        	return true;
        } else {
            return false;
        }
    }

    protected virtual float GetGravity()
    {
        return Physics.gravity.y * 4f * Time.deltaTime;
    }

	//Katze hört auf, sich zu bewegen
    private void stopWalking()
    {
        animator.SetBool("Walking", false);
        animator.speed = 1;
    }

	//Gibt den Bewegungsvektor zurück. Dieser wird verwendet, um die Bewegungsrichtung der Katze festzulegen.
    private Vector3 GetMoveVector()
    {
		//Current MovePoint wird bestimmt.
        GameObject movePoint = raceTrack.getMovePoint(currentMovePoint);
        
		//Bewegungsvektor geht von der aktuellen Position zum nächsten MovePoint
        Vector3 moveVector = new Vector3(movePoint.transform.position.x - transform.position.x,0, movePoint.transform.position.z - transform.position.z);
        
		//Ist der Betrag des Bewegungsvektors größer als die Strecke, um die sich die Katze bewegen würde, so wird der Bewegungsvektor gekürzt 
		//und der aktuelle MovePoint auf den nächsten gesetzt. 
        if (moveVector.magnitude < (speed * Time.deltaTime) && (currentMovePoint)!=raceTrack.getNumberOfMovePoints()-1)
        {
            float temp = moveVector.magnitude / (speed * Time.deltaTime);
            moveVector = moveVector * temp;
            currentMovePoint++;
        }

        return moveVector;
    }

	//Rennstrecke, die von der Katze abgelaufen wird, wird zugewiesen
    public void assignRaceTrack(RaceTrack rt)
    {
		this.raceTrack = rt;
		//aktueller MovePoint ist 0
        currentMovePoint = 0; 
    }

	//Rennstrecke wird zurückgesetzt
    public void resetRaceTrack()
    {
        currentMovePoint = 0;
        this.raceTrack = null;
        stopWalking();
    }

	//Setzt den aktuellen MovePoint zurück.
    public void resetCurrentMovePoint()
    {
        currentMovePoint = 0;
    }
}
