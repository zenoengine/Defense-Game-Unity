using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private GameObject ClosestItem = null;
    private GameObject CarriedItem = null;

    public static float MOVE_AREA_RADIUS = 15.0f;
    public static float MOVE_SPEED = 5.0f;
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
        mStepTimer += Time.deltaTime;
    }

    void OnTriggerStay(Collider other)
    {
        GameObject other_go = other.gameObject;
    }

    // 주목을 그만두게 한다.
    void OnTriggerExit(Collider other)
    {
    }
    
    // 접촉한 물건이 자신의 정면에 있는지 판단한다.
    private bool is_other_in_view(GameObject other)
    {
        bool ret = false;
        do
        {
            Vector3 heading = // 자신이 현재 향하고 있는 방향을 보관.
            this.transform.TransformDirection(Vector3.forward);
            Vector3 to_other = // 자신 쪽에서 본 아이템의 방향을 보관.
            other.transform.position - this.transform.position;
            heading.y = 0.0f;
            to_other.y = 0.0f;
            heading.Normalize(); // 길이를 1로 하고 방향만 벡터로.
            to_other.Normalize(); // 길이를 1로 하고 방향만 벡터로.
            float dp = Vector3.Dot(heading, to_other); // 양쪽 벡터의 내적을 취득.
            if (dp < Mathf.Cos(45.0f))
            { // 내적이 45도인 코사인 값 미만이면.
                break; // 루프를 빠져나간다.
            }
            ret = true; // 내적이 45도인 코사인 값 이상이면 정면에 있다.
        } while (false);
        return (ret);
    }

}
