using System;
using System.Collections.Generic;
using DG.Tweening;
using NUnit.Framework;
using UnityEngine;


namespace Challenges._6._Abstract_DOTween.Scripts
{
    
    public class PickUpAnimator : DoTweenAnimation
    {
        //Add parameters here

        [Header("Object")]
        [SerializeField]
        private Vector3[] vector3s;
        [SerializeField]
        private float durationMove; 
        [SerializeField]
        private Vector3[] scaleVectors;
        [SerializeField]
        private float durationscale; 
       
        [Header("ObjectShade")]
        [SerializeField]
        private Vector3 shadeVector;
        [SerializeField]
        private float shadeDuration, shadeFade, shadeFadeDuration;

        [Header("Pad")]
        [SerializeField]
        private Vector3 punchVector;
        [SerializeField]
        private float punchDur, punckElas;
        [SerializeField]
        private int punchVib, loop;

        
        /// <summary>
        /// Fill out this function
        /// </summary>
        /// <returns></returns>
        public override Tween StartPreview()
        {
            var sequence = DOTween.Sequence();

            for (int i = 0; i < vector3s.Length; i++)
            { 
               Tween tweenmov = CenterObject.transform.DOLocalMove(vector3s[i], durationMove);
               Tween tween = CenterObject.transform.DOScale(scaleVectors[i], durationscale);
               sequence.Append(tweenmov);
               sequence.Join(tween);
            }
            
            Tween floorPadTween = FloorPad.DOPunchScale(punchVector, punchDur, punchVib, punckElas).SetLoops(loop, LoopType.Incremental);

           
            
            Tween ObjectshadeTween = CenterObjectShade.DOScale(shadeVector, shadeDuration);
            Tween objectshadeFadeTween = CenterObjectShade.GetComponent<Renderer>().sharedMaterial.DOFade(shadeFade, shadeFadeDuration).SetEase(Ease.InExpo);

            sequence.Append(floorPadTween);
            
            sequence.Join(ObjectshadeTween);
            sequence.Join(objectshadeFadeTween);

            return sequence;
        }
    }
}
