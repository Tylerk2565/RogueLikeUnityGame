using UnityEngine;

public static class CombatSystem
{
    public static bool TryRaycastAttack(Vector3 origin, Vector3 direction,
        float range, LayerMask targetMask, out RaycastHit hit, bool drawDebug = true)
    {
        if (drawDebug)
        {
            Debug.DrawRay(origin, direction * range, Color.cyan, 1f);
        }

        bool hitDetected = Physics.Raycast(origin, direction, out hit, range, targetMask);

        if (drawDebug)
        {
            if (hitDetected)
            {
                Debug.DrawRay(hit.point, Vector3.up * 0.5f, Color.green, 1f);
            }
            else
            {
                Debug.DrawRay(origin, direction * range, Color.yellow, 1f);
            }
        }
         
        return hitDetected;
    }
}