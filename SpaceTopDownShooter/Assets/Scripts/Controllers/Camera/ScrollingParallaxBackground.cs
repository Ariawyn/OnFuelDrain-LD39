using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingParallaxBackground : MonoBehaviour {
	private MeshRenderer mr;
	private Material mat;

	public float parralax = 2f; 

	// Use this for initialization
	void Start () {
		this.mr = GetComponent<MeshRenderer>();
		this.mat = mr.material;
	}
	
	// Update is called once per frame
	void Update () {
		Vector2 offset = this.mat.mainTextureOffset;

		offset.x = transform.position.x / transform.localScale.x / parralax;
		offset.y = transform.position.y / transform.localScale.y / parralax;
		
		this.mat.mainTextureOffset = offset;
	}
}
