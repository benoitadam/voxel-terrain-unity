//using UnityEngine;
//using UnityEditor;
//
//[CustomEditor(typeof(VBrush))] 
//public class VoxelTerrainWindow : Editor
//{
//	
//	
//	public int toolbarInt = 0;
//    public string[] toolbarStrings = new string[] {"Toolbar1", "Toolbar2", "Toolbar3"};
//	
//	private bool    _paintEnable = false;
//	private float   _paintDistance = 10F;
//	private Vector3 _paintPosition;
//	private float   _paintSize = 1F; 
//	
//	public void OnSceneGUI()
//	{
//		VBrush brush = ((VBrush)target);
//		
//		Handles.BeginGUI();
//		
//		// Get event
//		Event e = Event.current;
//		if (e!=null)
//		{
//			// Get paintPosition
//			Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);
//			object hit = HandleUtility.RaySnap(ray);
//			if(hit!=null)
//			{
//				if(_paintEnable == false)
//					_paintDistance = ((RaycastHit)hit).distance;
//			}
//			_paintPosition = ray.GetPoint(_paintDistance);
//			
//			brush.transform.position = _paintPosition;
//			brush.transform.localScale = new Vector3(1,1,1)*_paintSize;
//			
//			// Get paint position
//			
//			
//			switch(e.type)
//			{
//			case EventType.MouseDown : _paintEnable = true;  break;
//			case EventType.MouseUp :   _paintEnable = false; break;
//			}
//			
//			if(_paintEnable == true)
//				brush.Paint(_paintPosition, _paintSize, VBrush.SFX.ADD_SPHERE);
//		}
//		
//		//GUILayout.Box(paintDistance.ToString());
//		
//		Handles.EndGUI();
//		//GUILayout.BeginArea(target.ViewRect);
//		
//		
////		Event e = Event.current;
////		if (e!=null) switch(e.type)
////		{
////		case EventType.MouseDown :
////		case EventType.MouseUp :
////		case EventType.MouseMove :
////			
////			Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);
////			RaycastHit hit = (RaycastHit)HandleUtility.RaySnap(ray);
////			
////			
////			//Display.Point(hit.point, Color.red);
////			
////			Event.current.Use();
////			
////			break;
////		}
//		
//		
//		
//		//wantsMouseMove = EditorGUILayout.Toggle("Receive Movement: ", wantsMouseMove);
//		//Gui.LabelField("Mouse Position: ", Event.current.mousePosition.ToString());
//		
//		
//		
//		//sceneview.camera.
//		
//		//selected = GUI.SelectionGrid(new Rect(25, 25, 250, 30), selected, new St
//		 //GUI.Button(new Rect(25, 25, 250, 30), "paint");
//		//layer = EditorGUI.LayerField(new Rect(25, 25, 250, 30), layer);
//		//toolbarInt = GUI.Toolbar(new Rect(25, 25, 250, 30), toolbarInt, toolbarStrings);
//        //GUILayout.Label("Base Settings", EditorStyles.boldLabel);
//		
//	}
//	
////	public override void OnInspectorGUI()
////	{
////	}
//}