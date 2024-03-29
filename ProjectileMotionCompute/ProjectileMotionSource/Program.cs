﻿using ProjectileMotionSource.Func;
using Utilities.Units;
using Utilities.Quantities;
using System;
using ProjectileMotionSource.WithResistance.Func;

class Program
{
    static void Main()
    {

        InitialVelocity v = new InitialVelocity(40, UnitVelocity.KilometrePerHour);
        ElevationAngle α = new ElevationAngle(41, UnitAngle.Degree);
        InitialHeight h = new InitialHeight(169, UnitLength.Centimetre);
        GravAcceleration g = new GravAcceleration(GravAcceleration.GravAccelerations.Earth);


        ProjectileMotionResultsUnits units = new ProjectileMotionResultsUnits()
        {
            Length = UnitLength.Metre,
            Time = UnitTime.Second,
            Velocity = UnitVelocity.KilometrePerHour,
            Angle = UnitAngle.Degree,
            Area = UnitArea.SquareMetre,
            GravAcceleration = UnitGravAcceleration.MetrePerSquareSecond
        };


        ProjectileMotion motion = new ProjectileMotion(
            new ProjectileMotionSettings(new ProjectileMotionQuantities(v, α, h, g, units))
            {
                RoundDigits = 4,
                PathToFiles = "C:\\Users\\oplan\\Documents\\c#\\ProjectileMotions",
                PointsForTrajectory = 160
            });

        motion.Saving.InfoToTxt();



        ProjectileMotionWithResistance motionWithResistance = new ProjectileMotionWithResistance(
           new ProjectileMotionWithResistanceSettings(
               new ProjectileMotionWithResistanceQuantities(
                   v, α, h, g,
                   new Mass(46, UnitMass.Gram),
                   new Density(Density.Densities.Air),
                   new FrontalArea(Math.Pow(new Length(12.9, UnitLength.Centimetre).GetBasicVal(), 2.0) * Math.PI, UnitArea.SquareMetre),
                   new DragCoefficient(DragCoefficient.DragCoefficients.Sphere),
                   units
               )
           )
           {
               RoundDigits = 4,
               PathToFiles = "C:\\Users\\oplan\\Documents\\c#\\ProjectileMotions",
               PointsForTrajectory = 153
           });

        motionWithResistance.Saving.InfoToTxt();
    }
}

