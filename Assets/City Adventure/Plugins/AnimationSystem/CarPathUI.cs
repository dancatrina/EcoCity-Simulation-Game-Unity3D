#if UNITY_EDITOR
using UnityEngine;
using System.Collections;
using UnityEditor;


[CustomEditor(typeof(CarPath))]
public class CarPathUI : Editor {

	public override void OnInspectorGUI(){

		CarPath script = (CarPath)target;

		if(GUILayout.Button("Add Node")){

			script.AddNode();

		}

		//if(GUILayout.Button("Remove Node")){

			//script.RemoveNode();

		//}

		if(GUILayout.Button("Recalculate path")){

			if(Application.isPlaying) {
				script.RecalculatePath();
			} else {
				Debug.Log("Use RecalculatePath in Play mode, the path gets updated automatically in Edit mode");
			}

		}

		if(GUILayout.Button("Play")){

			script.Play();

		}

		if(GUILayout.Button("Stop")){

			script.Stop();

		}

	
		if(GUILayout.Button("Reevaluate Path")){

			script.ReevaluatePath();

		}

		if(GUILayout.Button("Sample start rotation")){

			script.SampleRotation();

		}

		if(GUILayout.Button("Move To Start Position")){

			script.MTSP();

		}

		DrawDefaultInspector();

	}
}
#endif
