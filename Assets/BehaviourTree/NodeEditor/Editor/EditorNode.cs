using System;
using UnityEngine;
using UnityEditor;

public class EditorNode
{
    private Rect m_rect;    // neccessary for changing it's position
    public Rect Rect { get { return m_rect; } }
    public NodeConnectionPoint InPoint { get; private set; }
    public NodeConnectionPoint OutPoint { get; private set; }
    public Action<EditorNode> OnRemoveNode { get; private set; }
    public bool IsSelected { get; set; }

    private string m_title;
    private GUIStyle m_style;
    private GUIStyle m_defaultStyle;
    private GUIStyle m_selectedStyle;
    private bool m_dragging = false;
    private Vector2 m_drag;

    public EditorNode(Vector2 position, float width, float height, GUIStyle defaultStyle, GUIStyle selectedStyle, GUIStyle inPointStyle, GUIStyle outPointStyle, Action<NodeConnectionPoint> OnClickInPoint, Action<NodeConnectionPoint> OnClickOutPoint, Action<EditorNode> onRemoveNode)
    {
        m_rect = new Rect(position.x, position.y, width, height);
        m_style = m_defaultStyle = defaultStyle;
        m_selectedStyle = selectedStyle;
        InPoint = new NodeConnectionPoint(this, NodeConnectionPointType.In, inPointStyle, OnClickInPoint);
        OutPoint= new NodeConnectionPoint(this, NodeConnectionPointType.Out, inPointStyle, OnClickOutPoint);
        OnRemoveNode = onRemoveNode;
    }

    public void Drag(Vector2 delta)
    {
        m_rect.position += delta;
    }

    public void Draw()
    {
        InPoint.Draw();
        OutPoint.Draw();
        GUI.Box(Rect, m_title, m_style);
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
                if (Rect.Contains(e.mousePosition))
                {
                    m_dragging = true;
                    GUI.changed = true;
                    IsSelected = true;
                    m_style = m_selectedStyle;
                }
                else
                {
                    GUI.changed = true;
                    IsSelected = false;
                    m_style = m_defaultStyle;
                }
                break;
            case 1:
                if (IsSelected && Rect.Contains(e.mousePosition))
                {
                    ProcessContextMenu();
                    e.Use();
                }
                break;
        }
    }

    private void OnMouseUp(Event e)
    {
        switch (e.button)
        {
            case 0:
                if (m_dragging)
                {
                    m_dragging = false;
                }
                break;
        }
    }

    private void OnMouseDrag(Event e)
    {
        switch (e.button)
        {
            case 0:
                if (m_dragging)
                {
                    Drag(e.delta);
                    e.Use();
                }
                break;
        }
    }

    private void ProcessContextMenu()
    {
        GenericMenu genMen = new GenericMenu();
        genMen.AddItem(new GUIContent("Remove node"), false, () => { OnRemoveNode(this); });
        genMen.ShowAsContext();
    }
}
