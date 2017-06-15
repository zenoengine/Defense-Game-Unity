using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Animator mAnimator = null;
    private GameObject mClosestItem = null;
    private GameObject mCarriedItem = null;

    public static float MOVE_AREA_RADIUS = 15.0f;
    public static float MOVE_SPEED = 2.0f;
    private struct Key
    {
        public bool up;
        public bool down;
        public bool right;
        public bool left;
        public bool pick;
        public bool action;
    };

    private Key mKey;

    public enum STEP
    {
        NONE = -1,
        MOVE = 0,
        REPAIRING,
        EATING,
        NUM,
    };

    public STEP mCurrentStep = STEP.NONE;
    public STEP mNextStep = STEP.NONE;
    public float mStepTimer = 0.0f;

    void Start()
    {
        this.mCurrentStep = STEP.NONE;
        this.mNextStep = STEP.MOVE;
        mAnimator = GetComponent<Animator>();
        mAnimator.SetBool("Grounded", true);
    }

    private void GetInput()
    {
        mKey.up = false;
        mKey.down = false;
        mKey.right = false;
        mKey.left = false;

        mKey.up |= Input.GetKey(KeyCode.UpArrow);
        mKey.up |= Input.GetKey(KeyCode.Keypad8);
        mKey.down |= Input.GetKey(KeyCode.DownArrow);
        mKey.down |= Input.GetKey(KeyCode.Keypad2);
        mKey.right |= Input.GetKey(KeyCode.RightArrow);
        mKey.right |= Input.GetKey(KeyCode.Keypad6);
        mKey.left |= Input.GetKey(KeyCode.LeftArrow);
        mKey.left |= Input.GetKey(KeyCode.Keypad4);
        mKey.pick = Input.GetKeyDown(KeyCode.Z);
        mKey.action = Input.GetKeyDown(KeyCode.X);
    }

    private void MoveControl()
    {
        Vector3 moveVector = Vector3.zero;
        Vector3 position = this.transform.position;
        bool isMoved = false;
        if (mKey.right)
        {
            moveVector += Vector3.right;
            isMoved = true;
        }
        if (mKey.left)
        {
            moveVector += Vector3.left;
            isMoved = true;
        }
        if (mKey.up)
        {
            moveVector += Vector3.forward;
            isMoved = true;
        }
        if (mKey.down)
        {
            moveVector += Vector3.back;
            isMoved = true;
        }

        moveVector.Normalize();
        moveVector *= MOVE_SPEED * Time.deltaTime; 
        position += moveVector;
        position.y = 0.0f;
        mAnimator.SetFloat("MoveSpeed", moveVector.magnitude*MOVE_SPEED*5);


        Vector3 beginBoundary;
        Vector3 endBoundary;
        MapManager.instance.GetBoundary(out beginBoundary, out endBoundary);
        
        if (position.x >= endBoundary.x ||
            position.z >= endBoundary.z ||
            position.x <= beginBoundary.x ||
            position.z <= beginBoundary.x)
        {
            return;
        }

        position.y = this.transform.position.y;
        transform.position = position;

        if (moveVector.magnitude > 0.01f)
        {
            Quaternion q = Quaternion.LookRotation(moveVector, Vector3.up);
            this.transform.rotation = Quaternion.Lerp(this.transform.rotation, q, 0.1f);
        }

        moveVector.Normalize();
        moveVector *= MOVE_SPEED * Time.deltaTime;
        position += moveVector; 
        position.y = 0.0f;
        
        if (position.magnitude > MOVE_AREA_RADIUS)
        {
            position.Normalize();
            position *= MOVE_AREA_RADIUS;
        }

        if (moveVector.magnitude > 0.01f)
        {
            Quaternion q = Quaternion.LookRotation(moveVector, Vector3.up);
            this.transform.rotation =
            Quaternion.Lerp(this.transform.rotation, q, 0.1f);
        }
    }

    void Update()
    {
        GetInput();
        MoveControl();
        PickOrDropControl();
        mStepTimer += Time.deltaTime;
    }

    void OnTriggerStay(Collider other)
    {
        GameObject otherGameObject = other.gameObject;
        if (otherGameObject.layer == LayerMask.NameToLayer("Grabbable"))
        {
            if (mClosestItem == null)
            {
                if (IsOtherInView(otherGameObject))
                {
                    mClosestItem = otherGameObject;
                }
            }
            else if (mClosestItem == otherGameObject)
            {
                if (!IsOtherInView(otherGameObject))
                {
                    mClosestItem = null;
                }
            }
        }
    }
    
    void OnTriggerExit(Collider other)
    {
        if (mClosestItem == other.gameObject)
        {
            mClosestItem = null;
        }
    }

    private bool IsOtherInView(GameObject other)
    {
        bool ret = false;
        
        Vector3 heading = transform.TransformDirection(Vector3.forward);
        Vector3 toOther = other.transform.position - transform.position;

        heading.y = 0.0f;
        toOther.y = 0.0f;

        heading.Normalize();
        toOther.Normalize();

        float dp = Vector3.Dot(heading, toOther);

        if (dp < Mathf.Cos(45.0f))
        {
            return ret;
        }

        ret = true;

        return (ret);
    }

    private void PickOrDropControl()
    {
        if (!this.mKey.pick)
        {
            return;
        }

        if (this.mCarriedItem == null)
        {
            if (this.mClosestItem == null)
            {
                return;
            }

            mCarriedItem = mClosestItem;
            mCarriedItem.transform.parent = transform;
            mCarriedItem.transform.localPosition = Vector3.up * 1.8f;

            MachineGun machineGun = mCarriedItem.GetComponent<MachineGun>();
            if(machineGun)
            {
                machineGun.IsGrounded = false;
            }

            mClosestItem = null;
            mAnimator.SetBool("Pickup", true);
        }
        else
        {
            Vector3 heading = transform.position + transform.forward * 1.0f + transform.up*10;
            Ray ray = new Ray(heading, Vector3.down);
            RaycastHit hit;
            int layer = 1 << LayerMask.NameToLayer("Tile");
            if (Physics.Raycast(ray, out hit, 500, layer))
            {
                TileInfomation tileinfomation = hit.collider.gameObject.GetComponent<TileInfomation>();
                if(tileinfomation == null)
                {
                    return;
                }

                if (tileinfomation.currentTileStyle != TILESTYLE.NORMAL)
                {
                    return;
                }
            }
            else
            {
                return;
            }

            MachineGun machineGun = mCarriedItem.GetComponent<MachineGun>();
            if (machineGun)
            {
                machineGun.IsGrounded = true;
            }

            mCarriedItem.transform.localPosition = Vector3.forward * 1.0f;
            mCarriedItem.transform.parent = null;
            mCarriedItem = null;

            mAnimator.SetBool("Pickup", true);
        }
    }
}
