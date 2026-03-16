using UnityEngine;
using System.Collections.Generic;

namespace NB_ToolLibrary
{
    public class AudioRandomizer : MonoBehaviour
    {
        #region Variables declaration

        [SerializeField] private AudioClip[] _audioClips = new AudioClip[0];
        [SerializeField][Range(0,1)] private float _maxPitchModifier = 0.5f;
        [SerializeField][Range(0, 1)] private float _maxVolumeModifier = 0.5f;
        [SerializeField] private float _volumeMultiplier = 1.0f;

        #endregion

        public void PlayAudio()
        {
            if(_audioClips.Length <= 0)
            {
                Debug.LogWarning("Cannot play audio because no clip is selected", this);
                return;
            }

            AudioSource audioSource = gameObject.AddComponent<AudioSource>();

            int randomIndex = Random.Range(0, _audioClips.Length);
            float randomPitch = Random.Range(1 - _maxPitchModifier, 1 + _maxPitchModifier);
            float randomVolume = Random.Range(1 - _maxVolumeModifier, 1 + _maxVolumeModifier);

            AudioClip clip = _audioClips[randomIndex];
            audioSource.clip = clip;
            audioSource.pitch = randomPitch;
            audioSource.volume = randomVolume * _volumeMultiplier;

            audioSource.Play();
            Destroy(audioSource, clip.length / randomPitch);
        }


    }

}

