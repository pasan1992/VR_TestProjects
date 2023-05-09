using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class GameSword : MonoBehaviour
{
    // Start is called before the first frame update
    private AimConstraint aimConstraint;
    private GameObject aimTarget;

    public Transform pipe1Start;
    public Transform pipe1End;
    public Transform pipe2Start;
    public Transform pipe2End;

    public GameObject Pointer;
    public float pipeRadius = 0.5f;
    public bool inScabber=true;
    public bool hit = false;

    void Start()
    {
        aimConstraint = GetComponent<AimConstraint>();
        aimTarget = new GameObject();
    }

    public void onSaberedEnter()
    {
        Debug.Log("Enter");
        aimConstraint.weight = 1;
        aimConstraint.constraintActive = true;
        inScabber = true;
    }

    public void onSabberedLeave()
    {
         Debug.Log("Leave");
         aimConstraint.constraintActive = false;
        aimConstraint.weight = 0;
        inScabber = false;
    }

    public Transform Hand;
    public float lerpSpeed = 5f;

    private void Update()
    {
        transform.position = Vector3.Lerp(transform.position, Hand.position, lerpSpeed * Time.deltaTime);
        transform.rotation = Quaternion.Lerp(transform.rotation, Hand.rotation, lerpSpeed * Time.deltaTime);

            Debug.DrawLine(pipe1Start.transform.position, pipe1End.position, Color.red, 0.1f);
            Debug.DrawLine(pipe2Start.transform.position, pipe2End.position, Color.blue, 0.1f);

            var result = false;
            Vector3 intersection = Vector3.zero;
            Vector3 normal = Vector3.zero;

            LineCylinderIntersect(pipe1Start.position, (pipe1End.position - pipe1Start.position).normalized, 
            pipe2Start.position, pipe2End.position, 0.1f, out result, out intersection, out normal);
            if(!inScabber)
            {
                

                if(result && !hit)
                {
                    Pointer.transform.position = intersection;
                    aimConstraint.weight = 1;
                    aimConstraint.constraintActive = true;

                    var source = new ConstraintSource();
                    source.weight =1;
                    source.sourceTransform = Pointer.transform; 
                    var slist = new List<ConstraintSource>();
                    slist.Add(source);
                    aimConstraint.SetSources(slist);      
                }
                else if(!hit)
                {
                    aimConstraint.weight = 0;
                    aimConstraint.constraintActive = false;  

                }

                if(result)
                {
                    hit=true;
                }
                else
                {
                    hit=false;
                }
            }



            // Vector3 intersection;
            // Vector3 aDiff = pipe1End.position-pipe1Start.position;
            // Vector3 bDiff = pipe2End.position-pipe2Start.position;
            // var a1 = pipe1Start.position;
            // var a2 = pipe2Start.position;
            // var b1 = pipe1End.position;
            // var b2 = pipe2End.position;

            // if (LineLineIntersection(out intersection, pipe1Start.position, aDiff, pipe2Start.position, bDiff))
            // {
            //     float aSqrMagnitude = aDiff.sqrMagnitude;
            //     float bSqrMagnitude = bDiff.sqrMagnitude;

            //     if (    (intersection - a1).sqrMagnitude <= aSqrMagnitude  
            //         && (intersection - a2).sqrMagnitude <= aSqrMagnitude  
            //         && (intersection - b1).sqrMagnitude <= bSqrMagnitude 
            //         && (intersection - b2).sqrMagnitude <= bSqrMagnitude)
            //     {
            //         // there is an intersection between the two segments and 
            //         //   it is at intersection
            //         Pointer.transform.position = intersection;
            //     }
            // }
        
    }


            /// <summary>
        /// Check if a line intersects with a cylinder and return the instersection point
        /// </summary>
        /// <param name="startLine">Must be inside the cylinder</param>
        /// <param name="directionLine">Direction of the line (not the end point)</param>
        /// <param name="cylinderStart">Center of the bottom circle</param>
        /// <param name="cylinderEnd">Center of the top circle</param>
        /// <param name="cylinderRadius">Cylinder radius</param>
        /// <param name="result">Is there an intersection point?</param>
        /// <param name="intersection">Intersection point</param>
        /// <param name="normal">Normal vector</param>
        public static void LineCylinderIntersect(Vector3 startLine, Vector3 directionLine, Vector3 cylinderStart, Vector3 cylinderEnd, float cylinderRadius, out bool result, out Vector3 intersection, out Vector3 normal) {

            result = false;
            intersection = Vector3.zero;
            normal = Vector3.zero;

            // Calculate cylinder bounds for optimization
            float cxmin, cymin, czmin, cxmax, cymax, czmax;

            if (cylinderStart.z < cylinderEnd.z) {
                czmin = cylinderStart.z - cylinderRadius; 
                czmax = cylinderEnd.z + cylinderRadius;
            }
            else {
                czmin = cylinderEnd.z - cylinderRadius; 
                czmax = cylinderStart.z + cylinderRadius;
            }

            if (cylinderStart.y < cylinderEnd.y) {
                cymin = cylinderStart.y - cylinderRadius; 
                cymax = cylinderEnd.y + cylinderRadius;
            }
            else {
                cymin = cylinderEnd.y - cylinderRadius; 
                cymax = cylinderStart.y + cylinderRadius;
            }

            if (cylinderStart.x < cylinderEnd.x) {
                cxmin = cylinderStart.x - cylinderRadius; 
                cxmax = cylinderEnd.x + cylinderRadius;
            }
            else {
                cxmin = cylinderEnd.x - cylinderRadius; 
                cxmax = cylinderStart.x + cylinderRadius;
            }

            // Line out of bounds?
            if ((startLine.z >= czmax && (startLine.z + directionLine.z) > czmax)
                || (startLine.z <= czmin && (startLine.z + directionLine.z) < czmin)
                || (startLine.y >= cymax && (startLine.y + directionLine.y) > cymax)
                || (startLine.y <= cymin && (startLine.y + directionLine.y) < cymin)
                || (startLine.x >= cxmax && (startLine.x + directionLine.x) > cxmax)
                || (startLine.x <= cxmin && (startLine.x + directionLine.x) < cxmin)) {
                return;
            }

            Vector3 AB = cylinderEnd - cylinderStart;
            Vector3 AO = startLine - cylinderStart;
            Vector3 AOxAB = Vector3.Cross(AO, AB);
            Vector3 VxAB  = Vector3.Cross(directionLine, AB);
            float ab2 = Vector3.Dot(AB, AB);
            float a = Vector3.Dot(VxAB, VxAB);
            float b = 2 * Vector3.Dot(VxAB, AOxAB);
            float c = Vector3.Dot(AOxAB, AOxAB) - (cylinderRadius*cylinderRadius * ab2);
            float d = b * b - 4f * a * c;

            if (d < 0f) {
                return;
            }

            float time = (-b - Mathf.Sqrt(d)) / (2f * a);

            if (time < 0f) {
                return;
            }

            // intersection point
            intersection = startLine + directionLine * time;        

            // intersection projected onto cylinder axis
            Vector3 projection = cylinderStart + (Vector3.Dot(AB, intersection - cylinderStart) / ab2) * AB; 

            float test = (projection - cylinderStart).magnitude + (cylinderEnd - projection).magnitude;

            if (test - 0.01f > AB.magnitude) {
                return;
            }

            normal = intersection - projection;
            normal.Normalize();
            result = true;

        }

        public static bool LineLineIntersection(out Vector3 intersection, Vector3 linePoint1,
                Vector3 lineVec1, Vector3 linePoint2, Vector3 lineVec2){

            Vector3 lineVec3 = linePoint2 - linePoint1;
            Vector3 crossVec1and2 = Vector3.Cross(lineVec1, lineVec2);
            Vector3 crossVec3and2 = Vector3.Cross(lineVec3, lineVec2);

            float planarFactor = Vector3.Dot(lineVec3, crossVec1and2);
            //is coplanar, and not parallel
            if( Mathf.Abs(planarFactor) < 0.0001f 
                    && crossVec1and2.sqrMagnitude > 0.0001f)
            {
                float s = Vector3.Dot(crossVec3and2, crossVec1and2) 
                        / crossVec1and2.sqrMagnitude;
                intersection = linePoint1 + (lineVec1 * s);
                return true;
            }
            else
            {
                intersection = Vector3.zero;
                return false;
            }
        }
}
