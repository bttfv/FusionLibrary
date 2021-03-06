﻿using GTA;
using GTA.Math;
using GTA.Native;
using System;
using System.Collections.Generic;
using static FusionLibrary.FusionEnums;

namespace FusionLibrary.Extensions
{
    public static class MathExtensions
    {
        public static double NextDouble(this Random random, double minValue, double maxValue)
        {
            return random.NextDouble() * (maxValue - minValue) + minValue;
        }

        public static int NextExcept(this Random random, int minValue, int maxValue, int except)
        {
            int ret;

            do
            {
                ret = random.Next(minValue, maxValue);
            } while (ret == except);

            return ret;
        }

        public static int NextExcept(this Random random, int minValue, int maxValue, List<int> except)
        {
            int ret;

            do
            {
                ret = random.Next(minValue, maxValue);
            } while (except.Contains(ret));

            return ret;
        }

        public static float Lerp(this float firstFloat, float secondFloat, float by)
        {
            return firstFloat * by + secondFloat * (1 - by);
        }

        public static float Remap(this float value, float from1, float to1, float from2, float to2)
        {
            return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
        }

        public static float ToMPH(this float value)
        {
            return value * 2.23694f;
        }

        public static float ToMS(this float value)
        {
            return value * 0.44704f;
        }

        public static float ToDeg(this float value)
        {
            return value * (180 / (float)Math.PI);
        }

        public static float ToRad(this float value)
        {
            return value * ((float)Math.PI / 180);
        }

        public static void RequestCollision(this Vector3 position)
        {
            Function.Call(Hash.REQUEST_COLLISION_AT_COORD, position.X, position.Y, position.Z);
        }

        public static void LoadScene(this Vector3 position)
        {
            Function.Call(Hash.NEW_LOAD_SCENE_START, position.X, position.Y, position.Z, 0.0f, 0.0f, 0.0f, 20.0f, 0);
        }

        public static Vector3 SetToGroundHeight(this Vector3 position)
        {
            position.Z = World.GetGroundHeight(position);

            return position;
        }

        public static Vector3 TransferHeight(this Vector3 src, Vector3 dst)
        {
            dst.Z += src.Z - World.GetGroundHeight(src);

            return dst;
        }

        public static Vector3 GetDirectionTo(this Vector3 src, Vector3 dst)
        {
            Vector3 ret = Vector3.Subtract(dst, src);
            ret.Normalize();

            return ret;
        }

        public static float GetMostFreeDirection(this Vector3 position, Entity ignoreEntity)
        {
            float ret = 0;
            float maxDist = -1;
            Vector3 lastPos = Vector3.Zero;

            const float r = 1000f;

            position = position.GetSingleOffset(Coordinate.Z, 1);

            for (float i = 0; i <= 360; i += 15)
            {
                float angleRad = i * (float)Math.PI / 180;

                float x = r * (float)Math.Cos(angleRad);
                float y = r * (float)Math.Sin(angleRad);

                Vector3 circlePos = position;
                circlePos.X += y;
                circlePos.Y += x;

                // Then we check for every pos if it hits tracks material
                RaycastResult raycast = World.Raycast(position, circlePos, IntersectFlags.Everything, ignoreEntity);

                if (!raycast.DidHit)
                {
                    ret = i;
                    lastPos = circlePos;
                    break;
                }

                float curDist = raycast.HitPosition.DistanceTo2D(position);

                if (curDist > maxDist)
                {
                    maxDist = curDist;
                    ret = i;
                    lastPos = circlePos;
                }
            }

            //if (lastPos != Vector3.Zero)
            //    CommonUtils.DrawLine(position, lastPos, Color.Aqua);

            return ret;
        }

        public static float PositiveAngle(this float value)
        {
            if (value < 0)
                value = 360 - Math.Abs(value);

            return value;
        }

        public static float WrapAngle(this float value)
        {
            value = value % 360;

            if (value < 0)
                value += 360;

            return value;
        }

        public static float AngularSpeed(this float linearSpeed, float wheelRadius, float currentRotation, bool vehDirection, float modifier = 1)
        {
            float angVel = ((linearSpeed.ToDeg() / Math.Abs(wheelRadius)) / Game.FPS) / modifier;

            if (vehDirection)
                currentRotation -= angVel;
            else
                currentRotation += angVel;

            return currentRotation.WrapAngle();
        }

        public static double ArcCos(this double X)
        {
            return Math.Atan(-X / Math.Sqrt(-X * X + 1)) + 2 * Math.Atan(1);
        }

        public static bool Near(this float src, float to, float by = 5)
        {
            return (to - by) <= src && src <= (to + by);
        }

        public static bool Near(this DateTime src, DateTime to, TimeSpan by, bool onlyFromLeft = false)
        {
            return to.Subtract(by) <= src && ((onlyFromLeft && src <= to) || (!onlyFromLeft && src <= to.Add(by)));
        }

        public static bool Between(this DateTime src, DateTime start, DateTime end)
        {
            return src >= start && src <= end;
        }

        public static bool BetweenHours(this DateTime src, DateTime start, DateTime end)
        {
            int hour = int.Parse(src.ToString("hh"));
            int hourStart = int.Parse(start.ToString("hh"));
            int hourEnd = int.Parse(end.ToString("hh"));

            return hour >= hourStart && hour <= hourEnd && src.Minute >= start.Minute && src.Minute <= end.Minute && src.Second >= start.Second && src.Second <= end.Second;
        }

        public static Vector3 DirectionToRotation(this Vector3 dir, float roll)
        {
            dir.Normalize();

            Vector3 vector3_1 = Vector3.Zero;

            vector3_1.Z = -(ToDeg((float)Math.Atan2(dir.X, dir.Y)));

            Vector3 vector3_2 = Vector3.Normalize(new Vector3(dir.Z, new Vector3(dir.X, dir.Y, 0).Length(), 0));

            vector3_1.X = -(ToDeg((float)Math.Atan2(vector3_2.X, vector3_2.Y)));

            vector3_1.Y = roll;

            return vector3_1;
        }

        public static Vector3 GetSingleOffset(this Vector3 vect3, Coordinate coord, float value)
        {
            switch (coord)
            {
                case Coordinate.X:
                    vect3.X += value;
                    break;
                case Coordinate.Y:
                    vect3.Y += value;
                    break;
                default:
                    vect3.Z += value;
                    break;
            }

            return vect3;
        }

        public static Vector3 InvertCoordinate(this Vector3 vect3, Coordinate coord)
        {
            switch (coord)
            {
                case Coordinate.X:
                    vect3.X = -vect3.X;
                    break;
                case Coordinate.Y:
                    vect3.Y = -vect3.Y;
                    break;
                default:
                    vect3.Z = -vect3.Z;
                    break;
            }

            return vect3;
        }
    }
}
