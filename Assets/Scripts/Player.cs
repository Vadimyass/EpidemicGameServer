using System.Collections;
using System.Collections.Generic;
using Gameplay.Character.AnimationControllers;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] public CharacterAnimationController animator;
    [SerializeField] public FirstRemySkill firstRemySkill;
    public int id;
    public string username;
    public void Initialize(int _id, string _username)
    {
        id = _id;
        username = _username;
        animator = GetComponent<CharacterAnimationController>();
        firstRemySkill = GetComponent<FirstRemySkill>();
    }

    /// <summary>Processes player input and moves the player.</summary>
    /// 
    /// <summary>Calculates the player's desired movement direction and moves him.</summary>
    /// <param name="_inputDirection"></param>
    private void Move(Vector3 inputDirection)
    {
        transform.position = inputDirection;
        //ServerSend.PlayerPosition(this);
        //ServerSend.PlayerRotation(this);
    }

    public void RotateCharacter(Quaternion quaternion)
    {
        transform.rotation = quaternion;
    }
    /// <summary>Updates the player input with newly received input.</summary>
    /// <param name="_inputs">The new key inputs.</param>
    /// <param name="_rotation">The new rotation.</param>
    public void SetInput(Vector3 playerPosition,Vector3 movementInput)
    {
        transform.forward =  movementInput != Vector3.zero ? movementInput : transform.forward;
        animator.SetSpeedToBlendTree(movementInput.magnitude);
        Move(playerPosition);
    }
    
}
