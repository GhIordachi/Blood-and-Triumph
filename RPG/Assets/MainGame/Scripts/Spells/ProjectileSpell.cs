using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GI
{
    [CreateAssetMenu(menuName = "Spells/Projectile Spell")]
    public class ProjectileSpell : SpellItem
    {
        [Header("Projectile Damage")]
        public float baseDamage;

        [Header("ProjectileSpell Physics")]
        public float projectileForwardVelocity;
        public float projectileUpwardVelocity;
        public float projectileMass;
        public bool isAffectedByGravity;
        Rigidbody rigidbody;

        public override void AttemptToCastSpell
            (PlayerAnimatorManager animatorHandler, PlayerStatsManager playerStats, PlayerWeaponSlotManager weaponSlotManager)
        {
            base.AttemptToCastSpell(animatorHandler, playerStats, weaponSlotManager);
            //Instantiate the spell in the casting hand of the player
            GameObject instantiatedWarmUpSpellFX = Instantiate(spellWarmUpFX, weaponSlotManager.rightHandSlot.transform);
            instantiatedWarmUpSpellFX.gameObject.transform.localScale = new Vector3(100,100,100);
            //Play animation to cast the spell
            animatorHandler.PlayTargetAnimation(spellAnimation, true);
        }

        public override void SuccessfullyCastSpell
            (PlayerAnimatorManager animatorHandler, PlayerStatsManager playerStats, CameraHandler cameraHandler, PlayerWeaponSlotManager weaponSlotManager)
        {
            base.SuccessfullyCastSpell(animatorHandler, playerStats, cameraHandler, weaponSlotManager);
            GameObject instantiatedSpellFX = 
                Instantiate(spellCastFX, weaponSlotManager.rightHandSlot.transform.position, cameraHandler.cameraPivotTransform.rotation);
            SpellDamageCollider spellDamageCollider = instantiatedSpellFX.GetComponent<SpellDamageCollider>();
            spellDamageCollider.teamIDNumber = playerStats.teamIDNumber;
            rigidbody = instantiatedSpellFX.GetComponent<Rigidbody>();
            //spellDamageCollider = instantiatedSpellFX.GetComponent<SpellDamageCollider>();

            if(cameraHandler.currentLockOnTarget != null)
            {
                instantiatedSpellFX.transform.LookAt(cameraHandler.currentLockOnTarget.transform);
            }
            else
            {
                instantiatedSpellFX.transform.rotation = 
                    Quaternion.Euler(cameraHandler.cameraPivotTransform.eulerAngles.x, playerStats.transform.eulerAngles.y, 0);
            }

            rigidbody.AddForce(instantiatedSpellFX.transform.forward * projectileForwardVelocity);
            rigidbody.AddForce(instantiatedSpellFX.transform.up * projectileUpwardVelocity);
            rigidbody.useGravity = isAffectedByGravity;
            rigidbody.mass = projectileMass;
            instantiatedSpellFX.transform.parent = null;
        }
    }
}
