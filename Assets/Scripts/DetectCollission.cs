using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 *Gurshane Sidhu
 *260507632
 */

//Responsible for taking pictures from beneath the play area and sending the result to the snow displacer
public class DetectCollission : MonoBehaviour {

    //Where the picture from beneath the stage is sent to
    public SnowDisplacer target;

    //Who is walking on the stage
    public GameObject targetWalker;

    //Components required to take a picture and to store the information
    Camera myCam;
    RenderTexture renderTexture;
    RenderTexture temp;
    Texture2D myTex;

    int width;
    int height;
    
	// Use this for initialization
	void Start ()
    {
        myCam = GetComponent<Camera>();
        temp = myCam.targetTexture;
        myTex = new Texture2D(512, 512, TextureFormat.RGBA32, false);

        width = 512;
        height = 512;
    }
	
	// Update is called once per frame
	void Update ()
    {
		//Take a picture each frame
        StartCoroutine(saveRenderTexture());
	}

    //Takes a picture from the perspective of the orthognal camera placed below the stage and sends the result to the snow displacer
    IEnumerator saveRenderTexture()
    {
        renderTexture = new RenderTexture(512, 512, 24);

        //Let the camera know where to dump the pixels it renders
        myCam.targetTexture = renderTexture;

        //Make the camera render(take a picture)
        myCam.Render();

        //Set the reference to the active render texture to the one we just filled
        RenderTexture.active = renderTexture;

        //Read the pixels from the render texture we used above
        myTex.ReadPixels(new Rect(0, 0, 512, 512), 0, 0);
        
        //Get the pixels from the texture used to store the information from the render texture
        Color[] renderedPixels = myTex.GetPixels();

        //Give the resulting pixel array to the snow displacer
        target.resolveCollission(renderedPixels);

        //Reset the data structures for the next use
        myCam.targetTexture = null;
        RenderTexture.active = null;
        Destroy(renderTexture);
        
        myCam.targetTexture = temp;
        yield return new WaitForSeconds(0f);
    }
}
