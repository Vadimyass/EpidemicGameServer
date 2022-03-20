using System.Collections;
using Gameplay.Character.Ability;
using UnityEngine;

public class FirstRemySkill : Ability
{
    [SerializeField] private float _chargeSpeed;
    [SerializeField] private float _maximumDistance;
    //[SerializeField] private HPContoller _hp;
    private Vector3 originPosition;
    private float _originalDamage;

    public override void UpLevel()
    {
        base.UpLevel();
        _originalDamage *= 1.2f;
        _cooldown -= 1;
        if (level == 4)
        {
            _chargeSpeed *= 2;  
        }
    }
    public void Start()
    {
        _originalDamage = damage;
    }
    public override void OnPress()
    {
        if (!_onCooldown && level!=0)
        {
            base.OnPress();
            originPosition = transform.position;
            RotateCharacaterByTheMouse();
            StartCoroutine(Charge());
        }
    }
    private IEnumerator Charge()
    {
        //_hp.isImmune = true;
        float damageFromDistance;
        while (Vector3.Distance(transform.position, originPosition) < _maximumDistance)
        {
            damageFromDistance = 1 + (Vector3.Distance(transform.position, originPosition) / _maximumDistance);
            damage = _originalDamage * damageFromDistance;
            transform.position += transform.forward * _chargeSpeed * 0.01f * (1/(damageFromDistance));
            yield return new WaitForSeconds(0.01f);
        }
        //_hp.isImmune = false;
        damage = _originalDamage;
    }
    private void OnTriggerEnter(Collider other)
    {
        /*if (_hp.isImmune)
        {*/
            if (other.TryGetComponent(out ZombieMelee zombie))
            {
                zombie.ApplyDamage((int)realDamage);
            }
        /*}*/
    }
}
