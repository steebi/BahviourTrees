using UnityEngine;
using UnityEngine.AI;

public class NavMeshWanderer : MonoBehaviour
{
    [SerializeField]
    private float m_navMeshSearchRadius = 5;

    [SerializeField]
    private GameObject m_destinationMarker;

    [SerializeField]
    private LineRenderer m_linerenderer;

    [SerializeField]
    private float m_offset = 1.0f;

    private NavMeshAgent m_navAgent;
    public NavMeshAgent NavAgent
    {
        get
        {
            if (m_navAgent == null)
                m_navAgent = this.GetComponent<NavMeshAgent>();
            return m_navAgent;
        }
    }

    private Vector3 m_lineRendererEndpoint;

	// Update is called once per frame
	void Update ()
    {
        AgentUpdate();
	}

    public void AgentUpdate()
    {
        if (IsAtDestination())
        {
            NavAgent.destination = GetRandomDestinationOnNavMesh(m_navMeshSearchRadius);
            m_destinationMarker.transform.position = NavAgent.destination;
        }

        // Update the Line Renderer if count is different or endpoints are different
        if(!SameEndPoint())
            DrawLine();
    }

    public bool IsAtDestination()
    {
        return NavAgent.velocity == Vector3.zero && !NavAgent.hasPath;// Mathf.Abs(Vector3.Distance(NavAgent.destination, gameObject.transform.position)) < 1.0f;
    }

    public Vector3 GetRandomDestinationOnNavMesh(float radius)
    {
        Vector3 targetDirection = RandomDirection(radius);
        NavMeshHit targetPoint;
        if (NavMesh.SamplePosition(targetDirection, out targetPoint, 1.0f, 1))
            return targetPoint.position;
        else
            return gameObject.transform.position;
    }

    public Vector3 RandomDirection(float searchRadius)
    {
        return Random.insideUnitSphere * Random.Range(1.0f, m_navMeshSearchRadius);
    }

    private bool SameEndPoint()
    {
        Vector3 navEnd = NavAgent.path.corners[NavAgent.path.corners.Length - 1];
        return Mathf.Approximately(navEnd.x, m_lineRendererEndpoint.x) && Mathf.Approximately(navEnd.z, m_lineRendererEndpoint.z);
    }

    public void DrawLine()
    {
        Vector3[] path = GetLineRendererPoints(NavAgent.path, m_offset);
        m_linerenderer.positionCount = path.Length;
        m_linerenderer.SetPositions(path);
        m_lineRendererEndpoint = path[path.Length - 1];
    }

    public Vector3[] GetLineRendererPoints(NavMeshPath navPath, float offset)
    {
        Vector3[] path = new Vector3[navPath.corners.Length];
        for (int i = 0; i < navPath.corners.Length; ++i)
        {
            path[i] = new Vector3(navPath.corners[i].x, navPath.corners[i].y + offset, navPath.corners[i].z);
        }
        return path;
    }
}
