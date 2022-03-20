using UnityEngine;

namespace Gameplay.Character.AnimationControllers
{
    public class CharacterAnimationController : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        private bool _isAttacking = false;
        
        public void SetSpeedToBlendTree(float speedMagnitude)
        {
            _animator.SetFloat("Speed",speedMagnitude);
        }

        public void SetAnimationBool(AnimationNameType animationType,bool boolInput)
        {
            _animator.SetBool(animationType.ToString(), boolInput);
        }
        
        public void SetAnimationTrigger(AnimationNameType animationType)
        {
            _animator.SetTrigger(animationType.ToString());
        }

        public void PlayAttackAnimation()
        {
            _animator.SetTrigger("Attack");
            _isAttacking = true;
        }

        public void OnAttackFinished()
        {
            _isAttacking = false;
        }

        public void RefreshAttackSpeed(float attackSpeed)
        {
            float atkSpeed = attackSpeed / 100.0f;
            _animator.SetFloat("AttackSpeed", atkSpeed);
        }
        public void RefreshMovementSpeed(float movementSpeed)
        {
            float mvmSpeed = movementSpeed / 100.0f;
            //_characterMovement.movementSpeed = mvmSpeed;
            //_animator.SetFloat("MoveSpeed", mvmSpeed);
        }
    }
}