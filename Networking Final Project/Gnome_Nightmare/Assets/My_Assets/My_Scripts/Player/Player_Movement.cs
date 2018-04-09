using UnityEngine;

public class Player_Movement : MonoBehaviour {

    public GameObject Menu_Prefab;
    [HideInInspector]
    public float SpeedModifier = 1.0f;
    public float jumpSpeed = 20.0f;
    public float moveSpeed = 10.0f;
    public float runSpeed = 20.0f;
    public float SensitivityXAxis = 15.0f;
    public float SensitivityYAxis = 10.0f;
    public Vector3 Jump = new Vector3(3.0f, 0.0f, 10.0f); //MinJump, JumpPressure, MaxJumpPressure
    private bool ChargingJump = false;
    public Animator anim;

    public Collider IsGroundedCollider;
    public Collider l_IsGrounded;
    public Collider r_IsGrounded;
    private Rigidbody m_Rigidbody;
    private PlayerManager playerManager;
    private PlayerStats playerStats;
    //private AudioSource m_audioSource;

    private Vector3 moveDirection = Vector3.zero;
	private float rotX;
    private float rotY;
    public float animPer_H;
    public float animPer_V;
    private float RotationNeededForCamera;

    private bool m_IsGrounded = true;

    // Use this for initialization
    void Start () {
        //Spawns an empty gameObject named [World]
        GameObject EmptyWorld = GameObject.Find("World");
        //Spawns Prefab used for the Menu
        GameObject Menu = (GameObject)Instantiate(Menu_Prefab);
        Menu.transform.SetParent(EmptyWorld.transform);
        Menu.name = Menu_Prefab.name;
        //Spawns Prefab used for the Player
        this.transform.SetParent(EmptyWorld.transform);
        this.name = "Player";
        //
        playerManager = this.gameObject.GetComponent<PlayerManager>();
        playerStats = this.gameObject.GetComponent<PlayerStats>();

        m_Rigidbody = gameObject.GetComponent<Rigidbody>();
        //m_audioSource = gameObject.GetComponent<AudioSource>();
        Camera.main.GetComponent<CameraFollow>().SetPosition(this.gameObject.transform);
    }

    // Update is called once per frame
    void Update() {
        //If the player is in a menu they wont move
        if (playerManager.MenuOpen || playerStats.isDead) { return; }

        //Get the Xaxis rotation
        if (Input.GetAxis("Right Joystick X") != 0.0f) { rotX = Input.GetAxis("Right Joystick X") * SensitivityXAxis; }
        else { rotX = Input.GetAxis("Mouse X") * SensitivityXAxis; }

        //Get the Yaxis rotation
        if (Input.GetAxis("Right Joystick X") != 0.0f) { rotY += Input.GetAxis("Right Joystick Y") * SensitivityYAxis; }
        else { rotY -= Input.GetAxis("Mouse Y") * SensitivityYAxis; }

        //Clamp the camera rotation in the Yaxis
        rotY = Mathf.Clamp(rotY, -60f, 70f);  //high, low

        //Rotate player
        transform.Rotate(0, rotX, 0);
        m_Rigidbody.rotation = transform.rotation;
        //Rotate camera
        RotationNeededForCamera += rotX;
        Camera.main.GetComponent<CameraFollow>().SetRotation(new Vector2(RotationNeededForCamera, rotY));
    }
    
    void FixedUpdate() {
        //If the player is in a menu they wont move
        //if (this.gameObject.GetComponent<PlayerManager>().MenuOpen) {}
        if (playerStats.isDead) { m_Rigidbody.velocity = new Vector3(0,0,0); return; }

        if (playerManager.MenuOpen == false) {
            //Get movement
            if (Input.GetButton("Run") || Input.GetButton("LeftStickDown")) { moveDirection = new Vector3(Input.GetAxis("Horizontal") * (SpeedModifier*runSpeed), 0.0f, Input.GetAxis("Vertical") * (SpeedModifier*runSpeed)); }
            else { moveDirection = new Vector3(Input.GetAxis("Horizontal") * (SpeedModifier*moveSpeed), 0.0f, Input.GetAxis("Vertical") * (SpeedModifier*moveSpeed)); }

            //Debug.Log("["+ Jump.y + "]");
            //Holding jump button
            if (Input.GetButton("Jump") && m_IsGrounded && ChargingJump == false && Jump.y == 0.0f) { ChargingJump = true; Jump.y = Jump.x; }
            else if (Input.GetButton("Jump") && ChargingJump == true) {
                if (Jump.y < Jump.z) {
                    //Jump.y += (SpeedModifier*moveSpeed) * Time.deltaTime;
                    Jump.y += (((Jump.z - Jump.y) * ((SpeedModifier*jumpSpeed) * Time.deltaTime)) + 0.01f);
                    moveDirection.y = Jump.y;
                }
                else { Jump.y = Jump.z; ChargingJump = false; }
            }
            else if (ChargingJump == true) { ChargingJump = false; }
            //Not holding jump button, Not Grounded, Not Charging Jump, Jump.y is grater then 0.0f
            else {
                if (Jump.y > 0.0f) { Jump.y -= Jump.x * 0.55f; moveDirection.y = Jump.y; }
                else if (Jump.y < 0.0f) { Jump.y = 0.0f; }
            }
        }
        else {
            moveDirection = new Vector3(0.0f, 0.0f, 0.0f);
            if (Jump.y > 0.0f) { Jump.y -= Jump.x * 0.55f; moveDirection.y = Jump.y; }
            else if (Jump.y < 0.0f) { Jump.y = 0.0f; }
            ChargingJump = false;
        }

        //If the player is grounded do not apply additional gravity
        if (m_IsGrounded) { moveDirection.y -= 0.0f * Time.deltaTime; GetComponent<Rigidbody>().useGravity = false; }
        else { moveDirection.y -= (90.0f / Mathf.Clamp(Jump.y,0.25f,360.0f)) * Time.deltaTime; GetComponent<Rigidbody>().useGravity = true; } //((90/0.25) = 360)

        //Move player
        moveDirection = m_Rigidbody.rotation * moveDirection;
        m_Rigidbody.velocity = moveDirection;

        //Is the player on the ground?
        if (l_IsGrounded.GetComponent<CheckCollider>().IsTriggered || r_IsGrounded.GetComponent<CheckCollider>().IsTriggered || IsGroundedCollider.GetComponent<CheckCollider>().IsTriggered) { m_IsGrounded = true; }
        else if (m_IsGrounded == true) { m_IsGrounded = false; }

        //reset the anamation if in a menu
        if (playerManager.MenuOpen == false) {
            //Update Animator Perameters
            animPer_H = Input.GetAxis("Horizontal");
            animPer_V = Input.GetAxis("Vertical");
            //Set Animator Perameters
            anim.SetFloat("inputH", animPer_H);
            anim.SetFloat("inputV", animPer_V);
        }
        else {
            //Set Animator Perameters
            anim.SetFloat("inputH", 0);
            anim.SetFloat("inputV", 0);
        }

    }

    void OnCollisionEnter(Collision collision) {
        if (collision.contacts.Length > 0) {
            if (Vector3.Dot(transform.up, collision.contacts[0].normal) > 0.5f) {
                //m_IsGrounded = true;
            }
        }
    }
}
