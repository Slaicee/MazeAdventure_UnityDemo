using System.Collections.Generic;
using UnityEngine;

public class MazeGenerator : MonoBehaviour
{
    [Header("迷宫参数")]
    public int width = 21;          // 奇数
    public int height = 21;         // 奇数
    public float wallHeight = 2f;   // 墙体高度

    [Header("场景玩家引用")]
    public GameObject player;

    [Header("预制体")]
    public GameObject wallPrefab;    // 墙体预制件
    public Material groundMaterial;  // 地面材质
    

    [Header("出口")]
    public GameObject exitPrefab;

    private int[,] maze;             // 0 通路, 1 墙壁
    private GameObject mazeParent;   // 迷宫父对象

    void Start()
    {
        GenerateMaze();
        SpawnPlayer();
        SpawnExit();
    }

    //迷宫生成
    void GenerateMaze()
    {
        // 清空旧迷宫
        if (mazeParent != null) Destroy(mazeParent);
        mazeParent = new GameObject("MazeParent");

        maze = new int[width, height];

        // 初始化为全墙
        for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
                maze[x, y] = 1;

        // 从 (1,1) 开始挖通迷宫
        Dig(1, 1);

        // 生成墙体
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (maze[x, y] == 1)
                {
                    Vector3 pos = new Vector3(x, wallHeight / 2f, y);
                    GameObject wall = Instantiate(wallPrefab, pos, Quaternion.identity, mazeParent.transform);
                    wall.transform.localScale = new Vector3(1, wallHeight, 1); // 高度可调
                }
            }
        }

        // 生成地面
        GenerateGround();
    }

    void Dig(int x, int y)
    {
        maze[x, y] = 0;

        // 四个方向随机
        int[] dirs = { 0, 1, 2, 3 };
        System.Random rand = new System.Random();
        for (int i = 0; i < dirs.Length; i++)
        {
            int j = rand.Next(i, dirs.Length);
            (dirs[i], dirs[j]) = (dirs[j], dirs[i]);
        }

        foreach (int dir in dirs)
        {
            int dx = 0, dy = 0;
            switch (dir)
            {
                case 0: dx = 0; dy = 2; break;   // 上
                case 1: dx = 0; dy = -2; break;  // 下
                case 2: dx = 2; dy = 0; break;   // 右
                case 3: dx = -2; dy = 0; break;  // 左
            }

            int nx = x + dx;
            int ny = y + dy;

            if (nx > 0 && nx < width - 1 && ny > 0 && ny < height - 1 && maze[nx, ny] == 1)
            {
                maze[x + dx / 2, y + dy / 2] = 0;
                Dig(nx, ny);
            }
        }
    }

    void GenerateGround()
    {
        GameObject ground = GameObject.CreatePrimitive(PrimitiveType.Plane);
        ground.name = "Ground";
        ground.transform.position = new Vector3(width / 2f - 0.5f, 0f, height / 2f - 0.5f);
        ground.transform.localScale = new Vector3(width / 10f, 1, height / 10f);

        if (groundMaterial != null)
            ground.GetComponent<Renderer>().material = groundMaterial;

        ground.transform.parent = mazeParent.transform;
    }

    //玩家生成
    void SpawnPlayer()
    {
        if (player == null)
        {
            Debug.LogWarning("请在 Inspector 中指定 Player 对象");
            return;
        }

        int startX = 1;
        int startY = 1;

        if (maze[startX, startY] != 0)
        {
            Debug.LogWarning("[1,1] 不是空格，无法生成玩家");
            return;
        }

        // 获取 Cube 高度（scale.y）并计算 Y 坐标
        float cubeHeight = player.transform.localScale.y;
        float cubeHalfHeight = cubeHeight / 2f;

        // X/Z 偏移，避免卡墙
        float offset = 0.01f;

        // 设置玩家位置
        Vector3 startPos = new Vector3(startX + offset, cubeHalfHeight, startY + offset);
        player.transform.position = startPos;
    }

    //BFS函数
    (Vector2Int farthestCell, int distance) FindFarthestCell()
    {
        Queue<Vector2Int> q = new Queue<Vector2Int>();
        bool[,] visited = new bool[width, height];
        int[,] dist = new int[width, height];

        q.Enqueue(new Vector2Int(1, 1));
        visited[1, 1] = true;
        dist[1, 1] = 0;

        Vector2Int farthest = new Vector2Int(1, 1);

        while (q.Count > 0)
        {
            var cur = q.Dequeue();
            int x = cur.x, y = cur.y;

            foreach (var d in new (int, int)[] { (1, 0), (-1, 0), (0, 1), (0, -1) })
            {
                int nx = x + d.Item1;
                int ny = y + d.Item2;

                if (nx > 0 && nx < width && ny > 0 && ny < height && !visited[nx, ny] && maze[nx, ny] == 0)
                {
                    visited[nx, ny] = true;
                    dist[nx, ny] = dist[x, y] + 1;
                    q.Enqueue(new Vector2Int(nx, ny));

                    if (dist[nx, ny] > dist[farthest.x, farthest.y])
                        farthest = new Vector2Int(nx, ny);
                }
            }
        }
        return (farthest, dist[farthest.x, farthest.y]);
    }
    //生成出口
    void SpawnExit()
    {
        if (exitPrefab == null)
        {
            Debug.LogWarning("未设置出口预制体（exitPrefab）");
            return;
        }

        var (exitCell, distance) = FindFarthestCell();
        float offset = 0.01f;
        float heightOffset = exitPrefab.transform.localScale.y / 2f;

        Vector3 pos = new Vector3(exitCell.x + offset, heightOffset, exitCell.y + offset);
        Instantiate(exitPrefab, pos, Quaternion.identity, mazeParent.transform);

        Debug.Log($"出口位置：({exitCell.x},{exitCell.y})，距离起点 {distance}");
    }

}
