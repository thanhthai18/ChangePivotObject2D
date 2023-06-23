using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
#if UNITY_EDITOR
using UnityEditor;
#endif

/*-------------hathanhthai18@gmail.com-------------*/
public class PivotObject : MonoBehaviour
{
    [SerializeField] private bool isDrawGizmos;
    [SerializeField] private SpriteRenderer mySprite;
    [SerializeField] public Transform pivot;
    private Vector3 undoPos;
    public BoundaryPivot boundaryType;

    /// <summary>
    /// Change pivot of the MainObject to position of the Pivot
    /// </summary>
    public void SetPivot()
    {
        if (mySprite == null) return;

        Vector3 posPivot = pivot.position;
        undoPos = transform.position;
        Execute(posPivot);
    }

    /// <summary>
    /// Change pivot of the MainObject by BoundaryPivot enum
    /// </summary>
    /// <param name="boundaryType"></param>
    public void SetBoundarySpritePivot(BoundaryPivot boundaryType)
    {
        if (mySprite == null) return;

        Vector2 spriteSize = mySprite.sprite.bounds.size;
        Vector3 boundaryPosition = new Vector3(spriteSize.x * 0.5f, spriteSize.y * 0.5f, 0.0f);
        Vector3 posSize;


        switch (boundaryType)
        {
            case BoundaryPivot.TOP:
                posSize = mySprite.transform.TransformPoint(new Vector2(0, boundaryPosition.y));
                pivot.transform.position = posSize;
                break;
            case BoundaryPivot.BOTTOM:
                posSize = mySprite.transform.TransformPoint(new Vector2(0, -boundaryPosition.y));
                pivot.transform.position = posSize;
                break;
            case BoundaryPivot.LEFT:
                posSize = mySprite.transform.TransformPoint(new Vector2(-boundaryPosition.x, 0));
                pivot.transform.position = posSize;
                break;
            case BoundaryPivot.RIGHT:
                posSize = mySprite.transform.TransformPoint(new Vector2(boundaryPosition.x, 0));
                pivot.transform.position = posSize;
                break;
            case BoundaryPivot.TOP_LEFT:
                posSize = mySprite.transform.TransformPoint(new Vector2(-boundaryPosition.x, boundaryPosition.y));
                pivot.transform.position = posSize;
                break;
            case BoundaryPivot.TOP_RIGHT:
                posSize = mySprite.transform.TransformPoint(new Vector2(boundaryPosition.x, boundaryPosition.y));
                pivot.transform.position = posSize;
                break;
            case BoundaryPivot.BOTTOM_LEFT:
                posSize = mySprite.transform.TransformPoint(new Vector2(-boundaryPosition.x, -boundaryPosition.y));
                pivot.transform.position = posSize;
                break;
            case BoundaryPivot.BOTTOM_RIGHT:
                posSize = mySprite.transform.TransformPoint(new Vector2(boundaryPosition.x, -boundaryPosition.y));
                pivot.transform.position = posSize;
                break;
            default:
                break;
        }
        SetPivot();
    }

    /// <summary>
    /// Change pivot of the MainObject to center of this mySprite
    /// </summary>
    public void SetCenterPivot()
    {
        pivot.localPosition = mySprite.transform.localPosition;
        SetPivot();
    }

    /// <summary>
    /// Undo the last pivot
    /// </summary>
    public void UndoPosPivot()
    {
        pivot.transform.position = undoPos;
        SetPivot();
    }


    private void Execute(Vector3 pos)
    {
        var originalPosPivot = pivot.position;
        var originalPosMySprite = mySprite.transform.position;
        transform.position = pos;
        pivot.position = originalPosPivot;
        mySprite.transform.position = originalPosMySprite;
    }

    private void OnDrawGizmos()
    {
        if (!isDrawGizmos) return;
        if (mySprite == null) return;
        Gizmos.color = Color.green;

        Gizmos.DrawLine(transform.position, pivot.transform.position);
        Gizmos.DrawWireSphere(transform.position, 0.2f);
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(pivot.position, 0.2f);

    }

}
public enum BoundaryPivot
{
    TOP,
    BOTTOM,
    LEFT,
    RIGHT,
    TOP_LEFT,
    TOP_RIGHT,
    BOTTOM_LEFT,
    BOTTOM_RIGHT,
}
#if UNITY_EDITOR
[CustomEditor(typeof(PivotObject))]
public class PivotObjectEditor : Editor
{
    private PivotObject obj;
    private void OnEnable()
    {
        obj = (PivotObject)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("Set Pivot"))
        {
            obj.SetPivot();
        }
        if (GUILayout.Button("Set Boundary Pivot"))
        {
            obj.SetBoundarySpritePivot(obj.boundaryType);
        }
        if (GUILayout.Button("Set Center Pivot"))
        {
            obj.SetCenterPivot();
        }   
        if (GUILayout.Button("Undo"))
        {
            obj.UndoPosPivot();
        }

    }
    private void OnSceneGUI()
    {
        Handles.color = Color.red;
        obj.pivot.transform.position = Handles.FreeMoveHandle(obj.pivot.position, Quaternion.identity, 0.1f, Vector3.one * 0.1f, Handles.DotHandleCap);
        Handles.color = Color.green;
        
        Handles.DrawLine(obj.transform.position, obj.pivot.transform.position);
    }
}
#endif