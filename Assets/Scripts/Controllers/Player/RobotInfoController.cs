using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class RobotInfoController : MonoBehaviour
{
    TextMeshProUGUI title, speed, cdr, battery, bdr;
    Transform parent;
    Robot robot;
    private void Start()
    {
        parent = transform.GetChild(0).GetChild(0);
        title = parent.GetChild(0).GetComponent<TextMeshProUGUI>();
        speed = parent.GetChild(1).GetComponent<TextMeshProUGUI>();
        cdr = parent.GetChild(2).GetComponent<TextMeshProUGUI>();
        battery = parent.GetChild(3).GetComponent<TextMeshProUGUI>();
        bdr = parent.GetChild(4).GetComponent<TextMeshProUGUI>();
        robot = transform.parent.GetComponent<RobotController>().robot;

       

    }


    public IEnumerator FillInfo()
    {
        yield return new WaitForSeconds(2);
        title.text = robot.robotType.ToString();
        speed.text = "Speed: " + robot.speed.ToString();
        cdr.text = "CDR: " + robot.abilityCDR.ToString();
        battery.text = "Battery: " + robot.batteryValue.ToString() + "/" + robot.batteryCapacity.ToString();
        bdr.text = "Batter Discharge Rate: " + robot.batteryDischargeRate.ToString();
    }
}
