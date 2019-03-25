using System;
using UnityEngine;

namespace MiniPlanetDefense
{
    /// <summary>
    /// A primitive sound manager, playing <see cref="AudioSource"/>s when called.
    ///
    /// For a bigger project I'd recommend MasterAudio from the Unity Asset Store.
    /// </summary>
    public class SoundManager : MonoBehaviour
    {
        [SerializeField] AudioSource death;
        [SerializeField] AudioSource pickup;
        [SerializeField] AudioSource jump;
        [SerializeField] AudioSource touchPlanet;
        [SerializeField] AudioSource enemySpawned;

        public void PlaySound(Sound sound)
        {
            var audioSource = GetAudioSource(sound);
            if (audioSource != null)
                audioSource.Play();
        }

        AudioSource GetAudioSource(Sound sound)
        {
            switch (sound)
            {
                case Sound.Death:
                    return death;
                
                case Sound.Pickup:
                    return pickup;
                
                case Sound.Jump:
                    return jump;
                
                case Sound.TouchPlanet:
                    return touchPlanet;
                
                case Sound.EnemySpawned:
                    return enemySpawned;
                
                default:
                    throw new NotImplementedException(sound.ToString());
            }
        }
    }
}