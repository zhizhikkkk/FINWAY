using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentInteractionSignal : MonoBehaviour
{
    public readonly GameObject Agent;   // кто подошёл
    public readonly GameObject Target;  // к чему подошёл

    public AgentInteractionSignal(GameObject agent, GameObject target)
    {
        Agent = agent;
        Target = target;
    }
}
