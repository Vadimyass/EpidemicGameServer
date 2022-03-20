using UnityEngine;
using Gameplay.Character.Ability;

public class ThirdRemySkill : Ability
{
    [Range(0, 360)]
    [SerializeField] private float _angle;
    
    public override void UpLevel()
    {
        base.UpLevel();
        damage *= 1.2f;
        if (level == 4)
        {
            _angle += 20;
        }
    }
    public override void OnPress()
    {
        if (!_onCooldown && level != 0)
        {
            base.OnPress();
            RotateCharacaterByTheMouse();
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, 5);
            foreach (Collider hitCollider in hitColliders)
            {
                if (hitCollider.gameObject.TryGetComponent(out ZombieMelee target))
                {
                    Vector3 dirToTarget = (hitCollider.transform.position - transform.position).normalized;
                    if (Vector3.Angle(transform.forward, dirToTarget) < _angle / 2)
                    {
                        target.ApplyDamage((int)realDamage);
                    }
                }
            } 
        }
    }
}
