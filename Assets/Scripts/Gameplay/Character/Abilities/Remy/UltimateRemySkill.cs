using System.Collections;
using Gameplay.Character.Ability;

public class UltimateRemySkill : Ability
{
    public override void UpLevel()
    {
        base.UpLevel();
        damage *= 1.2f;
        if (level == 3)
        {
            _cooldown -= 20;
        }
    }
    public override void OnPress()
    {
        if (!_onCooldown && level != 0)
        {
            base.OnPress();
            //StartCoroutine(Buff());
        }
    }

    /*private IEnumerator Buff()
    {
        /*animationController.RefreshMovementSpeed(125);
        animationController.RefreshAttackSpeed(130);
        _combatController.RefreshDamage(1.25f);
        yield return new WaitForSeconds(8.0f);
        animationController.RefreshAttackSpeed(100);
        animationController.RefreshMovementSpeed(100);
        _combatController.RefreshDamage(1.0f);#1#
    }*/
}
