using System.Collections;
using UnityEngine;

/**
 * This class manages the in game behaviour of a card
 * prefab, regardless of its size. It moves, flips and
 * destroys the card when neccessary.
 */

public class CardBehaviour : MonoBehaviour {

	private Vector3 destination_position;
	private int destination_rotation_y_axis;
	private int card_state = StateConstants.STATE_BACK_FACING_IDLE;
	private float delay = StateConstants.NO_FLIPPING_DELAY;
	private float timer = 0;
	private bool can_flip = false;

	public delegate void OnCardFlipped();
	public event OnCardFlipped onCardFlipped;

	private Vector3 destruction_position = new Vector3(PhysicsConstants.X_DELETION_POS, PhysicsConstants.Y_DELETION_POS, 0);

	private void Start()
	{
		GameObject.Find("Scripter").GetComponent<GameController>().OnGameStateChange += SetCanFlip;
	}
	// Update is called once per frame
	void Update()
	{
		
		PerformOnState();
		CheckForFlippingDelay();
	}

	
	/* Updates the card based on its state, controlled by the card_state variable. */
	public void PerformOnState()
	{
		if(card_state == StateConstants.STATE_MOVING)
		{
			MoveToPos();
		}
		else if(card_state == StateConstants.STATE_FRONT_FLIPPING)
		{
			FlipToFront();
		}
		else if(card_state == StateConstants.STATE_BACK_FLIPPING)
		{
			FlipToBack();
		}
		
	}

	/* Sets the state to perform a flip to the opposite direction the card
	 * in currently facing. */
	public void FlipCard()
	{
		if(card_state == StateConstants.STATE_FRONT_FACING_IDLE)
		{
			card_state = StateConstants.STATE_BACK_FLIPPING;
		}
		else if(card_state == StateConstants.STATE_BACK_FACING_IDLE)
		{
			card_state = StateConstants.STATE_FRONT_FLIPPING;
		}
	}

	public void FlipCardWithDelay(float delay)
	{
		this.delay = delay;
	}

	private void CheckForFlippingDelay()
	{
		if (delay != StateConstants.NO_FLIPPING_DELAY)
		{
			timer += Time.deltaTime;
			if (timer >= delay)
			{
				delay = StateConstants.NO_FLIPPING_DELAY;
				timer = 0;
				FlipCard();
			}
		}
	}

	/* Moves the card to the position set in the destination_position variable,
	 * and checks if the card must be destroyed.*/
	private void MoveToPos()
	{
		if(transform.position != destination_position)
		{
			transform.position = Vector3.MoveTowards(transform.position, destination_position, PhysicsConstants.CARD_MOVEMENT_VELOCITY);
		}
		else
		{
			card_state = StateConstants.DEFAULT_CARD_STATE;
		}

		CheckForDestroy();
	}

	/* Destroys the card if it is in the destruction_position */
	private void CheckForDestroy()
	{
		if (transform.position == destruction_position && destination_position == destruction_position)
		{
			Destroy(gameObject);
		}
	}

	/* Method that flips the card to the back and sets the corresponding card_state when done */
	// TODO replace hardcoded values
	private void FlipToBack()
	{
		if(transform.rotation.eulerAngles.y > 180 || transform.rotation.eulerAngles.y == 0)
		{
			Vector3 targetAngles = transform.eulerAngles - 180f * Vector3.up; // what the new angles should be

			transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, targetAngles, PhysicsConstants.CARD_FLIP_VELOCITY * Time.deltaTime);
		}
		else
		{
			transform.rotation = new Quaternion(0, 180, 0, 0);
			card_state = StateConstants.STATE_BACK_FACING_IDLE;
		}
	}

	/* Method that flips the card to the front and sets the corresponding card_state when done */
	// TODO replace hardcoded values
	private void FlipToFront()
	{
		if(transform.rotation.eulerAngles.y > 0 && transform.rotation.eulerAngles.y <= 180)
		{
			Vector3 targetAngles = transform.eulerAngles - 180f * Vector3.up; // what the new angles should be

			transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, targetAngles, PhysicsConstants.CARD_FLIP_VELOCITY * Time.deltaTime);
		}
		else
		{
			transform.rotation = new Quaternion(0, 0, 0, 0);
			card_state = StateConstants.STATE_FRONT_FACING_IDLE;
		}
		
	}

	public int GetCardState()
	{
		return card_state;
	}

	public void SetCardState(int card_state)
	{
		this.card_state = card_state;
	}

	/* Sets the destination point and changes the state to moving so it can be
	 * detected by the PerformState() method */
	public void SetDestinationPosition(Vector3 destination_position)
	{
		this.destination_position = destination_position;
		card_state = StateConstants.STATE_MOVING;
	}

	/* Flips card on click */
	private void OnMouseDown()
	{
		if(can_flip && GameObject.Find("Scripter").GetComponent<CardCommander>().CheckIfCanFlip())
		{
			if (card_state == StateConstants.STATE_BACK_FACING_IDLE || card_state == StateConstants.STATE_FRONT_FACING_IDLE)
			{
				GameObject.Find("Moves").GetComponent<MovesBehaviour>().onMovement();
			}
			FlipCard();
		}
		
	}


	/* Sets the card destination position to the destruction_position */
	public void DeleteCard()
	{
		SetDestinationPosition(destruction_position);
	}

	private void SetCanFlip(bool can_flip)
	{
		this.can_flip = can_flip;
	}


	
}
