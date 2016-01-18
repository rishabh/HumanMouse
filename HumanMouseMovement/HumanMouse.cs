using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace HumanMouseMovement {
    public class HumanMouse {
        /// <summary>
        /// </summary>
        public Action<double, double> MoveMouseAction =
            (x, y) => { Cursor.Position = new Point(Convert.ToInt32(x), Convert.ToInt32(y)); };

        /// <summary>
        /// </summary>
        public HumanMouse() {}

        /// <summary>
        /// </summary>
        /// <param name="mouseSpeed"></param>
        public HumanMouse(int mouseSpeed) {
            MouseSpeed = mouseSpeed;
        }

        /// <summary>
        /// </summary>
        /// <param name="moveMouseAction"></param>
        public HumanMouse(Action<double, double> moveMouseAction) {
            MoveMouseAction = moveMouseAction;
        }

        /// <summary>
        /// </summary>
        /// <param name="mouseSpeed"></param>
        /// <param name="moveMouseAction"></param>
        public HumanMouse(int mouseSpeed, Action<double, double> moveMouseAction) {
            MouseSpeed = mouseSpeed;
            MoveMouseAction = moveMouseAction;
        }

        /// <summary>
        /// </summary>
        public double MouseSpeed { get; set; } = 100;

        /// <summary>
        /// </summary>
        public double Gravity { get; set; } = 7;

        /// <summary>
        /// </summary>
        public double Wind { get; set; } = 2;

        /// <summary>
        /// </summary>
        public double MarginOfError { get; set; } = 2;

        /// <summary>
        /// </summary>
        public double TargetArea { get; set; } = 10;

        /// <summary>
        /// </summary>
        public double MaxStep { get; set; } = 6;

        /// <summary>
        /// </summary>
        /// <param name="xStart"></param>
        /// <param name="yStart"></param>
        /// <param name="xEnd"></param>
        /// <param name="yEnd"></param>
        public void HumanMovement(double xStart, double yStart, double xEnd, double yEnd) {
            var r = new Random();

            var wind = Wind;
            double sqrt2 = Math.Sqrt(2), sqrt3 = Math.Sqrt(3), sqrt5 = Math.Sqrt(5);
            var tDist = Hypot(xStart - xEnd, yStart - yEnd);

            while (Hypot(xStart - xEnd, yStart - yEnd) > MarginOfError) {
                double veloX = 0, veloY = 0;
                double windX = 0, windY = 0;
                var dist = Math.Max(Hypot(xStart - xEnd, yStart - yEnd), 1);
                wind = Math.Min(wind, dist);

                var d = Math.Round((Math.Round(tDist)*0.3)/7);
                d = Math.Min(25, Math.Max(d, 5));

                var rCnc = (int) Math.Round(r.NextDouble()*6);
                if (rCnc == 1) {
                    d = r.NextDouble() + 2;
                }

                if (dist >= TargetArea) {
                    windX = windX/sqrt3 + (r.NextDouble()*(Math.Round(wind)*2 + 1) - wind*0.9)/sqrt5;
                    windY = windY/sqrt3 + (r.NextDouble()*(Math.Round(wind)*2 + 1) - wind*0.9)/sqrt5;
                } else {
                    windX = windX/sqrt2;
                    windY = windY/sqrt2;
                    MaxStep = Math.Min(d, Math.Round(dist));
                }
                veloX += windX;
                veloY += windY;

                veloX += Gravity*(xEnd - xStart)/dist;
                veloY += Gravity*(yEnd - yStart)/dist;

                if (Hypot(veloX, veloY) > MaxStep) {
                    var randomDist = MaxStep/2.0 + (r.NextDouble()*(Math.Round(MaxStep)/2.0));
                    var veloMag = Hypot(veloX, veloY);
                    veloX = veloX/veloMag*randomDist;
                    veloY = veloY/veloMag*randomDist;
                }

                var lastX = (int) Math.Round(xStart);
                var lastY = (int) Math.Round(yStart);

                xStart += veloX;
                yStart += veloY;

                if (lastX != (int) Math.Round(xStart) || lastY != (int) Math.Round(yStart)) {
                    MoveMouseAction(xStart, yStart);
                }

                var w = r.NextDouble()*Math.Round(100/MouseSpeed)*6;
                w = Math.Round(Math.Max(w, 5)*0.9);

                Thread.Sleep(Convert.ToInt32(w));
            }

            if ((int) Math.Round(xEnd) != (int) Math.Round(xStart) || (int) Math.Round(yStart) != (int) Math.Round(yEnd)) {
                MoveMouseAction(xEnd, yEnd);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        private double Hypot(double a, double b) {
            return Math.Sqrt(a*a + b*b);
        }
    }
}
