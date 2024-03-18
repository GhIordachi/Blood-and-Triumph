using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GI
{
    public class RangedProjectileDamageCollider : DamageCollider
    {
        public RangedAmmoItem ammoItem;
        protected bool hasAlreadyPenetratedASurface;

        Rigidbody arrowRigidBody;
        CapsuleCollider arrowCapsuleCollider;

        protected override void Awake()
        {
            damageCollider = GetComponent<Collider>();
            damageCollider.gameObject.SetActive(true);
            damageCollider.enabled = true;
            arrowRigidBody = GetComponent<Rigidbody>();
            arrowCapsuleCollider = GetComponent<CapsuleCollider>();
        }

        private void OnCollisionEnter(Collision collision)
        {
            shieldHasBeenHit = false;
            hasBeenParried = false;

            CharacterManager enemyManager = collision.gameObject.GetComponentInParent<CharacterManager>();

            Debug.Log(charactersDamagedDuringThisCalculation.Count);

            if (enemyManager != null)
            {
                if (enemyManager.characterStatsManager.teamIDNumber == teamIDNumber)
                    return;

                CheckForParry(enemyManager);
                CheckForBlock(enemyManager);

                if (hasBeenParried)
                    return;

                if (shieldHasBeenHit)
                    return;

                enemyManager.characterStatsManager.poiseResetTimer = enemyManager.characterStatsManager.totalPoiseResetTime;
                enemyManager.characterStatsManager.totalPoiseDefence = enemyManager.characterStatsManager.totalPoiseDefence - poiseDamage;

                //Detects where the collider is hit by the weapon
                contactPoint = collision.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);
                angleHitFrom = (Vector3.SignedAngle(characterManager.transform.forward, enemyManager.transform.forward, Vector3.up));

                TakeDamageEffect takeDamageEffect = Instantiate(WorldCharacterEffectsManager.instance.takeDamageEffect);
                takeDamageEffect.physicalDamage = physicalDamage;
                takeDamageEffect.fireDamage = fireDamage;
                takeDamageEffect.magicDamage = magicDamage;
                takeDamageEffect.poiseDamage = poiseDamage;
                takeDamageEffect.contactPoint = contactPoint;
                takeDamageEffect.angleHitFrom = angleHitFrom;
                enemyManager.characterEffectsManager.ProcessEffectInstantly(takeDamageEffect);
            }

            if (collision.gameObject.tag == "Illusionary Wall")
            {
                IllusionaryWall illusionaryWall = collision.gameObject.GetComponent<IllusionaryWall>();

                illusionaryWall.wallHasBeenHit = true;
            }

            if(!hasAlreadyPenetratedASurface)
            {
                hasAlreadyPenetratedASurface = true;
                arrowRigidBody.isKinematic = true;
                arrowCapsuleCollider.enabled = false;

                gameObject.transform.position = collision.GetContact(0).point;
                gameObject.transform.rotation = Quaternion.LookRotation(transform.forward);
                gameObject.transform.parent = collision.collider.transform;
                
            }
        }

        private void FixedUpdate()
        {
            if(arrowRigidBody.velocity != Vector3.zero)
            {
                arrowRigidBody.rotation = Quaternion.LookRotation(arrowRigidBody.velocity);
            }
        }
    }
}
