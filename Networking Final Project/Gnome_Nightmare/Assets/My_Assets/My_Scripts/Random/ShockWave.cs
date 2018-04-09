using UnityEngine;


public class ShockWave : MonoBehaviour
{
    #region Variables
    [Header("Children")]
    public GameObject RotateBlitzBot1;
    public GameObject RotateBlitzBot2;
    public GameObject RotateBlitzBot3;
    public GameObject BlitzBotCollider;

    [Space]

    [Header("Properties")]
    [Range(0.05f, 1.0f)]
    public float GrowthSpeed = 0.10f;
    [Range(1.0f, 20.0f)]
    public float MaxSpread = 9.0f;
    [Range(0.05f, 1.0f)]
    public float MaxGrowthLength = 0.50f;

    //
    private float timer;
    private float ExpandTimer;
    //
    private float ogXSpread;
    private float ogZSpread;
    private float ogRotSpeed;
    //
    private float ogColliderRadius;
    private bool expand;
    #endregion

    #region On Start
    // Use this for initialization
    void Start() {
        Orbit_Objects orbit_Objects = RotateBlitzBot1.GetComponent<Orbit_Objects>();
        ogXSpread = orbit_Objects.xSpread;
        ogZSpread = orbit_Objects.zSpread;
        ogRotSpeed = orbit_Objects.rotSpeed;

        SphereCollider sphereCollider = BlitzBotCollider.GetComponent<SphereCollider>();
        ogColliderRadius = sphereCollider.radius;
    }
    #endregion

    #region Using Shockwave
    // Update is called once per frame
    void Update() {
        Orbit_Objects orbit_Objects1 = RotateBlitzBot1.GetComponent<Orbit_Objects>();
        Orbit_Objects orbit_Objects2 = RotateBlitzBot2.GetComponent<Orbit_Objects>();
        Orbit_Objects orbit_Objects3 = RotateBlitzBot3.GetComponent<Orbit_Objects>();
        SphereCollider sphereCollider = BlitzBotCollider.GetComponent<SphereCollider>();

        
        if (Input.GetKey("e") || Input.GetButton("Fire1")) {
            if (orbit_Objects1.xSpread < MaxSpread && timer < MaxGrowthLength) {
                orbit_Objects1.rotSpeed += timer;
                orbit_Objects2.rotSpeed += timer;
                orbit_Objects3.rotSpeed += timer;
                timer += Time.deltaTime * GrowthSpeed;
            }
            expand = true;
        } else {
            if (expand) {
                if (orbit_Objects1.xSpread < MaxSpread && timer > ExpandTimer) {
                    orbit_Objects1.xSpread += timer * 2.0f;
                    orbit_Objects1.zSpread += timer * 2.0f;
                    orbit_Objects2.xSpread += timer * 2.0f;
                    orbit_Objects2.zSpread += timer * 2.0f;
                    orbit_Objects3.xSpread += timer * 2.0f;
                    orbit_Objects3.zSpread += timer * 2.0f;

                    sphereCollider.radius += timer * 2.0f;
                    ExpandTimer += timer * 0.05f;
                } else {
                    expand = false;
                    ExpandTimer = 0.0f;
                }
            } else if (timer > 0.0f && orbit_Objects1.xSpread > ogXSpread) {
                orbit_Objects1.xSpread  -= timer;
                orbit_Objects1.zSpread  -= timer;
                orbit_Objects1.rotSpeed -= timer;
                orbit_Objects2.xSpread  -= timer;
                orbit_Objects2.zSpread  -= timer;
                orbit_Objects2.rotSpeed -= timer;
                orbit_Objects3.xSpread  -= timer;
                orbit_Objects3.zSpread  -= timer;
                orbit_Objects3.rotSpeed -= timer;

                sphereCollider.radius -= timer;
                timer -= Time.deltaTime * GrowthSpeed * 0.30f;
            } else {
                orbit_Objects1.xSpread  = ogXSpread;
                orbit_Objects1.zSpread  = ogZSpread;
                orbit_Objects1.rotSpeed = ogRotSpeed;
                orbit_Objects2.xSpread  = ogXSpread;
                orbit_Objects2.zSpread  = ogZSpread;
                orbit_Objects2.rotSpeed = ogRotSpeed;
                orbit_Objects3.xSpread  = ogXSpread;
                orbit_Objects3.zSpread  = ogZSpread;
                orbit_Objects3.rotSpeed = ogRotSpeed;

                sphereCollider.radius = ogColliderRadius;
                timer = 0.0f;
            }
        }
    }
    #endregion
}
