using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LedgeClimber : MonoBehaviour
{

    [Header("Detect (OverlapSphere by Tag)")]
    [SerializeField] string climbableTag = "Interactable";
    [SerializeField] Vector3 chestLocalOffset = new Vector3(0f, 1.2f, 0.25f);
    [SerializeField] float sphereRadius = 0.5f;
    [SerializeField] float maxUpHeight = 1.5f;
    [SerializeField] float maxDownHeight = 0.5f;

    [Header("Climb Motion")]
    [SerializeField] float standUpOffset = 0.1f;
    [SerializeField] float climbDuration = 0.4f;
    [SerializeField] float climbArcHeight = 0.5f;
    [SerializeField] bool useBoundsCenterXZ = false;

    Rigidbody rb;
    bool isClimbing;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void OnAttack()
    {
        if (isClimbing) return;
        SnapToObjectTop();
    }

    void SnapToObjectTop()
    {
        Vector3 chest = transform.TransformPoint(chestLocalOffset);
        Collider[] hits = Physics.OverlapSphere(chest, sphereRadius, ~0, QueryTriggerInteraction.Ignore);
        if (hits == null || hits.Length == 0) return;

        Collider best = null;
        float bestScore = float.MaxValue;
        Vector3 bestClosest = Vector3.zero;

        foreach (var c in hits)
        {
            if (c.transform.root == transform.root) continue;
            if (!c.CompareTag(climbableTag)) continue;

            Vector3 p = c.ClosestPoint(chest);
            float horizDistSq = (new Vector2(chest.x - p.x, chest.z - p.z)).sqrMagnitude;

            float rise = c.bounds.max.y - transform.position.y;
            if (rise > maxUpHeight || rise < -maxDownHeight) continue;

            if (horizDistSq < bestScore)
            {
                bestScore = horizDistSq;
                best = c;
                bestClosest = p;
            }
        }

        if (!best) return;

        Bounds bnds = best.bounds;
        Vector3 standXZ = useBoundsCenterXZ
            ? new Vector3(bnds.center.x, 0f, bnds.center.z)
            : new Vector3(bestClosest.x, 0f, bestClosest.z);
        Vector3 targetPos = new Vector3(
            standXZ.x,
            bnds.max.y + standUpOffset,
            standXZ.z);

        StartCoroutine(ClimbRoutine(transform.position, targetPos));
    }

    IEnumerator ClimbRoutine(Vector3 startPos, Vector3 endPos)
    {
        isClimbing = true;

        // 중력 상태 저장 및 비활성화
        bool prevGravity = rb.useGravity;
        rb.useGravity = false;
        rb.velocity = Vector3.zero; // 기존 속도 제거

        float t = 0f;
        Vector3 lastPos = startPos;

        while (t < 1f)
        {
            t += Time.deltaTime / climbDuration;
            // 아치 모양의 경로 생성
            Vector3 apex = (startPos + endPos) * 0.5f + Vector3.up * climbArcHeight;
            Vector3 pos1 = Vector3.Lerp(startPos, apex, t);
            Vector3 pos2 = Vector3.Lerp(apex, endPos, t);
            Vector3 current = Vector3.Lerp(pos1, pos2, t);

            // 물리 이동
            rb.MovePosition(current);

            // 경로를 실시간으로 씬 뷰에 그려 줌
            Debug.DrawLine(lastPos, current, Color.green, 0f, false);
            lastPos = current;

            yield return null;
        }

        // 중력 원상 복구
        rb.useGravity = prevGravity;
        isClimbing = false;
    }

    void OnDrawGizmos()
    {
        // 탐지 구체를 씬 뷰에서 확인할 수 있도록 그리기
        Gizmos.color = Color.cyan;
        Vector3 chest = Application.isPlaying
            ? transform.TransformPoint(chestLocalOffset)
            : transform.localToWorldMatrix.MultiplyPoint3x4(chestLocalOffset);
        Gizmos.DrawWireSphere(chest, sphereRadius);
    }
}
