using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GI
{
    [CreateAssetMenu(menuName = "Spells/Ring of Fire Spell")]
    public class RingOfFireSpell : SpellItem
    {
        [Header("Ring of Fire Settings")]
        public GameObject fireballPrefab;
        public int numberOfFireballs = 8;
        public float radius = 5f;
        public float rotationSpeed = 50f;
        public int baseDamage;
        public float duration = 5f;
        public float spawnHeight = 1f;

        private List<GameObject> fireballs = new List<GameObject>();
        private bool isCasting = false;

        public override void AttemptToCastSpell(CharacterManager character)
        {
            base.AttemptToCastSpell(character);

            GameObject instantiatedWarmUpSpellFX = Instantiate(spellWarmUpFX, character.transform);
            instantiatedWarmUpSpellFX.gameObject.transform.localScale = new Vector3(100, 100, 100);
            character.characterAnimatorManager.PlayTargetAnimation(spellAnimation, true, false, character.isUsingLeftHand);
        }

        public override void SuccessfullyCastSpell(CharacterManager character)
        {
            base.SuccessfullyCastSpell(character);
            CoroutineHandler.instance.StartRoutine(CastRingOfFire(character));
        }

        private IEnumerator CastRingOfFire(CharacterManager character)
        {
            PlayerManager player = character as PlayerManager;

            if (player == null)
            {
                yield break;
            }

            isCasting = true;

            // Instantiate fireballs in a circular pattern around the player
            for (int i = 0; i < numberOfFireballs; i++)
            {
                float angle = i * Mathf.PI * 2 / numberOfFireballs;
                Vector3 fireballPosition = new Vector3(Mathf.Cos(angle), spawnHeight, Mathf.Sin(angle)) * radius + player.transform.position;
                GameObject fireball = Instantiate(fireballPrefab, fireballPosition, Quaternion.identity);
                SpellDamageCollider spellDamageCollider = fireball.GetComponent<SpellDamageCollider>();
                spellDamageCollider.teamIDNumber = player.playerStatsManager.teamIDNumber;
                spellDamageCollider.fireDamage = baseDamage;
                fireballs.Add(fireball);
            }

            float elapsedTime = 0f;

            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;

                // Rotate the fireballs around the player
                for (int i = 0; i < fireballs.Count; i++)
                {
                    if (fireballs[i] != null)
                    {
                        float angle = i * Mathf.PI * 2 / numberOfFireballs + rotationSpeed * elapsedTime;
                        Vector3 fireballPosition = new Vector3(Mathf.Cos(angle), spawnHeight, Mathf.Sin(angle)) * radius + player.transform.position;
                        fireballs[i].transform.position = fireballPosition;
                    }
                }

                yield return null;
            }

            // Destroy all fireballs after the duration ends
            foreach (var fireball in fireballs)
            {
                if (fireball != null)
                {
                    Destroy(fireball);
                }
            }

            fireballs.Clear();
            isCasting = false;
        }
    }
}
