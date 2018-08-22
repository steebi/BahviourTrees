using System;
using UnityEngine;

public class EditorNode
{
    private Rect m_rect;
    public Rect Rect { get { return m_rect; } }

    private string m_title;
    public string Title { get { return m_title; } set { m_title = value; } }

    private GUIStyle m_style;
    public GUIStyle Style { get { return m_style; } }

    public bool Dragging { get; set; }
    private int m_dragButtonIndex = -1;

    public NodeConnectionPoint InPoint { get; set; }
    public NodeConnectionPoint OutPoint { get; set; }

    public EditorNode(Vector2 position, float width, float height, GUIStyle nodeStyle, GUIStyle inPointStyle, GUIStyle outPointStyle, Action<NodeConnectionPoint> OnClickInPoint, Action<NodeConnectionPoint> OnClickOutPoint)
    {
        m_rect = new Rect(position.x, position.y, width, height);
        m_style = nodeStyle;
        InPoint = new NodeConnectionPoint(this, NodeConnectionPointType.In, inPointStyle, OnClickInPoint);
        OutPoint= new NodeConnectionPoint(this, NodeConnectionPointType.Out, inPointStyle, OnClickOutPoint);
    }

    public void Drag(Vector2 delta)
    {
        m_rect.position += delta;
    }

    public void Draw()
    {
        InPoint.Draw();
        OutPoint.Draw();
        GUI.Box(m_rect, Title, Style);
    }

    public bool ProcessEvents(Event e)
    {
        switch (e.type)
        {
            case EventType.MouseDown:
                OnMouseDown(e);
                break;
            case EventType.MouseUp:
                OnMouseUp(e);
                break;
            case EventType.MouseDrag:
                OnMouseDrag(e);
                break;
        }
        return e.type == EventType.Used;
    }

    private void OnMouseDown(Event e)
    {
        switch (e.button)
        {
            case 0:
                if (m_rect.Contains(e.mousePosition))
                {
                    Dragging = true;
                    GUI.changed = true;
                    m_dragButtonIndex = 0;
                }
                else
                    GUI.changed = true;
                break;
        }
    }

    private void OnMouseUp(Event e)
    {
        switch (e.button)
        {
            case 0:
                if (Dragging && m_dragButtonIndex == e.button)
                {
                    Dragging = false;
                    m_dragButtonIndex = -1;
                }
                break;
        }
    }

    private void OnMouseDrag(Event e)
    {
        switch (e.button)
        {
            case 0:
                if (Dragging && m_dragButtonIndex == e.button)
                {
                    Drag(e.delta);
                    e.Use();
                }
                break;
        }
    }
}
