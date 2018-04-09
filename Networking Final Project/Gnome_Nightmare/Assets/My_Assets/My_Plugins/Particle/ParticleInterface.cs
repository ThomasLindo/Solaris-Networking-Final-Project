using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class ParticleInterface : MonoBehaviour {

    static int m_NumParticles = 50;
    public int m_MaxDistance = 10;
    public float m_Attraction = 10;
    public GameObject m_SpawnableObject;



    [StructLayout(LayoutKind.Sequential)]
    public struct Vec {
        public float x, y, z;
    }
    [StructLayout(LayoutKind.Sequential)]
    public struct Particle {
        public Vec Pos;
        public Vec Vel;
    }

    [DllImport("ParticleSystem")]
    public static extern void initSystem();
    [DllImport("ParticleSystem")]
    public static extern void CleanUpSystem();
    [DllImport("ParticleSystem")]
    public static extern void initializeParticles(Particle[] p, int size, int MaxDisstance);
    [DllImport("ParticleSystem")]
    public static extern void updateSystemPosition(float x, float y, float z);
    [DllImport("ParticleSystem")]
    public static extern void updateAttraction(float f);
    [DllImport("ParticleSystem")]
    public static extern void updateParticles(Particle[] p, float dt);


    Particle[] g_Arr = new Particle[m_NumParticles];
    GameObject[] ParticleArr = new GameObject[m_NumParticles];

    void SetPosition() {
        for (int i = 0; i < m_NumParticles; i++) {
            ParticleArr[i].transform.position = new Vector3(g_Arr[i].Pos.x, g_Arr[i].Pos.y, g_Arr[i].Pos.z);
        }
    }


    void Start() {
        GameObject ParticleSpawnHere = new GameObject();
        ParticleSpawnHere.name = "ParticleSpawnHere";
        for (int i = 0; i < m_NumParticles; i++) {
            ParticleArr[i] = Instantiate<GameObject>(m_SpawnableObject);
            ParticleArr[i].name = m_SpawnableObject.name;
            ParticleArr[i].transform.SetParent(ParticleSpawnHere.transform);
        }
        initSystem();
        updateAttraction(m_Attraction);
        updateSystemPosition(this.transform.localPosition.x, this.transform.localPosition.y, this.transform.localPosition.z);
        initializeParticles(g_Arr, m_NumParticles, m_MaxDistance);
        SetPosition();
    }
    void Update() {
        updateParticles(g_Arr, Time.deltaTime);
        updateSystemPosition(this.transform.localPosition.x, this.transform.localPosition.y, this.transform.localPosition.z);
        SetPosition();
    }
}
