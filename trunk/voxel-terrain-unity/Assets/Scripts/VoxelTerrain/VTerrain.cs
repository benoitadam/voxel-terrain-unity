using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using VoxelTerrain;

public class VTerrain : MonoBehaviour {
	
	public enum OBJ {
		SPHERE,
		CUBE,
		RANDOM
	}
	
	public enum SFX {
		ADD,
		SUB,
		DILATION,
		EROSION,
		PAINT
	}
	
	private static VTerrain _instance;
	public static VTerrain Instance { get { return _instance; } }
	
	public int width=100, height=100, depth=100;
	
	
	private List<VCube> _cubes;				// All cube
	private List<VCube> _reBuild;			// Cube for rebuild
	private List<VCube> _reBuildCollider;	// Collider rebuild
	private int _reBuildColliderCount;		// Nombre of collider to rebuild
	
	// number of x, y, z in one cube
	private static int RES = 25;
	
	public byte[,,] _map;
	public Color[,,] _colors;
	
	public void Start () {
		
		if(_instance==null)
			_instance = this;
		
		_cubes					= new List<VCube>();
		_reBuild				= new List<VCube>();
		_reBuildCollider		= new List<VCube>();
		_reBuildColliderCount	= 0;
		
		_map					= new byte[width+1, height+1, depth+1];
		_colors					= new Color[width+1, height+1, depth+1];
		
		// Instantiate all cube
		for(int x=0; x<width/RES; x++)
		for(int y=0; y<height/RES; y++)
		for(int z=0; z<depth/RES; z++)
		{
			Bounds cubeBounds = new Bounds();
			cubeBounds.min = new Vector3(x, y, z)*RES;
			cubeBounds.max = cubeBounds.min + new Vector3(RES, RES, RES);
			VCube cube = new VCube(cubeBounds, this);
			_cubes.Add(cube);
		}
		
		// Initialise all point in map
		ResetMap();
		
	}
	
	// Update is called once per frame
	// build one cube per frame and collider when ReBuildCollider is call
	public void Update () {
		
		// Build mesh
		if(_reBuild.Count != 0)
		{
			_reBuild[0].ReBuild();
			_reBuild.RemoveAt(0);
		}
		
		// Build collider
		if(_reBuildColliderCount != 0)
		{
			// Build all cube in the last ReBuildCollider() call
			_reBuildColliderCount--;
			_reBuildCollider[0].ReBuildCollider();
			_reBuildCollider.RemoveAt(0);
		}
		
	}
	
	public void Alteration(Vector3 position, Vector3 scale, OBJ obj, SFX sfx, Color color)
	{
		// Get point in terrain location
		Matrix4x4 matrix = transform.worldToLocalMatrix;
		position = matrix.MultiplyPoint(position);
		scale = matrix.MultiplyVector(scale);
		Bounds bounds = new Bounds(position, scale);
		
		int sX = (int)scale.x/2+1,
			sY = (int)scale.y/2+1,
			sZ = (int)scale.z/2+1,
			bX = Mathf.Max((int)position.x-sX, 1),
			bY = Mathf.Max((int)position.y-sY, 1),
			bZ = Mathf.Max((int)position.z-sZ, 1),
			eX = Mathf.Min((int)position.x+sX+2, width-2),
			eY = Mathf.Min((int)position.y+sY+2, height-2),
			eZ = Mathf.Min((int)position.z+sZ+2, depth-2);
		
		// inverse effect for subtraction or erosion
		if(sfx==SFX.SUB || sfx==SFX.EROSION)
			for(int x=bX; x<eX; x++)
			for(int y=bY; y<eY; y++)
			for(int z=bZ; z<eZ; z++)
				_map[x,y,z] = (byte)(255-_map[x,y,z]);
		
		// Begin of effect
		
		switch(sfx)
		{
		case SFX.ADD :
		case SFX.SUB :
			switch(obj)
			{
			case OBJ.CUBE   : AddCube  (bounds, bX, bY, bZ, eX, eY, eZ, color);	break;
			case OBJ.SPHERE : AddSphere(bounds, bX, bY, bZ, eX, eY, eZ, color);	break;
			case OBJ.RANDOM : AddRandom(bounds, bX, bY, bZ, eX, eY, eZ, color);	break;
			}
			break;
		case SFX.DILATION :
		case SFX.EROSION :
			switch(obj)
			{
			case OBJ.CUBE   : DilationCube  (bounds, bX, bY, bZ, eX, eY, eZ, color);	break;
			case OBJ.SPHERE : DilationSphere(bounds, bX, bY, bZ, eX, eY, eZ, color);	break;
			case OBJ.RANDOM : DilationRandom(bounds, bX, bY, bZ, eX, eY, eZ, color);	break;
			}
			break;
		case SFX.PAINT :
			switch(obj)
			{
			case OBJ.CUBE   : PaintCube  (bounds, bX, bY, bZ, eX, eY, eZ, color);	break;
			case OBJ.SPHERE : PaintSphere(bounds, bX, bY, bZ, eX, eY, eZ, color);	break;
			case OBJ.RANDOM : PaintRandom(bounds, bX, bY, bZ, eX, eY, eZ, color);	break;
			}
			break;
		}
		
		// End effect
		
		// inverse effect for subtraction or erosion
		if(sfx==SFX.SUB || sfx==SFX.EROSION)
			for(int x=bX; x<eX; x++)
			for(int y=bY; y<eY; y++)
			for(int z=bZ; z<eZ; z++)
				_map[x,y,z] = (byte)(255-_map[x,y,z]);
		
		// rebuild map in this bounds
		bounds.SetMinMax(new Vector3(bX, bY, bZ), new Vector3(eX, eY, eZ));
		ReBuild(bounds);
		
	}
	
	// Effect
	private void AddCube(Bounds bounds, int bX, int bY, int bZ, int eX, int eY, int eZ, Color color)
	{
		for(int x=bX; x<eX; x++)
		for(int y=bY; y<eY; y++)
		for(int z=bZ; z<eZ; z++)
			_map[x,y,z] = 0;
	}
	private void AddSphere(Bounds bounds, int bX, int bY, int bZ, int eX, int eY, int eZ, Color color)
	{
		Vector3 center = bounds.center;
		float radius = Mathf.Min(Mathf.Min(bounds.size.x, bounds.size.y), bounds.size.z)/2F;
		for(int x=bX; x<eX; x++)
		for(int y=bY; y<eY; y++)
		for(int z=bZ; z<eZ; z++)
		{
			// get distance for marching cubes
			float dist = (Vector3.Distance(new Vector3(x,y,z), center)-radius)*255;
			byte bVal = (byte)(dist>255?255:(dist<0?0:dist));
			if(bVal<_map[x,y,z])
				_map[x,y,z] = bVal;
		}
	}
	private void AddRandom(Bounds bounds, int bX, int bY, int bZ, int eX, int eY, int eZ, Color color)
	{
		Vector3 center = bounds.center;
		float radius = Mathf.Min(Mathf.Min(bounds.size.x, bounds.size.y), bounds.size.z)/2F;
		for(int x=bX; x<eX; x++)
		for(int y=bY; y<eY; y++)
		for(int z=bZ; z<eZ; z++)
		{
			// get distance for marching cubes
			float dist = (Vector3.Distance(new Vector3(x,y,z), center)-radius)*0.5F;
			dist = (dist+Random.value)*255;
			byte bVal = (byte)(dist>255?255:(dist<0?0:dist));
			if(bVal<_map[x,y,z])
				_map[x,y,z] = bVal;
		}
	}
	private void DilationCube(Bounds bounds, int bX, int bY, int bZ, int eX, int eY, int eZ, Color color)
	{
		for(int x=bX; x<eX-1; x++)
		for(int y=bY; y<eY-1; y++)
		for(int z=bZ; z<eZ-1; z++)
		{
			byte min = _map[x,y,z];
			for(int ix=x; ix<x+1; ix++)
			for(int iy=y; iy<y+1; iy++)
			for(int iz=z; iz<z+1; iz++)
			{
				if(min>_map[ix,iy,iz])
					min=_map[ix,iy,iz];
				Debug.Log("min"+min+"map"+_map[ix,iy,iz]);
				Debug.Break();
			}
			_map[x,y,z] = min;
		}
	}
	private void DilationSphere(Bounds bounds, int bX, int bY, int bZ, int eX, int eY, int eZ, Color color)
	{
		Vector3 center = bounds.center;
		float radius = Mathf.Min(Mathf.Min(bounds.size.x, bounds.size.y), bounds.size.z)/2F;
		for(int x=bX+1; x<eX-1; x++)
		for(int y=bY+1; y<eY-1; y++)
		for(int z=bZ+1; z<eZ-1; z++)
		{
			if((Vector3.Distance(new Vector3(x,y,z), center)-radius)>0.5F)
				continue;
			byte min = _map[x,y,z];
			if(min>_map[x-1,y,z])min=_map[x-1,y,z];
			if(min>_map[x+1,y,z])min=_map[x+1,y,z];
			if(min>_map[x,y-1,z])min=_map[x,y-1,z];
			if(min>_map[x,y+1,z])min=_map[x,y+1,z];
			if(min>_map[x,y,z-1])min=_map[x,y,z-1];
			if(min>_map[x,y,z+1])min=_map[x,y,z+1];
			_map[x,y,z]=min;
		}
	}
	private void DilationRandom(Bounds bounds, int bX, int bY, int bZ, int eX, int eY, int eZ, Color color)
	{
		for(int x=bX+1; x<eX-1; x++)
		for(int y=bY+1; y<eY-1; y++)
		for(int z=bZ+1; z<eZ-1; z++)
		{
			if(Random.value>0.5F)
				continue;
			byte min = _map[x,y,z];
			if(min>_map[x-1,y,z])min=_map[x-1,y,z];
			if(min>_map[x+1,y,z])min=_map[x+1,y,z];
			if(min>_map[x,y-1,z])min=_map[x,y-1,z];
			if(min>_map[x,y+1,z])min=_map[x,y+1,z];
			if(min>_map[x,y,z-1])min=_map[x,y,z-1];
			if(min>_map[x,y,z+1])min=_map[x,y,z+1];
			_map[x,y,z]=min;
		}
	}
	private void PaintCube(Bounds bounds, int bX, int bY, int bZ, int eX, int eY, int eZ, Color color)
	{
			
	}
	private void PaintSphere(Bounds bounds, int bX, int bY, int bZ, int eX, int eY, int eZ, Color color)
	{
			
	}
	private void PaintRandom(Bounds bounds, int bX, int bY, int bZ, int eX, int eY, int eZ, Color color)
	{
		
	}
	
	/*
	public void AddSphere(Vector3 position, Vector3 scale)
	{
		
		
		
		
	}
	*/
	void OnDrawGizmosSelected()
	{
		Matrix4x4 matrix = transform.localToWorldMatrix;
		Gizmos.color = new Color(0.8F, 0.8F, 0.2F, 0.3F);
		for(int x=RES; x<width; x+=RES)
		{
			Vector3 start = matrix.MultiplyPoint(new Vector3(x, 0, 0));
			Vector3 end = matrix.MultiplyPoint(new Vector3(x, 0, depth));
			Gizmos.DrawLine(start, end);
		}
		for(float z=RES; z<depth; z+=RES)
		{
			Vector3 start = matrix.MultiplyPoint(new Vector3(0, 0, z));
			Vector3 end = matrix.MultiplyPoint(new Vector3(width, 0, z));
			Gizmos.DrawLine(start, end);
		}
		Vector3 size = new Vector3(width, height, depth);
		Vector3 center = size/2;
		Gizmos.color = new Color(0.8F, 0.4F, 0.3F, 0.6F);
		Display.GizmosDrawCube(center, size, matrix);
	}
	
	// Initialise all point in map to val
	private void ResetMap()
	{
		//heightmap = (Texture2D) Instantiate((Texture2D)Resources.Load("Heightmap", typeof(Texture2D)));
		//Debug.Log(heightmap.GetPixelBilinear(0.5F, 0.5F));
		//if(heightmap!=null)
			//{
				//Color c = heightmap.GetPixel(x, z);
				//float h = (float)y/height;
				//_map[x,y,z] = c.r<h&&y>1?float.MaxValue:(y==0?float.MaxValue:0F);
			//}
		for(int x=0; x<width +1; x++)
		for(int y=0; y<height+1; y++)
		for(int z=0; z<depth +1; z++)
		{
			_map[x,y,z] = 255;
		}
		
		for(int x=10; x<width +1; x+=10)
		for(int y=10; y<height+1; y+=10)
		for(int z=10; z<depth +1; z+=10)
		{
			float r = Random.value;
			for(int ix=x-10; ix<x; ix++)
			for(int iy=y-10; iy<y; iy++)
			for(int iz=z-10; iz<z; iz++)
			{
				_colors[ix,iy,iz] = new Color(r, 0F, 0F, 1F);
			}
		}
//		for(int i=0; i<10; i++)
//		{
//			for(int x=0; x<width; x++)
//			for(int y=0; y<height; y++)
//			for(int z=0; z<depth; z++)
//			{
//				_colors[x,y,z] = (_colors[x,y,z]+_colors[x+1,y,z]+_colors[x,y+1,z]+_colors[x,y,z+1])/4;
//			}
//		}
		ReBuild();
	}
	/*
	private void BordMap()
	{
		for(int z=0; z<2       ; z++)
		for(int x=0; x<width +1; x++)
		for(int y=0; y<height+1; y++)
			_map[x,y,z*depth] = 255;
		for(int y=0; y<2       ; y++)
		for(int x=0; x<width +1; x++)
		for(int z=0; z<depth +1; z++)
			_map[x,y*height,z] = 255;
		for(int x=0; x<2       ; x++)
		for(int y=0; y<height+1; y++)
		for(int z=0; z<depth +1; z++)
			_map[x*width,y,z] = 255;
	}
	*/
	
	// Rebuild this cube
	public void ReBuild(VCube cube)
	{
		// Add mesh to re build list
		if(!_reBuild.Contains(cube))
			_reBuild.Add(cube);
		
		// Add mesh collider to rebuild list
		if(!_reBuildCollider.Contains(cube))
			_reBuildCollider.Add(cube);
	}
	
	// Rebuild the cube contains point
	public void ReBuild(Vector3 point)
	{
		foreach(VCube cube in _cubes)
		if(cube.bounds.Contains(point))
		{
			ReBuild(cube);
			return;
		}
	}
	
	// Rebuild all cube in bounds of effect
	public void ReBuild(Bounds bounds)
	{
		foreach(VCube cube in _cubes)
			if(bounds.Intersects(cube.bounds))
				ReBuild(cube);
	}
	
	// Rebuild all cube
	public void ReBuild()
	{
		foreach(VCube cube in _cubes)
			ReBuild(cube);
	}
	
	// Rebuild collider after alteration
	public void ReBuildCollider()
	{
		// Get the count for ReBuild, for reduction of time process
		_reBuildColliderCount = _reBuildCollider.Count;
	}
	
	/*
	private class Alteration
	{
		private VTerrain _terrain;
		public int x0,y0,z0;
		public int x1,y1,z1;
		public Vector3 center;
		public float radius;
		public Alteration(VTerrain terrain, Bounds bounds)
		{
			x0 = Mathf.Max((int)bounds.min.x, 1);
			y0 = Mathf.Max((int)bounds.min.y, 1);
			z0 = Mathf.Max((int)bounds.min.z, 1);
			x1 = Mathf.Min((int)bounds.max.x+2, terrain.width-2);
			y1 = Mathf.Min((int)bounds.max.y+2, terrain.height-2);
			z1 = Mathf.Min((int)bounds.max.z+2, terrain.depth-2);
			center = bounds.center;
			radius = ( bounds.size.x + bounds.size.y + bounds.size.z ) / 6;
			_terrain = terrain;
		}
		public Bounds GetAlterationBounds()
		{
			Bounds b = new Bounds();
			b.min = new Vector3(x0, y0, z0);
			b.max = new Vector3(x1, y1, z1);
			return b;
		}
		public void End()
		{
			_terrain.ReBuild(GetAlterationBounds());
		}
	}
	*/
	/*
	public void AddSphere(Bounds bounds)
	{
		Alteration a = new Alteration(this, bounds);
		
		// For all point in bounds effect
		for(int x=a.x0; x<a.x1; x++)
		for(int y=a.y0; y<a.y1; y++)
		for(int z=a.z0; z<a.z1; z++)
		{
			// get distance for marching cubes
			float dist = (Vector3.Distance(new Vector3(x,y,z), a.center)-a.radius)*255;
			byte bVal = (byte)(dist>255?255:(dist<0?0:dist));
			if(bVal<_map[x,y,z])
				_map[x,y,z] = bVal;
		}
		
		// End of alteration
		a.End();
	}
	*/
	/*public void SubSphere(Bounds bounds)
	{
		Alteration a = new Alteration(this, bounds);
		
		// For all point in bounds effect
		for(int x=a.x0; x<a.x1; x++)
		for(int y=a.y0; y<a.y1; y++)
		for(int z=a.z0; z<a.z1; z++)
		{
			// get distance for marching cubes
			float dist = a.radius - Vector3.Distance(new Vector3(x,y,z), a.center);
			if(dist>_map[x,y,z])
				_map[x,y,z] = dist;
		}
		
		// End of alteration
		a.End();
	}*/
	
	/*public void Dilation(Bounds bounds, Vector3 center, int x, int y, int z)
	{
		// get distance for marching cubes
		float dist = Vector3.Distance(new Vector3(x,y,z), center) - bounds.size.x/2;
		if(dist<_map[x,y,z])
			_map[x,y,z] = dist;
	}
	
	public void Erosion(Bounds bounds, Vector3 center, int x, int y, int z)
	{
		// get distance for marching cubes
		float dist = Vector3.Distance(new Vector3(x,y,z), center) - bounds.size.x/2;
		if(dist<_map[x,y,z])
			_map[x,y,z] = dist;
	}*/
	/*
	public void Average()
	{
		for(int x=2; x<width -2; x++)
		for(int y=2; y<height-2; y++)
		for(int z=2; z<depth -2; z++)
		{
			int val = 0;
			int count = 0;
			for(int ax=x-1; ax<=x+1; ax++)
			for(int ay=y-1; ay<=y+1; ay++)
			for(int az=z-1; az<=z+1; az++)
			{
				val += _map[ax, ay, az];
				count++;
			}
			_map[x,y,z]=(byte)(val/count);
		}
	}
	*/
	// The dilation no rebuild cube
	/*private void DilationCube()
	{
		float[,,] dilation = (float[,,])_map.Clone();
		for(int x=2; x<width -2; x++)
		for(int y=2; y<height-2; y++)
		for(int z=2; z<depth -2; z++)
		{
			float min = float.MaxValue;
			for(int ax=-1; ax<=1; ax++)
			for(int ay=-1; ay<=1; ay++)
			for(int az=-1; az<=1; az++)
			{
				float val = _map[x+ax,y+ay,z+az];
				if(val<min)
					min=val;
			}
			dilation[x,y,z]=min;
		}
		_map = dilation;
	}*/
	
	/*private void DilationStar()
	{
		float[,,] dilation = (float[,,])_map.Clone();
		for(int x=2; x<width -2; x++)
		for(int y=2; y<height-2; y++)
		for(int z=2; z<depth -2; z++)
		{
			float min=dilation[x,y,z];
			if(_map[x+1,y,z]<min)min=_map[x+1,y,z];
			if(_map[x-1,y,z]<min)min=_map[x-1,y,z];
			if(_map[x,y+1,z]<min)min=_map[x,y+1,z];
			if(_map[x,y-1,z]<min)min=_map[x,y-1,z];
			if(_map[x,y,z+1]<min)min=_map[x,y,z+1];
			if(_map[x,y,z-1]<min)min=_map[x,y,z-1];
			dilation[x,y,z]=min;
		}
		_map = dilation;
	}*/
	
	public GameObject AddObject()
	{
		GameObject obj = new GameObject("VCube");
		obj.transform.parent = transform;
		obj.AddComponent<MeshRenderer>().materials = renderer.materials;
		obj.AddComponent<MeshFilter>();
		obj.AddComponent<MeshCollider>();
		return obj;
	}
	
}