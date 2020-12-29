using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ShooterV2))]
public class ShooterV2Editor : Editor
{
    private void OnSceneGUI()
    {
        ShooterV2 shooter = (ShooterV2) target;
        Handles.color = Color.white;
        //Handles.DrawWireArc(shooter.transform.position, Vector3.forward, Vector3.up, 360, shooter.AttackRadius);
        float angleA = (-(shooter.viewAngle) / 2) - (shooter.angleOffset);
        float angleB = ((shooter.viewAngle) / 2) - (shooter.angleOffset);
        Vector3 viewAngleA = DirFromAngle(angleA);
        Vector3 viewAngleB = DirFromAngle(angleB);

        Handles.DrawWireArc(shooter.transform.position, Vector3.forward, viewAngleB, shooter.viewAngle,
            shooter.attackRadius);
        Handles.DrawLine(shooter.transform.position, shooter.transform.position + viewAngleA * shooter.attackRadius);
        Handles.DrawLine(shooter.transform.position, shooter.transform.position + viewAngleB * shooter.attackRadius);
    }

    private Vector3 DirFromAngle(float angleDeg)
    {
        return new Vector3(Mathf.Sin((angleDeg + 0f) * Mathf.Deg2Rad), Mathf.Cos((angleDeg + 0f) * Mathf.Deg2Rad), 0);
    }
}
