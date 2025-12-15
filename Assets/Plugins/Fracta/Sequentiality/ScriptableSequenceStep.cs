using System;
using System.Collections;
using UnityEngine;

namespace Fracta.Sequentiality
{
    public abstract class ScriptableSequenceStep<T> : ScriptableObject, ISequenceStep<T>
    {
        public Action<SequencePartialResult<T>> OnStepComplete { get; set; }


        public abstract SequencePartialResult<T> ResolveStep(SequencePartialResult<T> partialResult);

        public virtual IEnumerator ResolveConditionalStep(SequencePartialResult<T> partialResult)
        {
            OnStepComplete.Invoke(partialResult);
            yield return null;
        }
    }
}