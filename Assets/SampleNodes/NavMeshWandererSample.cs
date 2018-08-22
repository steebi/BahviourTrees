using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;
using UnityEngine.AI;

public class NavMeshWandererSample : MonoBehaviour {

    [SerializeField]
    private NavMeshAgent m_agent;

    [SerializeField]
    private float m_searchRadius;

    private BehaviourTree.IBaseNode m_parentNodePoint;

	void Start () {
        m_parentNodePoint = new SelectorNode(new SequencerNode(new IsAtDestination(m_agent), new GetNewDestination(m_agent, m_searchRadius)), new AlwaysSuceed());
	}
	
	void Update () {
        m_parentNodePoint.OnTick();
	}
}
