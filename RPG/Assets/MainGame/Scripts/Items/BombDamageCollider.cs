using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GI
{
    public class BombDamageCollider : DamageCollider
    {
        [Header("Explosive Damage & Radius")]
        public int explosiveRadius = 1;
        public int explosionDamage;
        public int explosionSplashDamage;

        public Rigidbody bombRigidBody;
        private bool hasCollided = false;
        public GameObject impactParticles;

        protected override void Awake()
        {
            damageCollider = GetComponent<Collider>();
            bombRigidBody = GetComponent<Rigidbody>();
        }

        private void OnCollisionEnter(Collision collision)
        {
            if(!hasCollided)
            {
                hasCollided = true;
                impactParticles = Instantiate(impactParticles, transform.position, Quaternion.identity);
                Explode();

                CharacterManager character = collision.transform.GetComponent<CharacterManager>();

                if(character != null )
                {
                    if(character.characterStatsManager.teamIDNumber != teamIDNumber)
                    {
                        TakeDamageEffect takeDamageEffect = Instantiate(WorldCharacterEffectsManager.instance.takeDamageEffect);
                        takeDamageEffect.physicalDamage = physicalDamage;
                        takeDamageEffect.fireDamage = fireDamage;
                        takeDamageEffect.magicDamage = magicDamage;
                        takeDamageEffect.poiseDamage = poiseDamage;
                        takeDamageEffect.contactPoint = contactPoint;
                        takeDamageEffect.angleHitFrom = angleHitFrom;
                        character.characterEffectsManager.ProcessEffectInstantly(takeDamageEffect);
                    }
                }

                Destroy(impactParticles, 5f);
                Destroy(transform.parent.parent.gameObject);
            }
        }

        private void Explode()
        {
            Collider[] characters = Physics.OverlapSphere(transform.position, explosiveRadius);

            foreach(Collider objectsInExplosion in characters)
            {
                CharacterStatsManager character = objectsInExplosion.GetComponentInParent<CharacterStatsManager>();

                if(character != null)
                {
                    if (character.teamIDNumber != teamIDNumber)
                    {
                        //character.TakeDamage(0, explosionSplashDamage, currentDamageAnimation, characterManager);
                    }
                }
            }
        }
    }
}
