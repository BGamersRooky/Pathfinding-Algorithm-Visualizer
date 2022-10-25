using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Tile
{
    public enum TileType { Floor, Wall, Start, End, Path, Current };
    Action<Tile> tileTypeChanged, tileVisitedChanged, tileDistanceChanged, tileRootDistanceChanged, tileManDistanceChanged;

    int x;
    int y;
    int distance, manDistance, rootDistance;
    TileType type;
    Boolean visited;
    Boolean start, end = false;
    Tile parent;

    public int X
    {
        get
        {
            return x;
        }

        set
        {
            x = value;
        }
    }
    public int Y
    {
        get
        {
            return y;
        }
        set
        {
            y = value;
        }
    }
    public int Cost { get; set; }
    public TileType Type
    {
        get
        {
            return type;
        }
        set
        {
            type = value;

            if (tileTypeChanged != null)
                tileTypeChanged(this);
        }
    }
    public Boolean Visited
    {
        get
        {
            return visited;
        }
        set
        {
            visited = value;

            if (tileVisitedChanged != null)
                tileVisitedChanged(this);
        }
    }
    public Boolean Start
    {
        get
        {
            return start;
        }
        set
        {
            start = value;
        }
    }
    public Boolean End
    {
        get
        {
            return end;
        }
        set
        {
            end = value;
        }
    }
    public Tile Parent { get; set; }
    public int Distance
    {
        get
        {
            return distance;
        }
        set
        {
            distance = value;

            if (tileDistanceChanged != null)
                tileDistanceChanged(this);
        }
    }
    public int ManDistance 
    { 
        get 
        {
            return manDistance;
        } 
        set 
        {
            manDistance = value;

            if (tileManDistanceChanged != null)
                tileDistanceChanged(this);
        }
    }
    public int RootDistance 
    {
        get
        {
            return rootDistance;
        }
        set
        {
            rootDistance = value;

            if (tileRootDistanceChanged != null)
                tileRootDistanceChanged(this);
        }
    }

    public Tile()
    {
        this.X = X;
        this.Y = Y;
        this.Cost = Cost;
        this.type = Type;
        this.visited = Visited;
        this.start = Start;
        this.end = End;
        this.parent = Parent;
        this.distance = Distance;
        this.ManDistance = ManDistance;
        this.RootDistance = RootDistance;
    }
    public Tile(int X, int Y, int Cost, TileType Type, bool Visited, bool Start, bool End, Tile Parent, int Distance, int ManDistance, int RootDistance)
    {
        this.X = X;
        this.Y = Y;
        this.Cost = Cost;
        this.type = Type;
        this.visited = Visited;
        this.start = Start;
        this.end = End;
        this.parent = Parent;
        this.distance = Distance;
        this.ManDistance = ManDistance;
        this.RootDistance = RootDistance;        
    }

    public Tile(int x, int y)
    {
        this.x = x;
        this.y = y;
        this.distance = int.MaxValue;
        this.Cost = 0;
        this.type = TileType.Wall;
        this.visited = false;
    }
    public void RegisterTileTypeChangedCallback(Action<Tile> callback)
    {
        tileTypeChanged = callback;
    }
    public void RegisterTileVisitedChangedCallback(Action<Tile> callback)
    {
        tileVisitedChanged = callback;
    }
    public void RegisterTileDistanceChangedCallback(Action<Tile> callback)
    {
        tileDistanceChanged = callback;
    }
    public void RegisterTileRootDistanceChangedCallback(Action<Tile> callback)
    {
        tileRootDistanceChanged = callback;
    }
    public void RegisterTileManDistanceChangedCallback(Action<Tile> callback)
    {
        tileManDistanceChanged = callback;
    }

    public override string ToString()
    {
        if (visited)
            return "Tile coordinates are: " + x + "x and " + y + "y. This tile is visited already!";
        else
            return "Tile coordinates are: " + x + "x and " + y + "y. This tile is not visited.";
    }

    public Tile DeepClone()
    {
        //Debug.Log("@Tile/DeepClone() - " + this.Type);

        string output = JsonConvert.SerializeObject(this);
        //Debug.Log(output);
        return JsonConvert.DeserializeObject<Tile>(output);
    }
}
