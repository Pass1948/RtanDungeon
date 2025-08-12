using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LedgeClimber : MonoBehaviour
{
    [Header("Detect (OverlapSphere by Tag)")]
    [SerializeField] string climbableTag = "Interactable";
    [SerializeField] Vector3 chestLocalOffset = new Vector3(0f, 1.2f, 0.25f);
    [SerializeField] float sphereRadius = 0.45f;

    [Header("Place")]
    [SerializeField] float standUpOffset = 0.12f;     // 상단에서 살짝 띄우기
    [SerializeField] bool useBoundsCenterXZ = false;  // true: Bounds 중심 XZ 사용

    [Header("Guard Conditions")]
    [SerializeField] float minRiseToSnap = 0.2f;      // 후보 상단이 플레이어보다 최소 이 높이만큼 위일 때만
    [SerializeField, Range(-1f, 1f)] float minForwardDot = -0.2f; // 전방성(선택), -1~1

    [Header("Debug")]
    [SerializeField] bool drawGizmos = true;
    [SerializeField] Color gizmoColor = new Color(0f, 1f, 1f, 0.6f);

    Collider lastHit; Vector3 lastChest; Vector3 lastClosestPoint; Vector3 lastStandPos;

    void OnAttack() => SnapToObjectTop();

    void SnapToObjectTop()
    {
        Vector3 chest = transform.TransformPoint(chestLocalOffset);
        lastChest = chest;

        // 모든 레이어 대상으로 Overlap, 트리거 무시
        var hits = Physics.OverlapSphere(chest, sphereRadius, ~0, QueryTriggerInteraction.Ignore);
        if (hits == null || hits.Length == 0) return;

        Collider best = null;
        float bestScore = float.MaxValue;
        Vector3 bestClosest = Vector3.zero;

        foreach (var c in hits)
        {
            // 1) 자기 자신(같은 루트) 제외
            if (c.transform.root == transform.root) continue;

            // 2) Tag 필터
            var go = c.attachedRigidbody ? c.attachedRigidbody.gameObject : c.gameObject;
            if (!go.CompareTag(climbableTag)) continue;

            // 3) 수평거리 기반 가장 가까운 후보 계산
            Vector3 p = c.ClosestPoint(chest);
            Vector2 a = new Vector2(chest.x, chest.z);
            Vector2 b = new Vector2(p.x, p.z);
            float horizDistSq = (a - b).sqrMagnitude;

            // 4) 전방성(선택): 플레이어 전방과 후보 방향의 내적 체크
            Vector3 to = new Vector3(p.x - chest.x, 0f, p.z - chest.z);
            if (to.sqrMagnitude > 0.0001f)
            {
                float dot = Vector3.Dot(to.normalized, transform.forward);
                if (dot < minForwardDot) continue; // 너무 뒤쪽이면 제외
            }

            // 5) 최소 상승 높이 조건 (허공에서 아래 오브젝트로 텔레포트 방지)
            float rise = c.bounds.max.y - transform.position.y;
            if (rise < minRiseToSnap) continue;

            if (horizDistSq < bestScore)
            {
                bestScore = horizDistSq;
                best = c;
                bestClosest = p;
            }
        }

        if (!best) return;

        lastHit = best;
        lastClosestPoint = bestClosest;

        // 오브젝트 상단 Y로 스냅
        Bounds bnds = best.bounds;

        Vector3 standXZ = useBoundsCenterXZ
            ? new Vector3(bnds.center.x, 0f, bnds.center.z)
            : new Vector3(bestClosest.x, 0f, bestClosest.z);

        Vector3 standPos = new Vector3(standXZ.x, bnds.max.y + standUpOffset, standXZ.z);
        lastStandPos = standPos;

        transform.position = standPos; // Rigidbody 사용 시 rb.MovePosition(standPos);
    }

    void OnDrawGizmos()
    {
        if (!drawGizmos) return;

        Vector3 chest = Application.isPlaying
            ? transform.TransformPoint(chestLocalOffset)
            : transform.localToWorldMatrix.MultiplyPoint3x4(chestLocalOffset);

        Gizmos.color = gizmoColor;
        Gizmos.DrawWireSphere(chest, sphereRadius);

        if (lastHit)
        {
            Debug.DrawLine(lastChest, lastClosestPoint, Color.cyan);
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(lastStandPos, 0.06f);
            Gizmos.color = new Color(1f, 1f, 0f, 0.25f);
            Gizmos.DrawWireCube(lastHit.bounds.center, lastHit.bounds.size);
        }
    }
}
