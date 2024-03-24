using UnityEditor.Animations;
using UnityEngine;

public class CharacterScript : MonoBehaviour
{
    [SerializeField] private float speed = 3f;
    private float playerVelocityY;
    private float gravityValue = -9.80f;
    private float jumpHeight = 1f;
    private bool groundedPlayer;
    private CharacterController _characterController;
    private Animator _animator;
    void Start()
    {
        _characterController = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();

        playerVelocityY = 0f;
    }

    void Update()
    {
        int animatorState = 0;

        if(groundedPlayer && playerVelocityY < 0)
        {
            playerVelocityY = 0f;
        }

        float dx = Input.GetAxis("Horizontal");
        float dy = Input.GetAxis("Vertical");
        
        if(Mathf.Abs(dx) > 0 && Mathf.Abs(dy) > 0)
        {
            dx *= 0.707f; // /= Mathf.Sqrt(2f);
            dy *= 0.707f; // /= Mathf.Sqrt(2f);
        }

        if(dy != 0 && groundedPlayer) // якщо діагональ, то анімація ходу вперед
        {
            animatorState = 1;
        }
        else if(dx != 0 && groundedPlayer)
        {
            animatorState = 2;
        }

        if(Input.GetButton("Jump") && groundedPlayer)
        {
            animatorState = 4;
            playerVelocityY += Mathf.Sqrt(jumpHeight * -2.0f * gravityValue);
        }

        if (!groundedPlayer)
        {
            animatorState = 3;
        }

        // _characterController.SimpleMove(speed * Time.deltaTime * (dx * Camera.main.transform.right + dy * Camera.main.transform.forward));          // new Vector3(dx, 0, dy));

        Vector3 horizontalForward = Camera.main.transform.forward;
        horizontalForward.y = 0f;
        horizontalForward = horizontalForward.normalized;

        playerVelocityY += gravityValue * Time.deltaTime;
        _characterController.Move(
            (speed * (dx * Camera.main.transform.right + dy * horizontalForward ) + playerVelocityY * Vector3.up) * Time.deltaTime);

        transform.forward = horizontalForward;

        // задаємо стан аніматора
        _animator.SetInteger("State", animatorState);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Floor"))
        {
            groundedPlayer = true;
            _animator.SetInteger("State", 0);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Floor"))
        {
            groundedPlayer = false;
        }
    }

    public void OnJumpStart()
    {
        _animator.SetInteger("State", 3);
    }
}
