using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "AnimationClip", menuName = "Scriptable Objects/AnimationClip")]
public class AnimationClip_UI : ScriptableObject
{
    [Header("Component")]
    [SerializeField] private List<Sprite> m_sprites;

    [Header("Option")]
    [SerializeField] private bool m_loopTime;   // ÉãÅ[ÉvÇ∑ÇÈÇ©
    [SerializeField, Range(-1, 1)] private int m_playbackSpeed = 1;

    public List<Sprite> Sprites => m_sprites;
    public int SpritesCount => m_sprites.Count;
    public bool LoopTime => m_loopTime;
    public int PlaybackSpeed => m_playbackSpeed;
}
