using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GI
{
    public class CharacterEffectsManager : MonoBehaviour
    {
        CharacterManager character;

        [Header("Static Effects")]
        [SerializeField] List<StaticCharacterEffect> staticCharacterEffects;

        [Header("Timed Effects")]
        public List<CharacterEffect> timedEffects;
        [SerializeField] float effectTickTimer = 0;

        [Header("Timed Effect Visual FX")]
        public List<GameObject> timedEffectParticles;

        [Header("Current FX")]
        public GameObject instantiatedFXModel;

        [Header("Damage FX")]
        public GameObject bloodSplatterFX;

        [Header("Weapon FX")]
        public WeaponManager rightWeaponManager;
        public WeaponManager leftWeaponManager;

        [Header("Right Weapon Buff")]
        public WeaponBuffEffect rightWeaponBuffEffect;

        [Header("Poison")]
        public Transform buildUpTransform; //The location of build up particle FX that will spawn
        float timer;

        protected virtual void Awake()
        {
            character = GetComponent<CharacterManager>();
        }

        private void Start()
        {
            foreach(var effect in staticCharacterEffects)
            {
                effect.AddStaticEffect(character);
            }
        }

        public virtual void ProcessAllTimedEffects()
        {

            effectTickTimer = effectTickTimer + Time.deltaTime;

            if (effectTickTimer >= 1)
            {
                effectTickTimer = 0;
                ProcessWeaponBuffs();

                //Processes all active effects over game time
                for(int i = timedEffects.Count -1; i > -1; i--)
                {
                    timedEffects[i].ProcessEffect(character);
                }

                //Decays build up effects over game time
                ProcessBuildUpDecay();
            }
        }

        public virtual void ProcessEffectInstantly(CharacterEffect effect)
        {
            effect.ProcessEffect(character);
        }

        public void ProcessWeaponBuffs()
        {
            if(rightWeaponBuffEffect != null)
            {
                rightWeaponBuffEffect.ProcessEffect(character);
            }
        }

        public void AddStaticEffect(StaticCharacterEffect effect)
        {
            StaticCharacterEffect staticEffect;
            for(int i = staticCharacterEffects.Count - 1; i > -1; i--)
            {
                if (staticCharacterEffects[i] != null)
                {
                    if (staticCharacterEffects[i].effectID == effect.effectID)
                    {
                        staticEffect = staticCharacterEffects[i];
                        //We remove the actual effect from our character
                        staticEffect.RemoveStaticEffect(character);
                        //We remove the effect from the list of active effects
                        staticCharacterEffects.Remove(staticEffect);
                    }
                }
            }

            //We add the effect to our list of active effects
            staticCharacterEffects.Add(effect);
            //We add the actual effect to our character
            effect.AddStaticEffect(character);

            //Check the list for null items and remove them
            for( int i = staticCharacterEffects.Count -1; i > -1; i--)
            {
                if (staticCharacterEffects[i] == null)
                {
                    staticCharacterEffects.RemoveAt(i);
                }
            }
        }

        public void RemoveStaticEffect(int effectID)
        {
            StaticCharacterEffect staticEffect;

            for(int i = staticCharacterEffects.Count - 1;i > -1; i--)
            {
                if (staticCharacterEffects[i] != null)
                {
                    if (staticCharacterEffects[i].effectID == effectID)
                    {
                        staticEffect = staticCharacterEffects[i];
                        //We remove the actual effect from our character
                        staticEffect.RemoveStaticEffect(character);
                        //We remove the effect from the list of active effects
                        staticCharacterEffects.Remove(staticEffect);
                    }
                }
            }

            //Check the list for null items and remove them
            for (int i = staticCharacterEffects.Count - 1; i > -1; i--)
            {
                if (staticCharacterEffects[i] == null)
                {
                    staticCharacterEffects.RemoveAt(i);
                }
            }
        }

        public virtual void PlayWeaponFX(bool isLeft)
        {
            if(!isLeft)
            {
                if(rightWeaponManager != null)
                {
                    rightWeaponManager.PlayWeaponTrailFX();
                }
            }
            else
            {
                if(leftWeaponManager != null)
                {
                    leftWeaponManager.PlayWeaponTrailFX();
                }
            }
        }

        public virtual void PlayBloodSplatterFX(Vector3 bloodSplatterLocation)
        {
            GameObject blood = Instantiate(bloodSplatterFX, bloodSplatterLocation, Quaternion.identity);
        }

        public virtual void InteruptEffect()
        {
            //Can be used to destroy effects models (Drinking potions, arrows etc.)
            if(instantiatedFXModel != null)
            {
                Destroy(instantiatedFXModel);
            }

            //Firesthe character's bow and removes the arrow
            if(character.isHoldingArrow)
            {
                character.animator.SetBool("isHoldingArrow", false);
                Animator rangedWeaponAnimator = character.characterWeaponSlotManager.rightHandSlot.curentWeaponModel.GetComponentInChildren<Animator>();

                if(rangedWeaponAnimator != null)
                {
                    rangedWeaponAnimator.SetBool("isDrawn", false);
                    rangedWeaponAnimator.Play("Bow_TH_Fire_01");
                }
            }

            //Removes player from aiming state
            if (character.isAiming)
            {
                character.animator.SetBool("isAiming", false);
            }
        }

        protected virtual void ProcessBuildUpDecay()
        {
            if(character.characterStatsManager.poisonBuildup > 0)
            {
                character.characterStatsManager.poisonBuildup -= 1;
            }
        }

        public virtual void AddTimedEffectParticle(GameObject effect)
        {
            GameObject effectGameObject = Instantiate(effect, buildUpTransform);
            timedEffectParticles.Add(effectGameObject);
        }

        public virtual void RemoveTimedEffectParticle(EffectParticleType effectType)
        {
            for(int i= timedEffectParticles.Count-1; i > -1; i--)
            {
                if (timedEffectParticles[i].GetComponent<EffectParticle>().effectType == effectType)
                {
                    Destroy(timedEffectParticles[i]);
                    timedEffectParticles.RemoveAt(i);
                }
            }
        }
    }
}
