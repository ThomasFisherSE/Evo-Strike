using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGScroller : MonoBehaviour {
    public float scrollSpeed;
    private float tileSize;
    private Vector3 startPosition;

	void Start () {
        startPosition = transform.position;
        tileSize = transform.localScale.y;
	}
	
	void Update () {
        float newPosition = Mathf.Repeat(Time.time * scrollSpeed * -1, tileSize);
        transform.position = startPosition + Vector3.forward * newPosition;
	}
}
