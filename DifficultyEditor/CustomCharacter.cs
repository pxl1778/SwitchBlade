﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DifficultyEditor
{
    enum faction
    {
        Good, Tribal, Rich, Thief
    }

    class CustomCharacter
    {
        private faction currentFaction;
        private string name;
        //private int health, strength, speed;
        private double aggression, defense, ability, cowardice;

        public faction Faction
        {
            get { return currentFaction; }
            set { currentFaction = value; }
        }
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        public double Aggression
        {
            get { return aggression; }
            set { aggression = value; }
        }
        public double Defense
        {
            get { return defense; }
            set { defense = value; }
        }
        public double Ability
        {
            get { return ability; }
            set { ability = value; }
        }
        public double Cowardice
        {
            get { return cowardice; }
            set { cowardice = value; }
        }

        public CustomCharacter(faction currentFaction, string name, double aggression, double defense, double ability, double cowardice)
        {
            this.currentFaction = currentFaction;
            this.name = name;
            this.aggression = aggression;
            this.defense = defense;
            this.ability = ability;
            this.cowardice = cowardice;
        }
    }
}