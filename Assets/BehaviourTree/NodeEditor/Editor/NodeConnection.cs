using System;
using UnityEditor;
using UnityEngine;

public class NodeConnection
{

    public NodeConnectionPoint InPoint { get; private set; }
    public NodeConnectionPoint OutPoint { get; private set; }
    public Action<NodeConnection> m_onClickRemoveConnection;

    public NodeConnection(NodeConnectionPoint inPoint, NodeConnectionPoint outPoint, Action<NodeConnection> onClickRemove)
    {
        InPoint = inPoint;
        OutPoint = outPoint;
        m_onClickRemoveConnection = onClickRemove;
    }

    public void Draw()
    {
        Handles.DrawBezier(
            InPoint.Rect.center,
            OutPoint.Rect.center,
            InPoint.Rect.center + Vector2.left * 50f,
            OutPoint.Rect.center - Vector2.left * 50f,
            Color.white,
            null,
            2f
            );

        if (Handles.Button((InPoint.Rect.center + OutPoint.Rect.center) * .5f, Quaternion.identity, 4, 8, Handles.RectangleHandleCap))
        {
            if (m_onClickRemoveConnection != null)
                m_onClickRemoveConnection(this);
        }
    }

}
