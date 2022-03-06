// import java.util.LinkedList;
// import java.lang.Math;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Critter {
    private static int width = 20;
    private static int height = 20;

    private int FloorMod(int a, int b)
    {
        var res = a % b;
        if (res < 0) res += b;
        return res;
    }

    public int x, y, size, consumed, hydrated, stepSize;
    public int[] target;
  // color targetColor;
  // color c;
  
   public Critter() : this(Random.Range(0, width), Random.Range(0, height), 10)
    {
    }

    public Critter(int x, int y, int size)
    {
        this.x = x;
        this.y = y;
        this.size = size;
        this.consumed = 255;
        this.hydrated = 255;
        this.stepSize = 5;
        target = new int[] { x, y };
        //targetColor = color(0);
    }

    public bool tick(LinkedList<int[]> foodList, LinkedList<int[]> waterList, LinkedList<Critter> mates)
    {
        consumed -= 1;
        hydrated -= 1;
        if (consumed == 0 || hydrated == 0)
        {
            return false;
        }
        findTarget(foodList, waterList, mates);
        step();

        return true;
    }

    public void eat(int foodAmount)
    {
        consumed += foodAmount;// min(255, consumed+foodAmount);
    }

    public void drink(int waterAmount)
    {
        hydrated += waterAmount;// min(255, hydrated+waterAmount);
    }

    //public void draw(bool drawTarget)
    //{
    //    c = color(255, consumed, hydrated);
    //    fill(c);
    //    stroke(targetColor);
    //    ellipse(x, y, size, size);

    //    if (drawTarget)
    //    {
    //        int[] closeT = closerCoords(target);
    //        if (closeT[0] == target[0] && closeT[1] == target[1])
    //        {
    //            line(x, y, target[0], target[1]);
    //        }
    //        else
    //        {
    //            line(x, y, closeT[0], closeT[1]);
    //        }
    //    }
    //    stroke(0);
    //}

    private int[] closerCoords(int[] o)
    {
        int oX = o[0];
        int oY = o[1];
        if (x - oX > width / 2)
        {
            oX += width;
        }
        else if (oX - x > width / 2)
        {
            oX -= width;
        }
        if (y - oY > height / 2)
        {
            oY += height;
        }
        else if (oY - y > height / 2)
        {
            oY -= height;
        }

        return new int[] { oX, oY };
    }

    private float distanceTo(int[] point)
    {
        int[] closeP = closerCoords(point);

        return Mathf.Sqrt(Mathf.Pow(x - closeP[0], 2) + Mathf.Pow(y - closeP[1], 2));
    }

    private void findTarget(LinkedList<int[]> foodList, LinkedList<int[]> waterList, LinkedList<Critter> mates)
    {
        if (consumed > 230 && hydrated > 230 && mates.Count > 1)
        {
            // plenty food, plenty water, try to mate
            Critter minCritter = null;
            float minDist = -1;
            foreach (Critter other in mates)
            {
                if (other != this)
                {
                    if (minDist == -1)
                    {
                        minDist = distanceTo(new int[] { other.x, other.y });
                        minCritter = other;
                    }
                    else
                    {
                        float dist = distanceTo(new int[] { other.x, other.y });
                        if (dist < minDist)
                        {
                            minDist = dist;
                            minCritter = other;
                        }
                    }
                }
            }

            if (minCritter != null)
            {
                //targetColor = minCritter.c;
                target = new int[] {
             minCritter.x,
             minCritter.y
           };
            }
        }
        else if (foodList.Count == 0 && waterList.Count != 0)
        {
            // try to drink
            //targetColor = waterColor;
            target = findClosest(waterList);
        }
        else if (waterList.Count == 0 && foodList.Count != 0)
        {
            // try to eat
            //targetColor = foodColor;
            target = findClosest(foodList);
        }
        else if (foodList.Count != 0 && waterList.Count != 0)
        {
            // do whatever is the most worthwhile
            int[] targetFood = findClosest(foodList);
            int[] targetWater = findClosest(waterList);
            if (distanceTo(targetFood) / consumed < distanceTo(targetWater) / hydrated)
            {
                //targetColor = foodColor;
                target = targetFood;
            }
            else
            {
                //targetColor = waterColor;
                target = targetWater;
            }
        }
    }

    private int[] findClosest(LinkedList<int[]> resources)
    {
        int[] closest = null;
        float minDist = -1;
        foreach (int[] coord in resources)
        {
            if (minDist == -1)
            {
                minDist = distanceTo(coord);
                closest = coord;
            }
            else
            {
                float dist = distanceTo(coord);
                if (dist < minDist)
                {
                    minDist = dist;
                    closest = coord;
                }
            }
        }
        if (closest != null)
        {
            return closest;
        }
        return new int[] { x, y };
    }

    private void step()
    {
        int[] tC = closerCoords(target);
        if (tC[0] == x)
        {
            if (tC[1] > y)
            {
                y += stepSize;
            }
            else
            {
                y -= stepSize;
            }
        }
        else if (tC[1] == y)
        {
            if (tC[0] > x)
            {
                x += stepSize;
            }
            else
            {
                x -= stepSize;
            }
        }
        else
        {
            float theta = Mathf.Atan2((tC[1] - y), (tC[0] - x));
            int dX = (int)(stepSize * Mathf.Cos(theta));
            int dY = (int)(stepSize * Mathf.Sin(theta));
            x = FloorMod((x + dX), width);
            y = FloorMod((y + dY), height);
        }
    }
}
