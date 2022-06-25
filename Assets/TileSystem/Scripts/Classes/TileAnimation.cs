using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TileSystem
{
    [RequireComponent(typeof(Animator))]
    public class TileAnimation: MonoBehaviour
    {
        [SerializeField] private GameObject matchParticlePrefab;

        [SerializeField] private Color blinkColor = Color.white;
        [SerializeField] private float blinkTime = 1f;
        [SerializeField] private float blinkDelay;

        private Animator _animator;
        
        private static readonly int Match = Animator.StringToHash("Match");
        private static readonly int Punched = Animator.StringToHash("Punched");

        private void Start()
        {
            _animator = GetComponent<Animator>();
        }

        public void PlayMatchAnimation(Vector3 from, ITile to)
        {
            Instantiate(matchParticlePrefab, from, Quaternion.identity).GetComponent<ParticleMover>().MoveTo(to);
          //  PlayMatchAnimation();
        }

        public void PlayPunchedAnimation()
        {
            _animator.SetTrigger(Punched);
        }

        public void PlayMatchAnimation()
        {
            StartCoroutine(BlinkAnimation(blinkTime, blinkColor, blinkDelay));
        }

        private IEnumerator BlinkAnimation(float time, Color newColor, float delay)
        {
            var duration = Time.deltaTime + time;
            
            var material = GetComponent<MeshRenderer>().material;
            var color = material.color;
            
            while (Time.deltaTime < duration)
            {
                material.color = newColor;
                yield return new WaitForSeconds(delay);
                material.color = color;
                yield return new WaitForSeconds(delay);
            }
        }
    }
}