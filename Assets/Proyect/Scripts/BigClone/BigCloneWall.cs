using System.Collections;
using UnityEngine;

public class WallContactDetector
{
    private GameObject currentWall;
    private bool isTouching;

    public bool IsTouchingWall() { 
        return isTouching; 
    }

    public GameObject GetCurrentWall() { 
        return currentWall;
    }

    public void SetWallContact(GameObject wall, bool touching)
    {
        currentWall = wall;
        isTouching = touching;
    }
}


public class WallDestructor
{
    public void DestroyWall(GameObject wall)
    {
        if (wall != null && wall.CompareTag("Wall"))
        {
            Object.Destroy(wall);
        }
    }
}

public class BigCloneWallDestroyer
{
    public WallContactDetector wallContact;
    public WallDestructor wallDestructor;
    public bool isAttacking;
    [SerializeField] private BigCloneAttack bigCloneAttack;
    public BigCloneWallDestroyer(WallContactDetector wallContact, WallDestructor wallDestructor)
    {
        this.wallContact = wallContact;
        this.wallDestructor = wallDestructor;
    }

    public void CheckAndDestroyWall()
    {
        if (wallContact.IsTouchingWall() && Input.GetKeyDown(KeyCode.Mouse0))
        {

            GameObject wall = wallContact.GetCurrentWall();
            if (wall != null)
            {
                wallDestructor.DestroyWall(wall);

            }

        }
    }

    public IEnumerator AttackAnimation()
    {
        isAttacking = true;
        yield return new WaitForSeconds(1.20f);
        isAttacking = false;
    }

    public void StartAttack(MonoBehaviour runner)
    {
        runner.StartCoroutine(AttackAnimation());
    }
}

