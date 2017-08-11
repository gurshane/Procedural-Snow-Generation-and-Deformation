using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 *Gurshane Sidhu
 *260507632
 */

//Debug class that was used to get relevant information from inputted textures
public class DumpTextureData : MonoBehaviour {

    public string textureName;

    MeshRenderer meshRenderer;
    MeshFilter meshFilter;
    Texture texture;
    Texture heightMap;


	// Use this for initialization
	void Start ()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        meshFilter = GetComponent<MeshFilter>();
        texture = meshRenderer.material.GetTexture("_MainTex");
        heightMap = meshRenderer.material.GetTexture("_ParallaxMap");

        Debug.Log(name + " height: " + texture.height + " width: " + texture.width);
	}
}
