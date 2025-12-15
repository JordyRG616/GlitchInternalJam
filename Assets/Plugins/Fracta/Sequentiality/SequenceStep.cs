using System;
using System.Collections;

namespace Fracta.Sequentiality
{
    public interface ISequenceStep<T>
    {
        public Action<SequencePartialResult<T>> OnStepComplete { get; set; }
        public SequencePartialResult<T> ResolveStep(SequencePartialResult<T> partialResult);
        public IEnumerator ResolveConditionalStep(SequencePartialResult<T> partialResult);
    }

    public abstract class SequenceStep<T> : ISequenceStep<T>
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