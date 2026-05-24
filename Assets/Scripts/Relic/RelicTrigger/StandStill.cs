using System;
using System.Collections;
using UnityEngine;

namespace Relic.RelicTrigger {
    public class StandStill {
        private readonly Action<GameObject> _onComplete;
        private readonly int _seconds;
        private bool interrupt = false;
        private bool waitingCoroutine = false;

        public StandStill(RelicTriggerData relicTriggerData, Action<GameObject> onComplete) {
            _onComplete = onComplete;
            _seconds = RPNEvaluator.RPNEvaluator.Evaluate(relicTriggerData.Amount, new ());

            EventBus.Instance.MovementStarted += StartedMoving;
            EventBus.Instance.MovementStopped += StoppedMoving;
        }

        private void StartedMoving(GameObject subject) {
            interrupt = true;
        }

        private void StoppedMoving(GameObject subject) {
            if (waitingCoroutine) {
                return;
            }
            
            interrupt = false;
            int startWaitingMilliSec = Environment.TickCount; // Milliseconds elapsed since system boot
            // Any monobehavior will do. yea it's messed up
            subject.GetComponent<MonoBehaviour>().StartCoroutine(WaitSecondsInterruptable(startWaitingMilliSec, subject));
        }

        private IEnumerator WaitSecondsInterruptable(int startWaitingMilliSec, GameObject subject) {
            waitingCoroutine = true;
            while (Environment.TickCount - startWaitingMilliSec < _seconds * 1000) {
                if (interrupt) {
                    break;
                }
                yield return null;
            }

            if (!interrupt) {
                _onComplete(subject);
            }
            waitingCoroutine = false;
        }
    }
}