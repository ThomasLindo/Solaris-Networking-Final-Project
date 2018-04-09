using UnityEngine;
using Sirenix.OdinInspector;

public class CameraFollow : SerializedMonoBehaviour {

    private float Height = 0.0f;
    private float Depth = 0.0f;

    [HorizontalGroup("Head"), LabelWidth(60)]
    public bool isFollowingThis;
    [HorizontalGroup("Head"), LabelWidth(60)]
    public GameObject FollowThis;
    [HorizontalGroup("Head"), LabelWidth(60)]
    public float HeadHeight = 0.0f;
    [HorizontalGroup("Head"), LabelWidth(60)]
    public float HeadDepth = 0.0f;

    [HorizontalGroup("1st Preson"), LabelWidth(60)]
    public float Height1st = 0.50f;
    [HorizontalGroup("1st Preson"), LabelWidth(60)]
    public float Depth1st = 0.0f;

    [HorizontalGroup("3rd Preson"), LabelWidth(60)]
    public float Height3rd = 3.0f;
    [HorizontalGroup("3rd Preson"), LabelWidth(60)]
    public float Depth3rd = -4.0f;
    [HorizontalGroup("3rd Preson"), LabelWidth(60)]
    public float CameraFollowSpeed = 8.0f;
    private bool is3rdPerson = false;

    [HorizontalGroup("ADS"), LabelWidth(60)]
    public Transform endPoint; // The point where the camera translates to for A.D.S.
    [HorizontalGroup("ADS"), LabelWidth(60)]
    public float AdsSpeed = 8.0f;
    private bool isAiming = false;

    public bool Switch3rdPerson = false;
    private Transform playerTransform;
    private Vector2 playerRotation;

    void Start() { }

    // Update is called once per frame
    void Update() {
        if (is3rdPerson && Input.GetButton("Fire2")) { isAiming = true; }
        else if (is3rdPerson && Input.GetButtonUp("Fire2")) { isAiming = false; }
    }

    void LateUpdate() {
        if (is3rdPerson == Switch3rdPerson) { SwitchPerspective(); }

        if (isFollowingThis) {
            this.transform.position = (FollowThis.transform.position + (FollowThis.transform.up * HeadHeight) + (FollowThis.transform.forward * HeadDepth));
            //this.transform.rotation = FollowThis.transform.rotation;
            this.transform.rotation = Quaternion.Euler(playerRotation.y, playerRotation.x, 0);
            return;
        }

        //If player is active, set the camera position and rotation
        if (is3rdPerson && isAiming) { AdsCamera(); }
        else if (is3rdPerson && !isAiming) { LerpCamera(); }
        else if (!is3rdPerson) { Camera1stPerson(); }
    }
    //Set position of camera
    public void SetPosition(Transform m_transform) { playerTransform = m_transform; }
    //Set rotation of camera
    public void SetRotation(Vector2 m_rotation) { playerRotation = m_rotation; }

    public void SwitchPerspective() {
        if (is3rdPerson) {
            is3rdPerson = false;
            Switch3rdPerson = true;
            Height = Height1st;
            Depth = Depth1st;
        }
        else {
            is3rdPerson = true;
            Switch3rdPerson = false;
            Height = Height3rd;
            Depth  = Depth3rd;
            Camera1stPerson();
        }
    }

    public void Camera1stPerson() {
        if (playerTransform != null) {
            this.transform.position = playerTransform.position + new Vector3(0, Height, 0) + playerTransform.transform.forward * Depth;
            this.transform.rotation = Quaternion.Euler(playerRotation.y, playerRotation.x, 0);
        }
    }
    public void LerpCamera() {
        if (playerTransform != null) {
            float tempFloatY;
            if (playerRotation.y > -10.0f) { tempFloatY = Mathf.Clamp(playerRotation.y * 0.1f, -(Height * 0.9f), (Height * 0.2f))-1.0f; }
            else { tempFloatY = Mathf.Clamp(playerRotation.y * 0.1f, -(Height * 0.9f), (Height * 0.5f)) - 1.0f; }
            float tempFloatX;
            if (playerRotation.y > 10.0f) { tempFloatX = Mathf.Clamp(playerRotation.y * 0.1f, (Depth * 0.75f), (-Depth) - 1.0f) + 1.0f; }
            else { tempFloatX = Mathf.Clamp(playerRotation.y * 0.1f, (Depth * 0.75f), 0.50f) + 1.0f; }

            if (tempFloatX > 0.0f) { tempFloatX  = -tempFloatX; }
            //Debug.Log("[" + playerRotation.y + "] [" + tempFloatY + "] [" + tempFloatX + "]");
            this.transform.position = Vector3.Lerp(this.transform.position,  (playerTransform.position + new Vector3(0, (Height + tempFloatY), 0) + playerTransform.transform.forward * (Depth - tempFloatX)), Time.deltaTime * CameraFollowSpeed);
            this.transform.rotation = Quaternion.Euler(playerRotation.y, playerRotation.x, 0);
            //this.transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.Euler(playerRotation.y, playerRotation.x, 0), Time.deltaTime * CameraSpeed);
        }
    }

    public void AdsCamera() {
        float t = 0.0f;

        if (playerTransform != null) { 
            if (isAiming) {
                //currentY += Input.GetAxis("Mouse Y");
                //currentY = Mathf.Clamp(currentY, -72.0f, Y_ANGLE_MAX);

                t = AdsSpeed * Time.deltaTime;

                // set rotation and move to end position
                this.transform.rotation = Quaternion.Lerp(this.transform.rotation, endPoint.transform.rotation, t);
                this.transform.position = Vector3.MoveTowards(this.transform.position, endPoint.transform.position, t);

                //player.transform.rotation = Quaternion.Euler(-currentY, currentX, 0);
            }
            else {
                t = AdsSpeed * Time.deltaTime;
                Vector3 temp = (playerTransform.position + new Vector3(0, Height, 0) + (playerTransform.transform.forward * Depth));
                this.transform.position = Vector3.MoveTowards(this.transform.position, temp, t);

                //currentY -= Input.GetAxis("Mouse Y");
                //currentY = Mathf.Clamp(currentY, Y_ANGLE_MIN, Y_ANGLE_MAX);
            }
            this.transform.rotation = Quaternion.Euler(playerRotation.y, playerRotation.x, 0);
        }
    }
}