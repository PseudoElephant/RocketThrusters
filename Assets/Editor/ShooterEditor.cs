using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor (typeof (ShooterBehaviour))]
public class ShooterEditor : Editor
{
    private void OnSceneGUI()
    {
        ShooterBehaviour shooter = (ShooterBehaviour)target;
        Handles.color = Color.white;
        //Handles.DrawWireArc(shooter.transform.position, Vector3.forward, Vector3.up, 360, shooter.AttackRadius);
        float angleA = (-(shooter.ViewAngle) / 2) - (shooter.AngleOffset - 90f);
        float angleB = ((shooter.ViewAngle) / 2) - (shooter.AngleOffset - 90f);
        Vector3 viewAngleA = DirFromAngle(angleA);
        Vector3 viewAngleB = DirFromAngle(angleB);

        Handles.DrawWireArc(shooter.transform.position, Vector3.forward, viewAngleB, shooter.ViewAngle, shooter.AttackRadius);
        Handles.DrawLine(shooter.transform.position, shooter.transform.position + viewAngleA * shooter.AttackRadius);
        Handles.DrawLine(shooter.transform.position, shooter.transform.position + viewAngleB * shooter.AttackRadius);
    }

    private Vector3 DirFromAngle(float angleDeg)
    {
        return new Vector3(Mathf.Sin((angleDeg+0f) * Mathf.Deg2Rad), Mathf.Cos((angleDeg+0f) * Mathf.Deg2Rad), 0);
    }
}
