using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace TankFighter
{
    public class KeyBoardMap
    {

        Keys[] player1 = new Keys[10] { Keys.W, Keys.S, Keys.A, Keys.D, Keys.Q, Keys.E, Keys.Z, Keys.T, Keys.F, Keys.G };
        Keys[] player2 = new Keys[10] { Keys.Up, Keys.Down, Keys.Left, Keys.Right, Keys.NumPad1, Keys.NumPad2, Keys.RightControl, Keys.O, Keys.K, Keys.L };
        int count = 0;

        public KeyBoardMap()
        {

        }

        public void setKey(int player, int function, Keys key)
        {
            if (player == 1)
            {
                for (int i = 0; i < 10; i++)
                {
                    if (key != player1[i])
                        count++;
                    if (key != player2[i])
                        count++;

                }
                if (count == 20)
                    player1[function] = key;
                else
                    count = 0;

            }
            else
            {
                for (int i = 0; i < 10; i++)
                {
                    if (key != player1[i])
                        count++;
                    if (key != player2[i])
                        count++;

                }
                if (count == 20)
                    player2[function] = key;
                else
                    count = 0;
            }
        }

        public Keys getKey(int player, int function)
        {
            if (player == 1)
                return player1[function];
            else
                return player2[function];
        }
    }


}
