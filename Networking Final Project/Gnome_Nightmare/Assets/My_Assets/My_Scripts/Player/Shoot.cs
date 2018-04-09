using UnityEngine;

public class Shoot : MonoBehaviour {
    public GameObject s_bullet;
    public GameObject s_Spawner;
    public Transform main;
    public Transform weaponTransform;

    public float s_BulletSpeed;
    public float Sensitivity = 100.0f;
    public static int ammo;
    public bool weaponSwung;

    private GameObject s_clone = null;  
    private int swingAnimCount = 0;

    // Use this for initialization
    void Start () {
        ammo = 30;
        s_Spawner.transform.forward = transform.forward;
        weaponSwung = false;
    }

	// Update is called once per frame
	void Update () {
        //weaponTransform = transform.GetComponent<CharacterWeaponSpawn>().currentWeapon.transform;

        if (Input.GetMouseButtonDown(0)) //LMB  Input.GetButton("Fire1")
        {
            if (ammo > 0)
            {
                ammo--;
                s_clone = (GameObject)Instantiate(s_bullet, s_Spawner.transform.position, s_Spawner.transform.rotation); //spawn bullet

                if (CameraControl.isAiming)
                {
                    raycastProjectile();
                }
                else
                {                  
                    s_clone.GetComponent<Rigidbody>().AddForce(s_Spawner.transform.forward * s_BulletSpeed, ForceMode.Impulse); //shoot it
                }
            }
        }
        

        if (Input.GetKey(KeyCode.R))
        {
            ammo = 30;
        }
        if (s_clone != null) { Destroy(s_clone, 2); }

        //////////////////////////////////////////////////////////////// WEAPON SWING

        if (Input.GetKeyDown(KeyCode.E) && !CameraControl.isAiming) // Can only swing melee weapon if you're not aiming (avoids physics clipping issue)
        {                
            weaponSwung = true;
            swingAnimCount = 0;
        }
        else if (Input.GetKeyUp(KeyCode.E) && weaponSwung) // Only rotate back if the weapon was actually swung
        {
            weaponSwung = false;
        }

        // Swing weapon
        if (weaponSwung && swingAnimCount != 7)
        {
            weaponTransform.Rotate(-10, 0, 0);
            swingAnimCount++;
        }
        else if (!weaponSwung && swingAnimCount != 0)
        {
            weaponTransform.Rotate(10, 0, 0);
            swingAnimCount--;
        }
    }

    // Shoots a raycast from the camera position and in the camera's forward direction then fires a bullet in the direction of the ray's hit point
    void raycastProjectile()
    {
        RaycastHit hit;     

        Debug.Log("I'm here!");

        if (Physics.Raycast(main.position, main.forward, out hit, 1000.0f))
        {
            Debug.Log(hit.transform.name + " " + hit.point);
            Vector3 bulletDir = (hit.point - s_Spawner.transform.position).normalized;
            s_clone.GetComponent<Rigidbody>().AddForce(bulletDir * s_BulletSpeed, ForceMode.Impulse); //shoot it
        }
        else
        {
            Debug.Log("Projectile Raycast Failed!");
            s_clone.GetComponent<Rigidbody>().AddForce(s_Spawner.transform.forward * s_BulletSpeed, ForceMode.Impulse); //shoot it
        }
    }
}