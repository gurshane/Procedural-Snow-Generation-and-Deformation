using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 *Gurshane Sidhu
 *260507632
 */

//A previous attemppt to use mesh vertices and collission points to displace the mesh
//This was not used in the submitted implementation but was kept as it was part of my process
public class VertexCollissionDetection : MonoBehaviour {

    //Points hit
    ContactPoint[] contactedPoints;

    //Closest vertices to the hit
    List<Vector3> closest;

    //Vertices of the mesh collider
    Vector3[] vertices;

    //How close does a vertex have to be to the collision for it to be displaced
    public float distToCollision;

    //Initialization
    void Start()
    {
        contactedPoints = new ContactPoint[0];
        closest = new List<Vector3>();
        vertices = GetComponent<MeshCollider>().sharedMesh.vertices;
    }

    void Update()
    {
        //If we hit something
        if(contactedPoints.Length > 0)
        {
            //Go through each hit point
            foreach(ContactPoint contact in contactedPoints)
            {
                //Go through each vertex in the collider
                foreach(Vector3 vert in vertices)
                {
                    //Consider everything in world space
                    Vector3 transVert = transform.TransformPoint(vert);
                    Vector3 transCont = transform.TransformPoint(contact.point);
                    Debug.Log(transVert + " " + transCont + " " + Vector3.Distance(transVert, transCont));
                    //If this vertex is close to the hit point
                    if(Vector3.Distance(transVert, transCont) < distToCollision)
                    {
                        closest.Add(vert); // keep track of it
                        Debug.DrawRay(transVert, transCont, Color.blue, 20.0f);
                    }
                }
            }
            //forget the old hit poins
            contactedPoints = new ContactPoint[0];
        }

        //If we have points to displace
        if(closest.Count > 0)
        {
            //This section was meant to go through and displace the vertices that were close to the hit point by some amount
            //However, this would have required going through the vertex list again to find which one we wanted to update , update it,
            //and then update the mesh filter, mesh, and collider. O(N^2) all over the place
            //foreach (Vector3 vert in closest)
            //{
            //    foreach (Vector3 realVert in vertices)
            //    {
            //        realVert = realVert - new Vector3(0f, 2.0f, 0f);
            //    }
            //}
            //closest = new List<Vector3>();
        }

    }

    void OnCollisionEnter(Collision other)
    {
        //Go through all contact points
        foreach(ContactPoint contact in other.contacts)
        {
            //Debug.DrawRay(contact.point, contact.normal, Color.red, 10.0f);
            Debug.Log("contact point: " + transform.TransformPoint(contact.point));
        }
        //Remember what we hit
        contactedPoints = other.contacts;
    }
    
}
