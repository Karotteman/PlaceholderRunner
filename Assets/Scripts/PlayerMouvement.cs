using UnityEngine;

public class PlayerMouvement : MonoBehaviour
{
    public LevelManager levelManager;
    public AudioClip jumpAudio;
    public AudioClip rightAudio;
    public AudioClip leftAudio;

    [Header("Player Power")]
    public float speed;
    public float rotaionSpeed;
    public float jumpDuration;

    Transform targetGameObject;
    Vector3 startingPosition;
    AudioSource audioSource;
    Vector3 bodyOffset;
    float pathDuration;
    GameObject body;

    Vector3 target =  Vector3.right;
    bool isJumping = false;
    float pathTimer = 0;
    float airTimer = 0;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        body = transform.GetChild(0).gameObject;
        bodyOffset = body.transform.localPosition;
        startingPosition = transform.position;
    }

    private void FixedUpdate()
    {
        if (targetGameObject != null)
        {
            pathTimer++;

            transform.position = Vector3.Lerp(startingPosition, targetGameObject.position, pathTimer / pathDuration);
            target = Vector3.Lerp(transform.forward, targetGameObject.right, pathTimer / pathDuration);
            transform.rotation = Quaternion.LookRotation(target, transform.up);
        }

        if (!IsGrounded() && isJumping)
        {
            Jump();
        }
        else if (!IsGrounded() )
        {
            airTimer += Time.deltaTime;
            float valueInterpolation = airTimer * (1 / (jumpDuration / 2));
            body.transform.localPosition = bodyOffset * valueInterpolation * valueInterpolation;

            if (airTimer >= jumpDuration / 2)
            {
                airTimer = 0; 
                body.transform.localPosition = bodyOffset;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (IsGrounded() && Input.GetButtonDown("Jump"))
        {
            audioSource.PlayOneShot(jumpAudio);
            isJumping = true;
            Jump();
        }

        float axis = Input.GetAxis("Horizontal");
        if (axis != 0)
        {
            if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                audioSource.PlayOneShot(rightAudio);
            }
            if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            {
                audioSource.PlayOneShot(leftAudio);
            }

            transform.Rotate(transform.forward, (rotaionSpeed * axis)*Time.deltaTime, Space.World);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Module")
        {
            levelManager.CompareModule(other.gameObject.transform.parent.gameObject);
            SetNewTarget(other.gameObject.transform.parent.gameObject);
        }
    }

    bool IsGrounded()
    {
        if(body.transform.localPosition == bodyOffset) 
        {
            return true;
        }
        return false;
    }
    
    void Jump()
    {
        airTimer += Time.deltaTime;
        float valueInterpolation = airTimer / (jumpDuration / 2);
        body.transform.localPosition = body.transform.localPosition * valueInterpolation * (valueInterpolation - 2) + body.transform.localPosition;

        if (airTimer >= jumpDuration/2)
        {
            isJumping = false;
            airTimer = 0;
        }
    }

    void SetNewTarget(GameObject target)
    {
        startingPosition = transform.position;
        targetGameObject = target.transform.GetChild(2).transform;
        pathDuration = speed * target.GetComponent<Module>().Path.magnitude;
        pathTimer = 0;
    }
}
