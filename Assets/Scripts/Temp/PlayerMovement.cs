using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private string horizontalInputName;        // аліаси на кнопки A-D
    [SerializeField] private string verticalInputName;          // аліаси на кнопки W-S
    [SerializeField] private float movementSpeed;

    private CharacterController charController; // компонент, котрий спрощує програмування та керування персонажем

    [SerializeField] private AnimationCurve jumpFallOff; // графік прямої, за якою в мене стрибає персонаж
    [SerializeField] private float jumpMultiplier;
    [SerializeField] private KeyCode jumpKey;

    private bool isJumping;

    private void Awake() // Це майже те ж саме, що і Старт, але запускається при створенні об'єкту
    {
        charController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        PlayerMovements();
        if (this.transform.position.y < 0f)
        {
            this.transform.position = new Vector3(0, 1, 0);
        }
    }

    private void PlayerMovements()
    {
        float horizInput = Input.GetAxis(horizontalInputName) * movementSpeed;
        float vertInput = Input.GetAxis(verticalInputName) * movementSpeed;

        Vector3 forwardMovement = transform.forward * vertInput;
        Vector3 rightMovement = transform.right * horizInput;

        charController.SimpleMove(forwardMovement + rightMovement);

        JumpInput();

    }

    private void JumpInput()
    {
        if (Input.GetKeyDown(jumpKey) && !isJumping)
        {
            isJumping = true;
            StartCoroutine(JumpEvent());
        }
    }

    private IEnumerator JumpEvent() // Delay()
    {
        charController.slopeLimit = 90.0f;
        float timeInAir = 0.4f;

        do
        {
            float jumpForce = jumpFallOff.Evaluate(timeInAir);
            charController.Move(Vector3.up * jumpForce * jumpMultiplier * Time.deltaTime);
            timeInAir += Time.deltaTime;
            yield return null;
        } while (this.transform.position.y > 0 && !charController.isGrounded && charController.collisionFlags != CollisionFlags.Above);

        charController.slopeLimit = 45.0f;
        isJumping = false;
    }
}
