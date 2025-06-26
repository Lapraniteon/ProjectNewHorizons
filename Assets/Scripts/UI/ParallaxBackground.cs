using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    private float length, startpos;

    public GameObject cam;

    public float parallaxEffect;

    void Start()
    {
        startpos = transform.position.x;
        length = 2400;
    }

    void FixedUpdate()
    {
        float dist = (cam.transform.position.x * parallaxEffect);

        transform.position = new Vector3(startpos + dist, transform.position.y, transform.position.z);
    }
}
