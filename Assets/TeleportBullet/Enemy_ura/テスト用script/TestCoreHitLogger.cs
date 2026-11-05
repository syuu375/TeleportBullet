using System.Collections;
using UnityEngine;

//確認用(当たっているかの確認)
//弾幕が当たっていた時は点滅する
public class TestCoreHitLogger : MonoBehaviour, IDamage
{
    [Header("点滅")]
    [SerializeField] private Color flashColor = Color.yellow;
    [SerializeField] private float flashDuration = 0.15f;

    private Renderer coreRenderer;
    private Color originalColor;
    private Coroutine flashRoutine;

    private void Awake()
    {
        coreRenderer = GetComponentInChildren<Renderer>();

        if (coreRenderer != null)
            originalColor = coreRenderer.material.color;
    }

    public void TakeDamage(float damage, float multiplier)
    {
        Debug.Log($"コア「{name}」に弾が当たった（{damage} x {multiplier}）");

        if (coreRenderer == null) return;

       
        if (flashRoutine != null) StopCoroutine(flashRoutine);
        flashRoutine = StartCoroutine(Flash());
    }

    private IEnumerator Flash()
    {
        coreRenderer.material.color = flashColor;
        yield return new WaitForSeconds(flashDuration);
        coreRenderer.material.color = originalColor;
        flashRoutine = null;
    }
}