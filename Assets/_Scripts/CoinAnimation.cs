using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class CoinAnimation : MonoBehaviour
{
   private float _duration = 1f;
   private float _moveDistance = 0.1f;
   private float _rotateAngle = 360f;

   private void Start()
   {
      AnimateCoin();
   }

   private void AnimateCoin()
   {
      /*transform
         .DOMoveY(transform.position.y + _moveDistance, _duration)
         .SetEase(Ease.Linear)
         .SetLoops(-1, LoopType.Yoyo);*/
      
      transform
         .DORotate(new Vector3(0, -_rotateAngle, 0), _duration, RotateMode.FastBeyond360)
         .SetEase(Ease.Linear)
         .SetLoops(-1, LoopType.Restart);
   }
}
