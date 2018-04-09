using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleMesh : MonoBehaviour {


    // This script is placed in public domain. The author takes no responsibility for any possible harm.

    public float scale = 1.0f;
    public bool recalculateNormals = false;
    public Mesh mesh;

    private Vector3[] baseVertices;


    private void Update() {
        mesh = this.GetComponent<MeshFilter>().mesh;

        if (baseVertices == null) { baseVertices = mesh.vertices; }

        var vertices = new Vector3[baseVertices.Length];

        for (var i = 0; i < vertices.Length; i++) {
            var vertex = baseVertices[i];
            vertex.x = vertex.x * scale;
            vertex.y = vertex.y * scale;
            vertex.z = vertex.z * scale;

            vertices[i] = vertex;
        }

        mesh.vertices = vertices;

        if (recalculateNormals) { mesh.RecalculateNormals(); }
        mesh.RecalculateBounds();
    }
}
