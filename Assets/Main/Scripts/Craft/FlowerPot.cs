using System;
using System.Collections.Generic;
using Main.Scripts.Flowers;
using UnityEngine;
using Zenject;

namespace Main.Scripts.Craft
{
    public class FlowerPot : MonoBehaviour
    {
        public event Action<FlowerType> OnFlowerGrow;
        [Inject] private FlowerRecipeSetup _recipes;
        [Inject] private FlowersSetup _flowers;
        [SerializeField] private Transform _spawnPoint;
        [SerializeField] private GameObject _plant;
        [Space]
        [Header("Particles")]
        [SerializeField] private ParticleSystem _successParticles;
        [SerializeField] private ParticleSystem _failedParticles;
        private Flower _flower;
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Flask flask))
            {
                CheckRecipe(flask.GetFertilizer());
                GameObject.Destroy(flask.gameObject);
            }
        }

        private void CheckRecipe(HashSet<IngredientRatio> recipe)
        {
            var flower = _recipes.FindFlowerByRecipe(recipe);

            if (flower != FlowerType.NONE)
                CreateFlower(flower);
            else
                PlayFailAnimation();
        }

        private void CreateFlower(FlowerType flowerType)
        {
            if(_successParticles!=null)
                _successParticles.Play();
            _plant.SetActive(false);
            _flower = Instantiate(_flowers.Flowers.Find(f => f.Type == flowerType).Prefab);
            _flower.transform.position = _spawnPoint.position;
            OnFlowerGrow?.Invoke(flowerType);
        }

        private void  PlayFailAnimation()
        {
            if(_failedParticles!=null)
                _failedParticles.Play();
            Debug.Log("FAIL");
            OnFlowerGrow?.Invoke(FlowerType.NONE);
        }

        public void ResetPot(bool good)
        {
            if(good)
                _successParticles.Play();
            if(!good)
                _failedParticles.Play();
            
            _plant.SetActive(true);
            GameObject.Destroy(_flower.gameObject);
        }
    }
}
