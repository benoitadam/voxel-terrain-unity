using UnityEngine;
using System.Collections;

public class TestUv : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public bool init = true;
	void OnDrawGizmos()
	{
		if(init==false)
			return;
		
		init=false;
		
		Mesh mesh = GetComponent<MeshFilter>().mesh;
		
		//mesh.RecalculateBounds();
		//mesh.RecalculateNormals();
		
		Vector3[] vertices = mesh.vertices;
		Vector2[] uvX = new Vector2[vertices.Length];
		Vector2[] uvY = new Vector2[vertices.Length];
		Vector2[] uvZ = new Vector2[vertices.Length];
		Color[] colors = new Color[vertices.Length];
		
		float sX = mesh.bounds.max.x - mesh.bounds.min.x;
		float sY = mesh.bounds.max.y - mesh.bounds.min.y;
		float sZ = mesh.bounds.max.z - mesh.bounds.min.z;
		
		for(int i=0; i<vertices.Length; i++)
		{
			Vector3 v = vertices[i];
			uvX[i] = new Vector2(v.y, v.z);
			uvY[i] = new Vector2(v.x, v.z);
			uvZ[i] = new Vector2(v.x, v.y);
			colors[i] = new Color(v.x/sX, v.y/sY, v.z/sZ);
		}
		
		mesh.uv  = uvY;
		mesh.uv1 = uvX;
		mesh.uv2 = uvZ;
		mesh.colors = colors;
		
	}
}
