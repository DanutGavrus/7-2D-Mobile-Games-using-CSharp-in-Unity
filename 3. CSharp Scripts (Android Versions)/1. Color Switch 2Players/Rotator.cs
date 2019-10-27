using UnityEngine;

public class Rotator : MonoBehaviour {
    /* Used on any object in order to rotate it.*/
    public int rotationSpeedMin = 100;
    public int rotationSpeedMax = 120;
    private int rotationSpeed;

    private void Start()
    {
        // Get random speed for this object
        rotationSpeed = Random.Range(rotationSpeedMin, rotationSpeedMax + 1);
        // Get random direction for this object
        int rotationDirection = Random.Range(-1, 2);
        while (rotationDirection == 0) rotationDirection = Random.Range(-1, 2);
        // If parent is a double square, only rotate right
        if (gameObject.transform.parent != null && (gameObject.transform.parent.tag == "DoubleSquare" || gameObject.transform.parent.tag == "DoubleCircle"))
        {
            rotationDirection = -1;
            rotationSpeed = (rotationSpeedMin + rotationSpeedMax) / 2;
        }
        rotationSpeed *= rotationDirection;
    }

	private void Update ()
    {
        // Rotate the object
        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
	}
}
