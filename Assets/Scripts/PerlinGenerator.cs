using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 *Gurshane Sidhu
 *260507632
 */

//Responsible for using perlin noise and interpolation to generate a displacement map
//The technique is to color the pixel black if close to the centre of a black circle, an interpolated value if near a white circle centre, and white/black otherwise
public class PerlinGenerator : MonoBehaviour
{
    //Components of the GameObject
    MeshRenderer meshRenderer;
    Material myMaterial;
    Texture2D targetTex;
    Color[] pixels;

    //X and Y Coordinates of the black circles
    int[] blackCircleXCoor;
    int[] blackCircleYCoor;

    //X and Y Coordinates of the interpolated white circles
    int[] whiteCircleXCoor;
    int[] whiteCircleYCoor;

    //Radius of each circle
    float[] blackCircleCentres;
    float[] whiteCircleCentres;

    public int numBlackCircles;
    public int numWhiteCircles;

    public float minRadius;
    public float maxRadius;

    //Lets DisplacementMapGenerator know that this gameObject is done doing work
    public bool isDone;

    //The amount to lerp by (dist to nearest centre)/maxRadius
    float lerpDist;

    DisplacementMapGenerator displacementMapGenerator;

    // Initialization
    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        myMaterial = meshRenderer.material;
        targetTex = new Texture2D(512, 512, TextureFormat.RGBA32, false);
        pixels = new Color[targetTex.width * targetTex.height];

        blackCircleXCoor = new int[numBlackCircles];
        blackCircleYCoor = new int[numBlackCircles];

        whiteCircleXCoor = new int[numWhiteCircles];
        whiteCircleYCoor = new int[numWhiteCircles];

        blackCircleCentres = new float[numBlackCircles];
        whiteCircleCentres = new float[numWhiteCircles];

        StartCoroutine(doGeneratePerlinNoise());
	}

    //Creates the locations of the black and white cirlc
    IEnumerator doGeneratePerlinNoise()
    {
        yield return new WaitForSeconds(0f);

        //Initialize the black circle positions
        for (int i = 0; i < numBlackCircles; i++)
        {
            int xCoor = Random.Range(0, 511);
            int yCoor = Random.Range(0, 511);
            blackCircleXCoor[i] = xCoor;
            blackCircleYCoor[i] = yCoor;
            blackCircleCentres[i] = Random.Range(minRadius, maxRadius);
        }

        //Initialize the interpolated white circle positions
        for (int i = 0; i < numWhiteCircles; i++)
        {
            int xCoor = Random.Range(0, 511);
            int yCoor = Random.Range(0, 511);
            whiteCircleXCoor[i] = xCoor;
            whiteCircleYCoor[i] = yCoor;
            whiteCircleCentres[i] = Random.Range(minRadius, maxRadius);
        }

        displacementMapGenerator = GetComponentInParent<DisplacementMapGenerator>();

        generatePerlinTexture();
    }
	
    //Fills an array of pixels with the colors to use for the displacement map
    void generatePerlinTexture()
    {

        //Row
        for (float i = 0; i < targetTex.height; i++)
        {
            //Column
            for (float j = 0; j < targetTex.width; j++)
            {
                //Compute perlin noise number for pixel based on an x and y value < 1
                float xPos = j / targetTex.width * Random.Range(1, 15) + Random.Range(-0.15f, 0.15f);
                float yPos = i / targetTex.height * Random.Range(1, 15) + Random.Range(-0.15f, 0.15f);
                float perlinNumber = Mathf.PerlinNoise(xPos, yPos);

                //Position in color array is rowNumber * width + columnNumber
                int coord = (int)i * targetTex.width + (int)j;

                //If the current j, i coordinate is close to a black circle center
                if (isCloseToBlackCentre(j, i))
                {
                    //Make the pixel here black
                    pixels[coord] = new Color(0, 0, 0);
                }
                else if(isCloseToWhiteCentre(j, i))
                {
                    //If we are close to a white center then compute an interpolated value between 0(black) and 1(white) for the pixel depending on how far away they are from the centre
                    //of the circle ( distToCentre / maxRadius)
                    float newColVal = Mathf.Lerp(0, 1, lerpDist);
                    Color newCol = new Color(newColVal, newColVal, newColVal);
                    pixels[coord] = newCol;
                }
                else
                {
                    //If we are close to neither a black nor a white centre, then we will use the perlin number generated previously to determine if the pixel will be black or white
                    if (perlinNumber < 0.5f)
                    {
                        //Pixel is now black
                        pixels[coord] = new Color(0, 0, 0);
                    }
                    else
                    {
                        //Pixel is now white
                        pixels[coord] = new Color(1, 1, 1);
                    }
                }
                
            }
        }

        //Update the pixels in the texture
        targetTex.SetPixels(pixels);
        //Flush the current pixels to the GPU
        targetTex.Apply();

        //Update the material with the new texture
        meshRenderer.material.SetTexture("_MainTex", targetTex);

        //Let the displacement map generator know we are done working
        isDone = true;
    }

    //Determines if a i,j coordinate ( row, column) is close to a black circle centre
    bool isCloseToBlackCentre(float i, float j)
    {
        for(int k = 0; k < numBlackCircles; k++)
        {
            float distance = Mathf.Sqrt(Mathf.Pow(i - blackCircleXCoor[k], 2) + Mathf.Pow(j - blackCircleYCoor[k], 2));

            //If we are close to a centre(close meaning our distance to the centre is less than the radius of that circle)
            if (distance <= blackCircleCentres[k])
            {
                return true;
            }
        }
        return false;
    }

    //Determines if a i,j coordinate( row, column )  is close to a white circle centre
    bool isCloseToWhiteCentre(float i, float j)
    {
        for (int k = 0; k < numWhiteCircles; k++)
        {
            float distance = Mathf.Sqrt(Mathf.Pow(i - whiteCircleXCoor[k], 2) + Mathf.Pow(j - whiteCircleYCoor[k], 2));

            //If we are close to a centre(close meaning our distance to the centre is less than the radius of that circle)
            if (distance <= whiteCircleCentres[k])
            {
                lerpDist = distance / maxRadius;
                return true;
            }
        }
        return false;
    }
    
}
