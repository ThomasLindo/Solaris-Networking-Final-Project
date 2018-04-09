using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterNoise : MonoBehaviour {
    
    public float Power = 3; //How heigh the waves will go
    public float TimeScale = 1; //How fast the wave are
    public float Speed = 0.05f; //Don't change (pairs with Scale)

    private float Scale = 0.3f; //Don't change (pairs with TileOffset)
    private float TileOffset = 15; //Don't change (pairs with Scale)

    private float OffsetX;
    private float OffsetY;
    private MeshFilter mf; //Mesh that the noice will be applyed to

    private void Start() {
        mf = GetComponent<MeshFilter>();
        MakeNoise();
        OffsetX = Time.deltaTime * (this.gameObject.transform.position.x* TileOffset);
        OffsetY = Time.deltaTime * (this.gameObject.transform.position.z* TileOffset);
    }
    private void Update() {
        MakeNoise();
        OffsetX += Time.deltaTime * TimeScale; //Add to X offset by time
        OffsetY += Time.deltaTime * TimeScale; //Add to Y offset by time
    }

    private void MakeNoise() {
        Vector3[] verticies = mf.mesh.vertices; //Get Mesh verticies
        //Calculate Height of each verticies
        for (int i = 0; i < verticies.Length; i++) {
            verticies[i].y = CalculateHeight(verticies[i].x, verticies[i].z) * Power;
        }
        mf.mesh.vertices = verticies; //Set Mesh verticies
    }
    private float CalculateHeight(float x, float y) {
        float xCord = x * Scale + OffsetX; //Calculate X
        float yCord = y * Scale + OffsetY; //Calculate Y

        //Math magic to make it wavy
        return Mathf.PerlinNoise(xCord,yCord);
    }
}
