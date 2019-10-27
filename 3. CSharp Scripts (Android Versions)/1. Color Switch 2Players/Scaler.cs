using UnityEngine;

public class Scaler : MonoBehaviour
{
    /* Generates random scale for circles(like changing the radius). */
    public float minRadius = 0.9f;
    public float maxRadius = 1.2f;
    private float radius = 1f;

    private void Start()
    {
        if (gameObject.transform.parent == null)
        {
            ChangeRadius();
        }
        else if (gameObject.transform.parent.tag != "DoubleCircle" && gameObject.transform.parent.tag != "DoubleSquare")
        {
            ChangeRadius();
        }
    }

    public void ChangeRadius()
    {
        radius = Random.Range(minRadius, maxRadius);
        transform.localScale = new Vector3(radius, radius, 1f);
    }
}
