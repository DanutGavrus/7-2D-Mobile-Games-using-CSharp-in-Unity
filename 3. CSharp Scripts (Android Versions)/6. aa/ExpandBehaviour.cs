using UnityEngine;

public class ExpandBehaviour : StateMachineBehaviour
{
    public float expendAmount;
    private GameObject rotator;
    private Vector3 scale;

    private void Awake()
    {
        rotator = GameObject.Find("Rotator");
    }

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        scale = rotator.transform.localScale;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (stateInfo.length * (stateInfo.normalizedTime % 1) < stateInfo.length / 2)
        {
            rotator.transform.localScale += Vector3.one * expendAmount * Time.deltaTime;
        }
        else
        {
            rotator.transform.localScale -= Vector3.one * expendAmount * Time.deltaTime;
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        rotator.transform.localScale = scale;
    }
}
