using UnityEngine;

public class MazeGenerator : MonoBehaviour
{
    [Header("迷宫参数")]
    public int width = 51;          // 奇数
    public int height = 51;         // 奇数
    public float wallHeight = 2f;   // 墙体高度

    [Header("预制体")]
    public GameObject wallPrefab;    // 墙体预制件
    public Material groundMaterial;  // 地面材质
    public GameObject playerPrefab;  // 玩家 Cube

    private int[,] maze;             // 0 通路, 1 墙壁
    private GameObject mazeParent;   // 迷宫父对象
    private GameObject player;

    void Start()
    {
        GenerateMaze();
        SpawnPlayer();
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
        if (playerPrefab == null) return;

        int startX = 1;
        int startY = 1;

        if (maze[startX, startY] != 0)
        {
            Debug.LogWarning("[1,1] 不是空格，无法生成玩家");
            return;
        }

        // 获取 Cube 高度（scale.y）并计算 Y 坐标
        float cubeHeight = playerPrefab.transform.localScale.y;
        float cubeHalfHeight = cubeHeight / 2f;

        // X/Z 偏移，避免与墙体重叠
        float offset = 0.01f;

        Vector3 startPos = new Vector3(startX + offset, cubeHalfHeight, startY + offset);

        player = Instantiate(playerPrefab, startPos, Quaternion.identity);
    }
}
