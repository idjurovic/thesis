using UnityEngine;
using System.Collections;

public class card : MonoBehaviour {

	//decide if this card will be auto-setup or if suit, ranck, face ordientation and card type will be set manually
	public enum my_setup
	{
		automatic,
		manual
	}
	public my_setup my_setup_selected = my_setup.automatic;
	
	public enum card_type
	{
		normal,
		gold //this if for the win_condition_selected = collect_all_gold_cards into deck script
		
	}
	public card_type card_type_selected = card_type.normal;

	public enum card_suit
	{
		random = -1,
		clubs = 0,
		spades = 1,
		diamonds = 2,
		hearts = 3
	}
	public card_suit card_suit_selected = card_suit.random;//decide if this card will have a random suit at each game start/restart or always the same
	

	public enum card_rank
	{
		random = -1,
		_A = 0,
		_2 = 1,
		_3 = 2,
		_4 = 3,
		_5 = 4,
		_6 = 5,
		_7 = 6,
		_8 = 7,
		_9 = 8,
		_10 = 9,
		_J = 10,
		_Q = 11,
		_K = 12
	}
	public card_rank card_rank_selected = card_rank.random;//decide if this card will have a random rank at each game start/restart or always the same

	//the final rank and suit of this card
	public int my_suit;
	public int my_rank;

	//card appearance, it will be auto-setup from deck script
	public SpriteRenderer my_face;
	public SpriteRenderer my_back;

	public bool face_up;//the current face orientation
	public bool always_face_up;//if true, this card will be always face up, otherwise will be face up only if this card not have any card over itself
	public bool this_is_a_bottom_card;//this card not have any other card under itself (deck script use this for call bottom_card_score when this card is pick)

	public Transform[] cards_over_me;//only it is none, this card can be click

	//rotation settings for animation. The script auto-setup this
	Quaternion my_rotation_up;
	Quaternion my_rotation_down;

	//the overlap detectors that check if there are other card over this one
	public float big_radius;
	public float small_radius;
	public Transform[] overlap_small;

	public bool editor_show_overlap;//open/close the overlap detectors menu in inspector

	// Use this for initialization
	void Start () {


		my_rotation_up = this.transform.localRotation;
		Start_me ();

	}

	public void Reset_me()
	{
		this.gameObject.SetActive (true);
		if (!always_face_up)
			face_up = false;
	}

	public void Start_me()
	{

		Generate_me ();//set rank, suit and sprites
		
		Find_cards_over_me ();
		
		Turn_this_card ();//check if this card must be face up or down
	}
	
	void Find_cards_over_me()
	{
		//store the overlap detectors
			Collider2D[][] temp_colliders = new Collider2D[overlap_small.Length+1][];
			int total_lengh = 0;
			//small circles
			for (int i = 0; i < overlap_small.Length; i++)
			{
				temp_colliders[i] = Physics2D.OverlapCircleAll(new Vector2(overlap_small[i].position.x,overlap_small[i].position.y),small_radius);
				total_lengh+= temp_colliders[i].Length;
			}

			//big circle
			temp_colliders[overlap_small.Length]= Physics2D.OverlapCircleAll(new Vector2(this.transform.position.x,this.transform.position.y),big_radius);
			total_lengh+= temp_colliders[overlap_small.Length].Length;

		//check if there are cards over this one
		Transform[] temp_cards_over_me = new Transform[total_lengh];

		Transform[] temp_cards_over_me_check = new Transform[total_lengh];
		bool is_new = true;

		int temp_count = 0;
		int total = 0;

		this_is_a_bottom_card = true;
		//check small circles
		for (int i = 0; i < overlap_small.Length; i++)
		{
			for (int ii = 0; ii < temp_colliders[i].Length; ii++)
			{
				if (temp_colliders[i][ii].transform.position.z < this.transform.position.z)//there is a card over me
				{
					is_new = true;
					for (int c = 0; c < temp_cards_over_me_check.Length; c++)
						{
						if (temp_cards_over_me_check[c] == temp_colliders[i][ii].transform)
							is_new = false;
						}
					if (is_new)
						{
						temp_cards_over_me[temp_count] = temp_colliders[i][ii].transform;
						temp_cards_over_me_check[temp_count] = temp_colliders[i][ii].transform;
						total++;
						}
				}
				else if (temp_colliders[i][ii].transform.position.z > this.transform.position.z)//there is a card under me
				{
					this_is_a_bottom_card = false;
				}
				temp_count++;
			}
		}
		//check big circle
		for (int ii = 0; ii < temp_colliders[overlap_small.Length].Length; ii++)
		{
			if (temp_colliders[overlap_small.Length][ii].transform.position.z < this.transform.position.z)//there is a card over me
			{
				is_new = true;
				for (int c = 0; c < temp_cards_over_me_check.Length; c++)
					{
					if (temp_cards_over_me_check[c] == temp_colliders[overlap_small.Length][ii].transform)
						is_new = false;
					}
				if (is_new)
					{
					temp_cards_over_me[temp_count] = temp_colliders[overlap_small.Length][ii].transform;
					total++;
					}
			}
			else if (temp_colliders[overlap_small.Length][ii].transform.position.z > this.transform.position.z)//there is a card under me
			{
				this_is_a_bottom_card = false;
			}
		}

		//save card found in an array in order to know when the player will pick all the card above this one
		cards_over_me = new Transform[total];
		temp_count = 0;
		for (int i = 0; i < total_lengh; i++)
		{
			if (temp_cards_over_me[i] != null)
				{
				cards_over_me[temp_count] = temp_cards_over_me[i];
				temp_count++;
				}
		}
	}


	//show the overlap detectors
	void OnDrawGizmos() {
		//main circle
		Gizmos.color = Color.yellow;
		Gizmos.DrawWireSphere (this.transform.position, big_radius);
		//small circles
		Gizmos.color = Color.cyan;
		for (int i = 0; i < overlap_small.Length; i++)
			Gizmos.DrawWireSphere (overlap_small[i].position, small_radius);

	}
	
	void OnMouseDown()
	{
		if (deck.this_deck.player_can_move && Input.touches.Length <= 1)//if the palyer click or tap on this card
			{
			//Debug.Log ("S"+my_suit + "R" + my_rank + " * up = " + face_up + " * free = " +This_card_is_free());
			if (This_card_is_free()) //if this card is free
				{
				//check if this card can became the new current card
				if (my_rank == 0)
					{
					if (deck.this_deck.current_card_rank == 12 || (deck.this_deck.current_card_rank == (my_rank + 1)))
						This_is_the_new_current_card ();
					else
						Wrong_card();
					} 
				else if (my_rank == 12)
					{
					if (deck.this_deck.current_card_rank == 0 || (deck.this_deck.current_card_rank == (my_rank - 1)))
						This_is_the_new_current_card ();
					else
						Wrong_card();
					}
				else
					{
					if ( (deck.this_deck.current_card_rank == (my_rank + 1)) || (deck.this_deck.current_card_rank == (my_rank - 1)) )
						This_is_the_new_current_card ();
					else
						Wrong_card();
					}
				} 
			else
				Blocked_card();//this card have another card over itself
			}
	}
	
	public bool This_card_is_free()//check if the card not have others card over itself anymore
	{
		bool return_this = true;

		if (cards_over_me.Length == 0)//this card neved had other card over itself
			return_this = true;
		else
			{
			for (int i = 0; i < cards_over_me.Length; i++)
				{
				if (cards_over_me[i]!= null)//there is a card over me
					{
					if (cards_over_me[i].gameObject.activeSelf == true)//in it is active
						{
						return_this = false;//I found a card over me, so player can't pick me
						break;
						}
					}
				}
			}
		return return_this;
	}

	void Generate_me()
	{
		if (card_suit_selected == card_suit.random)
			my_suit = Random.Range(0,4);

		if (card_rank_selected == card_rank.random)
			{
			bool approved_rank = false;
			my_rank = Random.Range(0,13);

			//avoid to generate too many card with the same rank
			int safety_stop = 100;
			while (!approved_rank)
				{
				safety_stop--;
				if (safety_stop <= 0)
					{
					Debug.LogWarning("safety_stop");
					break;
					}

				if (deck.this_deck.card_on_board_by_rank[my_rank] <= deck.this_deck.max_number_of_card_with_the_same_rank_on_board)//if you don't have use this rank too much on the board
					{
					approved_rank = true;//this is a valid rank to use
					}
				else
					my_rank = Random.Range(0,13);
				}
			deck.this_deck.card_on_board_by_rank[my_rank]++;
			}

		//setup card appearance
		if (card_type_selected == card_type.normal)
			{
			my_face.sprite = deck.this_deck.card_deck_sprite [my_suit, my_rank];
			my_back.sprite = deck.this_deck.card_back_sprite;
			}
		else if (card_type_selected == card_type.gold)
		{
			my_face.sprite = deck.this_deck.gold_card_deck_sprite [my_suit, my_rank];
			my_back.sprite = deck.this_deck.gold_card_back_sprite;
			deck.this_deck.total_gold_cards++;
			deck.this_deck.Update_gold_cards_count();
		}

	}

	public void Turn_this_card()
	{
		//turn up random cards at start
		if (!deck.this_deck.game_started)
			{
			if (UnityEngine.Random.Range (1, 100) <= deck.this_deck.chance_of_turn_up_a_covered_card_at_start)
				always_face_up = true;
			}

		if (always_face_up) //turn up card set up manually
			face_up = always_face_up;
		else //turn up only top card
			face_up = This_card_is_free();

		if (face_up)
			{
			StartCoroutine(Rotate_me(my_rotation_up)); //rotate face up
			}
		else //rotate face down
			{
			if (transform.rotation.y ==  my_rotation_up.y)
				{
				if (deck.this_deck.game_started)
					StartCoroutine(Rotate_me(my_rotation_down)); //with animation because this card return turn down due an UNDO
				else //automatically at game start
					{	
					transform.Rotate (my_rotation_up.x, my_rotation_up.y + 180, my_rotation_up.z, Space.Self);
					my_rotation_down = this.transform.localRotation;
					}
				}
			}
	}

	IEnumerator Rotate_me(Quaternion end_point)//the rotate animation
	{
		Quaternion start_point = this.transform.rotation;
		float duration = 0.25f;
		float time = 0;
		
		while (time < 1)
		{
			time += Time.smoothDeltaTime / duration;
			transform.rotation = Quaternion.Lerp(start_point,end_point,time);
			yield return null;
		}
	}

	void This_is_the_new_current_card()//put this card in the target deck
	{
		//put this card sprite on the ghost card used to show the card movement from board to target deck
		if (card_type_selected == card_type.normal)
			deck.this_deck.ghost_card.GetComponent<SpriteRenderer>().sprite = deck.this_deck.card_deck_sprite [my_suit, my_rank];
		else if (card_type_selected == card_type.gold)
			deck.this_deck.ghost_card.GetComponent<SpriteRenderer>().sprite = deck.this_deck.gold_card_deck_sprite [my_suit, my_rank];

		//save the card valeus in order to recover them if player click undo
		deck.this_deck.current_last_move = deck.last_move.take_card_from_board;
		deck.this_deck.Update_undo_button (true);
		deck.this_deck.lastest_deactivate_board_card = this.gameObject;
		gameObject.SetActive (false);

		deck.this_deck.StartCoroutine (deck.this_deck.Move_to (transform,deck.this_deck.target_new_card.transform,false));//show the card movement from board to target deck (it is an illusion, this card just became inactive and the ghost card move from this card position to target deck position)
		deck.this_deck.StartCoroutine(deck.this_deck.New_current_card (my_suit, my_rank, card_type_selected, false, false));//set this card as new top card on the target deck

		//score
		if (card_type_selected == card_type.normal)
			{
			if (this_is_a_bottom_card)
				{
				deck.this_deck.Update_score (deck.this_deck.bottom_card_score);
				}
			else
				{
				deck.this_deck.Update_score (deck.this_deck.normal_card_score);
				}
			}
		else if (card_type_selected == card_type.gold)
			deck.this_deck.Update_score (deck.this_deck.gold_card_score);


		if (deck.this_deck.current_total_card_on_board > 0)//if there are cards left
			deck.this_deck.StartCoroutine(deck.this_deck.Refresh_card_rotation ());//rotate face up the cards now free

	}


	void Blocked_card()
	{
		Debug.Log ("this card have something over it");
		deck.this_deck.Play_sfx(deck.this_deck.blocked_card_sfx);

	}

	void Wrong_card()
	{
		Debug.Log ("wrong card rank");
		deck.this_deck.Play_sfx(deck.this_deck.wrong_card_sfx);
	}

}
