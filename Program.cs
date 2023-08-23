using System.Net.Security;
using static sparta_dungeon.Program;
using System.Xml.Linq;
using System.Reflection.Emit;
using System.Numerics;
using System.Reflection.PortableExecutable;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Reflection.Metadata.Ecma335;
using System.IO.Pipes;

namespace sparta_dungeon
{
    internal class Program
    {
        private static Character player;
        private static Character addedstat;
        private static Inventory inventory = new();
        private static ShopInventory shop = new();
        static void Main(string[] args)
        {
            BasicStatus();
            BasicInventory();
            BasicShop();
            GameStart();
        }
        static void GameStart()     // 게임 시작화면
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
        static void BasicStatus()       // 캐릭터의 기초 스탯
        {
            player = new Character("이홍준", "궁수", 1, 10, 5, 100, 1500);
            addedstat = new Character("이홍준", "궁수", 0, 0, 0, 0, 0);
        }
        static Inventory BasicInventory()       // 캐릭터의 기초 보유 인벤토리
        {
            Item weap1 = new Item("낡은 검", "공격력", 2, "쉽게 볼 수 있는 낡은 검입니다. (기본 아이템)", false, 0);
            Item armo1 = new Item("무쇠 갑옷", "방어력", 5, "무쇠로 만든 튼튼한 갑옷입니다. (기본 아이템)", false, 0);
            Item acce1 = new Item("도금 목걸이", "체력", 30, "싸구려에 도금시킨 목걸이입니다. (기본 아이템)", false, 0);

            inventory.items.Add(weap1);
            inventory.items.Add(armo1);
            inventory.items.Add(acce1);

            return inventory;
        }
        static ShopInventory BasicShop()        // 상점이 가지고 있는 인벤토리
        {
            Item weap2 = new Item("보급형 검", "공격력", 5, "첫 모험에 안성맞춤인 검입니다.", false, 200);
            Item weap3 = new Item("군용 검", "공격력", 8, "쉽게 볼 수 없는 군용 검입니다.", false, 400);
            Item armo2 = new Item("사슬 갑옷", "방어력", 6, "사슬로 만든 유연한 갑옷입니다.", false, 250);
            Item armo3 = new Item("판금 갑옷", "방어력", 10, "판금으로 만든 기사용 갑옷입니다.", false, 500);
            Item acce2 = new Item("은 목걸이", "체력", 50, "어둠 속에서도 빛나는 은 목걸이입니다.", false, 300);
            Item acce3 = new Item("금 목걸이", "체력", 100, "진짜 금 목걸이입니다.", false, 600);

            shop.shopitems.Add(weap2);
            shop.shopitems.Add(weap3);
            shop.shopitems.Add(armo2);
            shop.shopitems.Add(armo3);
            shop.shopitems.Add(acce2);
            shop.shopitems.Add(acce3);

            return shop;
        }

        static void ShowStatus()                // 상태 보기
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

        static void ShowInventory()             // 인벤토리 보기
        {
            Console.Clear();

            Console.WriteLine("\x1b[38;5;" + 3 + "m인벤토리 - 장착 관리");
            Console.WriteLine("보유 중인 아이템을 관리할 수 있습니다.");
            Console.WriteLine();
            Console.WriteLine("\x1b[38;5;" + 15 + "m[아이템 목록]");

            int i = 0;
            foreach (var Item in inventory.items)   // 장착여부에 따라 [E] 마크 표시
            {
                if (Item.Equip == true)
                {
                    Console.WriteLine($"{i + 1}. " + "\x1b[38;5;" + 6 + "m[E]" + "\x1b[38;5;" + 15 + $"m {Item.Name} | {Item.Type} +{Item.Effect} | {Item.Info}");
                    i++;
                }
                else if (Item.Equip == false)
                {
                    Console.WriteLine($"{i + 1}. {Item.Name} | {Item.Type} +{Item.Effect} | {Item.Info}");
                    i++;
                }
            }

            Console.WriteLine();
            Console.WriteLine("\x1b[38;5;" + 14 + "m0. 나가기");
            Console.WriteLine();
            Console.WriteLine("\x1b[38;5;" + 15 + "m원하시는 행동을 입력해주세요.");

            int answer = CheckValidInput(0, i);
            if (answer == 0)
            {
                GameStart();
            }
            else if (answer > 0)
            {
                EquipItem(inventory.items[answer - 1]);
                ShowInventory();
            }
        }
        static void ShowShop()                      // 상점 표시
        {
            Console.Clear();

            Console.WriteLine("\x1b[38;5;" + 3 + "m상점");
            Console.WriteLine("필요한 아이템을 얻을 수 있는 상점입니다.");
            Console.WriteLine();
            Console.WriteLine("\x1b[38;5;" + 15 + "m[보유 골드]");
            Console.WriteLine("\x1b[38;5;" + 9 + $"m{player.Gold} " + "\x1b[38;5;" + 15 + "mG");
            Console.WriteLine();
            Console.WriteLine("\x1b[38;5;" + 15 + "m[아이템 목록]");

            int i = 1;
            foreach (var Item in shop.shopitems)
            {
                Console.WriteLine($"{i}. {Item.Name} | {Item.Type} +{Item.Effect} | {Item.Price}G | {Item.Info}");
                i++;
            }

            Console.WriteLine();
            Console.WriteLine("\x1b[38;5;" + 14 + "m1. 아이템 구매");
            Console.WriteLine("2. 아이템 판매");
            Console.WriteLine("0. 나가기");
            Console.WriteLine();
            Console.WriteLine("\x1b[38;5;" + 15 + "m원하시는 행동을 입력해주세요.");

            int answer = CheckValidInput(0, 2);
            switch (answer)
            {
                case 0:
                    GameStart();
                    break;

                case 1:
                    Console.WriteLine("구매하려는 아이템의 번호를 입력해주세요.");        // 구매할 아이템을 고르면
                    int purchaseitem = CheckValidInput(1, i);
                    shop.shopitems[purchaseitem - 1].Info += " (보유중)";              // 1. (보유중) 표시 달아줌
                    inventory.items.Add(shop.shopitems[purchaseitem - 1]);            // 2. 인벤토리에 해당 아이템을 추가
                    player.Gold -= shop.shopitems[purchaseitem - 1].Price;            // 3. 플레이어의 골드에서 아이템 가격만큼 차감
                    ShowShop();
                    break;
            
                case 2:
                    ShowSellable();     // 아이템 판매화면으로
                    break;
            }
        }
        static void ShowSellable()      // 아이템 판매화면
        {
            Console.Clear();

            Console.WriteLine("\x1b[38;5;" + 3 + "m상점 - 아이템 판매");
            Console.WriteLine("불필요한 아이템을 팔 수 있습니다. 기본 아이템은 판매할 수 없습니다.");
            Console.WriteLine();
            Console.WriteLine("\x1b[38;5;" + 15 + "m[보유 골드]");
            Console.WriteLine("\x1b[38;5;" + 9 + $"m{player.Gold} " + "\x1b[38;5;" + 15 + "mG");
            Console.WriteLine();
            Console.WriteLine("\x1b[38;5;" + 15 + "m[아이템 목록]");

            int i = 0;
            foreach (var Item in inventory.items)       // 아이템 장착여부에 따라 [E] 마크 표시
            {
                if (Item.Equip == true)
                {
                    Console.WriteLine($"{i + 1}. " + "\x1b[38;5;" + 6 + "m[E]" + "\x1b[38;5;" + 15 + $"m {Item.Name} | {Item.Type} +{Item.Effect} | 판매가: {(Item.Price) * 0.85} | {Item.Info}");
                    i++;
                }
                else if (Item.Equip == false)
                {
                    Console.WriteLine($"{i + 1}. {Item.Name} | {Item.Type} +{Item.Effect} | 판매가: {(Item.Price) * 0.85} | {Item.Info}");
                    i++;
                }
            }

            Console.WriteLine();
            Console.WriteLine("\x1b[38;5;" + 14 + "m0. 나가기");
            Console.WriteLine();
            Console.WriteLine("\x1b[38;5;" + 15 + "m판매할 아이템의 번호를 입력해주세요.");

            int answer = CheckValidInput(0, i);     // 나가기
            if (answer == 0)
            {
                ShowShop();
            }
            else if (answer > 0 && answer <= 3)     // 기본 아이템 판매 불가능
            {
                ShowSellable();
            }
            else if (answer > 3)                    // 판매할 아이템을 고르면
            {
                if (inventory.items[answer - 1].Equip == true)      // 만약 장착중인 아이템이라면 장착해제 함수 실행
                {
                    EquipItem(inventory.items[answer - 1]);
                }

                string temp = inventory.items[answer - 1].Name;     // 임시로 판매할 아이템의 이름을 저장
                              
                for(int j = 0;  j < shop.shopitems.Count; j++)      
                {
                    if (temp.Equals(shop.shopitems[j].Name))        // 상점 아이템중에 판매할 아이템과 이름이 같은 아이템에게
                    {
                        string toRemove = " (보유중)";               
                        int x = shop.shopitems[j].Info.IndexOf(toRemove);   
                        shop.shopitems[j].Info = shop.shopitems[j].Info.Remove(x, toRemove.Length);     // (보유중) 제거 
                        double sellprice = shop.shopitems[j].Price * 0.85;     // 판매가는 구매가의 85%
                        player.Gold += (int)sellprice;                         // 판매가만큼 플레이어의 골드에 추가
                    }
                    else { }
                }

                inventory.items.RemoveAt(answer - 1);               // 인벤토리에서 해당 아이템 제거  
                ShowSellable();
            }
        }
        static int CheckValidInput(int min, int max)                // 플레이어의 입력이 유효한 입력인지 확인
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
        static void EquipItem(Item item)        // 장착해제 함수
        {
            if (item.Equip == true && item.Type.Equals("공격력"))              // 장착해제, 추가스탯과 플레이어스탯에서 공격력 차감
            {
                item.Equip = false;
                addedstat.Atk -= item.Effect;
                player.Atk -= item.Effect;
            }
            else if (item.Equip == false && item.Type.Equals("공격력"))        // 장착, 추가스탯과 플레이어스탯에 공격력 추가
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
        public int Price { get; set; }

        public Item(string name, string type, int effect, string info, bool equip, int price)
        {
            Name = name;
            Type = type;
            Effect = effect;
            Info = info;
            Equip = equip;
            Price = price;
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
    public class ShopInventory
    {
        public List<Item> shopitems;
        public ShopInventory()
        {
            shopitems = new List<Item>();
        }
    }
}