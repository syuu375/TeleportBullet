using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class MyAnimator : MonoBehaviour
{
    [System.Serializable]
    private class AnimClip
    {
        public AnimationClip_UI clip;
        public Image image;
        [ReadOnly] public bool isStop;
        [ReadOnly] public int timer;
    }

    [SerializeField] private List<AnimClip> m_animClips;

    private void Awake()
    {
        Application.targetFrameRate = 60;
        Initialize();
    }

    private void Update()
    {
        for (int i = 0; i < m_animClips.Count; i++)
        {
            PlayAnimation(i);
        }
    }

    private void Initialize()
    {
        for (int i = 0; i < m_animClips.Count; i++)
        {
            m_animClips[i].isStop = true;
            m_animClips[i].timer = 0;
        }
    }

    public void SetTrigger(int clipsIndex)
    {
        Debug.Log("SetTrigger");
        m_animClips[clipsIndex].isStop = false;
        m_animClips[clipsIndex].timer = 0; ;
    }

    public void SetIsStopped(int clipIndex, bool value)
    {
        m_animClips[clipIndex].isStop = value;
    }

    private void PlayAnimation(int clipIndex)
    {
        var animClip = m_animClips[clipIndex];
        if (m_animClips[clipIndex].isStop) return;

        Debug.Log($"[PA]timer: {animClip.timer}");
        animClip.image.sprite = animClip.clip.Sprites[Mathf.Abs(animClip.timer)];

        if (animClip.timer <= -(animClip.clip.SpritesCount + 1) || (animClip.clip.SpritesCount - 1) <= animClip.timer)
        {
            if (animClip.clip.LoopTime)
            {
                animClip.timer = 0;
                return;
            }
            animClip.isStop = true;
            animClip.timer = 0;
        }
        else
        {
            animClip.timer += animClip.clip.PlaybackSpeed;
        }
    }
}
