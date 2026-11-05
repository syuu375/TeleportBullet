using System;
using UnityEngine;

//コア(弱点)の状態管理
//防御部分(DefensePart)に応じて、攻撃判定を有効化
public class CoreExposure : MonoBehaviour
{
    [Header("コア露出条件")]
    [Tooltip("未設定なら自動習得")]
    [SerializeField] private DefensePart[] requiredParts;
    [Tooltip("ON:すべての部位が壊れた時露出 / OFF:どれか1つ壊れた時露出")]
    [SerializeField] private bool requireAllParts = true;

    [Header("露出コア")]
    [Tooltip("露出するまで攻撃判定無効")]
    [SerializeField] private Collider[] coreColliders;

    public bool IsExposed { get; private set; }

    //コア露出時に一回だけ呼ばれる
    public event Action Exposed;

    private void Awake()
    {
        if(requiredParts == null || requiredParts.Length == 0)
            requiredParts = GetComponentsInChildren<DefensePart>(true);

        if (requiredParts.Length == 0)
            Debug.LogWarning($"{name}: 条件になる DefensePart が見つかりません",this);

        foreach(var part in requiredParts)
        {
            if (part != null) part.Broken += OnPartBroken; 
        }

        Refresh();
    }

    //部位が壊れた状態の時、露出を決めなおす(主に再戦時)
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
