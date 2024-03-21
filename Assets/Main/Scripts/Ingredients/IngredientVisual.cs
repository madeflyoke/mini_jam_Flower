using System;
using EasyButtons;
using UnityEngine;

namespace Main.Scripts.Ingredients
{
    public class IngredientVisual : MonoBehaviour
    {
        public Color RelatedColor => _relatedColor;
       
        [SerializeField] private ParticleSystem _orbEffect;
        [SerializeField] private Color _relatedColor;
        
        public void DisableOrbEffect()
        {
            _orbEffect.gameObject.SetActive(false);
            _orbEffect.Stop();
        }
        
        private void OnValidate()
        {
            _orbEffect ??= GetComponentInChildren<ParticleSystem>();
            if (_orbEffect)
            {
                var color = _orbEffect.main.startColor.color;
                color.a = 1f;

                _relatedColor = color;
            }
        }
        
    }
}
