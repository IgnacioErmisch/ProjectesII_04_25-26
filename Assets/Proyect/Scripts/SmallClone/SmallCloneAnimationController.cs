using UnityEngine;

public class SmallCloneAnimationController : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private SmallCloneController smallCloneController;
    [SerializeField] private SmallCloneDoubleJump smallCloneDoubleJump;


    void Start()
    {
        animator = GetComponent<Animator>();

    }

    void Update()
    {
        PlayWalkAnimation();
        PlayJumpAnimation();

    }


    private void PlayWalkAnimation()
    {

      
    }

    private void PlayJumpAnimation()
    {

       
    }

}
