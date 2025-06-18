using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentInteractionSignal : MonoBehaviour
{
    public readonly GameObject Agent;  
    public readonly GameObject Target;  

    public AgentInteractionSignal(GameObject agent, GameObject target)
    {
        Agent = agent;
        Target = target;
    }
}
public static class Tags
{
    public const string Tv = "TV";
    public const string Bed = "Bed";
}