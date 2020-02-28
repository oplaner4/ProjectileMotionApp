﻿using ProjectileMotionSource.Func;
using ProjectileMotionSource.Point;
using ProjectileMotionSource.WithResistance.Func;
using ProjectileMotionWeb.Classes;
using ProjectileMotionWeb.Helpers;
using System;
using System.Collections.Generic;

namespace ProjectileMotionWeb.Models
{
    public class _MotionChartModel : BaseModel
    {
        public string GetSpeciallySerializedTrajectory (List<ProjectileMotionPoint> listPoints)
        {
            _MotionChartPoint[] points = new _MotionChartPoint[listPoints.Count];
            int i = 0;
            foreach (ProjectileMotionPoint point in listPoints)
            {
                points[i] = new _MotionChartPoint(point.Round());
                i++;
            }

            return new JsonSerializerHelper(points).Serialize();
        }

        public ProjectileMotion Motion { get; private set; }

        public _MotionChartModel(ProjectileMotion motion)
        {
            Motion = motion;
            DegradedMotion = Motion;
            ShowMotionWithoutResistanceTrajectoryToo = false;
        }


        public _MotionChartModel(ProjectileMotionWithResistance motion)
        {
            Motion = motion;
            DegradedMotion = motion.Degrade();
            ShowMotionWithoutResistanceTrajectoryToo = motion.Settings.ShowMotionWithoutResistanceTrajectoryToo;
        }

        public ProjectileMotion DegradedMotion { get; private set; }

        public bool ShowMotionWithoutResistanceTrajectoryToo { get; private set; }
    }
}