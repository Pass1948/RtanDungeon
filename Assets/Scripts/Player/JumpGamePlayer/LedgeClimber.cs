using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LedgeClimber : MonoBehaviour
{
    [Header("Detect (OverlapSphere)")]
    [SerializeField] LayerMask climbableMask = 0;               // Climbable ���̾�
    [SerializeField] Vector3 chestLocalOffset = new Vector3(0f, 1.2f, 0.25f);
    [SerializeField] float sphereRadius = 0.45f;

    [Header("Place")]
    [SerializeField] float standUpOffset = 0.12f;               // ������Ʈ ��ܿ��� ��¦ ����
    [SerializeField] bool useBoundsCenterXZ = false;            // true�� Bounds �߽� XZ ���

    [Header("Debug")]
    [SerializeField] bool drawGizmos = true;
    [SerializeField] Color gizmoColor = new Color(0f, 1f, 1f, 0.6f);

    Collider lastHit;           // ����׿�
    Vector3 lastChest;          // ����׿�
    Vector3 lastClosestPoint;   // ����׿�
    Vector3 lastStandPos;       // ����׿�

    void OnAttack() // ��Ŭ��(Attack) �׼� ȣ�� ��
    {
        SnapToObjectTop();
    }

    void SnapToObjectTop()
    {
        // 1) ���� ��ġ ���
        Vector3 chest = transform.TransformPoint(chestLocalOffset);
        lastChest = chest;

        // 2) OverlapSphere�� �ĺ� ����
        var hits = Physics.OverlapSphere(chest, sphereRadius, climbableMask, QueryTriggerInteraction.Ignore);
        if (hits == null || hits.Length == 0) return;

        // 3) ���� ����� �ݶ��̴� ����(���� �Ÿ� ����)
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

        // 4) ���ĵ� ��ġ ���: ������Ʈ "��� ����"�� ���
        Bounds b = best.bounds;

        Vector3 standXZ;
        if (useBoundsCenterXZ)
        {
            // Bounds �߽� XZ�� ����
            standXZ = new Vector3(b.center.x, 0f, b.center.z);
        }
        else
        {
            // �÷��̾�� ���� ����� ǥ������ XZ�� ����
            standXZ = new Vector3(bestClosest.x, 0f, bestClosest.z);
        }

        Vector3 standPos = new Vector3(standXZ.x, b.max.y + standUpOffset, standXZ.z);
        lastStandPos = standPos;

        // 5) ��� ����(�ʿ�� Rigidbody.MovePosition���� ��ü)
        transform.position = standPos;
    }

    void OnDrawGizmos()
    {
        if (!drawGizmos) return;

        Vector3 chest = Application.isPlaying
            ? transform.TransformPoint(chestLocalOffset)
            : transform.localToWorldMatrix.MultiplyPoint3x4(chestLocalOffset);

        // OverlapSphere �ð�ȭ
        Gizmos.color = gizmoColor;
        Gizmos.DrawWireSphere(chest, sphereRadius);

        // ���õ� �ݶ��̴��� ����Ʈ, ���� ���ĵ� ��ġ �����
        if (lastHit)
        {
            // ���� �� ���� ����� ǥ����
            Debug.DrawLine(lastChest, lastClosestPoint, Color.cyan);

            // ���� ���ĵ� ��ġ ��Ŀ
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(lastStandPos, 0.06f);

            // ���õ� �ݶ��̴� bounds ����(�뷫)
            Gizmos.color = new Color(1f, 1f, 0f, 0.25f);
            Gizmos.DrawWireCube(lastHit.bounds.center, lastHit.bounds.size);
        }
    }
}
