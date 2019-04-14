using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour
{
    public int rows, columns;
    public MazeGenerator generator;
    public Tilemap tiles;
    public Tile wall, player, goal;
    public Camera camera;
    private List<char[]> curMaze;

    private MazeGenerator.Point playerPos;

    private bool canMove;
    void Awake()
    {
        setCamera();
        generator = new MazeGenerator(rows, columns);
        newMaze();
    }

    void newMaze() {
        generator.generateMaze();
        curMaze = generator.getMaze();
        drawMaze();
    }

    void setCamera() {

        float ratio = (float) Screen.width/Screen.height;

        if (columns <= rows * ratio)
            camera.GetComponent<Camera>().orthographicSize = rows+1;
        else
            camera.GetComponent<Camera>().orthographicSize = columns/ratio + 1;
        camera.transform.position = new Vector3(columns+0.5f, rows+0.5f, -1);
    }

    void drawMaze() {

        canMove = false;
        tiles.ClearAllTiles();

        for (int row = -1; row < curMaze.Count + 1; row++) {
            for (int col = -1; col < curMaze[0].Length + 1; col++) {

                if (row < 0 || col < 0 || row == curMaze.Count || col == curMaze[0].Length)
                    tiles.SetTile(new Vector3Int(col+1, curMaze.Count-row, 0), wall);
                else if (row % 2 == 1 && col % 2 == 1)
                    tiles.SetTile(new Vector3Int(col+1, curMaze.Count-row, 0), wall);
                else if (curMaze[row][col] == '*')
                    tiles.SetTile(new Vector3Int(col+1, curMaze.Count-row, 0), wall);
                else if (curMaze[row][col] == '#')
                    tiles.SetTile(new Vector3Int(col+1, curMaze.Count-row, 0), goal);
                else if (curMaze[row][col] == '@') {
                    tiles.SetTile(new Vector3Int(col+1, curMaze.Count-row, 0), player);
                    playerPos = new MazeGenerator.Point(row, col, rows, columns);
                }
            }
        }
    }

    void moveRight() {
        if (!playerPos.isonRightBorder())
            move(1, 0);
    }
    void moveLeft() {
        if (!playerPos.isonLeftBorder())
            move(-1, 0);
    }
    void moveUp() {
        if (!playerPos.isonTopBorder())
            move(0, -1);
    }
    void moveDown() {
        if (!playerPos.isonBottomBorder())
            move(0, 1);
    }
    void move(int dx, int dy) {
        if (!canMove)
            return;
        int x = playerPos.x;
        int y = playerPos.y;

        if (curMaze[y+dy][x+dx] == '#') {
            newMaze();
            return;
        }

        if (curMaze[y+dy][x+dx] != ' ')
            return;

        curMaze[y][x] = ' ';
        curMaze[y+dy][x+dx] = '@';
        drawMaze();
    }

    void Update()
    {
        int horizontal = 0, vertical = 0;

        horizontal = (int) Input.GetAxisRaw("Horizontal");
        vertical = (int) Input.GetAxisRaw("Vertical");

        if (horizontal > 0)
            moveRight();
        else if (horizontal < 0)
            moveLeft();
        else if (vertical > 0)
            moveUp();
        else if (vertical < 0)
            moveDown();
        else
            canMove = true;
    }
}
