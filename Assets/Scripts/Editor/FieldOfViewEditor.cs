using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor (typeof (PlayerRaycaster))]
public class FieldOfViewEditor : Editor {

    void OnSceneGUI() {
        PlayerRaycaster fow = (PlayerRaycaster)target;
        Handles.color = Color.white;
        Handles.DrawWireArc (fow.transform.position, Vector3.forward, Vector3.up, 360, fow.viewRadius);
        Vector3 viewAngleA = fow.DirFromAngle (-fow.viewAngle / 2 + 90, false);
        Vector3 viewAngleB = fow.DirFromAngle (fow.viewAngle / 2 + 90, false);

        viewAngleA = Quaternion.AngleAxis(90, Vector3.right) * viewAngleA;
        viewAngleB = Quaternion.AngleAxis(90, Vector3.right) * viewAngleB;


        Handles.DrawLine (fow.transform.position, fow.transform.position + viewAngleA  * fow.viewRadius);
        Handles.DrawLine (fow.transform.position, fow.transform.position + viewAngleB * fow.viewRadius);
        
         viewAngleA = fow.DirFromAngle (-fow.viewAngle / 2 - 90, false);
         viewAngleB = fow.DirFromAngle (fow.viewAngle / 2 - 90, false);

         viewAngleA = Quaternion.AngleAxis(90, Vector3.right) * viewAngleA;
         viewAngleB = Quaternion.AngleAxis(90, Vector3.right) * viewAngleB;
         
        Handles.DrawLine (fow.transform.position , fow.transform.position + viewAngleA  * fow.viewRadius );
        Handles.DrawLine (fow.transform.position, fow.transform.position + viewAngleB * fow.viewRadius);
        
        Handles.color = Color.green;
        Handles.DrawLine(fow.transform.position, fow.transform.position + (fow.forwardRayDistance * Vector3.up));
        
        
    }

}