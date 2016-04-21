using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameEngine
{
    public class TimerComponent : IComponent
    {
        public double CurrentTime { get; set;}
        public double TargetTime{get;set;}
        public bool TimerDone {get; set;}
        

        public TimerComponent(double targetTime)
        {
            TargetTime = targetTime;
            TimerDone = false;
        }
    }
}
