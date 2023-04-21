using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogicGate : Activatable
{
    public bool and_gate = true;
    protected int active_count;
    public bool or_gate = false;
    // Things that get activated.
    public List<Activatable> activatees;
    // Things that feed logic into the gate.
    public List<Activatable> activators;

    protected virtual void FixedUpdate()
    {
        CheckInputs();
    }

    protected virtual void CheckInputs()
    {
        active_count = 0;
        for (int i = 0; i < activators.Count; i++)
        {
            if (activators[i].active)
            {
                active_count++;
            }
        }
        if (!active)
        {
            if (active_count == activators.Count)
            {
                SwitchOutput();
            }
            else if (active_count > 0 && or_gate)
            {
                SwitchOutput();
            }
        }
        else if (active)
        {
            if (active_count == 0)
            {
                SwitchOutput();
            }
            else if (active_count < activators.Count && and_gate)
            {
                SwitchOutput();
            }
        }
    }

    protected virtual void SwitchOutput()
    {
        active = !active;
        Activate();
    }

    public override void Activate()
    {
        for (int i = 0; i < activatees.Count; i++)
        {
            activatees[i].Activate();
        }
    }
}
