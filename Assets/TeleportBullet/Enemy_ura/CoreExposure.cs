using System;
using UnityEngine;

public class CoreExposure : MonoBehaviour
{
    [Header("ƒRƒA˜IoğŒ")]
    [Tooltip("–¢İ’è‚È‚ç©“®K“¾")]
    [SerializeField] private DefensePart[] requiredParts;
    [Tooltip("ON:‚·‚×‚Ä‚Ì•”ˆÊ‚ª‰ó‚ê‚½˜Io / OFF:‚Ç‚ê‚©1‚Â‰ó‚ê‚½˜Io")]
    [SerializeField] private bool requireAllParts = true;

    [Header("˜IoƒRƒA")]
    [Tooltip("˜Io‚·‚é‚Ü‚ÅUŒ‚”»’è–³Œø")]
    [SerializeField] private Collider[] coreColliders;

    public bool IsExposed { get; private set; }

    //ƒRƒA‚ª˜Io‚µ‚½ˆê‰ñ‚¾‚¯ŒÄ‚Î‚ê‚é
    public event Action Exposed;

    private void Awake()
    {
        if(requiredParts == null || requiredParts.Length == 0)
            requiredParts = GetComponentsInChildren<DefensePart>(true);

        if (requiredParts.Length == 0)
            Debug.LogWarning($"{name}: ğŒ‚É‚È‚é DefensePart ‚ªŒ©‚Â‚©‚è‚Ü‚¹‚ñ",this);

        foreach(var part in requiredParts)
        {
            if (part != null) part.Broken += OnPartBroken; 
        }

        Refresh();
    }

    private void OnDestroy()
    {
        if (requiredParts == null) return;

        foreach(var part in requiredParts)
        {
            if (part != null) part.Broken -= OnPartBroken;
        }
    }

    private void OnPartBroken(DefensePart part)
    {
        Refresh();
    }

    public void Refresh()
    {
        bool shouldExpose = IsConditionMet();
        SetCoreCollidersEnabled(shouldExpose);

        if(shouldExpose && !IsExposed)
        {
            IsExposed = true;
            Exposed?.Invoke();
        }
        else if(!shouldExpose)
        {
            IsExposed = false;
        }
    }

    private bool IsConditionMet()
    {
        if (requiredParts == null || requiredParts.Length == 0) return false;

        int brokenCount = 0;
        foreach(var part in requiredParts)
        {
            if (part != null && part.IsBroken) brokenCount++;
        }

        return requireAllParts ? brokenCount >= requiredParts.Length : brokenCount > 0;
    }

    private void SetCoreCollidersEnabled(bool endbled)
    {
        foreach(var c in coreColliders)
        {
            if (c != null) c.enabled = endbled;
        }
    }
}
