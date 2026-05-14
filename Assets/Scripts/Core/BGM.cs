using System.Collections.Generic;
using UnityEngine;

namespace BackroomsShooter.Core
{
    public class BGM : MonoBehaviour
    {
        public List<AudioClip> AudioClips;
        public float BaseVolume = 0.1f;

        private AudioSource _audioSource;

        private void Start()
        {
            _audioSource = GetComponent<AudioSource>();
            PlayBGM();
        }

        public void PlayBGM()
        {
            if (_audioSource == null) return;

            System.Random sysRand = new System.Random();
            int track = sysRand.Next(0, AudioClips.Count);

            _audioSource.clip = AudioClips[track];
            _audioSource.volume = BaseVolume;
            _audioSource.Play();
        }

        public void MuffleAudio()
        {
            _audioSource.volume = BaseVolume / 4;
        }

        public void RestoreAudio()
        {
            _audioSource.volume = BaseVolume;
        }

        public void PauseAudio()
        {
            _audioSource.Pause();
        }

        public void ResumeAudio()
        {
            _audioSource.UnPause();
        }
    }
}