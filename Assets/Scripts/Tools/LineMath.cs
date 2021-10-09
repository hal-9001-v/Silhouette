using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GeneralTools
{
    class LineMath
    {
        public static Vector3 GetClosestPointOnLine(LineRenderer line, Vector3 position, bool transformToWorldSpace)
        {
            Vector3 closestPosition;

            if (transformToWorldSpace)
                closestPosition = line.transform.TransformPoint(line.GetPosition(0));
            else
                closestPosition = (line.GetPosition(0));


            for (int i = 1; i < line.positionCount; i++)
            {
                Vector3 newPosition;
                if (transformToWorldSpace)
                    newPosition = line.transform.TransformPoint(line.GetPosition(i));
                else
                    newPosition = line.GetPosition(i);
                //Vector3 newPosition = line.GetPosition(i);

                if (Vector3.Distance(position, newPosition) < Vector3.Distance(closestPosition, position))
                {
                    closestPosition = newPosition;
                }


            }
            return closestPosition;

        }

        public static Vector3 GetClosestPointOnLines(LineRenderer[] lines, Vector3 position, bool transformToworldSpace)
        {
            Vector3 closestPoint = GetClosestPointOnLine(lines[0], position, transformToworldSpace);
            Vector3 newPoint;
            for (int i = 1; i < lines.Length; i++)
            {
                newPoint = GetClosestPointOnLine(lines[i], position, transformToworldSpace);

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

        public static float NonVerticalDistance(Vector3 a, Vector3 b)
        {
            return Vector2.Distance(new Vector2(a.x, a.z), new Vector2(b.x, b.z));
        }

    }


}