using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Fracta.Sequentiality
{
    public class Sequence<T>
    {
        public Signal<SequencePartialResult<T>> OnSequenceStarted = new();
        public Signal<SequencePartialResult<T>> OnSequenceComplete = new();

        private Dictionary<string, ISequenceStep<T>> steps = new();
        private SequencePartialResult<T> lastResult;


        public void AddStep(ISequenceStep<T> step, string key)
        {
            steps.TryAdd(key, step);
        }

        public void RemoveStep(string key)
        {
            if(steps.ContainsKey(key))
                steps.Remove(key);
        }

        public static Sequence<T> CreateFromList(List<ScriptableSequenceStep<T>> list)
        {
            int i = 0;
            var sequence = new Sequence<T>();
            
            foreach (var step in list)
            {
                sequence.AddStep(step, "step_" + i);
                i++;
            }
            
            return sequence;
        }

        public void Resolve(T value)
        {
            var partialResult = new SequencePartialResult<T>();
            partialResult.currentValue = value;
            OnSequenceStarted.Fire(partialResult);

            if (!OnSequenceStarted.Result.success)
            {
                Debug.Log("Sequence was cancelled because OnSequenceStarted.Result was not successful");
                return;
            }

            foreach (var step in steps)
            {
                step.Value.ResolveStep(partialResult);
            }

            lastResult = partialResult;
            OnSequenceComplete.Fire(lastResult);
        }

        public IEnumerator Play(T value, MonoBehaviour invoker)
        {
            var partialResult = new SequencePartialResult<T>();
            partialResult.currentValue = value;
            OnSequenceStarted.Fire(partialResult);

            void OnStepOnStepComplete(SequencePartialResult<T> result) => partialResult = result;

            if (!OnSequenceStarted.Result.success)
            {
                Debug.Log("Sequence was cancelled because OnSequenceStarted.Result was not successful");
                yield break;
            }

            foreach (var step in steps.Values)
            {
                step.OnStepComplete += OnStepOnStepComplete;
                yield return invoker.StartCoroutine(step.ResolveConditionalStep(partialResult));
                step.OnStepComplete -= OnStepOnStepComplete;
            }

            lastResult = partialResult;
            OnSequenceComplete.Fire(lastResult);
        }
    }
}