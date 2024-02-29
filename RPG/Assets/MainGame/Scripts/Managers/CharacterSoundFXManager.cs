using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GI
{
    public class CharacterSoundFXManager : MonoBehaviour
    {
        CharacterManager character;
        AudioSource audioSource;

        [Header("Taking Damage Sounds")]
        public AudioClip[] takingDamageSounds;
        private List<AudioClip> potentialDamageSounds;
        private AudioClip lastDamageSoundPlayed;

        [Header("Weapon Whooshes")]
        private List<AudioClip> potentialWeaponWhooshes;
        private AudioClip lastWeaponWhoosh;

        protected virtual void Awake()
        {
            character = GetComponent<CharacterManager>();
            audioSource = GetComponent<AudioSource>();
        }

        public virtual void PlayRandomDamageSoundFX()
        {
            potentialDamageSounds = new List<AudioClip>();

            foreach(var damageSound in takingDamageSounds)
            {
                if (damageSound != lastDamageSoundPlayed)
                {
                    potentialDamageSounds.Add(damageSound);
                }
            }

            int randomValue = Random.Range(0, potentialDamageSounds.Count);
            lastDamageSoundPlayed = takingDamageSounds[randomValue];
            audioSource.PlayOneShot(takingDamageSounds[randomValue], 0.4f);
        }

        public virtual void PlayRandomWeaponWhoosh()
        {
            potentialWeaponWhooshes = new List<AudioClip>();

            if (character.isUsingRightHand)
            {
                foreach(var whooshSound in character.characterInventoryManager.rightWeapon.weaponWhooshes)
                {
                    if(whooshSound != lastWeaponWhoosh)
                    {
                        potentialWeaponWhooshes.Add(whooshSound);
                    }

                    int randomValue = Random.Range(0, potentialWeaponWhooshes.Count);
                    lastWeaponWhoosh = character.characterInventoryManager.rightWeapon.weaponWhooshes[randomValue];
                    audioSource.PlayOneShot(character.characterInventoryManager.rightWeapon.weaponWhooshes[randomValue], 0.4f);
                }
            }
            else
            {
                foreach (var whooshSound in character.characterInventoryManager.leftWeapon.weaponWhooshes)
                {
                    if (whooshSound != lastWeaponWhoosh)
                    {
                        potentialWeaponWhooshes.Add(whooshSound);
                    }

                    int randomValue = Random.Range(0, potentialWeaponWhooshes.Count);
                    lastWeaponWhoosh = character.characterInventoryManager.leftWeapon.weaponWhooshes[randomValue];
                    audioSource.PlayOneShot(character.characterInventoryManager.leftWeapon.weaponWhooshes[randomValue], 0.4f);
                }
            }
        }

        public virtual void PlaySoundFX(AudioClip soundFX)
        {
            audioSource.PlayOneShot(soundFX);
        }

        public virtual void PlayRandomSoundFXFromArray(AudioClip[] soundArray)
        {
            int index = Random.Range(0, soundArray.Length);

            PlaySoundFX(soundArray[index]);
        }
    }
}
