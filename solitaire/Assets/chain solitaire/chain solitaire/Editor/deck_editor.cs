using UnityEngine.UI;
using UnityEngine;
using UnityEditor;
using System.Collections;


[CustomEditor(typeof(deck))]
internal class deck_editor : Editor {

	public override void OnInspectorGUI()
	{
		Rules ();
		Score ();
		Sprites ();
		Sfx ();
		Advanced();

	}

	void Rules()
	{
		deck my_target = (deck)target;
		EditorGUI.BeginChangeCheck ();
        Undo.RecordObject(my_target, "edit_rules");

        EditorGUILayout.LabelField("Rules");
		EditorGUI.indentLevel++;

			my_target.win_condition_selected = (deck.win_condition)EditorGUILayout.EnumPopup("win condition",my_target.win_condition_selected);
			EditorGUI.indentLevel++;
			if (my_target.win_condition_selected == deck.win_condition.reach_target_score)
			{
				if (my_target.win_target_score <= 0)
					GUI.color = Color.red;
				else
					GUI.color = Color.white;
				my_target.win_target_score = EditorGUILayout.IntField("target score", my_target.win_target_score);
				GUI.color = Color.white;
			}
			my_target.play_until_no_more_cards_then_check_win_condition = EditorGUILayout.Toggle("play until no more cards, then check win condition", my_target.play_until_no_more_cards_then_check_win_condition);
			EditorGUI.indentLevel--;


			EditorGUILayout.LabelField("Deck");
			EditorGUI.indentLevel++;
				my_target.deck_card_left = EditorGUILayout.IntField("total cards", my_target.deck_card_left);
				my_target.give_an_useful_card_not_less_than_n_new_card_from_the_deck = EditorGUILayout.IntField("max number of useless card to discover before find a good card", my_target.give_an_useful_card_not_less_than_n_new_card_from_the_deck);
			EditorGUI.indentLevel--;


			my_target.undo_charges = EditorGUILayout.IntField("undo charges", my_target.undo_charges);

			EditorGUILayout.LabelField("chence of:");
				EditorGUI.indentLevel++;
				my_target.chance_of_turn_up_a_covered_card_at_start = EditorGUILayout.IntSlider("turn up a covered card at start",my_target.chance_of_turn_up_a_covered_card_at_start, 0 , 100);
				EditorGUI.indentLevel--;

		EditorGUI.indentLevel--;

		if (EditorGUI.EndChangeCheck ())
			EditorUtility.SetDirty(my_target);

		EditorGUILayout.Space();
	}

	void Score()
	{
		deck my_target = (deck)target;
		EditorGUI.BeginChangeCheck ();

		EditorGUILayout.LabelField("Score");
		EditorGUI.indentLevel++;
			my_target.normal_card_score = EditorGUILayout.IntField("normal card", my_target.normal_card_score);
			my_target.gold_card_score = EditorGUILayout.IntField("gold card", my_target.gold_card_score);
			my_target.bottom_card_score = EditorGUILayout.IntField("bottom card", my_target.bottom_card_score);		
			my_target.score_for_each_deck_card_spared = EditorGUILayout.IntField("for each deck card spared", my_target.score_for_each_deck_card_spared);	
		EditorGUI.indentLevel--;

		EditorGUILayout.Space();

		EditorGUILayout.LabelField("Combo");
		EditorGUI.indentLevel++;
			my_target.combo_score_multiplier = EditorGUILayout.FloatField("normal multiplier", my_target.combo_score_multiplier);
				EditorGUI.indentLevel++;
					my_target.min_combo_lenght_to_trigger_the_multiplier = EditorGUILayout.IntSlider("min combo lenght to trigger the multiplier",my_target.min_combo_lenght_to_trigger_the_multiplier, 1 , 10);
				EditorGUI.indentLevel--;
			my_target.color_combo_score_multiplier = EditorGUILayout.FloatField("same color multiplier", my_target.color_combo_score_multiplier);
			my_target.suit_combo_score_multiplier = EditorGUILayout.FloatField("same suit multiplier", my_target.suit_combo_score_multiplier);
		EditorGUI.indentLevel--;

		if (EditorGUI.EndChangeCheck ())
			EditorUtility.SetDirty(my_target);

		EditorGUILayout.Space();
	}
	

	void Sprites()
	{
		deck my_target = (deck)target;
		EditorGUI.BeginChangeCheck ();
        Undo.RecordObject(my_target, "edit_sprite");

        my_target.editor_show_sprites = EditorGUILayout.Foldout(my_target.editor_show_sprites, "Sprites");
		if (my_target.editor_show_sprites)
		{

			EditorGUI.indentLevel++;
			my_target.editor_show_sprites_normal = EditorGUILayout.Foldout(my_target.editor_show_sprites_normal, "normal");
			if (my_target.editor_show_sprites_normal)
				{
				EditorGUI.indentLevel++;
				if (!my_target.card_back_sprite)
					GUI.color = Color.red;
				else
					GUI.color = Color.white;
				my_target.card_back_sprite = EditorGUILayout.ObjectField("back",my_target.card_back_sprite, typeof(Sprite), false) as Sprite;
				GUI.color = Color.white;

				my_target.editor_show_sprites_normal_clubs = EditorGUILayout.Foldout(my_target.editor_show_sprites_normal_clubs, "clubs");
				if (my_target.editor_show_sprites_normal_clubs)
					{
					EditorGUI.indentLevel++;
					for (int i = 0; i < 13; i++)
						{
						if (!my_target.clubs[i])
							GUI.color = Color.red;
						else
							GUI.color = Color.white;

						my_target.clubs[i] = EditorGUILayout.ObjectField(Sprite_name(i),my_target.clubs[i], typeof(Sprite), false) as Sprite;
						GUI.color = Color.white;
						}
					EditorGUI.indentLevel--;
					}

				my_target.editor_show_sprites_normal_spades = EditorGUILayout.Foldout(my_target.editor_show_sprites_normal_spades, "spades");
				if (my_target.editor_show_sprites_normal_spades)
				{
					EditorGUI.indentLevel++;
					for (int i = 0; i < 13; i++)
					{
						if (!my_target.spades[i])
							GUI.color = Color.red;
						else
							GUI.color = Color.white;

						my_target.spades[i] = EditorGUILayout.ObjectField(Sprite_name(i),my_target.spades[i], typeof(Sprite), false) as Sprite;
						GUI.color = Color.white;
					}
					EditorGUI.indentLevel--;
				}

				my_target.editor_show_sprites_normal_diamonds = EditorGUILayout.Foldout(my_target.editor_show_sprites_normal_diamonds, "diamonds");
				if (my_target.editor_show_sprites_normal_diamonds)
				{
					EditorGUI.indentLevel++;
					for (int i = 0; i < 13; i++)
					{
						if (!my_target.diamonds[i])
							GUI.color = Color.red;
						else
							GUI.color = Color.white;
						
						my_target.diamonds[i] = EditorGUILayout.ObjectField(Sprite_name(i),my_target.diamonds[i], typeof(Sprite), false) as Sprite;
						GUI.color = Color.white;
					}
					EditorGUI.indentLevel--;
				}

				my_target.editor_show_sprites_normal_hearts = EditorGUILayout.Foldout(my_target.editor_show_sprites_normal_hearts, "hearts");
				if (my_target.editor_show_sprites_normal_hearts)
				{
					EditorGUI.indentLevel++;
					for (int i = 0; i < 13; i++)
					{
						if (!my_target.hearts[i])
							GUI.color = Color.red;
						else
							GUI.color = Color.white;
						
						my_target.hearts[i] = EditorGUILayout.ObjectField(Sprite_name(i),my_target.hearts[i], typeof(Sprite), false) as Sprite;
						GUI.color = Color.white;
					}
					EditorGUI.indentLevel--;
				}
				
				EditorGUI.indentLevel--;
				}

			my_target.editor_show_sprites_gold = EditorGUILayout.Foldout(my_target.editor_show_sprites_gold, "gold");
			if (my_target.editor_show_sprites_gold)
			{
				EditorGUI.indentLevel++;
				if (!my_target.gold_card_back_sprite)
					GUI.color = Color.red;
				else
					GUI.color = Color.white;
				my_target.gold_card_back_sprite = EditorGUILayout.ObjectField("back",my_target.gold_card_back_sprite, typeof(Sprite), false) as Sprite;
				GUI.color = Color.white;

				my_target.editor_show_sprites_gold_clubs = EditorGUILayout.Foldout(my_target.editor_show_sprites_gold_clubs, "clubs");
				if (my_target.editor_show_sprites_gold_clubs)
				{
					EditorGUI.indentLevel++;
					for (int i = 0; i < 13; i++)
					{
						if (!my_target.gold_clubs[i])
							GUI.color = Color.red;
						else
							GUI.color = Color.white;
						
						my_target.gold_clubs[i] = EditorGUILayout.ObjectField(Sprite_name(i),my_target.gold_clubs[i], typeof(Sprite), false) as Sprite;
						GUI.color = Color.white;
					}
					EditorGUI.indentLevel--;
				}
				
				my_target.editor_show_sprites_gold_spades = EditorGUILayout.Foldout(my_target.editor_show_sprites_gold_spades, "spades");
				if (my_target.editor_show_sprites_gold_spades)
				{
					EditorGUI.indentLevel++;
					for (int i = 0; i < 13; i++)
					{
						if (!my_target.gold_spades[i])
							GUI.color = Color.red;
						else
							GUI.color = Color.white;
						
						my_target.gold_spades[i] = EditorGUILayout.ObjectField(Sprite_name(i),my_target.gold_spades[i], typeof(Sprite), false) as Sprite;
						GUI.color = Color.white;
					}
					EditorGUI.indentLevel--;
				}
				
				my_target.editor_show_sprites_gold_diamonds = EditorGUILayout.Foldout(my_target.editor_show_sprites_gold_diamonds, "diamonds");
				if (my_target.editor_show_sprites_gold_diamonds)
				{
					EditorGUI.indentLevel++;
					for (int i = 0; i < 13; i++)
					{
						if (!my_target.gold_diamonds[i])
							GUI.color = Color.red;
						else
							GUI.color = Color.white;
						
						my_target.gold_diamonds[i] = EditorGUILayout.ObjectField(Sprite_name(i),my_target.gold_diamonds[i], typeof(Sprite), false) as Sprite;
						GUI.color = Color.white;
					}
					EditorGUI.indentLevel--;
				}
				
				my_target.editor_show_sprites_gold_hearts = EditorGUILayout.Foldout(my_target.editor_show_sprites_gold_hearts, "hearts");
				if (my_target.editor_show_sprites_gold_hearts)
				{
					EditorGUI.indentLevel++;
					for (int i = 0; i < 13; i++)
					{
						if (!my_target.gold_hearts[i])
							GUI.color = Color.red;
						else
							GUI.color = Color.white;
						
						my_target.gold_hearts[i] = EditorGUILayout.ObjectField(Sprite_name(i),my_target.gold_hearts[i], typeof(Sprite), false) as Sprite;
						GUI.color = Color.white;
					}
					EditorGUI.indentLevel--;
				}
				
				EditorGUI.indentLevel--;
			}

			EditorGUI.indentLevel--;
		}

		if (EditorGUI.EndChangeCheck ())
			EditorUtility.SetDirty(my_target);

		EditorGUILayout.Space();
	}

	void Sfx()
	{
		deck my_target = (deck)target;
		EditorGUI.BeginChangeCheck ();
        Undo.RecordObject(my_target, "edit_sfx");

        my_target.editor_show_sfx = EditorGUILayout.Foldout(my_target.editor_show_sfx, "Audio sfx");
		if (my_target.editor_show_sfx)
		{
			EditorGUI.indentLevel++;
			my_target.take_this_card_sfx = EditorGUILayout.ObjectField("take card", my_target.take_this_card_sfx, typeof(AudioClip), false) as AudioClip;
			my_target.new_card_sfx = EditorGUILayout.ObjectField("new card", my_target.new_card_sfx, typeof(AudioClip), false) as AudioClip;

			my_target.blocked_card_sfx = EditorGUILayout.ObjectField("blocked card", my_target.blocked_card_sfx, typeof(AudioClip), false) as AudioClip;
			my_target.wrong_card_sfx = EditorGUILayout.ObjectField("wrong card", my_target.wrong_card_sfx, typeof(AudioClip), false) as AudioClip;

			my_target.win_sfx = EditorGUILayout.ObjectField("win", my_target.win_sfx, typeof(AudioClip), false) as AudioClip;
			my_target.lose_sfx = EditorGUILayout.ObjectField("lose", my_target.lose_sfx, typeof(AudioClip), false) as AudioClip;

			EditorGUI.indentLevel--;
		}
		if (EditorGUI.EndChangeCheck ())
			EditorUtility.SetDirty(my_target);

		EditorGUILayout.Space();
	}

	void Advanced()
	{
		deck my_target = (deck)target;
		EditorGUI.BeginChangeCheck ();
        Undo.RecordObject(my_target, "edit_advanced");

        my_target.editor_show_advanced = EditorGUILayout.Foldout(my_target.editor_show_advanced, "Advanced");
		if (my_target.editor_show_advanced)
		{
			EditorGUI.indentLevel++;
			my_target.editor_show_gui = EditorGUILayout.Foldout(my_target.editor_show_gui, "GUI");
			if (my_target.editor_show_gui)
			{
				EditorGUI.indentLevel++;
				my_target.show_win_or_lose_screen_after_delay = EditorGUILayout.FloatField("show win/lose screen after",my_target.show_win_or_lose_screen_after_delay);

				my_target.game_screen =  EditorGUILayout.ObjectField("game screen", my_target.game_screen , typeof(GameObject), true) as GameObject;
				EditorGUI.indentLevel++;
					my_target.score_text = EditorGUILayout.ObjectField("score text", my_target.score_text, typeof(Text), true) as Text;
					my_target.target_score_slider = EditorGUILayout.ObjectField("target score slider", my_target.target_score_slider, typeof(Slider), true) as Slider;
					my_target.target_score_text = EditorGUILayout.ObjectField("target score text", my_target.target_score_text, typeof(Text), true) as Text;


					my_target.combo_text = EditorGUILayout.ObjectField("combo text", my_target.combo_text, typeof(Text), true) as Text;
						my_target.end_combo_obj =  EditorGUILayout.ObjectField("end combo", my_target.end_combo_obj , typeof(GameObject), true) as GameObject;
						my_target.total_combo_multiplier_text = EditorGUILayout.ObjectField("total combo multiplier text", my_target.total_combo_multiplier_text, typeof(Text), true) as Text;

					my_target.deck_card_left_text = EditorGUILayout.ObjectField("deck card left text", my_target.deck_card_left_text, typeof(Text), true) as Text;
					//my_target.gold_cards_count_text = EditorGUILayout.ObjectField("gold card count", my_target.gold_cards_count_text, typeof(Text), true) as Text;


					my_target.undo_button = EditorGUILayout.ObjectField("undo button", my_target.undo_button, typeof(Button), true) as Button;
					my_target.undo_count_text = EditorGUILayout.ObjectField("undo count", my_target.undo_count_text, typeof(Text), true) as Text;
				EditorGUI.indentLevel--;

				my_target.end_screen =  EditorGUILayout.ObjectField("end screen", my_target.end_screen , typeof(GameObject), true) as GameObject;
					EditorGUI.indentLevel++;
						my_target.end_screen_name = EditorGUILayout.ObjectField("end screen name", my_target.end_screen_name, typeof(Text), true) as Text;
						my_target.final_score = EditorGUILayout.ObjectField("final score", my_target.final_score, typeof(Text), true) as Text;
						my_target.target_score_end_screen_text = EditorGUILayout.ObjectField("target score", my_target.target_score_end_screen_text, typeof(Text), true) as Text;
					EditorGUI.indentLevel--;

				EditorGUI.indentLevel--;
			}

			EditorGUILayout.Space();
			my_target.ghost_card =  EditorGUILayout.ObjectField("ghost card", my_target.ghost_card , typeof(Transform), true) as Transform;
			my_target.board =  EditorGUILayout.ObjectField("board", my_target.board , typeof(Transform), true) as Transform;
			my_target.target_new_card =  EditorGUILayout.ObjectField("target_new_card", my_target.target_new_card , typeof(SpriteRenderer), true) as SpriteRenderer;
			my_target.new_card_anim_sprite =  EditorGUILayout.ObjectField("new_card_anim_sprite", my_target.new_card_anim_sprite , typeof(SpriteRenderer), true) as SpriteRenderer;
			my_target.new_card_anim =  EditorGUILayout.ObjectField("new_card_anim", my_target.new_card_anim , typeof(Animation), true) as Animation;


			EditorGUI.indentLevel--;
		}
		
		if (EditorGUI.EndChangeCheck ())
			EditorUtility.SetDirty(my_target);

	}

	string Sprite_name(int card_value)
	{
		string return_this = "";

		if (card_value == 0)
			return_this = "A";
		else if (card_value == 10)
			return_this = "J";
		else if (card_value == 11)
			return_this = "Q";
		else if (card_value == 12)
			return_this = "K";
		else
			return_this = (card_value + 1).ToString();

		return return_this;
	}
}
