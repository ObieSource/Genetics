// import java.util.LinkedList;
// import java.util.Iterator;

using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class Genetics {
    private static int width = 20;
    private static int height = 20;

    // PrintWriter output;
    int dataTick = 50;

    int populationSize = 50;
    LinkedList<Critter> population;

    LinkedList<int[]> food;
    int foodInitAmount = 300;
    public static Color foodColor = new Color(0, 195, 39);
    LinkedList<int[]> water;
    int waterInitAmount = 300;
    public static Color waterColor = new Color(0, 39, 195);

    int ticks = 0;

    public void setup()
    {
        //background(117);
        //size(800, 500);
        initializeMap();
        initializePopulation();

        //output = createWriter("data.csv");
        //output.println("t, Population, Food, Water");
    }

    public void draw()
    {
        //background(117);
        tickResources();
        tickPopulation();
        //drawResources();
        //drawPopulation();

        if (ticks % dataTick == 0)
        {
            string strOut = "";
            strOut += (ticks / dataTick);
            Debug.Log(strOut);
            strOut += ", " + population.Count;
            strOut += ", " + food.Count;
            strOut += ", " + water.Count;
            //output.println(strOut);
        }
        ticks += 1;
    }

    //void keyPressed()
    //{
    //    if (keyCode == ESC)
    //    {
    //        output.flush();
    //        output.close();
    //        exit();
    //    }
    //}

    public void initializeMap()
    {
        food = new LinkedList<int[]>();
        water = new LinkedList<int[]>();
        for (int i = 0; i < foodInitAmount; i++)
        {
            food.AddLast(new int[] { Random.Range(0, width), Random.Range(0, height) });
        }
        for (int i = 0; i < waterInitAmount; i++)
        {
            water.AddLast(new int[] { Random.Range(0, width), Random.Range(0, height) });
        }
    }

    public void initializePopulation()
    {
        population = new LinkedList<Critter>();
        for (int i = 0; i < populationSize; i++)
        {
            population.AddLast(new Critter());
        }
    }

    public void tickResources()
    {
        for (int i = 0; i < 5; i++)
        {
            water.AddLast(new int[] { Random.Range(0, width), Random.Range(0, height) });
        }
        for (int i = 0; i < 4; i++)
        {
            food.AddLast(new int[] { Random.Range(0, width), Random.Range(0, height) });
        }
    }

    public void tickPopulation()
    {
        LinkedList<int[]> babies = new LinkedList<int[]>();
        IEnumerator<Critter> critIt = population.GetEnumerator();
        while (critIt.MoveNext())
        {
            Critter c = critIt.Current;
            if (c.tick(food, water, population))
            {
                IEnumerator<int[]> foodIt = food.GetEnumerator();
                while (foodIt.MoveNext())
                {
                    int[] coord = foodIt.Current;
                    if (distance(coord[0], coord[1], c.x, c.y) < 5)
                    {
                        //TODO
                        //foodIt.remove();
                        c.eat(10);
                    }
                }

                IEnumerator<int[]> waterIt = water.GetEnumerator();
                while (waterIt.MoveNext())
                {
                    int[] coord = waterIt.Current;
                    if (distance(coord[0], coord[1], c.x, c.y) < 5)
                    {
                        ////TODO
                        //waterIt.remove();
                        c.drink(10);
                    }
                }

                IEnumerator<Critter> critIt2 = population.GetEnumerator();
                if (c.consumed > 230 && c.hydrated > 230)
                {
                    while (critIt2.MoveNext())
                    {
                        Critter o = critIt2.Current;
                        if (o != c && o.consumed > 230 && o.hydrated > 230 && distance(o.x, o.y, c.x, c.y) < 5)
                        {
                            c.consumed -= 100;
                            c.hydrated -= 100;
                            o.consumed -= 100;
                            o.hydrated -= 100;
                            babies.AddLast(new int[] { c.x, c.y });
                            break;
                        }
                    }
                }
            }
            else
            {
                food.AddLast(new int[] { c.x, c.y });
                //critIt.remove();
            }
        }

        foreach (int[] baby in babies)
        {
            population.AddLast(new Critter(baby[0], baby[1], 5));
        }
    }

    //void drawPopulation()
    //{
    //    foreach (Critter c in population)
    //    {
    //        c.draw(true);
    //    }
    //}

    //void drawResources()
    //{
    //    fill(foodColor);
    //    foreach (int[] coord in food)
    //    {
    //        ellipse(coord[0], coord[1], 5, 5);
    //    }
    //    fill(waterColor);
    //    foreach (int[] coord in water)
    //    {
    //        ellipse(coord[0], coord[1], 5, 5);
    //    }
    //}


    private float distance(float x1, float y1, float x2, float y2)
    {
        return Mathf.Sqrt(Mathf.Pow(x1 - x2, 2) + Mathf.Pow(y1 - y2, 2));
    }
}