using Gameplay.Character.AnimationControllers;
using System.Collections;
using UnityEngine;

namespace Gameplay.Character.Ability
{
    public abstract class Ability : MonoBehaviour
    {
        [SerializeField] public Sprite abilityImage;
        private int _level;
        [SerializeField] public float _cooldown;
        public float currentCooldown;
        [SerializeField] public CharacterAnimationController animationController;

        [SerializeField] public float damage;
        public float damageMultiplier = 1.0f;
        public float realDamage => damage * damageMultiplier;

        private int _name;
        private int _description;
        public bool _onCooldown;

        public int level;
        public int maxLevel;

        public int[] minLvlForUpgrade = new int[4];

        public virtual void UpLevel()
        {
            level++;
        }
        /*public virtual void OnPress(EventBase eventBase)
        {
            OnPress();
        }*/
        public virtual void OnPress()
        {
            StartCoroutine(OnCooldown());
        }
        private IEnumerator OnCooldown()
        {
            _onCooldown = true;
            currentCooldown = _cooldown;
            while (currentCooldown > 0)
            {
                currentCooldown -= 0.01f;
                yield return new WaitForSeconds(0.01f);
            }
            OnEndCooldown();
        }

        public virtual void OnEndCooldown()
        {
            _onCooldown = false;
        }
        public void RotateCharacaterByTheMouse()
        {
            Plane playerplane = new Plane(Vector3.up, transform.position);
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            float hitdist;

            if (playerplane.Raycast(ray, out hitdist))
            {
                Vector3 targetpoint = ray.GetPoint(hitdist);
                Quaternion targetrotation = Quaternion.LookRotation(targetpoint - transform.position);
                transform.rotation = targetrotation;
            }
        }
    }
}
