using System.Threading;
using Cysharp.Threading.Tasks;
using GGPlugins.GGStateMachine.Scripts.Abstract;
using UnityEngine;
using DG.Tweening;

namespace Challenges._1._GGStateMachineCharacterPhysics.Scripts.States
{
    /// <summary>
    /// You can edit this
    /// </summary>
    public class MovState : GGStateBase
    {
        private Transform charTransform;

        private const float ScaleFac = 1.25f;
        private const float ScaleTime = 1f;
        
        public MovState(Transform characterTransform)
        {
            charTransform = characterTransform;
        }

        public override void Setup()
        {
        }

        public override async UniTask Entry(CancellationToken cancellationToken)
        {
            charTransform.DOScale(charTransform.localScale * ScaleFac, ScaleTime)
                .SetLoops(-1, LoopType.Yoyo);
        }

        public override async UniTask Exit(CancellationToken cancellationToken)
        {
            DOTween.KillAll();
            charTransform.localScale = Vector3.one;
        }

        public override void CleanUp()
        {
        }
    }
}