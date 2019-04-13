using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using D = System.Diagnostics;

public class MazeGenerator : MonoBehaviour
{
    private List<char[]> maze;

    public int rows, columns;

    protected void Start()
    {
        maze = new List<char[]>();
        for (int i = 0; i < this.rows * 2 + 1; i++)
        {
            maze.Add(new char[this.columns * 2 + 1]);
        }
        D.Debug.WriteLine("HJFEIJ");
        generateMaze();
        print(maze);
    }

    public void generateMaze()
    {

        List<Point> frontier = new List<Point>();
        List<Point> connectedNodes = new List<Point>();
        //@ is the start point of the maze and # is the end of the maze
        //the start point is (0, 0)
        // implementing using Prims

        set();
        Point origin = new Point(rows / 2, columns / 2, rows, columns);
        connectedNodes.Add(origin);

        findFrontier(frontier, connectedNodes);
        while (!(frontier.Count == 0))
        {
            D.Debug.WriteLine(frontier.Count);
            Point next = frontier[(int)((Random.Range(0, 1) * frontier.Count))];
            addPoint(next, connectedNodes);
            frontier.Remove(next);
            connectedNodes.Add(next);
            findFrontier(frontier, connectedNodes);
        }

    }

    public List<char[]> getMaze()
    {
        return maze;
    }
    private void findFrontier(List<Point> frontier, List<Point> connected)
    {

        foreach (Point pt in connected)
        {
            Point upper = new Point(pt.y - 2, pt.x, rows, columns);
            Point lower = new Point(pt.y + 2, pt.x, rows, columns);
            Point right = new Point(pt.y, pt.x + 2, rows, columns);
            Point left = new Point(pt.y, pt.x - 2, rows, columns);
            if (!pt.isonTopBorder() && connected.IndexOf(upper) == -1 && frontier.IndexOf(upper) == -1)
            {
                frontier.Add(upper);
            }
            if (!pt.isonBottomBorder() && connected.IndexOf(lower) == -1 && frontier.IndexOf(lower) == -1)
            {
                frontier.Add(lower);
            }
            if (!pt.isonLeftBorder() && connected.IndexOf(left) == -1 && frontier.IndexOf(left) == -1)
            {
                frontier.Add(left);
            }
            if (!pt.isonRightBorder() && connected.IndexOf(right) == -1 && frontier.IndexOf(right) == -1)
            {
                frontier.Add(right);
            }
        }
    }


    private void addPoint(Point point, List<Point> connected)
    {
        foreach (Point p in connected)
        {
            if (nearDistance(point, p))
            {
                if (point.y - p.y == 2)
                {
                    maze[p.y + 1][p.x] = ' ';
                }
                else if (p.y - point.y == 2)
                {
                    maze[point.y + 1][point.x] = ' ';
                }
                else if (point.x - p.x == 2)
                {
                    maze[point.y][p.x + 1] = ' ';
                }
                else if (p.x - point.x == 2)
                {
                    maze[point.y][point.x + 1] = ' ';
                }
                break;
            }
        }
    }
    private void set()
    {
        for (int i = 0; i < maze.Count; i++)
        {
            for (int j = 0; j < maze[i].Length; j++)
            {
                if ((i % 2 == 0 && j % 2 == 1) || (i % 2 == 1 && j % 2 == 0))
                {
                    maze[i][j] = '*';
                }
            }
        }
        for (int i = 0; i < maze.Count; i++)
        {
            for (int j = 0; j < maze[i].Length; j++)
            {
                if (i % 2 == 0 && j % 2 == 0)
                {
                    maze[i][j] = ' ';
                }
            }
        }
        maze[0][0] = '@';
        maze[(int)(Random.Range(0, 1) * (rows - 1) + 1) * 2][(int)(Random.Range(0, 1) * (columns + 1)) * 2] = '#';
    }
    private bool nearDistance(Point p1, Point p2)
    {
        if (Mathf.Abs(p1.x - p2.x) == 2 && p1.y == p2.y)
        {
            return true;
        }
        else if (Mathf.Abs(p1.y - p2.y) == 2 && p1.x == p2.x)
        {
            return true;
        }

        return false;
    }

    class Point
    {
        public int x, y, rows, columns;
        public Point(int y, int x, int rows, int columns)
        {
            this.x = x;
            this.y = y;
            this.rows = rows * 2;
            this.columns = columns * 2;
        }
        public bool isonTopBorder()
        {
            return y <= 0;
        }
        public bool isonBottomBorder()
        {
            return y >= rows - 1;
        }
        public bool isonLeftBorder()
        {
            return x <= 0;
        }
        public bool isonRightBorder()
        {
            return x >= columns - 1;
        }

        public string toString()
        {
            return "(" + x + "," + y + ")";
        }

    }



}


