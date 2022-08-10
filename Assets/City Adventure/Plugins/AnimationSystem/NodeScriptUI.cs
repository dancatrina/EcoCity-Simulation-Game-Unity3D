#if UNITY_EDITOR
using UnityEngine;
using System.Collections;
using UnityEditor;


[CanEditMultipleObjects]
[CustomEditor(typeof(NodeScript))]
public class NodeScriptUI : Editor {

	public override void OnInspectorGUI(){

		NodeScript script = (NodeScript)target;

		if(GUILayout.Button("Add Node")){

			script.AddNode();

		}

		if(GUILayout.Button("Remove Node")){

			script.RemoveNode();

		}



		DrawDefaultInspector();

	}
}

#endif
