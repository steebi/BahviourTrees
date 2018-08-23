using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class BehaviourTreeEditorWindow : EditorWindow {

    private List<EditorNode> m_nodes = new List<EditorNode>();
    private List<NodeConnection> m_nodeConnections = new List<NodeConnection>();

    private GUIStyle m_nodeStyle;
    private GUIStyle m_selectedNodeStyle;
    private GUIStyle m_inPointStyle;
    private GUIStyle m_outPointStyle;

    private NodeConnectionPoint m_selectedInPoint;
    private NodeConnectionPoint m_selectedOutPoint;

    private Vector2 offset;
    private Vector2 m_drag;

    private bool m_dragginWorkspace;

    [MenuItem("Window/Behaviour Tree Editor")]
    private static void OpenWindow()
    {
        BehaviourTreeEditorWindow window = GetWindow<BehaviourTreeEditorWindow>();
        window.titleContent = new GUIContent("Behaviour Tree Editor");
    }

    private void OnEnable()
    {
        m_nodeStyle = new GUIStyle();
        m_nodeStyle.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/node1.png") as Texture2D;
        m_nodeStyle.border = new RectOffset(12, 12, 12, 12);

        m_selectedNodeStyle = new GUIStyle();
        m_selectedNodeStyle.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/node1 on.png") as Texture2D;
        m_nodeStyle.border = new RectOffset(12, 12, 12, 12);

        m_inPointStyle = new GUIStyle();
        m_inPointStyle.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/btn left.png") as Texture2D;
        m_inPointStyle.active.background = EditorGUIUtility.Load("builtin skins/darkskin/images/btn left on.png") as Texture2D;
        m_inPointStyle.border = new RectOffset(4, 4, 12, 12);

        m_outPointStyle = new GUIStyle();
        m_outPointStyle.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/btn right.png") as Texture2D;
        m_outPointStyle.active.background = EditorGUIUtility.Load("builtin skins/darkskin/images/btn right on.png") as Texture2D;
        m_outPointStyle.border = new RectOffset(4, 4, 12, 12);

    }

    private void OnGUI()
    {
        EditorGUILayout.LabelField("Current delta: ", m_drag.ToString());

        DrawGrid(20f, .2f, Color.grey);
        DrawGrid(100f, .4f, Color.gray);

        DrawNodes();
        DrawConnections();
        DrawConnectionLine(Event.current);

        ProcessNodeEvents(Event.current);
        ProcessEvents(Event.current);

        //if (GUI.changed)
            Repaint();
    }

    private void DrawNodes()
    {
        for (int i = 0; i < m_nodes.Count; ++i)
        {
            m_nodes[i].Draw();
        }
    }

    private void DrawConnections()
    {
        for (int i = 0; i < m_nodeConnections.Count; ++i)
        {
            m_nodeConnections[i].Draw();
        }
    }

    private void DrawConnectionLine(Event e)
    {
        if (m_selectedInPoint != null && m_selectedOutPoint == null)
        {
            Handles.DrawBezier(
                m_selectedInPoint.Rect.center,
                e.mousePosition,
                m_selectedInPoint.Rect.center + Vector2.left * 50f,
                e.mousePosition - Vector2.left * 50f,
                Color.white,
                null,
                2f
                );
        }
        else if (m_selectedInPoint == null && m_selectedOutPoint != null)
        {
            Handles.DrawBezier(
                m_selectedOutPoint.Rect.center,
                e.mousePosition,
                m_selectedOutPoint.Rect.center + Vector2.left * 50f,
                e.mousePosition - Vector2.left * 50f,
                Color.white,
                null,
                2f
                );
        }
        GUI.changed = true;
    }

    private void ProcessEvents(Event e)
    {
        switch (e.type)
        {
            case EventType.MouseDown:
                OnMouseDown(e);
                break;
            case EventType.MouseDrag:
                if (e.button == 0)
                    OnDrag(e.delta);
                break;
            case EventType.MouseUp:
                OnMouseUp(e);
                break;
        }
    }

    private void OnDrag(Vector2 delta)
    {
        if (!m_dragginWorkspace && mouseOverWindow)
            m_dragginWorkspace = true;

        if (m_dragginWorkspace)
        {
            m_drag = delta;
            if (m_nodes != null)
            {
                for (int i = 0; i < m_nodes.Count; ++i)
                {
                    m_nodes[i].Drag(delta);
                }
            }
            GUI.changed = true;
        }
    }

    private void OnMouseDown(Event e)
    {
        switch (e.button)
        {
            case 1:
                ProcessContextMenu(e.mousePosition);
                break;
        }
    }

    private void OnMouseUp(Event e)
    {
        m_dragginWorkspace = false;
        m_drag = Vector2.zero;
    }

    private void ProcessContextMenu(Vector2 mousePosition)
    {
        GenericMenu genMen = new GenericMenu();
        genMen.AddItem(new GUIContent("Add node"), false, () => OnClickAddNode(mousePosition));
        genMen.ShowAsContext();
    }

    private void OnClickAddNode(Vector2 mousePosition)
    {
        if (m_nodes == null)
            m_nodes = new List<EditorNode>();

        m_nodes.Add(new EditorNode(mousePosition, 200, 50, m_nodeStyle, m_selectedNodeStyle, m_inPointStyle, m_outPointStyle, OnClickInPoint, OnClickOutPoint, OnClickRemoveNode));
    }

    private void OnClickInPoint(NodeConnectionPoint inPoint)
    {
        m_selectedInPoint = inPoint;

        if (m_selectedOutPoint != null && m_selectedOutPoint.Node != m_selectedInPoint.Node)
        {
            CreateConnection(m_selectedInPoint, m_selectedOutPoint);
            ClearCachedSelectionPoints();
        }
        else if (m_selectedOutPoint != null)
        {
            ClearCachedSelectionPoints();
        }
    }

    private void OnClickOutPoint(NodeConnectionPoint outPoint)
    {
        m_selectedOutPoint = outPoint;
        if (m_selectedInPoint != null && m_selectedInPoint.Node != m_selectedOutPoint.Node)
        {
            CreateConnection(m_selectedInPoint, m_selectedOutPoint);
            ClearCachedSelectionPoints();
        }
        else if (m_selectedInPoint != null)
        {
            ClearCachedSelectionPoints();
        }
    }

    private void CreateConnection(NodeConnectionPoint inP, NodeConnectionPoint outP)
    {
        m_nodeConnections.Add(new NodeConnection(inP, outP, OnClickRemoveConnection));
    }

    private void ClearCachedSelectionPoints()
    {
        m_selectedInPoint = null;
        m_selectedOutPoint = null;
    }

    private void OnClickRemoveConnection(NodeConnection connection)
    {
        m_nodeConnections.Remove(connection);
    }

    private void ProcessNodeEvents(Event e)
    {
        for (int i = m_nodes.Count - 1; i >= 0; --i)
        {
            if (m_nodes[i].ProcessEvents(e))
            {
                GUI.changed = true;
            }
        }
    }

    private void OnClickRemoveNode(EditorNode node)
    {
        DeleteNodeConnections(node);
        m_nodes.Remove(node);
    }

    private void DeleteNodeConnections(EditorNode node)
    {
        List<int> deletableindeices = new List<int>();
        if (m_nodeConnections != null)
        {
            for (int i = 0; i < m_nodeConnections.Count; ++i)
            {
                if (m_nodeConnections[i].InPoint == node.InPoint || m_nodeConnections[i].OutPoint == node.OutPoint)
                {
                    deletableindeices.Add(i);
                }
            }
            for (int i = 0; i < deletableindeices.Count; ++i)
            {
                m_nodeConnections.RemoveAt(deletableindeices[i]);
            }
        }
    }

    private void DrawGrid(float gridSpacing, float gridOpacity, Color gridColor)
    {
        int widthDivs = Mathf.CeilToInt(position.width / gridSpacing);
        int heightDivs = Mathf.CeilToInt(position.height / gridSpacing);

        Handles.BeginGUI();
        Handles.color = new Color(gridColor.r, gridColor.g, gridColor.b, gridOpacity);

        offset += m_drag * 0.5f;
        Vector3 newOffset = new Vector3(offset.x % gridSpacing, offset.y % gridSpacing, 0);

        for (int i = 0; i < widthDivs; i++)
        {
            Handles.DrawLine(new Vector3(gridSpacing * i, -gridSpacing, 0) + newOffset, new Vector3(gridSpacing * i, position.height, 0f) + newOffset);
        }

        for (int j = 0; j < heightDivs; j++)
        {
            Handles.DrawLine(new Vector3(-gridSpacing, gridSpacing * j, 0) + newOffset, new Vector3(position.width, gridSpacing * j, 0f) + newOffset);
        }

        Handles.color = Color.white;
        Handles.EndGUI();
    }
}
