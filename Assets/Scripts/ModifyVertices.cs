using UnityEngine;
using System.Collections;

/*
 *Gurshane Sidhu
 *260507632
 */

//A class that was not used in my implementation but is included as I did try alternative methods to 
//displace the snow
public class ModifyVertices : MonoBehaviour
{

    Mesh mesh;
    Vector3[] vertices;
    Vector3[] normals;

    //Displaces the mesh of this gameObject based on the colours in a color array
    public void displaceMesh(Color[] inColors)
    {
        mesh = GetComponent<MeshFilter>().mesh;
        vertices = mesh.vertices;
        normals = mesh.normals;

        int i = 0;
        float displacementModifier = 1;

        //Go through all vertices
        while (i < vertices.Length)
        {
            //Get color of red channel
            displacementModifier = inColors[i].r;
            //Use the red channel to modify the displacement of the vertex
            vertices[i] += normals[i] * Mathf.Sin(Time.deltaTime) * Random.Range(15.0f, 25.0f) * displacementModifier;
            i++;
        }

        UpdateMeshAndCollider(vertices, normals);
    }

    //Updates our mesh and collider with the new vertices and normals
    public void UpdateMeshAndCollider(Vector3[] newVertices, Vector3[] newNormals)
    {
        GetComponent<MeshFilter>().mesh.vertices = newVertices;
        GetComponent<MeshCollider>().sharedMesh = GetComponent<MeshFilter>().mesh;
    }
}
