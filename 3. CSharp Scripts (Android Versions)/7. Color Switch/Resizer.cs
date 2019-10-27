using UnityEngine;

public class Resizer : MonoBehaviour
{
    /* Constantly enlarging and reducing object size. */
    public float minRadius = 0.9f;
    public float maxRadius = 1.2f;
    public float resizingFactor = 0.1f;
    private float radius = 1f;
    private bool enlarge = true;

    private void Start()
    {
        radius = minRadius;
    }

    private void Update()
    {
        if (enlarge)
        {
            radius += resizingFactor * Time.deltaTime;
            transform.localScale = new Vector3(radius, radius, 1f);
            if (radius >= maxRadius) enlarge = false;
        }
        else
        {
            radius -= resizingFactor * Time.deltaTime;
            transform.localScale = new Vector3(radius, radius, 1f);
            if (radius <= minRadius) enlarge = true;
        }
    }
}
