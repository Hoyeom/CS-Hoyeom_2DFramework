using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI
{
    public class UI_EventHandler : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler,IPointerExitHandler
    {
        private float _defaultSize = 1.0f;
        private float _downSize = 0.85f;
        private float _tweenSpeed = 0.2f;
        
        public event Action<PointerEventData> OnClickHandler = null;
        public event Action<PointerEventData> OnDownHandler = null;
        public event Action<PointerEventData> OnUpHandler = null;

        public void OnPointerClick(PointerEventData eventData)
            => OnClickHandler?.Invoke(eventData);

        public void OnPointerDown(PointerEventData eventData)
        {
            OnDownHandler?.Invoke(eventData);
            TweenScale(_downSize,_tweenSpeed);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            OnUpHandler?.Invoke(eventData);
            TweenScale(_defaultSize,_tweenSpeed);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            TweenScale(_defaultSize,_tweenSpeed);
        }

        private void TweenScale(float scale, float tweenSpeed)
        {
            transform.DOScale(scale, tweenSpeed);
        }
    }
}