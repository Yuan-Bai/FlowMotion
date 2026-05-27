
using System;
using System.Collections.Generic;
using Unity.VisualScripting;

public class CountdownTimer
{
    public float duration; // 总时长
    public float remaining; // 剩余时长
    public bool isRunning;
    public bool useUnscaledTime; // 是否受Time.timeScale影响
    public Action onComplete;
    public bool isCanceled;
    public PlayerLocomotionStateId owner;
}

public class PlayerTimeHub
{
    private readonly Dictionary<string, CountdownTimer> timers = new();
    private StateMachine<PlayerLocomotionStateId> _stateMachine;

    public PlayerTimeHub(StateMachine<PlayerLocomotionStateId> stateMachine)
    {
        _stateMachine = stateMachine;
    }

    public void Start(string name, float duration, bool useUnScaledTime, PlayerLocomotionStateId owner = PlayerLocomotionStateId.Null, Action onComplete = null)
    {
        if (timers.TryGetValue(name, out var cdt))
        {
            cdt.duration = duration;
            cdt.remaining = duration;
            cdt.isRunning = true;
            cdt.useUnscaledTime = useUnScaledTime;
            cdt.onComplete = onComplete;
            cdt.isCanceled = false;
            cdt.owner = owner;
        }
        else
        {
            cdt = new()
            {
                duration = duration,
                remaining = duration,
                isRunning = true,
                useUnscaledTime = useUnScaledTime,
                onComplete = onComplete,
                isCanceled = false,
                owner = owner,

            };
            timers.Add(name, cdt);
        }
    }

    public void Update(float deltaTime, float unScaledDeltaTime)
    {
        CountdownTimer cdt;
        foreach (var timer in timers)
        {
            cdt = timer.Value;
            if (cdt.owner != PlayerLocomotionStateId.Null && cdt.owner != _stateMachine.CurrentStateId)
            {
                Cancel(timer.Key);
            }
            else if (cdt.isRunning)
            {
                cdt.remaining -= cdt.useUnscaledTime ? unScaledDeltaTime:deltaTime;
                if (cdt.remaining <= 0)
                {
                    cdt.isRunning = false;
                    cdt.onComplete?.Invoke();
                }
            }
        }
    }

    public bool IsRunning(string name)
    {
        if (timers.TryGetValue(name, out var cdt))
        {
            return cdt.isRunning;
        }
        return false;
    }

    public bool IsFinished(string name)
    {
        if (timers.TryGetValue(name, out var cdt))
        {
            if (cdt.isCanceled)
            {
                return false;
            }
            return cdt.remaining <= 0;
        }
        return false;
    }

    public bool IsContainCDT(string name)
    {
        return timers.ContainsKey(name);
    }

    public void Cancel(string name)
    {
        if (timers.TryGetValue(name, out var cdt))
        {
            cdt.isRunning = false;
            cdt.isCanceled = true;
        }
    }

    public void Restart(string name)
    {
        if (timers.TryGetValue(name, out var cdt))
        {
            cdt.remaining = cdt.duration;
            cdt.isRunning = true;
            cdt.isCanceled = false;
        }
    }

    public void Resume(string name)
    {
        if (timers.TryGetValue(name, out var cdt))
        {
            if (cdt.remaining > 0)
            {
                cdt.isRunning = true;
                cdt.isCanceled = false;
            }
        }
    }
}