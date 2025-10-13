using UnityEngine;

public class MazeGenerator : MonoBehaviour
{
    public int width = 15;   // �Թ����
    public int height = 15;  // �Թ��߶�
    public GameObject wallPrefab; // ǽ��Ԥ�Ƽ���Cube��

    int[,] maze; // 0 ͨ·, 1 ǽ��

    void Start()
    {
        GenerateMazeData();
        BuildMaze();
    }

    // Step 1������һ���򵥵��Թ�����
    void GenerateMazeData()
    {
        maze = new int[width, height];

        // ��ʼ��Ϊǽ
        for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
                maze[x, y] = 1;

        // �� (1,1) ��ʼ��ͨ·
        Dig(1, 1);
    }

    // �ݹ���ͨ·��������ȣ�
    void Dig(int x, int y)
    {
        maze[x, y] = 0;

        // �ĸ������������
        int[] dirs = { 0, 1, 2, 3 };
        System.Random rand = new System.Random();
        for (int i = 0; i < dirs.Length; i++)
        {
            int j = rand.Next(i, dirs.Length);
            (dirs[i], dirs[j]) = (dirs[j], dirs[i]);//ϴ���㷨
        }

        foreach (int dir in dirs)
        {
            int dx = 0, dy = 0;
            switch (dir)
            {
                case 0: dx = 0; dy = 1; break;   // ��
                case 1: dx = 1; dy = 0; break;   // ��
                case 2: dx = 0; dy = -1; break;  // ��
                case 3: dx = -1; dy = 0; break;  // ��
            }

            int nx = x + dx * 2;
            int ny = y + dy * 2;

            // �ж��Ƿ�Խ����δ��ͨ
            if (nx > 0 && ny > 0 && nx < width - 1 && ny < height - 1 && maze[nx, ny] == 1)
            {
                // ��ͨ�м��ǽ
                maze[x + dx, y + dy] = 0;
                Dig(nx, ny);
            }
        }
    }

    // Step 2�����Թ����ݱ��������
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
