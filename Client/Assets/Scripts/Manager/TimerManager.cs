using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace Timer
{
    public class TimerHandler
    {
        public float RemainTime { get; set; }
        public UnityEvent Action { get; set; }

        public TimerHandler(UnityEvent inAction, float inTime)
        {
            Action = inAction;
            RemainTime = inTime;
        }

        public bool Update()
        {
            RemainTime -= Time.deltaTime;
            return RemainTime <= 0.0;
        }
    }

    public class TimerManager
    {
        object _lock = new object();
        LinkedList<TimerHandler> _timers = new LinkedList<TimerHandler>();

        public void Update()
        {
            List<TimerHandler> finishTimers = new List<TimerHandler>();
            lock (_lock)
            {
                foreach (var timer in _timers)
                {
                    if (timer.Update())
                        finishTimers.Add(timer);
                }

                foreach (var timer in finishTimers)
                {
                    _timers.Remove(timer);
                    try
                    {
                        if (timer.Action != null)
                            timer.Action.Invoke();
                    }
                    catch (System.Exception ex)
                    {
                        Debug.LogError(ex);
                    }
                }
            }
        }

        public TimerHandler SetTimer(UnityEvent inAction, float inTime)
        {
            lock (_lock)
            {
                TimerHandler result = new TimerHandler(inAction, inTime);
                _timers.AddLast(result);
                return result;
            }    
        }

        public TimerHandler SetTimerNextUpdate(UnityEvent inAction)
        {
            return SetTimer(inAction, 0.0f);
        }

        public bool Remove(TimerHandler timer)
        {
            lock(_lock)
            {
                return _timers.Remove(timer);
            }
        }
    }
}
