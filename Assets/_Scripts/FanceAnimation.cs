using DG.Tweening;
using UnityEngine;

public class FanceAnimation : MonoBehaviour
{
    public float duration = 1f;

    public void Animate()
    {
        transform.DORotate(new Vector3(90f, 0f, 0f), duration, RotateMode.WorldAxisAdd)
                 .SetEase(Ease.InBack);
    }
}
