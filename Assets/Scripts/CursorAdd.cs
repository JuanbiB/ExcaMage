using UnityEngine;
using System.Collections;

public class CursorAdd : MonoBehaviour {

	public Texture2D cursor;

	// Use this for initialization
	void Awake () {
		Cursor.SetCursor(cursor, Vector2.zero, CursorMode.Auto);
	}
}
