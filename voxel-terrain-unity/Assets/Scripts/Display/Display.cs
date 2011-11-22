using UnityEngine;
using System.Collections;

public class Display {
	
	private static Vector3 VX = new Vector3(0.1F,0F,0F);
	private static Vector3 VY = new Vector3(0F,0.1F,0F);
	private static Vector3 VZ = new Vector3(0F,0F,0.1F);
	
	public static Color R = new Color(1.0F, 0.1F, 0.1F, 0.4F);
	public static Color G = new Color(0.1F, 1.0F, 0.1F, 0.4F);
	public static Color B = new Color(0.1F, 0.1F, 1.0F, 0.4F);
	
	public static void Sphere(Vector3 center, float rayon){
		float div = Mathf.PI / 16;
		float all = Mathf.PI * 2 + div;
		float old = 0;
		rayon /= 2;
		for(float angle=div; angle<all; angle+=div){
			float s1 = Mathf.Sin(old);
			float s2 = Mathf.Sin(angle);
			float c1 = Mathf.Cos(old);
			float c2 = Mathf.Cos(angle);
			Vector3 a = center + new Vector3(s1, c1, 0) * rayon;
			Vector3 b = center + new Vector3(s2, c2, 0) * rayon;
			Vector3 c = center + new Vector3(s1, 0, c1) * rayon;
			Vector3 d = center + new Vector3(s2, 0, c2) * rayon;
			Vector3 e = center + new Vector3(0, s1, c1) * rayon;
			Vector3 f = center + new Vector3(0, s2, c2) * rayon;
			Debug.DrawLine(a, b, Color.green);
			Debug.DrawLine(c, d, Color.green);
			Debug.DrawLine(e, f, Color.green);
			old = angle;
		}
	}
	
	public static void Vertices(Vector3[] vertices){
		for(int i=1; i<vertices.Length; i++){
			Debug.DrawLine(vertices[i-1], vertices[i], Color.green, 0);
		}
	}
	
	public static void Point(Vector3 pos, Color color)
	{
		Debug.DrawLine(pos-VX, pos+VX, color);
		Debug.DrawLine(pos-VY, pos+VY, color);
		Debug.DrawLine(pos-VZ, pos+VZ, color);	
	}
	
	public static void DrawGrid(int width, int depth, int res, Matrix4x4 toWorld)
	{
		for(int x=0; x<width; x+=res)
		{
			Vector3 start = new Vector3(x, 0, 0);
			Vector3 end = new Vector3(x, 0, depth);
			Gizmos.DrawLine(start, end);
		}
		for(float z=0; z<depth; z+=res)
		{
			Vector3 start = new Vector3(0, 0, z);
			Vector3 end = new Vector3(width, 0, z);
			Gizmos.DrawLine(start, end);
		}
	}
	
	public static void DebugDrawCube(Vector3 center, Vector3 size, Color c)
	{
		/*
		 *      4-----5
		 *     /|    /|
		 * y  7-----6 |
		 * |  | 0---|-1
		 * |  |/    |/
		 * |  3-----2
		 * | z
		 * |/_______x
		 */
		Bounds bounds = new Bounds(center, size);
		Vector3 p3 = bounds.min;
		Vector3 p5 = bounds.max;
		Vector3 p0 = new Vector3(p3.x, p3.y, p5.z);
		Vector3 p1 = new Vector3(p5.x, p3.y, p5.z);
		Vector3 p2 = new Vector3(p5.x, p3.y, p3.z);
		Vector3 p4 = new Vector3(p3.x, p5.y, p5.z);
		Vector3 p6 = new Vector3(p5.x, p5.y, p3.z);
		Vector3 p7 = new Vector3(p3.x, p5.y, p3.z);
		Debug.DrawLine(p0, p1, c);
		Debug.DrawLine(p1, p2, c);
		Debug.DrawLine(p2, p3, c);
		Debug.DrawLine(p3, p0, c);
		Debug.DrawLine(p4, p5, c);
		Debug.DrawLine(p5, p6, c);
		Debug.DrawLine(p6, p7, c);
		Debug.DrawLine(p7, p4, c);
		Debug.DrawLine(p0, p4, c);
		Debug.DrawLine(p1, p5, c);
		Debug.DrawLine(p2, p6, c);
		Debug.DrawLine(p3, p7, c);
	}
	
	public static void GizmosDrawCube(Vector3 center, Vector3 size, Matrix4x4 matrix)
	{
		/*
		 *      4-----5
		 *     /|    /|
		 * y  7-----6 |
		 * |  | 0---|-1
		 * |  |/    |/
		 * |  3-----2
		 * | z
		 * |/_______x
		 */
		Bounds bounds = new Bounds(center, size);
		Vector3 p3 = bounds.min;
		Vector3 p5 = bounds.max;
		Vector3 p0 = matrix.MultiplyPoint(new Vector3(p3.x, p3.y, p5.z));
		Vector3 p1 = matrix.MultiplyPoint(new Vector3(p5.x, p3.y, p5.z));
		Vector3 p2 = matrix.MultiplyPoint(new Vector3(p5.x, p3.y, p3.z));
		Vector3 p4 = matrix.MultiplyPoint(new Vector3(p3.x, p5.y, p5.z));
		Vector3 p6 = matrix.MultiplyPoint(new Vector3(p5.x, p5.y, p3.z));
		Vector3 p7 = matrix.MultiplyPoint(new Vector3(p3.x, p5.y, p3.z));
		p3 = matrix.MultiplyPoint(p3);
		p5 = matrix.MultiplyPoint(p5);
		Gizmos.DrawLine(p0, p1);
		Gizmos.DrawLine(p1, p2);
		Gizmos.DrawLine(p2, p3);
		Gizmos.DrawLine(p3, p0);
		Gizmos.DrawLine(p4, p5);
		Gizmos.DrawLine(p5, p6);
		Gizmos.DrawLine(p6, p7);
		Gizmos.DrawLine(p7, p4);
		Gizmos.DrawLine(p0, p4);
		Gizmos.DrawLine(p1, p5);
		Gizmos.DrawLine(p2, p6);
		Gizmos.DrawLine(p3, p7);
	}
	
}
