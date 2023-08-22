using System.Net.Security;
using static sparta_dungeon.Program;
using System.Xml.Linq;
using System.Reflection.Emit;
using System.Numerics;
using System.Reflection.PortableExecutable;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Reflection.Metadata.Ecma335;

namespace sparta_dungeon
{
    internal class Program
    {
        private static Character player;
        private static Character addedstat;
        private static Inventory inventory = new();
        private static Inventory shop = new();
        static void Main(string[] args)
        {
            BasicStatus();
            BasicInventory();
            GameStart();
        }
        static void GameStart()
        {
            Console.Clear();

            Console.WriteLine("\x1b[38;5;" + 3 + "m스파르타 마을에 오신 여러분 환영합니다.");
            Console.WriteLine("이곳에서 던전으로 들어가기 전 활동을 할 수 있습니다.");
            Console.WriteLine();
            Console.WriteLine("\x1b[38;5;" + 15 + "m1. 상태 보기");
            Console.WriteLine("2. 인벤토리");
            Console.WriteLine("3. 상점");
            Console.WriteLine();
            Console.WriteLine("\x1b[38;5;" + 15 + "m원하시는 행동을 입력해주세요.");

            int answer = CheckValidInput(1, 3);
            switch (answer)
            {
                case 1:
                    ShowStatus();
                    break;

                case 2:
                    ShowInventory();
                    break;

                case 3:
                    ShowShop();
                    break;
            }
        }
        static void BasicStatus()
        {
            player = new Character("이홍준", "궁수", 1, 10, 5, 100, 1500);
            addedstat = new Character("이홍준", "궁수", 0, 0, 0, 0, 0);
        }
        static Inventory BasicInventory()
        {
            Item weap1 = new Item("낡은 검", "공격력", 2, "쉽게 볼 수 있는 낡은 검입니다.", false);
            Item armo1 = new Item("무쇠 갑옷", "방어력", 5, "무쇠로 만든 튼튼한 갑옷입니다.", false);
            Item acce1 = new Item("도금 목걸이", "체력", 30, "싸구려에 도금시킨 목걸이입니다.", false);

            inventory.items.Add(weap1);
            inventory.items.Add(armo1);
            inventory.items.Add(acce1);

            return inventory;
        }
        static Inventory Shop()
        {
            Item weap2 = new Item("보급형 검", "공격력", 5, "첫 모험에 안성맞춤인 검입니다.", false);
            Item weap3 = new Item("군용 검", "공격력", 8, "쉽게 볼 수 없는 군용 검입니다.", false);
            Item armo2 = new Item("사슬 갑옷", "방어력", 6, "사슬로 만든 유연한 갑옷입니다.", false);
            Item armo3 = new Item("판금 갑옷", "방어력", 10, "판금으로 만든 기사용 갑옷입니다.", false);
            Item acce2 = new Item("은 목걸이", "체력", 50, "어둠 속에서도 빛나는 은 목걸이입니다.", false);
            Item acce3 = new Item("금 목걸이", "체력", 100, "진짜 금 목걸이입니다.", false);

            return shop;
        }

        static void ShowStatus()
        {
            Console.Clear();

            Console.WriteLine("\x1b[38;5;" + 3 + "m내 상태");
            Console.WriteLine("캐릭터의 정보가 표시됩니다.");

            Console.WriteLine();
            Console.WriteLine("\x1b[38;5;" + 15 + $"mLv. {player.Level}");
            Console.WriteLine($"{player.Name} ({player.Job})");
            Console.WriteLine($"공격력 : {player.Atk} (" + "\x1b[38;5;" + 2 + $"m+{addedstat.Atk}" + "\x1b[38;5;" + 15 + "m)");
            Console.WriteLine($"방어력 : {player.Def} (" + "\x1b[38;5;" + 2 + $"m+{addedstat.Def}" + "\x1b[38;5;" + 15 + "m)");
            Console.WriteLine($"체력 : {player.Hp} (" + "\x1b[38;5;" + 2 + $"m+{addedstat.Hp}" + "\x1b[38;5;" + 15 + "m)");
            Console.WriteLine($"Gold : {player.Gold}G");
            Console.WriteLine();
            Console.WriteLine("\x1b[38;5;" + 14 + "m0. 나가기");
            Console.WriteLine();
            Console.WriteLine("\x1b[38;5;" + 15 + "m원하시는 행동을 입력해주세요.");

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
            Console.Clear();

            Console.WriteLine("\x1b[38;5;" + 3 + "m인벤토리 - 장착 관리");
            Console.WriteLine("보유 중인 아이템을 관리할 수 있습니다.");
            Console.WriteLine();
            Console.WriteLine("\x1b[38;5;" + 15 + "m[아이템 목록]");

            int i = 1;
            foreach (var Item in inventory.items)
            {
                if (Item.Equip == true)
                {
                    Console.WriteLine($"{i}. " + "\x1b[38;5;" + 6 + "m[E]" + "\x1b[38;5;" + 15 + $"m {Item.Name} | {Item.Type} +{Item.Effect} | {Item.Info}");
                    i++;
                }
                else if (Item.Equip == false)
                {
                    Console.WriteLine($"{i}. {Item.Name} | {Item.Type} +{Item.Effect} | {Item.Info}");
                    i++;
                }
            }

            Console.WriteLine();
            Console.WriteLine("\x1b[38;5;" + 14 + "m0. 나가기");
            Console.WriteLine();
            Console.WriteLine("\x1b[38;5;" + 15 + "m원하시는 행동을 입력해주세요.");

            int answer = CheckValidInput(0, i - 1);
            switch (answer)
            {
                case 0:
                    GameStart();
                    break;
                case 1:
                    EquipItem(inventory.items[0]);
                    ShowInventory();
                    break;
                case 2:
                    EquipItem(inventory.items[1]);
                    ShowInventory();
                    break;
                case 3:
                    EquipItem(inventory.items[2]);
                    ShowInventory();
                    break;
            }
        }
        static void ShowShop()
        {
            Console.Clear();

            Console.WriteLine("\x1b[38;5;" + 3 + "m상점");
            Console.WriteLine("필요한 아이템을 얻을 수 있는 상점입니다.");
            Console.WriteLine();
            Console.WriteLine("\x1b[38;5;" + 15 + "m[보유 골드]");
            Console.WriteLine("\x1b[38;5;" + 9 + $"m{player.Gold} " + "\x1b[38;5;" + 15 + "mG");


            Console.WriteLine();
            Console.WriteLine("\x1b[38;5;" + 14 + "m0. 나가기");
            Console.WriteLine();
            Console.WriteLine("\x1b[38;5;" + 15 + "m원하시는 행동을 입력해주세요.");

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
                Console.Write(">> ");
                string input = Console.ReadLine();

                bool parseSuccess = int.TryParse(input, out var ret);
                if (parseSuccess)
                {
                    if (ret >= min && ret <= max)
                        return ret;
                }

                Console.WriteLine("\x1b[38;5;" + 1 + "m잘못된 입력입니다.");
                Console.ResetColor();
            }
        }
        static void EquipItem(Item item)
        {
            if (item.Equip == true && item.Type.Equals("공격력"))              // 장착해제 (순서바꾸면좋겠는데)
            {
                item.Equip = false;
                addedstat.Atk -= item.Effect;
                player.Atk -= item.Effect;
            }
            else if (item.Equip == false && item.Type.Equals("공격력"))        // 장착 (함수로 바꿔서 줄이면좋겠다)
            {
                item.Equip = true;
                addedstat.Atk += item.Effect;
                player.Atk += item.Effect;
            }
            else if (item.Equip == true && item.Type.Equals("방어력"))
            {
                item.Equip = false;
                addedstat.Def -= item.Effect;
                player.Def -= item.Effect;
            }
            else if (item.Equip == false && item.Type.Equals("방어력"))
            {
                item.Equip = true;
                addedstat.Def += item.Effect;
                player.Def += item.Effect;
            }
            else if (item.Equip == true && item.Type.Equals("체력"))
            {
                item.Equip = false;
                addedstat.Hp -= item.Effect;
                player.Hp -= item.Effect;
            }
            else if (item.Equip == false && item.Type.Equals("체력"))
            {
                item.Equip = true;
                addedstat.Hp += item.Effect;
                player.Hp += item.Effect;
            }
            // 아이템의 장착 여부를 확인한다
            // 아이템이 미장착 상태라면 장착시켜 ShowInventory 에서 E 를 붙여주고 장비의 능력치를 캐릭터의 능력치에 합산시켜 보여준다.
            // 아이템이 장착 상태라면 미장착시켜 E를 제거하고 장비의 능력치를 캐릭터의 능력치에 합산시켜 보여준다.
        }

    }
    public class Character
    {
        public string Name { get; }
        public string Job { get; }
        public int Level { get; set; }
        public int Atk { get; set; }
        public int Def { get; set; }
        public int Hp { get; set; }
        public int Gold { get; set; }

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
        public string Name { get; set; }
        public string Type { get; set; }
        public int Effect { get; set; }
        public string Info { get; set; }
        public bool Equip { get; set; }

        public Item(string name, string type, int effect, string info, bool equip)
        {
            Name = name;
            Type = type;
            Effect = effect;
            Info = info;
            Equip = equip;
        }
    }

    public class Inventory
    {
        public List<Item> items;

        public Inventory()
        {
            items = new List<Item>();
        }
    }
}