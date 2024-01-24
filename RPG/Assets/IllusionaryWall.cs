using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GI
{
    public class IllusionaryWall : MonoBehaviour
    {
        public bool wallHasBeenHit;
        public float alpha;
        public float fadeTimer = 2.5f;

        // Use a reference to the original material
        public Material originalMaterial;

        // Use a reference to the instantiated material
        private Material illusionaryWallMaterial;

        public MeshCollider wallCollider;

        public AudioSource audioSource;
        public AudioClip illusionaryWallSound;

        private void Awake()
        {
            // Instantiate a copy of the original material for this object
            illusionaryWallMaterial = new Material(originalMaterial);
            GetComponent<Renderer>().material = illusionaryWallMaterial;
        }

        private void Update()
        {
            if (wallHasBeenHit)
            {
                FadeIllusionaryWall();
            }
        }

        public void FadeIllusionaryWall()
        {
            alpha = illusionaryWallMaterial.color.a;
            alpha = alpha - Time.deltaTime / fadeTimer;
            Color fadedWallColor = new Color(1, 1, 1, alpha);
            illusionaryWallMaterial.color = fadedWallColor;

            if (wallCollider.enabled)
            {
                wallCollider.enabled = false;
                if (audioSource != null)
                {
                    audioSource.PlayOneShot(illusionaryWallSound);
                }
            }

            if (alpha <= 0)
            {
                Destroy(this.gameObject);
            }
        }
    }
}
