using UnityEngine;
using System.Collections;

public class VBrush : MonoBehaviour {
	
	// IHM
	/*public Texture addTex;
	public Texture subTex;
	public Texture dilationTex;
	public Texture paintTex;
	public Texture randomTex;
	public Texture sphereTex;
	public Texture cubeTex;*/
	
	// Paint
	private float _distance   = 100F;
	private Vector3 _position = Vector3.zero;
	private float _size       = 2F;
	private bool _paintEnable = false;
	private VTerrain.OBJ _obj = VTerrain.OBJ.SPHERE;
	private VTerrain.SFX _sfx = VTerrain.SFX.ADD;
	private Color _color      = new Color(1F,1F,1F,1F);
	private Ray _ray;
	private RaycastHit _hit;
	private float _lastClick=0;
	
	// Camera
	private bool _rotationEnable = false;
	
	void DrawGUI()
	{
		// Wrap everything in the designated GUI Area
		GUILayout.BeginArea(new Rect(2,2,100,Screen.height-4));
		
			GUILayout.BeginVertical();
			
				GUILayout.Label("Effect");
		
				if (GUILayout.RepeatButton("Addition"))		{_sfx = VTerrain.SFX.ADD;}
				if (GUILayout.RepeatButton("Subtraction"))	{_sfx = VTerrain.SFX.SUB;}
				if (GUILayout.RepeatButton("Dilation"))		{_sfx = VTerrain.SFX.DILATION;}
				if (GUILayout.RepeatButton("Erosion"))		{Debug.Log("erosion");}
				
				GUILayout.Label("Object");
				
				if (GUILayout.RepeatButton("Random"))		{_obj = VTerrain.OBJ.RANDOM;}
				if (GUILayout.RepeatButton("Sphere"))		{_obj = VTerrain.OBJ.SPHERE;}
				if (GUILayout.RepeatButton("Cube"))			{_obj = VTerrain.OBJ.CUBE;  }
		
				GUILayout.Label("Paint");
		
				if (GUILayout.RepeatButton("Paint"))		{_sfx = VTerrain.SFX.PAINT;}
				
				GUILayout.Label("Texture");
		
				_color.r = GUILayout.HorizontalSlider(_color.r, 0, 10);
		
				GUILayout.Label("Brush size");
		
				_size = GUILayout.HorizontalSlider(_size, 0, 10);
			
			GUILayout.EndVertical();
		
		GUILayout.EndArea();
		
		/*
		if(GUI.RepeatButton(new Rect(10,10,50,50), randomTex))
		{
			
		}
		if(GUI.enabled)
		{
			Debug.Log("click");	
		}
		GUILayout.Button(sphereTex);
		/*Rect box = GetRect(0,0,1,5);
		GUI.Box(GetRect(0,0,1,5), "Paint Menu");
		NewBox("Paint Menu", 7);
		if(GUI.Button(InBox(), randomTex))   { _obj = VTerrain.OBJ.RANDOM;   _lastClick=Time.time; return; }
		if(GUI.Button(InBox(), sphereTex))   { _obj = VTerrain.OBJ.SPHERE;   _lastClick=Time.time; return; }
		if(GUI.Button(InBox(), cubeTex))     { _obj = VTerrain.OBJ.CUBE;     _lastClick=Time.time; return; }
		if(GUI.Button(InBox(), addTex))      { _sfx = VTerrain.SFX.ADD;      _lastClick=Time.time; return; }
		if(GUI.Button(InBox(), subTex))      { _sfx = VTerrain.SFX.SUB;      _lastClick=Time.time; return; }
		if(GUI.Button(InBox(), dilationTex)) { _sfx = VTerrain.SFX.DILATION; _lastClick=Time.time; return; }
		if(GUI.Button(InBox(), paintTex))    { _sfx = VTerrain.SFX.PAINT;    _lastClick=Time.time; return; }
	*/
	}
	
	void OnGUI () {
		
		DrawGUI();
		
		//GUI.skin.button.stretchHeight = false;
		
		// Button
//		if(GUI.Button(GetRectPos(0,0), randomTex))   { _obj = VTerrain.OBJ.RANDOM;   _lastClick=Time.time; return; }
//		if(GUI.Button(GetRectPos(0,1), sphereTex))   { _obj = VTerrain.OBJ.SPHERE;   _lastClick=Time.time; return; }
//		if(GUI.Button(GetRectPos(0,2), cubeTex))     { _obj = VTerrain.OBJ.CUBE;     _lastClick=Time.time; return; }
//		if(GUI.Button(GetRectPos(1,0), addTex))      { _sfx = VTerrain.SFX.ADD;      _lastClick=Time.time; return; }
//		if(GUI.Button(GetRectPos(1,1), subTex))      { _sfx = VTerrain.SFX.SUB;      _lastClick=Time.time; return; }
//		if(GUI.Button(GetRectPos(2,0), dilationTex)) { _sfx = VTerrain.SFX.DILATION; _lastClick=Time.time; return; }
//		if(GUI.Button(GetRectPos(3,0), paintTex))    { _sfx = VTerrain.SFX.PAINT;    _lastClick=Time.time; return; }
//		
		// Display color
		//GUI.color = _color;
		//GUI.Box(GetRectPos(3,1), "color");
		
		//Rect rectColor = GetRectPos(4,0);
//		rectColor.width *= 2;
//		rectColor.height = 15;
//		_color.r = GUI.HorizontalScrollbar(rectColor, _color.r, 0.1F, 0F, 1F);
//		rectColor.y += 20;
//		_color.g = GUI.HorizontalScrollbar(rectColor, _color.g, 0.1F, 0F, 1F);
//		rectColor.y += 20;
//		_color.b = GUI.HorizontalScrollbar(rectColor, _color.b, 0.1F, 0F, 1F);
		
		
		
		// left click
		if(Input.GetMouseButtonDown(0))
		{
			if(_lastClick<Time.time-1F)
				_paintEnable = true;
		}
		if(Input.GetMouseButtonUp(0))	_paintEnable = false;
		
		// right click
		if(Input.GetMouseButtonDown(1))	_rotationEnable=true;
		if(Input.GetMouseButtonUp(1))	_rotationEnable=false;
		
		// Camera controle with right click
		if(_rotationEnable)
		{
			float h = 100F * Input.GetAxis ("Mouse X");
    		float v = -100F * Input.GetAxis ("Mouse Y");
			Camera.main.transform.RotateAround(new Vector3(100,100,100), Vector3.up,    h*Time.deltaTime);
			Camera.main.transform.RotateAround(new Vector3(100,100,100), Vector3.right, v*Time.deltaTime);
			return;
		}
		
		// Paint
		if(_paintEnable)
		{
			// Get paintPosition
			_ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			
			// Find point to paint, if point find
			if(Physics.Raycast(_ray, out _hit))
				
				// Get distance
				_distance = _hit.distance;
			
			// Get point
			_position = _ray.GetPoint(_distance);
			
			// Display mesh in paint point
			transform.localScale = new Vector3(_size, _size, _size);
			transform.position = _position;
			
		}
		else VTerrain.Instance.ReBuildCollider();
		
		VTerrain.Instance.Alteration(_position, new Vector3(_size, _size, _size), _obj, _sfx, _color);
		
	}
	
	/*private Rect GetRectPos(int y, int x)
	{
		Rect r = new Rect(rect);
		r.x += (r.x + r.width)  * x;
		r.y += (r.y + r.height) * y;
		return r;
	}*/
	
}
