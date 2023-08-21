using System.Net.Security;
using static sparta_dungeon.Program;
using System.Xml.Linq;
using System.Reflection.Emit;
using System.Numerics;
using System.Reflection.PortableExecutable;

namespace sparta_dungeon
{
    internal class Program
    {
        private static Character player;
        static void Main(string[] args)
        {
            BasicStatus();
            GameStart();
        }
        static void GameStart()
        {
            Console.Clear();

            Console.WriteLine("스파르타 마을에 오신 여러분 환영합니다.");
            Console.WriteLine("이곳에서 던전으로 들어가기 전 활동을 할 수 있습니다.");
            Console.WriteLine();
            Console.WriteLine("1. 상태 보기");
            Console.WriteLine("2. 인벤토리");
            Console.WriteLine();
            Console.WriteLine("원하시는 행동을 입력해주세요.");

            int answer = CheckValidInput(1, 2);
            switch (answer)
            {
                case 1:
                    ShowStatus();
                    break;

                case 2:
                    ShowInventory();
                    break;
            }
        }

        static void BasicStatus()
        {
            Inventory inventory = new Inventory();

            player = new Character("이홍준", "궁수", 1, 10, 5, 100, 1500);

            inventory.GetItem(new Item("낡은 검", "공격력", 2, "쉽게 볼 수 있는 낡은 검입니다."));
            inventory.GetItem(new Item("무쇠 갑옷", "방어력", 5, "무쇠로 만든 튼튼한 갑옷입니다."));
        }

        static void ShowStatus()
        {
            Console.Clear();

            Console.WriteLine("내 상태");
            Console.WriteLine("캐릭터의 정보가 표시됩니다.");

            Console.WriteLine();
            Console.WriteLine($"Lv. {player.Level}");
            Console.WriteLine($"{player.Name} ({player.Job})");
            Console.WriteLine($"공격력 : {player.Atk}");
            Console.WriteLine($"방어력 : {player.Def}");
            Console.WriteLine($"체력 : {player.Hp}");
            Console.WriteLine($"Gold : {player.Gold} G");
            Console.WriteLine();
            Console.WriteLine("0. 나가기");
            Console.WriteLine();
            Console.WriteLine("원하시는 행동을 입력해주세요.");

            int answer = CheckValidInput(0, 0);
            switch (answer)
            {
                case 0:
                    GameStart();
                    break;
            }
        }

        static void ShowInventory()
        {
            Inventory inventory = new Inventory();
            Console.Clear();

            Console.WriteLine("인벤토리 - 장착 관리");
            Console.WriteLine("보유 중인 아이템을 관리할 수 있습니다.");
            Console.WriteLine();
            Console.WriteLine("[아이템 목록]");

            for (int i = 0; i < inventory.items.Count; i++)
            {
    
                Console.WriteLine($"- {i + 1} {inventory.items[i].Name} | {inventory.items[i].Type} +{inventory.items[i].Effect} | {inventory.items[i].Info}");
            }
            Console.WriteLine();
            Console.WriteLine("0. 나가기");
            Console.WriteLine();
            Console.WriteLine("원하시는 행동을 입력해주세요.");

            int answer = CheckValidInput(0, 0);
            switch (answer)
            {
                case 0:
                    GameStart();
                    break;
            }
        }
        static int CheckValidInput(int min, int max)
        {
            while (true)
            {
                string input = Console.ReadLine();

                bool parseSuccess = int.TryParse(input, out var ret);
                if (parseSuccess)
                {
                    if (ret >= min && ret <= max)
                        return ret;
                }

                Console.WriteLine("잘못된 입력입니다.");
            }
        }
    }
    public class Character
    {
        public string Name { get; }
        public string Job { get; }
        public int Level { get; }
        public int Atk { get; }
        public int Def { get; }
        public int Hp { get; }
        public int Gold { get; }

        public Character(string name, string job, int level, int atk, int def, int hp, int gold)
        {
            Name = name;
            Job = job;
            Level = level;
            Atk = atk;
            Def = def;
            Hp = hp;
            Gold = gold;
        }
    }
    public class Item
    {
        public string Name { get; }
        public string Type { get; }
        public int Effect { get; }
        public string Info { get; }

        public Item(string name, string type, int effect, string info)
        {
            Name = name;
            Type = type;
            Effect = effect;
            Info = info;
        }
    }

    public class Inventory
    {
        public List<Item> items;

        public Inventory()
        {
            items = new List<Item>();
        }

        public void GetItem(Item item)
        {
            items.Add(item);
        }
    }
}