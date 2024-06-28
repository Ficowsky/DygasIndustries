using System;
using System.Collections.Generic;
using System.Numerics;

class Program
{
    static void Main(string[] args)
    {
        Game game = new Game();
        game.Start();
    }
}

class Game
{
    private Map map;
    private Player player;
    private List<NPC> npcs;
    private List<Item> items;

    public Game()
    {
        map = new Map(20, 20); // Zwiększona mapa
        player = new Player(0, 0);
        npcs = new List<NPC>
        {
            new NPC(5, 5),
            new NPC(7, 3)
        };
        items = new List<Item>
        {
            new Item(2, 2, "Sword"),
            new Item(6, 6, "Shield")
        };
    }

    public void Start()
    {
        while (true)
        {
            Console.Clear();
            map.Draw(player, npcs, items);
            HandleInput();
            MoveNPCs();
            CheckInteractions();
            CheckItemPickup();
        }
    }

    private void HandleInput()
    {
        ConsoleKeyInfo keyInfo = Console.ReadKey();
        switch (keyInfo.Key)
        {
            case ConsoleKey.UpArrow:
                player.Move(0, -1, map);
                break;
            case ConsoleKey.DownArrow:
                player.Move(0, 1, map);
                break;
            case ConsoleKey.LeftArrow:
                player.Move(-1, 0, map);
                break;
            case ConsoleKey.RightArrow:
                player.Move(1, 0, map);
                break;
        }
    }

    private void MoveNPCs()
    {
        Random random = new Random();
        foreach (var npc in npcs)
        {
            int dx = random.Next(-1, 2);
            int dy = random.Next(-1, 2);
            npc.Move(dx, dy, map);
        }
    }

    private void CheckInteractions()
    {
        foreach (var npc in npcs)
        {
            if (npc.X == player.X && npc.Y == player.Y)
            {
                Console.WriteLine("Interakcja z NPC!");
                Console.ReadKey();
            }
        }
    }

    private void CheckItemPickup()
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].X == player.X && items[i].Y == player.Y)
            {
                player.PickupItem(items[i]);
                items.RemoveAt(i);
                i--;
                Console.WriteLine("Podniosłeś przedmiot!");
                Console.ReadKey();
            }
        }
    }
    class Map
    {
        private int width;
        private int height;
        private bool[,] tiles;

        public Map(int width, int height)
        {
            this.width = width;
            this.height = height;
            tiles = new bool[width, height];
            GenerateMap();
        }

        private void GenerateMap()
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    tiles[x, y] = true; // Example: all tiles passable
                }
            }
        }

        public bool IsPassable(int x, int y)
        {
            return x >= 0 && y >= 0 && x < width && y < height && tiles[x, y];
        }

        public void Draw(Player player, List<NPC> npcs, List<Item> items)
        {
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (player.X == x && player.Y == y)
                    {
                        Console.Write('P');
                    }
                    else if (npcs.Exists(n => n.X == x && n.Y == y))
                    {
                        Console.Write('N');
                    }
                    else if (items.Exists(i => i.X == x && i.Y == y))
                    {
                        Console.Write('I');
                    }
                    else
                    {
                        Console.Write('.');
                    }
                }
                Console.WriteLine();
            }
        }
    }
    class Item
    {
        public int X { get; private set; }
        public int Y { get; private set; }
        public string Name { get; private set; }

        public Item(int x, int y, string name)
        {
            X = x;
            Y = y;
            Name = name;
        }
    }

}
