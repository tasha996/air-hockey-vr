using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatIK : MonoBehaviour
{
    public Animator anim;
    public Transform puck;
    public Transform paddle;
    public AvatarIKGoal hand;

    private void OnAnimatorIK(int layerIndex)
    {
        anim.SetLookAtWeight(1f);
        anim.SetLookAtPosition(puck.position);

        anim.SetIKPositionWeight(hand, 1);
        anim.SetIKRotationWeight(hand, 1);
        anim.SetIKPosition(hand, paddle.position);
        anim.SetIKRotation(hand, paddle.rotation);
    }

}
