using UnityEngine;

public class Food : MonoBehaviour
{
    public float amplitude = 0.1f;
    public float frequency = 0.2f;

    public bool isReadyToEat = false;

    public float growingSpeed = 5;
    [SerializeField]
    private float growingStatus = 0;

    private Vector3 posOffset = new();
    private Vector3 tempPos = new();

    private Renderer render;
    private Transform model;
    private SphereCollider col;

    public void Eated()
    {
        growingStatus = 0;

    }
    void Start()
    {
        model = transform.GetChild(0);
        col = GetComponent<SphereCollider>();
        col.enabled = false;
        growingStatus = 100;
        posOffset = model.transform.position;
        render = model.GetComponent<Renderer>();

    }

    void Update()
    {
        Floating();
        Grow();
    }
    public void Eat(){
        growingStatus = 0;
    }
    void Grow()
    {
        if (growingStatus <= 100)
        {
            isReadyToEat = false;
            growingStatus += Time.deltaTime * growingSpeed;
            render.enabled = false;
            col.enabled = false;
        }
        else
        {
            isReadyToEat = true;
            render.enabled = true;
            col.enabled = true;
        }

    }
    void Floating()
    {
        tempPos = posOffset;
        tempPos.y += Mathf.Sin(Time.fixedTime * Mathf.PI * frequency) * amplitude;
        model.position = tempPos;
    }
}
