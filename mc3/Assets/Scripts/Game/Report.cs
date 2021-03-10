using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Report : MonoBehaviour {
    public static string result;

    public static int floor;
    public static int playerLevel;
    public static int playerHP;

    public static new string ToString() {
        string report = "";
        report += "Max Floor " + floor + "\n\n";
        report += "Level " + playerLevel + "\n\n";
        report += "HP " + playerHP + "\n\n";
        return report;
    }
}
