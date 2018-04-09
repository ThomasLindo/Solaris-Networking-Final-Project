using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterPlaneGenerator : MonoBehaviour {

    public float Size = 1.0f; //Scale of the grid
    public int GridSize = 16; //Size of the grid

    [HideInInspector]
    public MeshFilter Filter; //Mesh that will be generated

    // Use this for initialization
    private void Start () {
        Filter = GetComponent<MeshFilter>(); //Set the Generated Mesh to the object 
        Filter.mesh = GenerateMesh(); //Generate the Mesh
    }
    private Mesh GenerateMesh() {
        Mesh m = new Mesh(); //Create a new Mesh
        var verticies = new List<Vector3>();
        var normals = new List<Vector3>();
        var uvs = new List<Vector2>();

        //Add a new (verticies, normals, uvs) that will be built to make the Mesh
        for (int x = 0; x <= GridSize; x++) {
            for (int y = 0; y <= GridSize; y++) {
                verticies.Add(new Vector3((-Size*0.5f) + (Size*(x/((float)GridSize))), 0, (-Size*0.5f) + (Size*(y/((float)GridSize))) ));
                normals.Add(Vector3.up);
                uvs.Add(new Vector2((x/(float)GridSize), (y/(float)GridSize)));
            }
        }

        var triangles = new List<int>(); //Make a list of triangles
        var vertCount = GridSize + 1;

        //Add a new triangle up of the (verticies, normals, uvs) 
        for (int i = 0; i < ((vertCount * vertCount) - vertCount); i++) {
            if ((i+1)%vertCount == 0) { continue; }
            triangles.AddRange(new List<int>() { (i+1+vertCount), (i+vertCount), (i), (i), (i+1), (i+vertCount+1)});
        }

        m.SetVertices(verticies); //Set Mesh verticies
        m.SetNormals(normals); //Set Mesh normals
        m.SetUVs(0, uvs); //Set Mesh uvs
        m.SetTriangles(triangles, 0); //Set Mesh triangles


        return m;
    }
}
