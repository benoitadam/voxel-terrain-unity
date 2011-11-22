//using UnityEngine;
//using System.Collections;
//using System.Collections.Generic;
//
//public class VTTriangles
//{
//	private VTVertices _vertices;
//	
//	
//	private Dictionary<Vector3,int> _find;
//	private int _index;
//	
//	public VTTriangles()
//	{
//		_vertices = new V
//		_triangles = new List<Vector3>();
//		_find = new Dictionary<Vector3, int>();
//		_index=0;
//	}
//	
//	public int GetIndex(Vector3 vertex)
//	{
//		int i;
//		if(_find.TryGetValue(vertex, out i))
//			return i;
//		_vertices.Add(vertex);
//		_find.Add(vertex, _index);
//		return _index++;
//	}
//	
//	public Vector3[] ToArray()
//	{
//		return _vertices.ToArray();
//	}
//}