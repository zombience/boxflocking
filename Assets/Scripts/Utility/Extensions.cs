using UnityEngine;
using System.Linq;

public static class Extensions
{
	
	/* *********************************************************
	 * Transform
	 * *********************************************************/
	#region transform
	public static void Set(this Transform transform, float x, float y, float z)
	{
		Vector3 newPosition = new Vector3(x, y, z);
		transform.position = newPosition;
	}
	
	public static void SetX(this Transform transform, float x)
	{
		Vector3 newPosition = new Vector3(x, transform.position.y, transform.position.z);
		transform.position = newPosition;
	}
	
	public static void SetY(this Transform transform, float y)
	{
		Vector3 newPosition = new Vector3(transform.position.x, y, transform.position.z);
		transform.position = newPosition;
	}
	
	public static void SetZ(this Transform transform, float z)
	{
		Vector3 newPosition = new Vector3(transform.position.x, transform.position.y, z);
		transform.position = newPosition;
	}
	
	public static void SetXY(this Transform transform, float x, float y)
	{
		Vector3 newPosition = new Vector3(x, y, transform.position.z);
		transform.position = newPosition;
	}
	
	public static void SetXZ(this Transform transform, float x, float z)
	{
		Vector3 newPosition = new Vector3(x, transform.position.y, z);
		transform.position = newPosition;
	}
	
	public static void SetYZ(this Transform transform, float y, float z)
	{
		Vector3 newPosition = new Vector3(transform.position.x, y, z);
		transform.position = newPosition;
	}

	public static void SetLocalX(this Transform transform, float x)
	{
		Vector3 newPosition = transform.localPosition;
		newPosition.x = x;
		transform.localPosition = newPosition;
	}

	public static void SetLocalY(this Transform transform, float y)
	{
		Vector3 newPosition = transform.localPosition;
		newPosition.y = y;
		transform.localPosition = newPosition;
	}

	public static void SetLocalZ(this Transform transform, float z)
	{
		Vector3 newPosition = transform.localPosition;
		newPosition.z = z;
		transform.localPosition = newPosition;
	}

	public static void SetLocalXY(this Transform transform, float x, float y)
	{
		Vector3 newPosition = transform.localPosition;
		newPosition.x = x;
		newPosition.y = y;
		transform.localPosition = newPosition;
	}

	public static void SetLocalXZ(this Transform transform, float x, float z)
	{
		Vector3 newPosition = transform.localPosition;
		newPosition.x = x;
		newPosition.z = z;
		transform.localPosition = newPosition;
	}

	public static void SetLocalYZ(this Transform transform, float y, float z)
	{
		Vector3 newPosition = transform.localPosition;
		newPosition.y = y;
		newPosition.z = z;
		transform.localPosition = newPosition;
	}

	public static void SetLocal(this Transform transform, float x, float y, float z)
	{
		Vector3 newPosition = new Vector3(x, y, z);
		transform.localPosition = newPosition;
	}

	public static void SetLocal(this Transform transform, Vector3 newPosition)
	{
		transform.localPosition = newPosition;
	}
	
	public static void Scale(this Transform transform, float xyz)
	{
		Vector3 newScale = new Vector3(xyz, xyz, xyz);
		transform.localScale = newScale;
	}
	
	public static void Scale(this Transform transform, float x, float y, float z)
	{
		Vector3 newScale = new Vector3(x, y, z);
		transform.localScale = newScale;
	}
	
	public static void Scale(this Transform transform, Vector3 scale)
	{
		transform.localScale = scale;
	}

	public static void RecursiveDisableParticles(this Transform transform)
	{
		if (transform.particleEmitter)
			transform.particleEmitter.emit = false;
		if (transform.particleSystem)
			transform.particleSystem.enableEmission = false;

		if (transform.childCount < 1)
		{
			//end
		}
		else
		{
			foreach (Transform c in transform)
				RecursiveDisableParticles(c);
		}
	}

	public static void RecursiveDisableAudio(this Transform transform)
	{
		if (transform.audio)
			transform.audio.enabled = false;

		if (transform.childCount < 1)
		{
			//end
		}
		else
		{
			foreach (Transform c in transform)
				RecursiveDisableAudio(c);
		}
	}


	#endregion
	
	#region float
	/* ***********************************************
	 * Float
	 * ************************************************/
	
	//return the average value of an array
	public static float Avg(this float[] f)
	{
		float c = 0;
		for(int i = 0; i < f.Length; i++)
		{
			c += f[i];
		}
		c = Mathf.Ceil(c * 100) * .01f;
		return c/f.Length;
	}
	
	/// <summary>
	/// Finds largest item in array
	/// </summary>
	/// <returns>
	/// max float value
	/// </returns>
	public static float Max(this float[] f)
	{
		return Mathf.Max(f);
	}

	/// <summary>
	/// Finds smallest item in array
	/// </summary>
	/// <returns>
	/// min float value
	/// </returns>
	public static float Min(this float[] f)
	{
		return Mathf.Min(f);
	}
	
	/// <summary>
	/// Maps input value across range
	/// </summary>
	/// <returns>
	/// float value
	/// </returns>
	/// <param name='f'>
	/// input value
	/// </param>
	/// <param name='inMin'>
	/// input floor
	/// </param>
	/// <param name='inMax'>
	/// input ceiling
	/// </param>
	/// <param name='outMin'>
	/// desired out minimum (exceedable)
	/// </param>
	/// <param name='outMax'>
	/// desired out maxmimum (exceedable)
	/// </param>
	public static float Map(this float f, float inMin, float inMax, float outMin, float outMax)
	{
		return ((outMax - outMin) * (f - inMin)) / (inMax - inMin) + outMin;
	}
	
	/// <summary>
	/// Maps input value across clamped output range
	/// </summary>
	/// <returns>
	/// clamped float value
	/// </returns>
	/// <param name='f'>
	/// input value
	/// </param>
	/// <param name='inMin'>
	/// input floor
	/// </param>
	/// <param name='inMax'>
	/// input ceiling
	/// </param>
	/// <param name='outMin'>
	/// clamped out minimum
	/// </param>
	/// <param name='outMax'>
	/// clamped out maxmimum
	/// </param>
	public static float MapClamp(this float f, float inMin, float inMax, float outMin, float outMax)
	{	
		return Mathf.Clamp((((outMax - outMin) * (f - inMin)) / (inMax - inMin) + outMin), outMin, outMax);
	}

	/// <summary>
	/// Scales a number by dimension
	/// e.g. if current dimension is "10" and new dimension is "100", inputting "1" will result in a return of "10"
	/// </summary>
	/// <param name="f"></param>
	/// <param name="curScale">the current dimension</param>
	/// <param name="newScale">the new dimension</param>
	/// <returns>scaled number</returns>
	public static float ScaleBy(this float f, float curScale, float newScale)
	{
		return f *= (newScale / curScale);
	}
	
	
	#endregion
	
	#region vector3
	/* ***********************************************
	 * Vector3
	 * ************************************************/
	
	///<summary>return the average of a vector array</summary>
	public static Vector3 Avg(this Vector3[] v)
	{
		Vector3 av = Vector3.zero;
		for(int i = 0; i < v.Length; i++)
		{
			av += v[i];
		}
		return av/v.Length;
	}

	///<summary>return the average magnitude of a vector array</summary>
	public static float AvgMag(this Vector3[] v)
	{
		float av = 0;
		for(int i = 0; i < v.Length; i++)
		{
			av += v[i].magnitude;
		}
		return av/v.Length;
	}
	
	public static float MaxMag(this Vector3[] v)
	{
		float[] n = new float[v.Length];
		for (int i = 0; i < v.Length; i++)
		{
			n[i] = v[i].magnitude;
		}

		return n.Max();
	}
	
	///<summary>return the largest axis in a vector array, per axis</summary>
	public static float Max(this Vector3[] v, string xyz)
	{
		float[] n = new float[v.Length];
		switch (xyz)
		{
			case "x":
				for (int i = 0; i < v.Length; i ++)
					n[i] = v[i].x;
				break;
			case "y":
				for (int i = 0; i < v.Length; i ++)
					n[i] = v[i].y;
				break;
			case "z":
				for (int i = 0; i < v.Length; i ++)
					n[i] = v[i].z;
				break;
			default:
				throw new System.ArgumentException("argument must be single lower case letter x, y, or z", "xyz");
		}
		return Mathf.Max(n);
	}
	
	///<summary>return the smallest axis in a vector array, per axis</summary>
	public static float Min(this Vector3[] v, string xyz)
	{
		float[] n = new float[v.Length];
		switch (xyz)
		{
			case "x":
				for (int i = 0; i < v.Length; i++)
					n[i] = v[i].x;
				break;
			case "y":
				for (int i = 0; i < v.Length; i++)
					n[i] = v[i].y;
				break;
			case "z":
				for (int i = 0; i < v.Length; i++)
					n[i] = v[i].z;
				break;
			default:
				throw new System.ArgumentException("argument must be single lower case letter x, y, or z", "xyz");
		}
		return Mathf.Min(n);
	}

	/// <summary>
	/// Returns vertices at the 2D max positions y, x, -x
	/// </summary>
	/// <param name="v">a Vector3 array</param>
	/// <returns>Vector3 array</returns>
	public static Vector3[] MaxTriBounds(this Vector3[] v)
	{
		int points = 3;
		Vector3[] returns = new Vector3[points];
		returns[0] = v.Aggregate((current, next) => current.x > next.x ? current : next); // topmost
		returns[1] = v.Aggregate((current, next) => current.x < next.x ? current : next); // leftmost
		returns[2] = v.Aggregate((current, next) => current.z > next.z ? current : next); // rightmost
		return returns;
	}
	#endregion
	
	#region Quaternion & Matrix
	
	/// <summary>
	/// Position from matrix.
	/// </summary>
	/// <returns>
	/// Vector3
	/// </returns>
	/// <param name='m'>
	/// Matrix
	/// </param>
	public static Vector3 PositionFromMatrix(Matrix4x4 m){
 
		Vector4 vector4Position = m.GetColumn(3);
		return new Vector3(vector4Position.x, vector4Position.y, vector4Position.z);
	}
	
	/// <summary>
	/// Returns Quaternion from Matrix Input
	/// </summary>
	/// <returns>
	/// Quaternion
	/// </returns>
	/// <param name='m'>
	/// Matrix Input
	/// </param>
	public static Quaternion QuaternionFromMatrix(Matrix4x4 m){ 
 
		return Quaternion.LookRotation(m.GetColumn(2), m.GetColumn(1)); 
	}
	
	#endregion
	
	#region color
	/* ***********************************************
	 * Color
	 * ************************************************/
	public static void SetCol(this Renderer r, Color color)
	{
		r.material.color = color;
	}
	
	public static void SetCol(this Renderer r, float colorValue)
	{
		r.material.color = new Color(colorValue, colorValue, colorValue, colorValue);
	}
	
	public static void SetA(this Renderer r, float alpha)
	{
		r.material.color = new Color(r.material.color.r, r.material.color.g, r.material.color.b, alpha);
	}
	
	public static void SetR(this Renderer r, float red)
	{
		r.material.color = new Color(red, r.material.color.g, r.material.color.b, r.material.color.a);
	}
	
	public static void SetG(this Renderer r, float green)
	{
		r.material.color = new Color(r.material.color.r, green, r.material.color.b, r.material.color.a);
	}
	
	public static void SetB(this Renderer r, float blue)
	{
		r.material.color = new Color(r.material.color.r, r.material.color.g, blue, r.material.color.a);
	}
 
    /// 
    /// Output a hex string from a color
    /// 
    ///
    ///Set to true to include a # character at the start
    /// 
    public static string ColorToHex(this Color color, bool includeHash = false) 
	{
        string red = Mathf.FloorToInt(color.r*255).ToString("X2");
        string green = Mathf.FloorToInt(color.g * 255).ToString("X2");
        string blue = Mathf.FloorToInt(color.b * 255).ToString("X2");
        return (includeHash ? "#" : "") + red + green + blue;
    }
 
    ///<summary> Create Color from hex string (with or without #)</summary>
	/// <returns>Color</returns>
	/// <param name='color'>hex input as string</param>
    public static Color HexToColor(string color) 
	{
        // remove the # character if there is one.
        color = color.TrimStart('#');
        float red = (HexToInt(color[1]) + HexToInt(color[0]) * 16f) / 255f;
        float green = (HexToInt(color[3]) + HexToInt(color[2]) * 16f) / 255f;
        float blue = (HexToInt(color[5]) + HexToInt(color[4]) * 16f) / 255f;
        Color finalColor = new Color { r = red, g = green, b = blue, a = 1 };
        return finalColor;
    }
 
    ///<summary>Translate RGBA values of 0-255 to 0.0 to 1.0</summary>
	/// <returns>Color</returns>
    public static Color IntToColor(int r, int g, int b, int a = 255) 
	{
        return new Color(r/255f, g/255f, b/255f, a/255f);
    }
 
    private static int HexToInt(char hexValue) 
	{
        return int.Parse(hexValue.ToString(), System.Globalization.NumberStyles.HexNumber);
    }

	#endregion

	#region mesh
	public static void SetVertexColor(this Mesh mesh, Color color)
	{
		Color[] colors = new Color[mesh.vertexCount];

		for (int i = 0; i < mesh.vertexCount; i++)
		{
			colors[i] = color;
		}

		mesh.colors = colors;
		mesh.RecalculateNormals();
	}

	public static void SetVertexColor(this Mesh mesh, Color top, Color bottom, float offset)
	{
		float min = float.MinValue;
		float max = float.MaxValue;
		float height;

		min = mesh.vertices.Min("y");
		max = mesh.vertices.Max("y");

		height = max - min;

		Color[] colors = new Color[mesh.vertices.Length];
		for (int i = 0; i < mesh.vertices.Length; i++)
		{
			float c = Mathf.Abs(mesh.vertices[i].y / height);

			colors[i] = Color.Lerp(top, bottom, c - offset);
		}

		mesh.colors = colors;
		mesh.RecalculateNormals();

	}


	public static Vector3[] GetTriPoints(this Mesh m)
	{
		return m.vertices.MaxTriBounds();
	}

	#endregion
}