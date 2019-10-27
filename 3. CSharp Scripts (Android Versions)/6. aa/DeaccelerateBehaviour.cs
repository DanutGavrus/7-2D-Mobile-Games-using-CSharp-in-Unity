using UnityEngine;

public class DeaccelerateBehaviour : StateMachineBehaviour
{
    public float deaccelerateAmount;
    private Rotator rotator;
    private float rotationSpeed;

    private void Awake()
    {
        rotator = GameObject.Find("RotatorFix").GetComponent<Rotator>();
    }

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        rotationSpeed = rotator.GetRotationSpeed();
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (stateInfo.length * (stateInfo.normalizedTime % 1) < stateInfo.length / 2)
        {
            rotator.SetRotationSpeed(rotationSpeed - deaccelerateAmount * Time.deltaTime);
        }
        else
        {
            rotator.SetRotationSpeed(rotationSpeed + deaccelerateAmount * Time.deltaTime);
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        rotator.SetRotationSpeed(rotationSpeed);
    }
}
