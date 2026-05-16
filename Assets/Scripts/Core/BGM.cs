using System.Collections;
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
            if (_audioSource != null && AudioClips.Count > 0) PlayBGM();
        }

        public void PlayBGM()
        {
            if (_audioSource == null || AudioClips.Count == 0) return;

            StopAllCoroutines();

            int trackIndex = GetNewTrackIndex();
           
            _audioSource.clip = AudioClips[trackIndex];
            _audioSource.volume = BaseVolume;
            _audioSource.Play();

            StartCoroutine(TrackEndCoroutine());
        }

        private int GetNewTrackIndex()
        {
            System.Random sysRand = new System.Random();
            int track = sysRand.Next(0, AudioClips.Count);
            return track;
        }

        private IEnumerator TrackEndCoroutine()
        {
            while (_audioSource.isPlaying)
                yield return new WaitForSeconds(1f);

            PlayBGM();
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