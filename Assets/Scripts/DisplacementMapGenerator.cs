using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 *Gurshane Sidhu
 *260507632
 */

//Responsible for adding together two displacement maps(for the sake of variety)
public class DisplacementMapGenerator : MonoBehaviour {

    public GameObject sourceOne;
    public GameObject sourceTwo;
    public GameObject target;
    
    Texture2D sourceOneTex;
    Texture2D sourceTwoTex;
    
	// Update is called once per frame
	void Update ()
    {
        //If both perlin generators have finished generatring their respective displacement maps, add them together
        if (sourceOne.GetComponent<PerlinGenerator>().isDone && sourceTwo.GetComponent<PerlinGenerator>().isDone)
        {
            StartCoroutine(updateDisplacementMap());
        }
    }

    //Adds together the given displacement maps
    IEnumerator updateDisplacementMap()
    {
        yield return new WaitForSeconds(0f);

        //Get the displacement maps
        sourceOneTex = (Texture2D)sourceOne.GetComponent<MeshRenderer>().material.GetTexture("_MainTex");
        sourceTwoTex = (Texture2D)sourceTwo.GetComponent<MeshRenderer>().material.GetTexture("_MainTex");

        //Get the pixels from the inputted displacement map
        Color[] sourceOnePixels = sourceOneTex.GetPixels();
        Color[] sourceTwoPixels = sourceTwoTex.GetPixels();

        //Initialize the data structures necessary to store the resulting displacement map
        Texture2D displacementMap = new Texture2D(512, 512, TextureFormat.RGBA32, false);
        Color[] pixels = new Color[512 * 512];

        //Row
        for (int i = 0; i < 512; i++)
        {
            //Column
            for (int j = 0; j < 512; j++)
            {
                //row * width + column
                int coord = i * 512 + j;
                //For each pixel, add the corresponding pixels in the inputted displacement maps together and store them in our output data structures
                pixels[coord] = sourceOnePixels[coord] + sourceTwoPixels[coord];
            }
        }

        //Update the texture we created with the new pixels
        displacementMap.SetPixels(pixels);
        displacementMap.Apply();


        //Row
        for (int i = 0; i < 512; i++)
        {
            //Column
            for (int j = 0; j < 512; j++)
            {
                //row * width + column
                int coord = i * 512 + j;
                //For each pixel, add the corresponding pixels in the inputted displacement maps together and store them in our output data structures
                pixels[coord] = sourceOnePixels[coord] + sourceTwoPixels[coord];
            }
        }
        
        //Let the snow displacer know what the new displacement map is
        target.GetComponent<SnowDisplacer>().displace(displacementMap);

        //Destroy this game object so we dont generate the map again
        Destroy(gameObject);
    }
    
}
