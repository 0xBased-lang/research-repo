using UnityEngine;
using System.Collections;

namespace MergeBounce
{
    public class GameJuice : MonoBehaviour
    {
        [Header("Particle Effects")]
        public ParticleSystem pegHitParticles;
        public ParticleSystem mergeExplosion;
        public ParticleSystem comboFireworks;

        [Header("Screen Shake")]
        public float pegHitShakeDuration = 0.1f;
        public float pegHitShakeMagnitude = 2f;
        public float mergeShakeDuration = 0.3f;
        public float mergeShakeMagnitude = 10f;

        private Camera mainCamera;

        void Start()
        {
            mainCamera = Camera.main;
        }

        public void OnPegHit(Vector3 position)
        {
            if (pegHitParticles != null)
            {
                var particles = Instantiate(pegHitParticles, position, Quaternion.identity);
                particles.Play();
                Destroy(particles.gameObject, 2f);
            }

            StartCoroutine(CameraShake(pegHitShakeDuration, pegHitShakeMagnitude));
        }

        public void OnMerge(Vector3 position, int ballValue)
        {
            Time.timeScale = 0.3f;
            Invoke(nameof(ResetTimeScale), 0.3f);

            if (mergeExplosion != null)
            {
                var explosion = Instantiate(mergeExplosion, position, Quaternion.identity);
                explosion.Play();
                Destroy(explosion.gameObject, 2f);
            }

            StartCoroutine(CameraShake(mergeShakeDuration, mergeShakeMagnitude));
            StartCoroutine(CameraFlash(Color.white, 0.2f));
        }

        IEnumerator CameraShake(float duration, float magnitude)
        {
            Vector3 originalPos = mainCamera.transform.localPosition;
            float elapsed = 0f;

            while (elapsed < duration)
            {
                float x = Random.Range(-1f, 1f) * magnitude;
                float y = Random.Range(-1f, 1f) * magnitude;
                mainCamera.transform.localPosition = new Vector3(x, y, originalPos.z);
                elapsed += Time.deltaTime;
                yield return null;
            }

            mainCamera.transform.localPosition = originalPos;
        }

        IEnumerator CameraFlash(Color color, float duration)
        {
            // TODO: Implement camera flash overlay
            yield return new WaitForSeconds(duration);
        }

        void ResetTimeScale()
        {
            Time.timeScale = 1f;
        }
    }
}
