using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 *Gurshane Sidhu
 *260507632
 */

//Responsible for receiving and updating the displacement map of the snow surface
public class SnowDisplacer : MonoBehaviour {

    //Components related to the snow surface
    Texture2D myTex;
    MeshRenderer myMeshRenderer;

    void Start()
    {
        myMeshRenderer = GetComponent<MeshRenderer>();
        myTex = (Texture2D)myMeshRenderer.material.GetTexture("_DispTex");
    }

    //Receives a diplacement map and updates the snow surface with this new one
    public void displace(Texture2D displacementMap)
    {
        StartCoroutine(coroutineDisplace(displacementMap));
    }

    //Updates the material of the snow surface with a new displacement map
    IEnumerator coroutineDisplace(Texture2D displacementMap)
    {
        yield return new WaitForSeconds(0f);

        myMeshRenderer.material.SetTexture("_DispTex", displacementMap);

        //Get the current displacement map
        myTex = (Texture2D)myMeshRenderer.material.GetTexture("_DispTex");
        //Update its pixels with the new ones
        myTex.SetPixels(displacementMap.GetPixels());
        //Flush this change to the GPU
        myTex.Apply();
    }

    //Receives an array of new pixel colors that will be added to the existing displacement map
    public void resolveCollission(Color[] colors)
    {
        StartCoroutine(coroutineResolveCollision(colors));
    }

    //Update the current displacement map with the inputted array of pixel colors
    IEnumerator coroutineResolveCollision(Color[] footprints)
    {
        yield return new WaitForSeconds(0f);

        //Current displacement map
        myTex = (Texture2D)myMeshRenderer.material.GetTexture("_DispTex");

        //New array of pixels
        Color[] pixels = new Color[512 * 512];
        //Pixels of current displacement map
        Color[] currentDisp = myTex.GetPixels();

        //Row
        for (int i = 0; i < 512; i++)
        {
            //Column
            for (int j = 0; j < 512; j++)
            {
                //row * width + column
                int coord = i * 512 + j;

                //Current pixel from the given pixel array
                Color current = footprints[coord];

                //If this pixel is not white, make it black
                if (current.r != 1.0f)
                {
                    current = Color.black;
                    footprints[coord] = current;
                }

                //If the current pixel is white, make it black because white pixels at this point will be everything except the foot prints
                if (current.r == 1.0f)
                {
                    current = Color.black;

                }
                else if (current.r == 0.0f)
                {
                    //If the current pixel is black, make it white because this means it is part of the foot print
                    current = Color.white;
                }
                
                //Now we subtract the new displacement map from the old one and update the new array of pixels with the result
                pixels[coord] = currentDisp[coord] - current;
            }
        }

        //Update our displacement map
        myTex.SetPixels(pixels);
        //Flush the change to the GPU
        myTex.Apply();

    }

}
