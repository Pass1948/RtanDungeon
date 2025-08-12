using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LedgeClimber : MonoBehaviour
{
    [Header("Detect (OverlapSphere)")]
    [SerializeField] LayerMask climbableMask = 0;               // Climbable 레이어
    [SerializeField] Vector3 chestLocalOffset = new Vector3(0f, 1.2f, 0.25f);
    [SerializeField] float sphereRadius = 0.45f;

    [Header("Place")]
    [SerializeField] float standUpOffset = 0.12f;               // 오브젝트 상단에서 살짝 띄우기
    [SerializeField] bool useBoundsCenterXZ = false;            // true면 Bounds 중심 XZ 사용

    [Header("Debug")]
    [SerializeField] bool drawGizmos = true;
    [SerializeField] Color gizmoColor = new Color(0f, 1f, 1f, 0.6f);

    Collider lastHit;           // 디버그용
    Vector3 lastChest;          // 디버그용
    Vector3 lastClosestPoint;   // 디버그용
    Vector3 lastStandPos;       // 디버그용

    void OnAttack() // 좌클릭(Attack) 액션 호출 시
    {
        SnapToObjectTop();
    }

    void SnapToObjectTop()
    {
        // 1) 가슴 위치 계산
        Vector3 chest = transform.TransformPoint(chestLocalOffset);
        lastChest = chest;

        // 2) OverlapSphere로 후보 수집
        var hits = Physics.OverlapSphere(chest, sphereRadius, climbableMask, QueryTriggerInteraction.Ignore);
        if (hits == null || hits.Length == 0) return;

        // 3) 가장 가까운 콜라이더 선택(수평 거리 기준)
        Collider best = null;
        float bestDist = float.MaxValue;
        Vector3 bestClosest = Vector3.zero;

        foreach (var c in hits)
        {
            Vector3 p = c.ClosestPoint(chest);
            float d = (new Vector2(chest.x, chest.z) - new Vector2(p.x, p.z)).sqrMagnitude;
            if (d < bestDist)
            {
                bestDist = d;
                best = c;
                bestClosest = p;
            }
        }
        if (!best) return;

        lastHit = best;
        lastClosestPoint = bestClosest;

        // 4) 스탠드 위치 계산: 오브젝트 "상단 높이"를 사용
        Bounds b = best.bounds;

        Vector3 standXZ;
        if (useBoundsCenterXZ)
        {
            // Bounds 중심 XZ에 서기
            standXZ = new Vector3(b.center.x, 0f, b.center.z);
        }
        else
        {
            // 플레이어와 가장 가까운 표면점의 XZ에 서기
            standXZ = new Vector3(bestClosest.x, 0f, bestClosest.z);
        }

        Vector3 standPos = new Vector3(standXZ.x, b.max.y + standUpOffset, standXZ.z);
        lastStandPos = standPos;

        // 5) 즉시 스냅(필요시 Rigidbody.MovePosition으로 대체)
        transform.position = standPos;
    }

    void OnDrawGizmos()
    {
        if (!drawGizmos) return;

        Vector3 chest = Application.isPlaying
            ? transform.TransformPoint(chestLocalOffset)
            : transform.localToWorldMatrix.MultiplyPoint3x4(chestLocalOffset);

        // OverlapSphere 시각화
        Gizmos.color = gizmoColor;
        Gizmos.DrawWireSphere(chest, sphereRadius);

        // 선택된 콜라이더와 포인트, 최종 스탠드 위치 디버그
        if (lastHit)
        {
            // 가슴 → 가장 가까운 표면점
            Debug.DrawLine(lastChest, lastClosestPoint, Color.cyan);

            // 최종 스탠드 위치 마커
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(lastStandPos, 0.06f);

            // 선택된 콜라이더 bounds 윤곽(대략)
            Gizmos.color = new Color(1f, 1f, 0f, 0.25f);
            Gizmos.DrawWireCube(lastHit.bounds.center, lastHit.bounds.size);
        }
    }
}
