using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterPhysics : MonoBehaviour {

    public Vector3 Force = new Vector3(0,20,0);
    public float Viscosity = 2.0f;
    public GameObject Other;
    public GameObject Water;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void Float() {
        Other.GetComponent<Rigidbody>().AddForce(Force);
    }
    private void Drag() {
        Other.GetComponent<Rigidbody>().AddForce(Other.GetComponent<Rigidbody>().velocity * -1 * Viscosity);
    }
    private void UnderwaterCenter() {
        BoxCollider b = Other.GetComponent<BoxCollider>();
        Vector3 size = b.size;
        Vector3 center = b.center;

        Vector3[] vec = new Vector3[9];

        vec[0] = Other.transform.TransformPoint(center + (size * 0.5f));                                                    //Vector3 TopLeftFront     
        vec[1] = Other.transform.TransformPoint(center + new Vector3((-size.x * 0.5f), ( size.y * 0.5f), (-size.z * 0.5f)));//Vector3 TopRightBack     
        vec[2] = Other.transform.TransformPoint(center + new Vector3((-size.x * 0.5f), ( size.y * 0.5f), ( size.z * 0.5f)));//Vector3 TopRightFront    
        vec[3] = Other.transform.TransformPoint(center + new Vector3(( size.x * 0.5f), ( size.y * 0.5f), (-size.z * 0.5f)));//Vector3 TopLeftBack      

        vec[4] = Other.transform.TransformPoint(center + new Vector3(( size.x * 0.5f), (-size.y * 0.5f), ( size.z * 0.5f)));//Vector3 BottomLeftFront  
        vec[5] = Other.transform.TransformPoint(center + (-size * 0.5f));                                                   //Vector3 BottomRightBack  
        vec[6] = Other.transform.TransformPoint(center + new Vector3((-size.x * 0.5f), (-size.y * 0.5f), ( size.z * 0.5f)));//Vector3 BottomRightFront 
        vec[7] = Other.transform.TransformPoint(center + new Vector3(( size.x * 0.5f), (-size.y * 0.5f), (-size.z * 0.5f)));//Vector3 BottomLeftBack   

        vec[8] = center;


        //Need to compare all points against water, and find out the center
        Vector3 sum = Vector3.zero;
        if (vec != null && vec.Length != 0) {
            foreach (Vector3 vec3 in vec) {
                //If point is above water do nothing
                //If point is bellow water add to sum
                sum += vec3;
            }
            sum = (sum / vec.Length);
        }

    }
    //Only use when half of the points are underwater
    private void SurfaceArea() {
        //BoxCollider b = Other.GetComponent<BoxCollider>();
        //Vector3 width = (b.center + (b.size * 0.5f));
        int width = 5;


        Vector3 v1 = Vector3.Cross(Other.GetComponent<Rigidbody>().velocity, Vector3.up);
        Vector3 v2 = Vector3.Cross(Other.GetComponent<Rigidbody>().velocity, v1);

        Vector3 pointOutFront = Other.transform.position + (Other.transform.forward * 10);

        RaycastHit hit;
        
        for (int x = -width; x <= width; x++) {
            for (int y = -width; y <= width; y++) {
                Vector3 start = pointOutFront + (v1 * x) + (v2 * y);
                if (Physics.Raycast(start, -Other.transform.forward, out hit, 10)) {
                    //Apply force
                }
            }
        }
    }
}
