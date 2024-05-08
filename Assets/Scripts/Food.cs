using UnityEngine;

public class Food : MonoBehaviour
{
    public float amplitude = 0.1f;
    public float frequency = 0.2f;

    public bool isReadyToEat = false;

    public float growingSpeed = 5;
    [SerializeField]
    private float growingStatus = 0;

    private Renderer render;
    private BoxCollider col;

    public void Eated()
    {
        growingStatus = 0;

    }
    void Start()
    {
        col = GetComponent<BoxCollider>();
        col.enabled = false;
        growingStatus = 100;
        render = GetComponent<Renderer>();

    }

    void Update()
    {
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
}
