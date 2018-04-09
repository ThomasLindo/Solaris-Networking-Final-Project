
using UnityEngine;

namespace Random_Utils {

    public class RandomUtils : MonoBehaviour {
        static public int RandomCoin() { return Random.Range(0, 2); }

        static public float RandomFloat(int min, int max) { return Random.Range(min, max+0.0f); }
        static public float RandomFloat(float min, float max) { return Random.Range(min, max); }
        
        static public int RandomInt(int min, int max) { return Random.Range(min, max+1); }
        static public int RandomInt(float min, float max) { return (int)Random.Range(min, max); }
    
    
        //static public float RandomRange(float min, float max, float range) { return Random.Range(min+(range*0.5f), (max+1.0f)-(range*0.5f)); }
    
    
        static public Vector3 RandomVector3InBox(Vector3 maxPosition) {
            Vector3 minPosition = new Vector3(0.0f, 0.0f, 0.0f);
            Vector3 randomPosition = new Vector3(Random.Range(minPosition.x, maxPosition.x), Random.Range(minPosition.y, maxPosition.y), Random.Range(minPosition.z, maxPosition.z));
            return randomPosition;
        }
        static public Vector3 RandomVector3InBox(Vector3 minPosition, Vector3 maxPosition) {
            Vector3 randomPosition = new Vector3(Random.Range(minPosition.x, maxPosition.x), Random.Range(minPosition.y, maxPosition.y), Random.Range(minPosition.z, maxPosition.z));
            return randomPosition;
        }
    }
}