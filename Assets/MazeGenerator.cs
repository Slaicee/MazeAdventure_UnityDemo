using UnityEngine;

public class MazeGenerator : MonoBehaviour
{
    public int width = 15;   // 迷宫宽度
    public int height = 15;  // 迷宫高度
    public GameObject wallPrefab; // 墙体预制件（Cube）

    int[,] maze; // 0 通路, 1 墙壁

    void Start()
    {
        GenerateMazeData();
        BuildMaze();
    }

    // Step 1：生成一个简单的迷宫数据
    void GenerateMazeData()
    {
        maze = new int[width, height];

        // 初始化为墙
        for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
                maze[x, y] = 1;

        // 从 (1,1) 开始挖通路
        Dig(1, 1);
    }

    // 递归挖通路（深度优先）
    void Dig(int x, int y)
    {
        maze[x, y] = 0;

        // 四个方向随机打乱
        int[] dirs = { 0, 1, 2, 3 };
        System.Random rand = new System.Random();
        for (int i = 0; i < dirs.Length; i++)
        {
            int j = rand.Next(i, dirs.Length);
            (dirs[i], dirs[j]) = (dirs[j], dirs[i]);//洗牌算法
        }

        foreach (int dir in dirs)
        {
            int dx = 0, dy = 0;
            switch (dir)
            {
                case 0: dx = 0; dy = 1; break;   // 上
                case 1: dx = 1; dy = 0; break;   // 右
                case 2: dx = 0; dy = -1; break;  // 下
                case 3: dx = -1; dy = 0; break;  // 左
            }

            int nx = x + dx * 2;
            int ny = y + dy * 2;

            // 判断是否越界且未挖通
            if (nx > 0 && ny > 0 && nx < width - 1 && ny < height - 1 && maze[nx, ny] == 1)
            {
                // 打通中间的墙
                maze[x + dx, y + dy] = 0;
                Dig(nx, ny);
            }
        }
    }

    // Step 2：把迷宫数据变成立方体
    void BuildMaze()
    {
        GameObject mazeParent = new GameObject("Maze");

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (maze[x, y] == 1)
                {
                    Vector3 pos = new Vector3(x, 0.5f, y);
                    GameObject wall = Instantiate(wallPrefab, pos, Quaternion.identity);
                    wall.transform.parent = mazeParent.transform;
                }
            }
        }
    }
}
