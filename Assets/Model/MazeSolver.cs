using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MazeSolver : MonoBehaviour
{
    Dropdown algoA, algoB;
    MazeController mazeController;
    UIController uiController;
    WorldController worldController;
    Maze mazeA, mazeB;

    private bool setup;

    public float defaultCooldownTime = 0.2f;
    public float cooldownTime;
    private float nextFireTime = 0;

    private bool running = false;
    private int stage = 0;

    private int endX, endY;

    Queue<Tile> queue;
    Stack<Tile> stack;
    PriorityQueue<Tile> priorityQueue;
    Tile currentTile;


    void Start()
    {
        cooldownTime = defaultCooldownTime;

        mazeController = GameObject.Find("MazeController").GetComponent<MazeController>();
        uiController = GameObject.Find("UIController").GetComponent<UIController>();
        worldController = GameObject.Find("GameWorld").GetComponent<WorldController>();

        setup = true;
    }

    void Update()
    {
        if (setup)
        {
            algoA = GameObject.Find("selAlgorithmA").GetComponent<Dropdown>();
            mazeA = mazeController.mazeData[0];
            if (mazeController.Compare)
            {
                algoB = GameObject.Find("selAlgorithmB").GetComponent<Dropdown>();
                mazeB = mazeController.mazeData[1];
            }

            setup = false;
        }

        if (Time.time > nextFireTime && running)
        {
            if (this.name == "Maze 1")
            {
                worldController.mazeRunning1 = true;

                switch (algoA.value)
                {
                    case 0:
                        print("Running BFS... ");
                        BFS(mazeController.mazeData[0]);
                        nextFireTime = Time.time + cooldownTime;
                        return;
                    case 1:
                        print("Running DFS...");
                        DFS(mazeA);
                        nextFireTime = Time.time + cooldownTime;
                        return;
                    case 2:
                        print("Running Dijkstra...");
                        Dijkstra(mazeA);
                        nextFireTime = Time.time + cooldownTime;
                        return;
                    case 3:
                        print("Running A*...");
                        AStar(mazeA);
                        nextFireTime = Time.time + cooldownTime;
                        return;
                    default:
                        return;
                }
            }
            else if (this.name == "Maze 2")
            {
                worldController.mazeRunning2 = true;

                switch (algoB.value)
                {
                    case 0:
                        print("Running BFS... ");
                        BFS(mazeController.mazeData[1]);
                        nextFireTime = Time.time + cooldownTime;
                        return;
                    case 1:
                        print("Running DFS...");
                        DFS(mazeB);
                        nextFireTime = Time.time + cooldownTime;
                        return;
                    case 2:
                        print("Running Dijkstra...");
                        Dijkstra(mazeB);
                        nextFireTime = Time.time + cooldownTime;
                        return;
                    case 3:
                        print("Running A*...");
                        AStar(mazeB);
                        nextFireTime = Time.time + cooldownTime;
                        return;
                    default:
                        return;
                }
            }
        }

    }

    void BFS(Maze maze)
    {
        switch (stage)
        {
            case 0:
                queue = new Queue<Tile>();

                foreach (Tile t in maze.tiles)
                {
                    if (t.Start)
                    {
                        currentTile = maze.GetTileAt(t.X + 1, t.Y);
                        currentTile.Parent = t;

                        break;
                    }
                }

                currentTile.Visited = true;
                currentTile.Type = Tile.TileType.Current;

                queue.Enqueue(currentTile);

                stage++;

                return;

            case 1:
                currentTile = queue.Dequeue();

                currentTile.Visited = true;
                currentTile.Type = Tile.TileType.Current;

                if (currentTile.End)
                {
                    stage = 3;
                    return;
                }

                stage++;

                return;

            case 2:
                List<Tile> neighbours = UnvisitedNeighbours(currentTile, maze);
                for (int i = 0; i < neighbours.Count; i++)
                {
                    neighbours[i].Visited = true;
                    neighbours[i].Parent = currentTile;
                    queue.Enqueue(neighbours[i]);
                }

                stage--;

                return;

            case 3:
                FinalPath(maze);

                return;
        }
    }


    void DFS(Maze maze)
    {
        switch (stage)
        {
            case 0:
                stack = new Stack<Tile>();

                foreach (Tile t in maze.tiles)
                {
                    if (t.Start)
                    {
                        currentTile = maze.GetTileAt(t.X + 1, t.Y);
                        currentTile.Parent = t;

                        break;
                    }
                }

                currentTile.Visited = true;
                currentTile.Type = Tile.TileType.Current;

                stack.Push(currentTile);

                stage++;

                return;

            case 1:
                currentTile = stack.Pop();

                currentTile.Visited = true;
                currentTile.Type = Tile.TileType.Current;

                if (currentTile.End)
                {
                    stage = 3;
                    break;
                }

                stage++;

                return;

            case 2:
                List<Tile> neighbours = UnvisitedNeighbours(currentTile, maze);
                for (int i = 0; i < neighbours.Count; i++)
                {
                    neighbours[i].Visited = true;
                    neighbours[i].Parent = currentTile;
                    stack.Push(neighbours[i]);
                }

                stage--;

                return;

            case 3:
                FinalPath(maze);

                return;
        }
    }

    void Dijkstra(Maze maze)
    {
        switch (stage)
        {
            case 0:
                priorityQueue = new PriorityQueue<Tile>();

                foreach (Tile t in maze.tiles)
                {
                    if (t.Start)
                    {
                        currentTile = maze.GetTileAt(t.X + 1, t.Y);
                        currentTile.Parent = t;
                        t.Distance = 0;
                        continue;
                    }

                    t.Distance = 100000;
                }

                currentTile.Distance = currentTile.Cost + 1;

                priorityQueue.Enqueue(currentTile);

                stage++;

                return;

            case 1:
                currentTile = priorityQueue.Dequeue();

                currentTile.Visited = true;
                currentTile.Type = Tile.TileType.Current;

                if (currentTile.End)
                {
                    stage = 3;
                    break;
                }

                stage++;

                return;

            case 2:
                List<Tile> neighbours = UnvisitedNeighbours(currentTile, maze);
                print("@MazeSolver/DijkstraCase2: Neighbour block");


                for (int i = 0; i < neighbours.Count; i++)
                {
                    int neigDistance = neighbours[i].Distance + neighbours[i].Cost;
                    int currDistance = currentTile.Distance + neighbours[i].Cost + 1;
                    print("@MazeSolver/DijkstraCase2: neigDistance = " + neigDistance + ", currDistance = " + currDistance);

                    int minDistance = Mathf.Min(neigDistance, currDistance);

                    print("@MazeSolver/DijkstraCase2: MinDistance = " + minDistance + " , neigDistance = " + neigDistance);

                    if (minDistance != neigDistance)
                    {
                        print("@MazeSolver/DijkstraCase2: Entered new parent with Distance = " + currDistance);

                        neighbours[i].Visited = true;
                        neighbours[i].Distance = minDistance;
                        neighbours[i].Parent = currentTile;

                        if (priorityQueue.Contains(neighbours[i]))
                        {
                            priorityQueue.SetPriority(neighbours[i], minDistance);
                            print("Dijkstra - prioQueue containst, setPrio");
                        }
                    }

                    if (!priorityQueue.Contains(neighbours[i]))
                    {
                        priorityQueue.Enqueue(neighbours[i], minDistance);
                        print("Dijkstra - prioQueue not containst, Enqueue");
                    }
                }

                stage--;

                return;

            case 3:
                FinalPath(maze);

                return;
        }
    }

    void AStar(Maze maze)
    {
        switch (stage)
        {
            case 0:
                priorityQueue = new PriorityQueue<Tile>();

                foreach (Tile t in maze.tiles)
                {
                    if (t.End)
                    {
                        endX = t.X;
                        endY = t.Y;
                    }
                }
                foreach (Tile t in maze.tiles)
                {
                    if (t.Start)
                    {
                        currentTile = maze.GetTileAt(t.X + 1, t.Y);
                        currentTile.Parent = t;

                        t.RootDistance = 0;
                        t.ManDistance = Mathf.Abs(endX - t.X) + Mathf.Abs(endY - t.Y);
                        t.Distance = t.RootDistance + t.ManDistance;

                        continue;
                    }

                    t.RootDistance = 100000;
                    t.Distance = 100000;
                    t.ManDistance = Mathf.Abs(endX - t.X) + Mathf.Abs(endY - t.Y);
                }

                currentTile.Visited = true;
                currentTile.RootDistance = currentTile.Cost + 1;
                currentTile.ManDistance = Mathf.Abs(endX - currentTile.X) + Mathf.Abs(endY - currentTile.Y);
                currentTile.Distance = currentTile.RootDistance + currentTile.ManDistance;
                currentTile.Type = Tile.TileType.Current;

                priorityQueue.Enqueue(currentTile);

                stage++;

                return;

            case 1:
                currentTile = priorityQueue.Dequeue();

                currentTile.Visited = true;
                currentTile.Type = Tile.TileType.Current;

                if (currentTile.End)
                {
                    stage = 3;
                    break;
                }

                stage++;

                return;

            case 2:
                List<Tile> neighbours = FindNeighbours(currentTile, maze);

                print("@MazeSolver/A* - Neighbour Block:");
                for (int i = 0; i < neighbours.Count; i++)
                {
                    print("@MazeSolver/A* - Update RootDistance - neigRoot = " + neighbours[i].RootDistance + ", currRoot = " + currentTile.RootDistance);
                    neighbours[i].RootDistance = Mathf.Min(neighbours[i].RootDistance, currentTile.RootDistance + neighbours[i].Cost + 1);
                    int minDistance = Mathf.Min(neighbours[i].Distance, neighbours[i].RootDistance + neighbours[i].ManDistance);

                    print("@MazeSolver/A* - minDistance = " + minDistance + ", comp = " + neighbours[i].Distance);
                    if (minDistance != neighbours[i].Distance)
                    {
                        neighbours[i].Visited = true;
                        neighbours[i].Distance = minDistance;
                        neighbours[i].Parent = currentTile;
                        print("@MazeSolver/A* - New Parent written with distance: " + minDistance);

                        if (priorityQueue.Contains(neighbours[i]))
                        {
                            priorityQueue.SetPriority(neighbours[i], minDistance);
                        }

                    }

                    if (!priorityQueue.Contains(neighbours[i]))
                    {
                        priorityQueue.Enqueue(neighbours[i], minDistance);
                    }
                }

                stage--;

                return;

            case 3:
                FinalPath(maze);

                return;
        }
    }

    void FinalPath(Maze maze)
    {
        int totalDistance = 0;
        foreach (Tile t in maze.tiles)
        {
            if (t.End)
                currentTile = t;
        }
        while (!currentTile.Start)
        {
            totalDistance += currentTile.Cost + 1;
            currentTile.Type = Tile.TileType.Path;
            currentTile = currentTile.Parent;
        }

        stage = 0;
        running = false;
        SaveTime();

        print("@MazeSolver/FinalPath - Total cost + distance = " + totalDistance);

        if (this.name == "Maze 1")
            worldController.mazeRunning1 = false;
        else
            worldController.mazeRunning2 = false;
    }

    List<Tile> UnvisitedNeighbours(Tile currentTile, Maze maze)
    {
        List<Tile> result = new List<Tile>();
        int[,] order = { { 0, 1 }, { 1, 0 }, { 0, -1 }, { -1, 0 } };
        Tile check;


        for (int i = 0; i < order.Length / 2; i++)
        {
            check = maze.GetTileAt(currentTile.X + order[i, 0], currentTile.Y + order[i, 1]);
            print("Neig " + i + ", isVisited: " + check.Visited);
            if (check.Visited == false && (check.Type == Tile.TileType.Floor || check.Type == Tile.TileType.End))
            {
                result.Add(check);
            }
        }

        print("Unvisited neig count: " + result.Count);

        return result;
    }

    List<Tile> FindNeighbours(Tile currentTile, Maze maze)
    {
        List<Tile> result = new List<Tile>();
        int[,] order = { { 0, 1 }, { 1, 0 }, { 0, -1 }, { -1, 0 } };
        Tile check;


        for (int i = 0; i < order.Length / 2; i++)
        {
            check = maze.GetTileAt(currentTile.X + order[i, 0], currentTile.Y + order[i, 1]);
            print("Neig " + i + ", isVisited: " + check.Visited);
            if ((check.Type == Tile.TileType.Floor || check.Type == Tile.TileType.End))
            {
                result.Add(check);
            }
        }

        print("Unvisited neig count: " + result.Count);

        return result;
    }

    public void SolveMaze()
    {
        running = true;
        if (stage == 0)
        {
            if (this.name == "Maze 1")
                mazeController.ResetVisited(mazeA);
            else
                mazeController.ResetVisited(mazeB);
        }
    }

    public void PauseSolving()
    {
        running = false;
    }

    public void ResetSolving()
    {
        stage = 0;

        if (this.name == "Maze 1")
            mazeController.ResetVisited(mazeA);
        else
            mazeController.ResetVisited(mazeB);
    }

    void SaveTime()
    {
        Canvas canvas;
        Text text;
        GameObject myCanvas = new GameObject();
        GameObject myText = new GameObject();

        myCanvas.transform.SetParent(this.transform);
        myCanvas.name = "Final time";
        myCanvas.AddComponent<Canvas>();

        myCanvas.transform.position = new Vector3(this.transform.position.x + Mathf.FloorToInt(mazeA.Width / 2), -2, 1);
        myCanvas.GetComponent<RectTransform>().sizeDelta = new Vector2(mazeA.Width * 100, 300);
        myCanvas.GetComponent<RectTransform>().localScale = new Vector3(0.01f, 0.01f, 1f);

        canvas = myCanvas.GetComponent<Canvas>();
        canvas.renderMode = RenderMode.WorldSpace;
        canvas.worldCamera = Camera.main;
        myCanvas.AddComponent<CanvasScaler>();
        myCanvas.AddComponent<GraphicRaycaster>();

        myText.transform.parent = myCanvas.transform;
        myText.name = "value";
        myText.transform.position = new Vector3(0, 0, 1);

        text = myText.AddComponent<Text>();

        myText.GetComponent<RectTransform>().anchorMin = new Vector2(0, 0);
        myText.GetComponent<RectTransform>().anchorMax = new Vector2(1, 1);
        myText.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);

        myText.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);

        text.font = mazeController.font;
        text.text = GameObject.Find("TimeController").GetComponent<TimeController>().timeDisplay.text;
        text.alignment = TextAnchor.MiddleCenter;
        text.fontStyle = FontStyle.Bold;
        text.resizeTextForBestFit = true;
        text.resizeTextMaxSize = 300;
        text.color = mazeController.colorWall;
    }
}
