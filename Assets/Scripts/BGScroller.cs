using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGScroller : MonoBehaviour {
    public float scrollSpeed;
    private float tileSize;
    private Vector3 startPosition;

	/// <summary>
    /// Set the size and start position of the background from values set in the editor.
    /// </summary>
    void Start () {
        startPosition = transform.position;
        tileSize = transform.localScale.y;
	}
	
	/// <summary>
    /// Move the background to a new position based on the scrollSpeed and size.
    /// </summary>
    void Update () {
        float newPosition = Mathf.Repeat(Time.time * -scrollSpeed, tileSize);
        transform.position = startPosition + Vector3.forward * newPosition;
	}
}
