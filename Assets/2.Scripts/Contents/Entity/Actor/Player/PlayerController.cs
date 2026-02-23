using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveTime = 0.5f; // 1칸 이동에 소요되는 시간

    KeySetting keySetting;
    private bool isMoving;

    private void Awake()
    {
        keySetting = Managers.Setting.KeySetting;
    }

    private void Update()
    {
        if (isMoving)
            return;

        if (Input.GetKeyDown(keySetting.GetKeyCode(InputAction.LeftMove)))
        {
            StartCoroutine(MoveCoroutine(Vector3.left));
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            StartCoroutine(MoveCoroutine(Vector3.right));
        }
        else if (Input.GetKeyDown(keySetting.GetKeyCode(InputAction.UpMove)))
        {
            StartCoroutine(MoveCoroutine(Vector3.up));
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            StartCoroutine(MoveCoroutine(Vector3.down));
        }


    }

    private IEnumerator MoveCoroutine(Vector3 dir)
    {
        Vector3 start = transform.position;
        Vector3 end = start + dir;
        float current = 0;
        float percent = 0;

        isMoving = true;

        while (percent < 1)
        {
            current += Time.deltaTime;
            percent = current / moveTime;

            transform.position = Vector3.Lerp(start, end, percent);

            yield return null;
        }

        isMoving = false;
    }
}
