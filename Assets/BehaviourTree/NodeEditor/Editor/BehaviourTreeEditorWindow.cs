using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class BehaviourTreeEditorWindow : EditorWindow {

    private List<EditorNode> m_nodes = new List<EditorNode>();
    private List<NodeConnection> m_nodeConnections = new List<NodeConnection>();

    private GUIStyle m_nodeStyle;
    private GUIStyle m_inPointStyle;
    private GUIStyle m_outPointStyle;

    private NodeConnectionPoint m_selectedInPoint;
    private NodeConnectionPoint m_selectedOutPoint;

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
        DrawNodes();
        DrawConnections();

        ProcessNodeEvents(Event.current);
        ProcessEvents(Event.current);

        if (GUI.changed)
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

    private void ProcessEvents(Event e)
    {
        switch (e.type)
        {
            case EventType.MouseDown:
                if (e.button == 1)
                {
                    ProcessContextMenu(e.mousePosition);
                }
                break;
        }
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

        m_nodes.Add(new EditorNode(mousePosition, 200, 50, m_nodeStyle, m_inPointStyle, m_outPointStyle, OnClickInPoint, OnClickOutPoint));
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
}
