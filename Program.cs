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

        static Inventory BasicStatus()
        {
            player = new Character("이홍준", "궁수", 1, 10, 5, 100, 1500);
            addedstat = new Character("이홍준", "궁수", 0, 0, 0, 0, 0);

            Item weap1 = new Item("낡은 검", "공격력", 2, "쉽게 볼 수 있는 낡은 검입니다.", false);
            Item armo1 = new Item("무쇠 갑옷", "방어력", 5, "무쇠로 만든 튼튼한 갑옷입니다.", false);
            Item acce1 = new Item("도금 목걸이", "체력", 30, "싸구려에 도금시킨 목걸이입니다.", false);

            inventory.items.Add(weap1);
            inventory.items.Add(armo1);
            inventory.items.Add(acce1);

            return inventory;
        }

        static void ShowStatus()
        {
            Console.Clear();

            Console.WriteLine("내 상태");
            Console.WriteLine("캐릭터의 정보가 표시됩니다.");

            Console.WriteLine();
            Console.WriteLine($"Lv. {player.Level}");
            Console.WriteLine($"{player.Name} ({player.Job})");
            Console.WriteLine($"공격력 : {player.Atk} (+{addedstat.Atk})");
            Console.WriteLine($"방어력 : {player.Def} (+{addedstat.Def})");
            Console.WriteLine($"체력 : {player.Hp} (+{addedstat.Hp})");
            Console.WriteLine($"Gold : {player.Gold}G");
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
            Console.Clear();

            Console.WriteLine("인벤토리 - 장착 관리");
            Console.WriteLine("보유 중인 아이템을 관리할 수 있습니다.");
            Console.WriteLine();
            Console.WriteLine("[아이템 목록]");

            int i = 1;
            foreach (var Item in inventory.items)
            {
                if (Item.Equip == true)
                {
                    Console.WriteLine($"- {i} [E]{Item.Name} | {Item.Type} +{Item.Effect} | {Item.Info}");
                    i++;
                }
                else if (Item.Equip == false)
                {
                    Console.WriteLine($"- {i} {Item.Name} | {Item.Type} +{Item.Effect} | {Item.Info}");
                    i++;
                }
            }

            Console.WriteLine();
            Console.WriteLine("0. 나가기");
            Console.WriteLine();
            Console.WriteLine("원하시는 행동을 입력해주세요.");

            int answer = CheckValidInput(0, i);
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
        static void EquipItem(Item item)
        {
            if (item.Equip == true && item.Type.Equals("공격력"))              // 장착해제 (순서바꾸면좋겠는데)
            {
                item.Equip = false;
                addedstat.Atk -= item.Effect;
                player.Atk -= addedstat.Atk;
            }
            else if (item.Equip == false && item.Type.Equals("공격력"))        // 장착 
            {
                item.Equip = true;
                addedstat.Atk += item.Effect;
                player.Atk += addedstat.Atk;
            }
            else if (item.Equip == true && item.Type.Equals("방어력"))
            {
                item.Equip = false;
                addedstat.Def -= item.Effect;
                player.Def -= addedstat.Def;
            }
            else if (item.Equip == false && item.Type.Equals("방어력"))
            {
                item.Equip = true;
                addedstat.Def += item.Effect;
                player.Def += addedstat.Def;
            }
            else if (item.Equip == true && item.Type.Equals("체력"))
            {
                item.Equip = false;
                addedstat.Hp -= item.Effect;
                player.Hp -= addedstat.Hp;
            }
            else if (item.Equip == false && item.Type.Equals("체력"))
            {
                item.Equip = true;
                addedstat.Hp += item.Effect;
                player.Hp += addedstat.Hp;
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