using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class deck : MonoBehaviour {

	
	//rules
	public bool show_unused_card_when_player_take_a_new_card_from_the_deck;//if true, when player ask a new card and miss to use the current, the miss move shake
	public int give_an_useful_card_not_less_than_n_new_card_from_the_deck;//because deck card are random, in order to avoid to have a long sequence of useless cards by accident, you can set here the max lenght of the useless sequence. After that the deck will give an useful for sure. If you don't want give this help to the player, just put here a number greater than total card
	int unused_card_count;//the remaining deck card when the player win. It is used to give the bonus score "score_for_each_deck_card_spared"
	public bool play_until_no_more_cards_then_check_win_condition;//instead to win when reach the win condition, continue the game until no move avaibles, then calculate the player outcome 
	public enum win_condition
	{
		collect_all_cards,
		collect_all_gold_cards,
		reach_target_score
	}
	public win_condition win_condition_selected = win_condition.collect_all_cards;
	public int win_target_score;
	public Slider target_score_slider;//the GUI slider that show the player progress
	public Text target_score_text;
	
	public int chance_of_turn_up_a_covered_card_at_start;//usually card covered by other card are face down, but you can roll for each a chance from 0 to 100 to turn up a covered card

	//combo
	public int min_combo_lenght_to_trigger_the_multiplier = 1;
	public GameObject end_combo_obj;// show the "end combo" text over the deck
	int combo_count;
	int previous_combo_count;//for undo
		public Text combo_text;
		public float combo_score_multiplier;
	int same_color_combo;//how many card with the same color in a row
		public float color_combo_score_multiplier;
	int same_suit_combo;//how many card with the same suit in a row
		public float suit_combo_score_multiplier;
	public Text total_combo_multiplier_text;
	float score_multiplier = 0;

	//score
	int current_score;
	int undo_previous_score;
	public Text score_text;
	public int normal_card_score;
	public int gold_card_score;
	public int bottom_card_score;
	public int score_for_each_deck_card_spared;

	
	public bool game_started;//game autosetup itself, then set game_started = true and player_can_move = true so player can play
	bool game_end;//true no more moves or win condition reached
	bool player_win;//if true when game_end is true, show win screen, then lose screen
	[HideInInspector]
	public bool player_can_move;//it is false before game start, after game end and through card animations

	public static deck this_deck;//this script
	float animation_duration = 0.25f;

	public int deck_card_left;//how many card the player can ask
	int start_deck_card_left;
	public Text deck_card_left_text;
	int current_deck_position;//the current position in the deck array
	int current_target_deck_position;//the current position in the target deck array
	
	//the card in the target deck
	public int current_card_suit = -99;
	public int current_card_rank = -99;
	int previous_suit = -99;
	int previus_rank = -99;

	[System.Serializable]
	public class card_class
	{
		public int suit = -99;
		public int rank = -99;
	}

	public card_class[] deck_cards;//store the cards in the deck
	public card_class[] target_deck_cards;//keep trak of the card put in the target deck from deck and board

	public SpriteRenderer target_new_card;//the new current card to show
	public SpriteRenderer new_card_anim_sprite;//the card to show in the animantion from deck to target deck
	public Animation new_card_anim;//animation from deck to target deck

	public Sprite card_back_sprite;
	public Sprite gold_card_back_sprite;

	public int total_gold_cards; //need for collect_all_gold_cards
	int gold_cards_taken;
	//public Text gold_cards_count_text;
	
	//black
	public Sprite[] clubs;
	public Sprite[] spades;

	//red
	public Sprite[] diamonds;
	public Sprite[] hearts;

	//gold version:
		//black
		public Sprite[] gold_clubs;
		public Sprite[] gold_spades;
	
		//red
		public Sprite[] gold_diamonds;
		public Sprite[] gold_hearts;
	
	public Sprite[,] card_deck_sprite;//suit,rank
	public Sprite[,] gold_card_deck_sprite;//suit,rank

	public Transform board;//the child of this trasnform are the card on board
	public int current_total_card_on_board;
	int total_card_on_board;
	//these variables aim to avoid to have too many cards with the same rank
		public int max_number_of_card_with_the_same_rank_on_board;
		public int[] card_on_board_by_rank;

	card[] cards_on_board;

	//undo
	public GameObject lastest_deactivate_board_card;
	public Button undo_button;
	public Text undo_count_text;
	public int undo_charges;
	int start_undo_charges;
	public enum last_move
	{
		none,
		ask_new_card_from_deck,
		take_card_from_board
	}
	public last_move current_last_move = last_move.none;
	public card.card_type previous_card_type;
	public card.card_type current_card_type;

	public Transform ghost_card;//it is use to show an animation when a card pass from board to target deck or conversely

	//GUI screen
	public GameObject game_screen;
	public GameObject end_screen;
		public Text end_screen_name;
		public Text final_score;
		public Text target_score_end_screen_text;
	public float show_win_or_lose_screen_after_delay;

	//sfx
	public AudioClip take_this_card_sfx;//take a card from the board
	public AudioClip blocked_card_sfx; //this card have something over it
	public AudioClip wrong_card_sfx; //wrong card rank
	public AudioClip new_card_sfx; //take a new card from the deck
	public AudioClip win_sfx;
	public AudioClip lose_sfx;

	//editor (theso bool open and close the menus of this script in inspector
	public bool editor_show_sprites;
		public bool editor_show_sprites_normal;
			public bool editor_show_sprites_normal_clubs;
			public bool editor_show_sprites_normal_spades;
			public bool editor_show_sprites_normal_diamonds;
			public bool editor_show_sprites_normal_hearts;
		public bool editor_show_sprites_gold;
			public bool editor_show_sprites_gold_clubs;
			public bool editor_show_sprites_gold_spades;
			public bool editor_show_sprites_gold_diamonds;
			public bool editor_show_sprites_gold_hearts;
	public bool editor_show_sfx;
	public bool editor_show_advanced;
		public bool editor_show_gui;
	
	void Awake()//varaibles to initiate only one time for scene
	{

		this_deck = this;//this script

		Generate_card_deck_sprite ();//fuse clubs[], spades[], diamonds[] and hearts[] in one array

		//setup deck sprite
		GetComponent<SpriteRenderer>().sprite = card_back_sprite;
		new_card_anim.transform.GetChild(0).transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = card_back_sprite;

		start_undo_charges = undo_charges;
		start_deck_card_left = deck_card_left;

		total_card_on_board = board.childCount; //count cards on board
		cards_on_board = new card[total_card_on_board];
		for (int i = 0; i < total_card_on_board; i++)
			cards_on_board [i] = board.GetChild (i).GetComponent<card> ();//put all board card script in an array to quick reference
		max_number_of_card_with_the_same_rank_on_board = Mathf.CeilToInt(total_card_on_board * 0.25f);//decide the max number of card with the same rank to put on board

		Debug.Log ("board cards = " + total_card_on_board + " ...max same rank = " + max_number_of_card_with_the_same_rank_on_board);

	}

	// Use this for initialization
	void Start () {

		Restart (false);//variables that need to be initiated at each play (false mean that this is the first time, not a restart)

	}

	public void Restart(bool after_reset)
	{
		//reset variables
		game_started = false;
		game_end = false;
		player_win = false;
		player_can_move = false;
		current_total_card_on_board = total_card_on_board;
		total_gold_cards = 0;
		current_deck_position = 0;
		current_target_deck_position = 0;
		unused_card_count = 0;
		undo_previous_score = 0;
			//combo
			previous_combo_count = 0;
			combo_count = 0;
			score_multiplier = 0;
			//gold cards
			gold_cards_taken = 0;
			Update_gold_cards_count();
			//score
			current_score = 0;
			score_text.text = current_score.ToString();

		this.gameObject.GetComponent<SpriteRenderer>().enabled = true;
		target_new_card.gameObject.SetActive(false);
		end_screen.SetActive(false);
		game_screen.SetActive(true);
		Update_undo_button (false);

		card_on_board_by_rank = new int[13];


		if (after_reset)//restart all board cards. Return in place all board card
		{
			undo_charges = start_undo_charges;
			undo_count_text.text = undo_charges.ToString();

			deck_card_left = start_deck_card_left;
			for (int i = 0; i < total_card_on_board; i++)
			{
				cards_on_board[i].Reset_me();
			}
			//this is a separate loop because Start_me() need that all in original start position in order to check if a card is cover or not
			for (int i = 0; i < total_card_on_board; i++)
			{
				cards_on_board[i].Start_me();
			}
		}


		Update_combo_text ();
		
		Fill_deck ();//crate the contents of the deck

		StartCoroutine(New_current_card(deck_cards[current_deck_position].suit,deck_cards[current_deck_position].rank,card.card_type.normal,true,false));//show the first card of the deck


		if (win_condition_selected == win_condition.reach_target_score) 
		{
			target_score_slider.gameObject.SetActive (true);
			target_score_slider.maxValue = win_target_score;
			target_score_slider.value = 0;
			target_score_text.text = "Target = " + win_target_score.ToString("N0");
		} 
		else
			target_score_slider.gameObject.SetActive (false);
		


	}

	public void Update_gold_cards_count()
		{/*
		if (total_gold_cards > 0)
		{
			gold_cards_count_text.text = total_gold_cards.ToString() + " / " + gold_cards_taken.ToString();
			gold_cards_count_text.transform.parent.gameObject.SetActive(true);
		}
		else
			gold_cards_count_text.transform.parent.gameObject.SetActive(false);*/
		}

	void Fill_deck()//decide what card put in the deck, but some can change if give_an_useful_card_not_less_than_n_new_card_from_the_deck is trigger
	{

		deck_cards = new card_class[deck_card_left];
		for (int i = 0; i < deck_card_left; i++)
		{
			deck_cards [i] = new card_class ();
			deck_cards[i].suit = Random.Range(0,4);
			deck_cards[i].rank = Random.Range(0,13);

		}

		target_deck_cards = new card_class[deck_card_left + current_total_card_on_board +1]; 
		for (int i = 0; i < target_deck_cards.Length; i++)
			target_deck_cards[i] = new card_class ();


		deck_card_left_text.text = (deck_card_left-1).ToString();

	}

	void Generate_card_deck_sprite()//fuse the card sprite arrays in one
	{
		card_deck_sprite = new Sprite[4,13];
		gold_card_deck_sprite = new Sprite[4,13];

		Sprite[] temp_rank = new Sprite[13];
		Sprite[] temp_rank_gold = new Sprite[13];

		for (int suit = 0; suit < 4; suit++)
		{

			if (suit == 0)
				{
				temp_rank = clubs;
				temp_rank_gold = gold_clubs;
				}
			else if (suit == 1)
				{
				temp_rank = spades;
				temp_rank_gold = gold_spades;
				}
			else if (suit == 2)
				{
				temp_rank = diamonds;
				temp_rank_gold = gold_diamonds;
				}
			else if (suit == 3)
				{
				temp_rank = hearts;
				temp_rank_gold = gold_hearts;
				}

			for (int rank = 0; rank < 13; rank++)
			{
				card_deck_sprite[suit,rank] = temp_rank[rank];
				gold_card_deck_sprite[suit,rank] = temp_rank_gold[rank];
			}
		}
	}

    //ISABELLA CODE
    public void RunNewCard() {
        Invoke("OnMouseDown", 0f);
    }


	void OnMouseDown()//click here the player ask a new card
	{
		if (player_can_move && Input.touches.Length <= 1)
			{
			if (deck_card_left > 1)//if there is a card to give
				{
				lastest_deactivate_board_card = null;

				//update deck count
				deck_card_left--;
				deck_card_left_text.text = (deck_card_left-1).ToString();
				current_deck_position++;

				StartCoroutine(New_current_card(deck_cards[current_deck_position].suit,deck_cards[current_deck_position].rank,card.card_type.normal,true,false));//show animation

				//update undo
				current_last_move = last_move.ask_new_card_from_deck;
				Update_undo_button (true);

				if (deck_card_left == 1)//if this was the last card
					{
					this.gameObject.GetComponent<SpriteRenderer>().enabled = false;
					Debug.Log ("No more cards");
					}
				}
			else// no more card, the game end
				{
				//no card
				current_card_suit = -99;
				current_card_rank = -99;

				//show win or lose screen
				if (play_until_no_more_cards_then_check_win_condition)
					{
					if (player_win)
						{
						Calculate_final_score();
						StartCoroutine(You_win());
						}
					else
						{
						Calculate_final_score();
						StartCoroutine(You_lose());
						}
					}
				else
					{
					Calculate_final_score();
					StartCoroutine(You_lose());
					}
				}
			}
	}

	public void Undo_last_move()
	{
		if (undo_charges > 0 && player_can_move)//if player can ask an undo
		{
			undo_charges--;
			Update_undo_button(false);

			current_target_deck_position--;//remove last card from target deck
			
			if (current_last_move == last_move.ask_new_card_from_deck)//return last card to deck
			{
				combo_count = previous_combo_count;

				//Debug.Log("current_last_move == last_move.ask_new_card_from_deck");
				deck_card_left++;
				deck_card_left_text.text = (deck_card_left-1).ToString();
				current_deck_position--;

				StartCoroutine(New_current_card(target_deck_cards[current_target_deck_position].suit,target_deck_cards[current_target_deck_position].rank,previous_card_type,true,true));

				
				if (!this.gameObject.GetComponent<SpriteRenderer>().enabled)
					this.gameObject.GetComponent<SpriteRenderer>().enabled = true;
			}
			else if (current_last_move == last_move.take_card_from_board) //return last card to board
			{
				//Debug.Log("last_move.take_card_from_board");
				current_total_card_on_board++;

				StartCoroutine(Move_to(target_new_card.transform,lastest_deactivate_board_card.transform,true));

				StartCoroutine(New_current_card(target_deck_cards[current_target_deck_position].suit,target_deck_cards[current_target_deck_position].rank,previous_card_type,false,true));

			}
		}
	}
	
	public void Update_undo_button(bool can_undo)
	{
		if ((undo_charges == 0) || (current_last_move == last_move.none) || !can_undo)
			undo_button.interactable = false;
		else
			undo_button.interactable = true;
		
		undo_count_text.text = undo_charges.ToString();
		
	}

	public IEnumerator Move_to(Transform start_point,Transform end_point, bool refresh_board)//move the ghost card
	{
		ghost_card.position = start_point.position;
		ghost_card.rotation = start_point.rotation;
		ghost_card.gameObject.SetActive(true);

		//animation
		float duration = animation_duration;
		float time = 0;

		while (time < 1)
			{
			time += Time.smoothDeltaTime / duration;
			ghost_card.position = Vector3.Lerp(start_point.position,end_point.position,time);
			ghost_card.rotation = Quaternion.Lerp(start_point.rotation,end_point.rotation,time);
			yield return null;
			}
		ghost_card.gameObject.SetActive(false);

		if (refresh_board)
			StartCoroutine(Refresh_card_rotation ());

	}

	public IEnumerator New_current_card(int suit, int rank, card.card_type card_type, bool from_deck, bool undo)//show the new current card in target deck
	{

		player_can_move = false;//because animation

		previous_card_type = current_card_type;
		previous_suit = current_card_suit;
		previus_rank = current_card_rank;

		current_card_type = card_type;
		current_card_suit = suit;
		current_card_rank = rank;


        if (from_deck)//the new card is give from the deck
            {
            //int count = 0;  //COUNTER
            //while (count < 3) {     //COUNTER


                Play_sfx(new_card_sfx);

                if (undo) {
                    new_card_anim.Play("undo_to_deck_card_anim");
                    yield return new WaitForSeconds(0.05f);
                    unused_card_count--;
                    Update_combo_text();
                }
                else {
                    current_target_deck_position++;

                    if (game_started) {
                        unused_card_count++;
                        //check if give an useful card for sure
                        if (unused_card_count > give_an_useful_card_not_less_than_n_new_card_from_the_deck) {
                            //search a face up card from the board
                            int random_start_point = Random.Range(0, cards_on_board.Length);
                            bool card_found = false;
                            for (int i = random_start_point; i < cards_on_board.Length; i++) {
                                if (cards_on_board[i].This_card_is_free() && cards_on_board[i].gameObject.activeSelf == true && cards_on_board[i].face_up) //if this card is active
                                    {

                                    deck_cards[current_deck_position].rank = cards_on_board[i].my_rank;
                                    card_found = true;
                                    break;
                                }
                            }
                            if (!card_found) {
                                for (int i = 0; i < random_start_point; i++) {
                                    if (cards_on_board[i].This_card_is_free() && cards_on_board[i].gameObject.activeSelf == true && cards_on_board[i].face_up) //if this card is active
                                        {
                                        deck_cards[current_deck_position].rank = cards_on_board[i].my_rank;
                                        card_found = true;
                                        break;
                                    }
                                }
                            }

                            //decide if the next card will have a rank +1 or -1
                            if (Random.Range(0, 2) > 0) {
                                deck_cards[current_deck_position].rank++;
                                if (deck_cards[current_deck_position].rank > 12)
                                    deck_cards[current_deck_position].rank = 0;
                            }
                            else {
                                deck_cards[current_deck_position].rank--;
                                if (deck_cards[current_deck_position].rank < 0)
                                    deck_cards[current_deck_position].rank = 12;
                            }

                            current_card_rank = deck_cards[current_deck_position].rank;
                        }

                        if (show_unused_card_when_player_take_a_new_card_from_the_deck) {
                            int avaible_move = Check_avaible_moves(previus_rank);
                            if (avaible_move >= 0) //if there is a move
                                StartCoroutine(Shake_card(cards_on_board[avaible_move].transform));

                        }
                    }
                    target_deck_cards[current_target_deck_position] = new card_class();
                    target_deck_cards[current_target_deck_position].suit = current_card_suit;
                    target_deck_cards[current_target_deck_position].rank = current_card_rank;


                    if (current_card_type == card.card_type.normal)
                        new_card_anim_sprite.sprite = card_deck_sprite[current_card_suit, current_card_rank];
                    else if (current_card_type == card.card_type.gold)
                        new_card_anim_sprite.sprite = gold_card_deck_sprite[current_card_suit, current_card_rank];

                    new_card_anim.Play("new_card_anim");
                    yield return new WaitForSeconds(animation_duration + 0.1f);

                    previous_combo_count = combo_count;
                    combo_count = 0;
                    Update_combo_text();

                    same_color_combo = 0;
                    same_suit_combo = 0;
                    score_multiplier = 0;

                    Update_score_multiplier();
                }

                if (!game_started) {
                    game_started = true;
                    target_new_card.gameObject.SetActive(true);

                    if (total_gold_cards == 0 && win_condition_selected == win_condition.collect_all_gold_cards)
                        Debug.LogWarning("win condition is *collect_all_gold_cards*, but you don't have any gold card on board. Click on at least one card on board, select manual, and *card type* = gold");
                }

                if (current_card_type == card.card_type.normal)
                    target_new_card.sprite = card_deck_sprite[current_card_suit, current_card_rank];
                else if (current_card_type == card.card_type.gold)
                    target_new_card.sprite = gold_card_deck_sprite[current_card_suit, current_card_rank];

                //MORE JANK CODE... COUNTER
                //if (count == 1) {
                //    target_new_card.gameObject.transform.position = new Vector3(target_new_card.gameObject.transform.position.x + 50, target_new_card.gameObject.transform.position.y + 50, target_new_card.gameObject.transform.position.z);
                //}
                //count++;    //COUNTER
            //}
        }
        else //from board
            {
            Play_sfx(take_this_card_sfx);
            if (undo) {
                if (current_card_type == card.card_type.normal)
                    target_new_card.sprite = card_deck_sprite[current_card_suit, current_card_rank];
                else if (current_card_type == card.card_type.gold)
                    target_new_card.sprite = gold_card_deck_sprite[current_card_suit, current_card_rank];


                yield return new WaitForSeconds(animation_duration);//wait the end of the animation

                Debug.Log("undo from board " + lastest_deactivate_board_card.name);
                lastest_deactivate_board_card.SetActive(true);

                Refresh_card_rotation();

                combo_count--;
                Update_combo_text();

                same_color_combo--;
                if (same_color_combo < 0)
                    same_color_combo = 0;

                same_suit_combo--;
                if (same_suit_combo < 0)
                    same_suit_combo = 0;

                current_score -= undo_previous_score;
                score_text.text = current_score.ToString("N0");

                if (previous_card_type == card.card_type.gold) {
                    gold_cards_taken--;
                    Update_gold_cards_count();
                }

            }
            else //card take from board and this not is an undo move
                {
                yield return new WaitForSeconds(animation_duration);//wait the end of the animation
                current_target_deck_position++;

                target_deck_cards[current_target_deck_position] = new card_class();
                target_deck_cards[current_target_deck_position].suit = current_card_suit;
                target_deck_cards[current_target_deck_position].rank = current_card_rank;


                if (current_card_type == card.card_type.normal)
                    target_new_card.sprite = card_deck_sprite[current_card_suit, current_card_rank];
                else if (current_card_type == card.card_type.gold)
                    target_new_card.sprite = gold_card_deck_sprite[current_card_suit, current_card_rank];

                unused_card_count = 0;

                if (card_type == card.card_type.gold) {
                    gold_cards_taken++;
                    Update_gold_cards_count();
                }

                current_total_card_on_board--;

                if (current_total_card_on_board == 0) {
                    Calculate_final_score();


                    if (win_condition_selected == win_condition.collect_all_cards || win_condition_selected == win_condition.collect_all_gold_cards)
                        StartCoroutine(You_win());
                    else {
                        if (win_condition_selected == win_condition.reach_target_score && current_score >= win_target_score)
                            StartCoroutine(You_win());
                        else
                            StartCoroutine(You_lose());
                    }
                }
                else {
                    if (win_condition_selected == win_condition.collect_all_gold_cards && gold_cards_taken == total_gold_cards) {
                        if (play_until_no_more_cards_then_check_win_condition)
                            player_win = true;
                        else {
                            Calculate_final_score();
                            StartCoroutine(You_win());
                        }
                    }
                }

            }

        }

		player_can_move = true;

	}

	void Calculate_final_score()
	{
		int temp_current_score = current_score;
		current_score += score_for_each_deck_card_spared * (deck_card_left-1);

		target_score_slider.value = current_score;
		score_text.text = current_score.ToString ("N0");

		if (win_condition_selected == win_condition.reach_target_score)
			{
			target_score_end_screen_text.text = " target score = " + win_target_score.ToString ("N0");
			target_score_end_screen_text.gameObject.SetActive (true);
			}
		else
			target_score_end_screen_text.gameObject.SetActive (false);


		if ((deck_card_left-1) > 0)
			final_score.text = "Score: " + temp_current_score.ToString ("N0") + " + " + (deck_card_left-1).ToString ("N0") + " left cards * " + score_for_each_deck_card_spared.ToString ("N0") + " = " + current_score.ToString ("N0");
		else
			final_score.text = "Score: " + current_score.ToString ("N0");

	}

	int Check_avaible_moves(int target_rank)
	{
		int return_this = -1;
		for (int i = 0; i < cards_on_board.Length; i++)
		{
			if (cards_on_board [i].This_card_is_free() && cards_on_board [i].gameObject.activeSelf == true && cards_on_board [i].face_up) //if this card is active
			{

				if ((cards_on_board [i].my_rank > 0) && 
				    (cards_on_board [i].my_rank < 12))
					{
					if ((target_rank+1 == cards_on_board[i].my_rank)
					    ||(target_rank-1 == cards_on_board[i].my_rank))
							{
							return_this = i;
							break;
							}
					}
				else if (cards_on_board [i].my_rank == 12)
					{
					if ((target_rank == 0)
					    ||(target_rank+1 == cards_on_board[i].my_rank))
						{
						return_this = i;
						break;
						}
					}
				else if (cards_on_board [i].my_rank == 0)
					{
					if ((target_rank-1 == cards_on_board[i].my_rank)
					    ||(target_rank == 12))
						{
						return_this = i;
						break;
						}
					}

			}
			
		}

		return return_this;

	}

	IEnumerator Shake_card(Transform this_card)//show the miss move
	{
		Vector3 start_position = this_card.position;
		float min_magnitude = -0.05f;
		float max_magnitude = 0.05f;
		int n_shakes = 5;
		while (n_shakes > 0)
		{
			n_shakes--;

			this_card.position = new Vector3(this_card.position.x+Random.Range(min_magnitude,max_magnitude),
			                                 this_card.position.y+Random.Range(min_magnitude,max_magnitude),
			                                 this_card.position.z);


			yield return new WaitForSeconds(0.05f);
		}
		this_card.position = start_position;
	}
	

	public IEnumerator Refresh_card_rotation()//turn face up all free cards on board
	{
		yield return new WaitForSeconds(0.25f);

		for (int i = 0; i < cards_on_board.Length; i++)
		{
			if (cards_on_board [i].gameObject.activeSelf == true) //if this card is active
				cards_on_board [i].Turn_this_card(); //check if it can be turn up

		}
	}

	void Update_combo_text()
	{
		combo_text.text = "Combo: " + combo_count.ToString ();

		if (combo_count >= min_combo_lenght_to_trigger_the_multiplier)
			{
			combo_text.gameObject.SetActive (true);
			end_combo_obj.SetActive (true);
			}
		else
			{
			combo_text.gameObject.SetActive (false);
			end_combo_obj.SetActive (false);
			}
	}

	public void Update_score(int add_to_score)
	{
		previous_combo_count = combo_count;
		combo_count++;
		Update_combo_text ();

		score_multiplier = 0;

		if (combo_count >= min_combo_lenght_to_trigger_the_multiplier && add_to_score > 0)
			score_multiplier += (float)(combo_count-min_combo_lenght_to_trigger_the_multiplier) * combo_score_multiplier;//Mathf.CeilToInt((float)combo_count * combo_score_multiplier);

		if (current_card_suit == previous_suit)
		{
			same_suit_combo++;

			if (add_to_score > 0)
				score_multiplier += (float)same_suit_combo * suit_combo_score_multiplier;//Mathf.CeilToInt((float)same_suit_combo * suit_combo_score_multiplier);

			same_color_combo++;

			if (add_to_score > 0)
				score_multiplier += (float)same_color_combo * color_combo_score_multiplier;//Mathf.CeilToInt((float)same_color_combo * color_combo_score_multiplier);
		}
		else
		{
			same_suit_combo = 0;

			if (((current_card_suit <= 1) && (previous_suit <= 1))//black clor
				|| ((current_card_suit >= 2) && (previous_suit >= 2)))//red color
				{
				same_color_combo++;

				if (add_to_score > 0)
					score_multiplier += (float)same_color_combo * color_combo_score_multiplier;//Mathf.CeilToInt((float)same_color_combo * color_combo_score_multiplier);
				}
			else
				{
				same_color_combo = 0;
				}
		}

		Update_score_multiplier ();
		float temp_add_to_score = add_to_score;
		temp_add_to_score *= score_multiplier;


		add_to_score += Mathf.FloorToInt(temp_add_to_score);
		undo_previous_score = add_to_score;

		if (add_to_score > 0)
			{
			current_score += add_to_score;
			score_text.text = current_score.ToString ("N0");
			}

		if (win_condition_selected == win_condition.reach_target_score)
		{
			target_score_slider.value = current_score;

			if (current_score >= win_target_score)
				{
				if (play_until_no_more_cards_then_check_win_condition)
					player_win = true;
				else
					{
					Calculate_final_score();
					StartCoroutine(You_win());
					}
				}
		}
	}

	void Update_score_multiplier()
	{
		total_combo_multiplier_text.text = "x" + score_multiplier.ToString();
	}

	IEnumerator You_win()//show win screen
	{
		if (!game_end)
			{
			Debug.Log ("win");	

			game_end = true;
			player_win = true;
			player_can_move = false;

			end_screen_name.text = "WIN!";

			yield return new WaitForSeconds(show_win_or_lose_screen_after_delay);

			Play_sfx(win_sfx);



			game_screen.SetActive(false);
			end_screen.SetActive(true);
			}
	}

	IEnumerator You_lose()//show lose screen
	{
		if (!game_end)
			{
			Debug.Log ("lose");
			game_end = true;
			player_can_move = false;
			player_win = false;

			end_screen_name.text = "LOSE!";

			yield return new WaitForSeconds(show_win_or_lose_screen_after_delay);

			Play_sfx(lose_sfx);

			game_screen.SetActive(false);
			end_screen.SetActive(true);
			}
	}

	public void Play_sfx(AudioClip my_clip)
	{
		if (my_clip)
		{
			GetComponent<AudioSource>().clip = my_clip;
			GetComponent<AudioSource>().Play();
		}
	}

}
