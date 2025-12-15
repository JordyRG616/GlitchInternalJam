using System.Collections.Generic;
using UnityEngine;

namespace Fracta.Sequentiality
{
    public abstract class Sequencer<T> : MonoBehaviour
    {
        [SerializeField] private List<ScriptableSequenceStep<T>> steps = new();
        
        private Sequence<T> sequence;


        protected virtual void Awake()
        {
            foreach (var step in steps)
            {
                sequence.AddStep(step, step.name);
            }
        }

        public virtual void AddStep(ISequenceStep<T> step, string key)
        {
            sequence.AddStep(step, key);
        }

        public virtual void RemoveStep(string key)
        {
            sequence.RemoveStep(key);
        }
        
        public virtual void RemoveStep(ScriptableSequenceStep<T>  step)
        {
            sequence.RemoveStep(step.name);
        }

        public virtual void Resolve(T value)
        {
            sequence.Resolve(value);
        }

        public virtual void Play(T value)
        {
            StartCoroutine(sequence.Play(value, this));
        }
    }
}