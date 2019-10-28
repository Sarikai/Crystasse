using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour
{
    [SerializeField]
    private float _moveSpeed = 5f;

    private void Update()
    {
        if(Input.GetKey(KeyCode.A))
            transform.position -= transform.right * _moveSpeed * Time.deltaTime;
        if(Input.GetKey(KeyCode.D))
            transform.position += transform.right * _moveSpeed * Time.deltaTime;

        if(Input.GetKey(KeyCode.W))
            transform.position += new Vector3(transform.forward.x, 0, transform.forward.z) * _moveSpeed * Time.deltaTime;
        if(Input.GetKey(KeyCode.S))
            transform.position -= new Vector3(transform.forward.x, 0, transform.forward.z) * _moveSpeed * Time.deltaTime;
    }
}
