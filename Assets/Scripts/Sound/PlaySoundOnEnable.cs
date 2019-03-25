using UnityEngine;
using Zenject;

namespace MiniPlanetDefense
{
    /// <summary>
    /// Play a sound using the <see cref="SoundManager"/> in OnEnable().
    /// </summary>
    public class PlaySoundOnEnable : MonoBehaviour
    {
        [SerializeField] Sound sound;
        
        [Inject] SoundManager soundManager;
        
        void OnEnable()
        {
            soundManager.PlaySound(sound);
        }
    }
}