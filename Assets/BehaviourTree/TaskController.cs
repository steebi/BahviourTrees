using BehaviourTrees;

public class TaskController {

    private bool m_completed;
    public bool Completed { get { return m_completed; } }

    private bool m_success;
    public bool Success { get { return m_success; } }

    private bool m_started;
    public bool Started { get { return m_started; } }

    private Node m_node;

    public TaskController(Node node)
    {
        m_node = node;
        Initialize();
    }

    private void Initialize()
    {
        m_completed = false;
        m_success = true;
        m_started = false;
    }

    public void SafeStart()
    {
        m_started = true;
        m_node.Start();
    }

    public void SafeEnd()
    {
        m_completed = false;
        m_started = false;
        m_node.End();
    }

    public void FinishWithSuccess()
    {
        m_success = true;
        m_completed = true;
    }

    public void FinishWithFailure()
    {
        m_success = false;
        m_completed = true;
    }

    public void Reset()
    {
        m_completed = false;
    }
}
