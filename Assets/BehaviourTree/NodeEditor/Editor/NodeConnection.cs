using System;
using UnityEditor;
using UnityEngine;

public class NodeConnection
{

    private NodeConnectionPoint m_inPoint;
    public NodeConnectionPoint m_outPoint;
    public Action<NodeConnection> m_onClickRemoveConnection;

    public NodeConnection(NodeConnectionPoint inPoint, NodeConnectionPoint outPoint, Action<NodeConnection> onClickRemove)
    {
        m_inPoint = inPoint;
        m_outPoint = outPoint;
        m_onClickRemoveConnection = onClickRemove;
    }

    public void Draw()
    {
        Handles.DrawBezier(
            m_inPoint.Rect.center,
            m_outPoint.Rect.center,
            m_inPoint.Rect.center + Vector2.left * 50f,
            m_outPoint.Rect.center - Vector2.left * 50f,
            Color.white,
            null,
            2f
            );

        if (Handles.Button((m_inPoint.Rect.center + m_outPoint.Rect.center) * .5f, Quaternion.identity, 4, 8, Handles.RectangleHandleCap))
        {
            if (m_onClickRemoveConnection != null)
                m_onClickRemoveConnection(this);
        }
    }

}
