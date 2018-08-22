using UnityEngine;
using System;

public enum NodeConnectionPointType { In = 0, Out}

public class NodeConnectionPoint
{

    private Rect m_rect;
    public Rect Rect { get { return m_rect; } }
    private EditorNode m_node;
    public EditorNode Node { get { return m_node; } }

    private NodeConnectionPointType m_type;
    private GUIStyle m_style;
    private Action<NodeConnectionPoint> m_OnClickConnectionpoint;

    public NodeConnectionPoint(EditorNode node, NodeConnectionPointType type, GUIStyle style, Action<NodeConnectionPoint> OnClickConnectionPoint)
    {
        m_node = node;
        m_type = type;
        m_style = style;
        m_OnClickConnectionpoint = OnClickConnectionPoint;
        m_rect = new Rect(0, 0, 10f, 20f);
    }

    public void Draw()
    {
        m_rect.y = m_node.Rect.y + (m_node.Rect.height * .5f) - m_rect.height * .5f;
        switch (m_type)
        {
            case NodeConnectionPointType.In:
                m_rect.x = m_node.Rect.x - m_rect.width + 8f;
                break;
            case NodeConnectionPointType.Out:
                m_rect.x = m_node.Rect.x + m_node.Rect.width - 8f;
                break;
        }

        if (GUI.Button(m_rect, "", m_style))
        {
            if (m_OnClickConnectionpoint != null)
                m_OnClickConnectionpoint(this);
        }
    }
}
