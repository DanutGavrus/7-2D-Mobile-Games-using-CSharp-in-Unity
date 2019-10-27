using UnityEngine;

public class Rotator : MonoBehaviour {
    /*Controls the rotation of the circle, has a parent for fixing pins skewing.*/
    public float rotationSpeed;
    public float speedToAddEachLevel;
    public float sizeToAddEachLevel;
    public int maxSpeedReachedAtLevel;
    public bool orientationLeft;

    private void Start()
    {
        // Calculate the rotation speed
        if (Level.CurrentLevel <= maxSpeedReachedAtLevel)
        {
            rotationSpeed += speedToAddEachLevel * (Level.CurrentLevel - 1);
        }
        else
        {
            rotationSpeed += speedToAddEachLevel * (maxSpeedReachedAtLevel - 1);
        }
        // Calculate the size of the circle
        for (int i = 0; i <= Level.CurrentLevel; i++)
        {
            transform.localScale += Vector3.one * sizeToAddEachLevel;
        }
    }

    private void Update()
    {
        if (orientationLeft)
        {
            transform.Rotate(0f, 0f, rotationSpeed * Time.deltaTime);
        }
        else
        {
            transform.Rotate(0f, 0f, -rotationSpeed * Time.deltaTime);
        }
    }

    public void ChangeRotation()
    {
        if (orientationLeft)
        {
            orientationLeft = false;
        }
        else
        {
            orientationLeft = true;
        }
    }

    public float GetRotationSpeed()
    {
        return rotationSpeed;
    }

    public void SetRotationSpeed(float rotationSpeed)
    {
        this.rotationSpeed = rotationSpeed;
    }
}
