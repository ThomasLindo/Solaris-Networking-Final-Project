using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour
{
    public GameObject player;  
    public GameObject endPoint; // The point where the camera translates to for A.D.S.
    public Transform lookAt;    // Player transform
    public Transform camTransform;  // Camera's transform
    public static Vector3 currentPos = new Vector3();   // Camera's current position (really for other classes to access)
    

    public float adsSpeed = 100.0f; // How fast the camera translation is for A.D.S.
    public float distance = 90.0f;  // Distance from player
    public float currentX = 0.0f;   
    public float currentY = 0.0f;
    public static bool isAiming = false;    // Is the player currently aiming

    private Vector3 originalPos = new Vector3();    // Camera position before A.D.S.
    public Quaternion originalPlayerRot;    // Player rotation before A.D.S.
    private float Y_ANGLE_MIN = 0.0f;    // Minimum camera angle
    private float Y_ANGLE_MAX = 72.0f;    // Maximum camera angle
    //private bool lookForward = true;
   

    void Start() {
        camTransform = transform;
        originalPos = camTransform.position;
    }

    //current values
    void Update()  {
        float t = 0.0f;

        currentPos = transform.position;

        currentX += Input.GetAxis("Mouse X");
        if (isAiming)  {
            
            currentY += Input.GetAxis("Mouse Y");
            currentY = Mathf.Clamp(currentY, -72.0f, Y_ANGLE_MAX);

            t = adsSpeed * Time.deltaTime;

            // set rotation and move to end position
            transform.rotation = Quaternion.Lerp(transform.rotation, endPoint.transform.rotation, t);
            transform.position = Vector3.MoveTowards(transform.position, endPoint.transform.position, t);

            Quaternion rotation = Quaternion.Euler(-currentY, currentX, 0);
            player.transform.rotation = rotation;            
        }
        else {    
            t = adsSpeed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, originalPos, t);

            currentY -= Input.GetAxis("Mouse Y");
            currentY = Mathf.Clamp(currentY, Y_ANGLE_MIN, Y_ANGLE_MAX);
        }

        if (Input.GetMouseButtonDown(1)) {
            originalPlayerRot = new Quaternion(0.0f, player.transform.rotation.y, player.transform.rotation.z, player.transform.rotation.w);
            Debug.Log(t);
            originalPos = transform.position;           
            isAiming = true;         
        }
        else if (Input.GetMouseButtonUp(1)) {
            isAiming = false;
            player.transform.rotation = originalPlayerRot;
        }
    }

    //when the camera moves with player
    void LateUpdate() {
        if (!isAiming) {
            //put camera in the center then got angle where player is looking and throw camera behind that angle
            Vector3 dir = new Vector3(0, 0, -distance);
            Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);
            camTransform.position = lookAt.position + rotation * dir;            
            camTransform.LookAt(lookAt.position);
            originalPos = transform.position;
        }
        else {
            camTransform.forward = player.transform.forward;
        }
    }
}
