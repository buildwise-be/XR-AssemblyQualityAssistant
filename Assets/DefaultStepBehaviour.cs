using UnityEngine;

public class DefaultStepBehaviour : MonoBehaviour, IStepBehaviour
{
    [SerializeField] private int Step;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EnterStep()
    {
        
    }

    public void LeaveStep()
    {
        
    }

    public void EnterStepReverse()
    {
        
    }

    public void Initialize(int i)
    {
        gameObject.SetActive(Step<=i);
    }
}
