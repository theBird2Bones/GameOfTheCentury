﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Game
{
    public enum MonsterType {
        fatMonster,
        normalMonster,
        fastMonster,
    }
    public class Monster : DynamicObject {
        public int Damage { get; private set; }
        public MonsterType MonsterType;
        private readonly Dictionary<MonsterType, string> _mosterType = new Dictionary<MonsterType, string>()
        {
            {MonsterType.fatMonster, "fatMonster.png"},
            {MonsterType.normalMonster, "normalMonster.png"},
            {MonsterType.fastMonster, "fastMonster.png"}
        };
        public Monster(int health, int speed, int jumpHeight,int damage, Point location, Size size, MonsterType monsterType)
            : base(health, speed, jumpHeight, location, size)
        {
            Tag = "monster";
            Image = new Bitmap(PathToImages + _mosterType[monsterType]);
            MonsterType = monsterType;
            Damage = damage;
            Visible = false;
        }

        public void MoveToHero(GameModel game, int travelSpeed) {
            if (this.Left >= game.Hero.Right) 
                this.Left -= travelSpeed;
            if (this.Right <= game.Hero.Left)
                this.Left += travelSpeed;
        }
        
        public bool ActInConflict(Hero hero, bool conflictObject)
        {
            if (conflictObject)
            {
                this.Health -= hero.Weapon.Damage;
                return this.Health > 0;
            }
            
            hero.Health -= this.Damage;
            return hero.Health > 0;
            
        }
        
    }
}
