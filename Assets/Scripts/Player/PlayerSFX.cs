using UnityEngine;

namespace BackroomsShooter.Player
{
    public class PlayerSFX : MonoBehaviour
    {
        public AudioClip PlayerHurtClip;
        [Range(0f, 1f)]
        public float PlayerHurtVolume;

        public AudioClip ReloadClip;
        [Range(0f, 1f)]
        public float ReloadVolume;

        private AudioSource _audioSource;

        private void Start()
        {
            _audioSource = GetComponent<AudioSource>();
        }

        public void TakeDamage()
        {
            _audioSource.PlayOneShot(PlayerHurtClip, PlayerHurtVolume);
        }

        public void Shoot(AudioClip clip, float volume)
        {
            _audioSource.PlayOneShot(clip, volume);
        }

        public void Reload()
        {
            _audioSource.PlayOneShot(ReloadClip, ReloadVolume);
        }
    }
}