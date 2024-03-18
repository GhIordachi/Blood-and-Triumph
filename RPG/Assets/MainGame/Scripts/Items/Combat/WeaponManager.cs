using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GI
{
    public class WeaponManager : MonoBehaviour
    {
        [Header("Buff FX")]
        [SerializeField] GameObject physiscalBuffFX;
        [SerializeField] GameObject fireBuffFX;
        [SerializeField] GameObject magicBuffFX;

        [Header("Trail FX")]
        [SerializeField] ParticleSystem defaultTrailFX;
        [SerializeField] ParticleSystem fireTrailFX;
        [SerializeField] ParticleSystem magicTrailFX;

        private bool weaponIsBuffed;
        private BuffClass weaponBuffClass;
        [HideInInspector] public MeleeWeaponDamageCollider damageCollider;
        public AudioSource audioSource;

        private void Awake()
        {
            damageCollider = GetComponentInChildren<MeleeWeaponDamageCollider>();
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        public void BuffWeapon(BuffClass buffClass, float physicalBuffDamage, float fireBuffDamage,float magicBuffDamage, float poiseBuffDamage)
        {
            // Reset any active buff
            DebuffWeapon();
            weaponIsBuffed = true;
            weaponBuffClass = buffClass;
            audioSource.Play();

            switch(buffClass)
            {
                case BuffClass.Physical:
                    physiscalBuffFX.SetActive(true);
                    break;
                case BuffClass.Fire:
                    fireBuffFX.SetActive(true);
                    break;
                case BuffClass.Magic:
                    magicBuffFX.SetActive(true);
                    break;
                default: 
                    break;

            }

            damageCollider.physicalBuffDamage = physicalBuffDamage;
            damageCollider.fireBuffDamage = fireBuffDamage;
            damageCollider.magicBuffDamage = magicBuffDamage;
            damageCollider.poiseBuffDamage = poiseBuffDamage;
        }

        public void DebuffWeapon()
        {
            weaponIsBuffed = false;
            audioSource.Stop();
            physiscalBuffFX.SetActive(false);
            fireBuffFX.SetActive(false);

            damageCollider.physicalBuffDamage = 0;
            damageCollider.fireBuffDamage = 0;
            damageCollider.magicBuffDamage = 0;
            damageCollider.poiseBuffDamage = 0;
        }

        public void PlayWeaponTrailFX()
        {
            if(weaponIsBuffed)
            {
                switch(weaponBuffClass)
                {
                    //If our weapon is physically buffed, play the default trail
                    case BuffClass.Physical:
                        if (defaultTrailFX == null)
                            return;
                        defaultTrailFX.Play();
                        break;
                    //If our weapon is firelly buffed, play the fire trail
                    case BuffClass.Fire:
                        if(fireTrailFX == null) 
                            return;
                        fireTrailFX.Play();
                        break;
                    case BuffClass.Magic:
                        if (magicTrailFX == null)
                            return;
                        magicTrailFX.Play();
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
