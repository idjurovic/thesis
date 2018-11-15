using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(card))]
internal class card_editor : Editor {

	public override void OnInspectorGUI()
	{
		card my_target = (card)target;
		EditorGUI.BeginChangeCheck ();
        Undo.RecordObject(my_target, "edit_card");

        my_target.my_setup_selected = (card.my_setup)EditorGUILayout.EnumPopup("setup",my_target.my_setup_selected);
		if (my_target.my_setup_selected == card.my_setup.manual)
		{
			EditorGUI.indentLevel++;
			my_target.card_type_selected = (card.card_type)EditorGUILayout.EnumPopup("card type",my_target.card_type_selected);
			//if (my_target.card_type_selected == card.card_type.normal)
			//{
				EditorGUI.indentLevel++;

				my_target.card_suit_selected = (card.card_suit)EditorGUILayout.EnumPopup("suit",my_target.card_suit_selected);
				if (my_target.card_suit_selected != card.card_suit.random)
					my_target.my_suit = (int)my_target.card_suit_selected;

				my_target.card_rank_selected = (card.card_rank)EditorGUILayout.EnumPopup("rank",my_target.card_rank_selected);
				if (my_target.card_rank_selected != card.card_rank.random)
					my_target.my_rank = (int)my_target.card_rank_selected;

				my_target.always_face_up = EditorGUILayout.Toggle("always face up", my_target.always_face_up);


				EditorGUI.indentLevel--;
			//}
			EditorGUI.indentLevel--;
		}
		else //automatic
		{
			my_target.card_type_selected = card.card_type.normal;
			my_target.card_suit_selected = card.card_suit.random;
			my_target.card_rank_selected = card.card_rank.random;
			my_target.face_up = false;
		}



		my_target.editor_show_overlap = EditorGUILayout.Foldout(my_target.editor_show_overlap, "overlap detector");
		if (my_target.editor_show_overlap)
		{
			EditorGUI.indentLevel++;
			if (my_target.big_radius <= 0)
				GUI.color = Color.red;
			else
				GUI.color = Color.white;
			my_target.big_radius = EditorGUILayout.FloatField("big radius",my_target.big_radius);
			GUI.color = Color.white;

			if (my_target.small_radius <= 0)
				GUI.color = Color.red;
			else
				GUI.color = Color.white;
			my_target.small_radius = EditorGUILayout.FloatField("small radius",my_target.small_radius);
			GUI.color = Color.white;

			for (int i = 0; i < my_target.overlap_small.Length; i++)
			{
				my_target.overlap_small[i] =  EditorGUILayout.ObjectField("small " + i, my_target.overlap_small[i] , typeof(Transform), true) as Transform;

			}

			EditorGUI.indentLevel--;
		}

		if (EditorGUI.EndChangeCheck ())
			EditorUtility.SetDirty(my_target);
		
	}

}
