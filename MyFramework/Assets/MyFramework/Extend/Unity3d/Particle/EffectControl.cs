using UnityEngine;

namespace MyFramework
{
    /// <summary>
    /// 特效控制
    /// </summary>
    public class EffectControl : MonoBehaviour
    {
        [SerializeField, MFWAttributeRename("特效显示时间")]
        private float ShowTime;
        [SerializeField, MFWAttributeRename("是否销毁")]
        private bool IsDestory;
        private ParticleSystem[] ParticleSystems;

        void Awake()
        {
            ParticleSystems = GetComponentsInChildren<ParticleSystem>();
        }

        public void SHow()
        {
            gameObject.SetActive(true);
            for (int i = 0; i < ParticleSystems.Length; i++)
            {
                ParticleSystems[i].Play();
            }
            Invoke("ShowFinish", ShowTime);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        private void ShowFinish()
        {
            if (IsDestory)
            {
                GameObject.Destroy(this);
            }
            else
            {
                Hide();
            }
        }

    }
}
