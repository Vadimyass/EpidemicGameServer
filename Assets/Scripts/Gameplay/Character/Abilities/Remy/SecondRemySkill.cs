using UnityEngine;
using Gameplay.Character.Ability;

public class SecondRemySkill : Ability
{
    private float radius = 5;
    public override void UpLevel()
    {
        base.UpLevel();
        damage *= 1.2f;
        if(level == 4)
        {
            radius *= 1.5f;
        }
    }
    public override void OnPress()
    {
        if (!_onCooldown && level != 0)
        {
            base.OnPress();
            Collider[] hitColliders = Physics.OverlapSphere(this.gameObject.transform.position, radius);
            foreach (Collider hitCollider in hitColliders)
            {
                if (hitCollider.gameObject.TryGetComponent(out ZombieMelee target))
                {
                    target.ApplyDamage((int)realDamage);
                }
            }
        }
    }
}
