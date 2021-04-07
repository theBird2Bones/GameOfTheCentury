﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Game
{
    public class Weapon : IWeaponInterface
    {
        public Weapon(int bulletCount, int reloadingTime, double splashRadius, 
            Vector bulletSpeed, Vector bulletGravity)
        {
            BulletCount = bulletCount;
            ReloadingTime = reloadingTime;
            SplashRadius = splashRadius;
            BulletSpeed = bulletSpeed;
            BulletGravity = bulletGravity;
        }
        public int BulletCount { get; set; }
        public int ReloadingTime { get; set; }
        public double SplashRadius { get; set; }
        public Vector BulletSpeed { get; set; }
        public Vector BulletGravity { get; set; }

        public Vector GetTotalBulletVector()
        {
            throw new NotImplementedException();
        }
    }
}
