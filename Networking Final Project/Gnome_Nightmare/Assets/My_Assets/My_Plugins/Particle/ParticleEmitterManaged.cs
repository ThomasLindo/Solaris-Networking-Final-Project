using System;
using UnityEngine;
using ParticleEmitterPlugin;

public class ParticleEmitterManaged : MonoBehaviour {

    ParticleEmitterPlugin.ParticleEmitter temp = new ParticleEmitterPlugin.ParticleEmitter();
    public GameObject particlePrefab;
    public int AmountOfParticles = 4;
    public Vector3 EmitterPosition = new Vector3(0,  0, 0);
    public float MinParticleLifeSpan = 1.0f;
    public float MaxParticleLifeSpan = 10.0f;
    public Vector3 ParticleForceMin = new Vector3(1,  1, 1);
    public Vector3 ParticleForceMax = new Vector3(10,10,10);

    // Use this for initialization
    private void Start () {
        temp.SetParticlePrefab(particlePrefab);
        temp.SetEmitterPosition( new ParticleEmitterPlugin.Vec3(this.transform.position.x, this.transform.position.y, this.transform.position.z) );
        temp.SetParticleLifeSpan(MinParticleLifeSpan, MaxParticleLifeSpan);
        temp.SetParticleForceMin(new ParticleEmitterPlugin.Vec3(ParticleForceMin.x, ParticleForceMin.y, ParticleForceMin.z));
        temp.SetParticleForceMax(new ParticleEmitterPlugin.Vec3(ParticleForceMax.x, ParticleForceMax.y, ParticleForceMax.z));
        temp.Initialize(AmountOfParticles);
        temp.Play();
    }

    // Update is called once per frame
    private void Update () {
        temp.Update(Time.deltaTime);
    }

    private void OnDestroy() {
        temp.Pause();
        temp.FreeMemory();
    }
}
