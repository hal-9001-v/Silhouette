using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GeneralTools
{
    class LineMath
    {
        public static Vector3 GetClosestPointOnLine(LineRenderer line, Vector3 position)
        {
            // Vector3 closestPosition = line.transform.TransformPoint(line.GetPosition(0));
            Vector3 closestPosition = (line.GetPosition(0));

            for (int i = 1; i < line.positionCount; i++)
            {

                //Vector3 newPosition = line.transform.TransformPoint(line.GetPosition(i));
                Vector3 newPosition = line.GetPosition(i);
                //Vector3 newPosition = line.GetPosition(i);

                if (Vector3.Distance(position, newPosition) < Vector3.Distance(closestPosition, position))
                {
                    closestPosition = newPosition;
                }


            }
            return closestPosition;

        }

        public static Vector3 GetClosestPointOnLines(LineRenderer[] lines, Vector3 position)
        {
            Vector3 closestPoint = GetClosestPointOnLine(lines[0], position);
            Vector3 newPoint;
            for (int i = 1; i < lines.Length; i++)
            {
                newPoint = GetClosestPointOnLine(lines[i], position);

                if (Vector3.Distance(closestPoint, position) > Vector3.Distance(newPoint, position))
                {
                    closestPoint = newPoint;
                }
            }

            return closestPoint;
        }
    }


    class PhysicsMath
    {

        public static float LaunchSpeedForHeight(float height)
        {
            float launchMagnitude = 2 * Mathf.Abs(Physics.gravity.y) * height;

            launchMagnitude = Mathf.Pow(launchMagnitude, 0.5f);

            return launchMagnitude;

        }

    }


}