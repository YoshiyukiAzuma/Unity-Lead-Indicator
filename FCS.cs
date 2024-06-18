using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FCS : MonoBehaviour
{
    public GameObject obj2;
    public GameObject lead_markPrefab;
    [HideInInspector]
    public GameObject lead_mark;// world space mark
    Rigidbody rb;
    public Camera cam;
    public Image indicatorPrefab;
    [HideInInspector]
    public Image indicator;// screen space mark
    public float self_bulletSpeed;
    float obj2Speed;
    Vector3 line_of_sight;
    Vector3 obj2_globalDirection;

    // Start is called before the first frame update
    void Start()
    {
        rb = obj2.GetComponent<Rigidbody>();
        lead_mark = Instantiate(lead_markPrefab, transform.position, transform.rotation);
        indicator = Instantiate(indicatorPrefab, transform.position, transform.rotation);
    }

    public float distance()
    {
        float distance = Vector3.Distance(obj2.transform.position, transform.position);
        return distance;
    }

    public float includedAngle()
    {
        line_of_sight = (transform.position - obj2.transform.position).normalized;
        obj2_globalDirection = rb.velocity.normalized;// without velocity, the value will be 0
        float includedAngle = Vector3.Angle(line_of_sight, obj2_globalDirection);
        return includedAngle;
    }

    public float lead()
    {
        float obj2Speed = rb.velocity.magnitude;// only physics force are taken into account
        float r = self_bulletSpeed/obj2Speed;
        float angleIAB = ((Mathf.Asin(Mathf.Sin((includedAngle() * Mathf.PI)/180)/r)) * Mathf.PI)/180;
        float angleAIB = ((180 - includedAngle() - angleIAB) * Mathf.PI)/180;
        float lead = (distance()/Mathf.Sin(angleAIB))*(Mathf.Sin((includedAngle() * Mathf.PI)/180)/r);
        return lead;
    }

    public Vector3 point_of_impact()
    {
        Vector3 point_of_impact = obj2.transform.position + obj2_globalDirection * lead();
        return point_of_impact;
    }

    // Update is called once per frame
    void Update()
    {
        lead_mark.transform.position = point_of_impact();
        indicator.transform.position = cam.WorldToScreenPoint(point_of_impact());
    }
}
