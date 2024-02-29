using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GI
{
    public class SpellDamageCollider : DamageCollider
    {
        public GameObject impactParticles;
        public GameObject projectileParticles;
        public GameObject muzzleParticles;

        bool hasCollided = false;

        CharacterManager spellTarget;

        Vector3 impactNormal; //Used to rotate the impact particles

        private void Start()
        {
            projectileParticles = Instantiate(projectileParticles,transform.position,transform.rotation);
            projectileParticles.transform.parent = transform;

            if (muzzleParticles)
            {
                muzzleParticles = Instantiate(muzzleParticles,transform.position,transform.rotation);
                Destroy(muzzleParticles, 2f);
            }
        }

        protected override void Awake()
        {
            base.Awake();            
        }

        private void OnCollisionEnter(Collision other)
        {
            if(!hasCollided)
            {
                spellTarget = other.transform.GetComponent<CharacterManager>();
                if(spellTarget != null && spellTarget.characterStatsManager.teamIDNumber != teamIDNumber)
                {
                    TakeDamageEffect takeDamageEffect = Instantiate(WorldCharacterEffectsManager.instance.takeDamageEffect);
                    takeDamageEffect.physicalDamage = physicalDamage;
                    takeDamageEffect.fireDamage = fireDamage;
                    takeDamageEffect.poiseDamage = poiseDamage;
                    takeDamageEffect.contactPoint = contactPoint;
                    takeDamageEffect.angleHitFrom = angleHitFrom;
                    spellTarget.characterEffectsManager.ProcessEffectInstantly(takeDamageEffect);
                }

                hasCollided = true;
                impactParticles = Instantiate(impactParticles,transform.position,Quaternion.FromToRotation(Vector3.up, impactNormal));

                Destroy(projectileParticles);
                Destroy(impactParticles, 2f);
                Destroy(gameObject, 5f);
            }
        }
    }
}
