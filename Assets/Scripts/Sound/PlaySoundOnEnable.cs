using UnityEngine;
using Zenject;

namespace MiniPlanetDefense
{
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