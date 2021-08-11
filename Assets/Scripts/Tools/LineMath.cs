using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GeneralTools
{
    class LineMath
    {
        public static Vector3 GetClosestPointOnLine(LineRenderer line, Vector3 position)
        {
            Vector3 closestPosition = line.transform.TransformPoint(line.GetPosition(0));

            for (int i = 1; i < line.positionCount; i++)
            {

                Vector3 newPosition = line.transform.TransformPoint(line.GetPosition(i));
                //Vector3 newPosition = line.GetPosition(i);

                if (Vector3.Distance(position, newPosition) < Vector3.Distance(closestPosition, position))
                {
                    closestPosition = newPosition;
                }


            }
            return closestPosition;

        }
    }



}