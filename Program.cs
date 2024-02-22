/*use Class and Interface*/

var store = new Warehouse();
store.ReportStock();
store.MakeOrder();
store.ReportStock();
store.CheckOut();
store.ReportStock();

class Warehouse 
{
    //Inventory: entries of in-flow and out-flow
    public List<InventoryEntry> Inventory = [];
    
    //Update inventory
    InventoryEntry UpdateInventory(string name, int quantity, decimal price, EntryType type)
    {
        var newEntry = new InventoryEntry
            {
                Entry_time = DateTime.Now,
                Entry_type = type,
                Entry = new ItemEntry 
                    {
                        // Item_id = , 
                        Item_name = name, 
                        Item_quantity = type == EntryType.Order ? quantity : -quantity, 
                        Item_price = price,
                    },
            };
        // update inventory
        Inventory.Add(newEntry);
        return newEntry;
    }
    //Calculate current stock
    Dictionary<string, int> CurrentStock()
    {
        var stock = Inventory
            .GroupBy(inventory => inventory.Entry.Item_name)
            .ToDictionary(group => group.Key, group => group.Sum(inventory => inventory.Entry.Item_quantity));

        return stock;
    }

    //Print current stock
    public void ReportStock()
    {
        Console.WriteLine("=====Current Stock=====");
        foreach (var kvp in CurrentStock())
        {
            Console.WriteLine($"Item: {kvp.Key}, Quantity: {kvp.Value}");
        }
    }
    //Get stock of given item
    public int CheckStock(string name)
    {
        var stock = CurrentStock();
        return stock.ContainsKey(name) ? stock[name] : -1;
    }

    //Make order
    public void MakeOrder()
    {
        bool orderMore;
        do
        {
            Console.WriteLine("Please enter the name of product to order:");
            string inputName = Console.ReadLine() ?? "";
            Console.WriteLine("Please enter the quantity to order:");
            int inputQuantity = int.Parse(Console.ReadLine() ?? "");
            Console.WriteLine("Please enter the current unit price:");
            decimal inputPrice = decimal.Parse(Console.ReadLine() ?? "");

            // var newEntry = new InventoryEntry
            // {
            //     Entry_time = DateTime.Now,
            //     Entry_type = EntryType.Order,
            //     Entry = new ItemEntry 
            //         {
            //             // Item_id = , 
            //             Item_name = inputName, 
            //             Item_quantity = inputQuantity, 
            //             Item_price = inputPrice,
            //         },
            // };

            // // update inventory
            // Inventory.Add(newEntry);
            UpdateInventory(inputName, inputQuantity, inputPrice, EntryType.Order);

            Console.WriteLine("Do you want to order more? (y/n)");
            orderMore = Console.ReadLine() == "y";

        } while (orderMore);

    }

    public void CheckOut()
    {
        bool buyMore;
        var basket = new List<InventoryEntry>();
        decimal payment = 0;
        do
        {
            Console.WriteLine("Please enter the name of product to buy:");
            string inputName = Console.ReadLine() ?? "";

            //check item stock, preceed only when stock > 0
            int stock = CheckStock(inputName);
            if (stock == -1)
            {
                Console.WriteLine($"Sorry, we don't sell {inputName} here.");
            }
            else if (stock == 0)
            {
                Console.WriteLine($"Sorry, {inputName} is out of stock now. Would you like to make an order?");
                // if order, take information
            }
            else
            {
                Console.WriteLine($"We have {stock} in stock, how many {inputName} would you like to buy:");
                int inputQuantity = int.Parse(Console.ReadLine() ?? "");
                while (inputQuantity > stock)
                {
                    Console.WriteLine($"Sorry, we don't have enough in stock. Please enter a smaller amount:");
                    inputQuantity = int.Parse(Console.ReadLine() ?? "");
                }

                Console.WriteLine("Please enter the current unit price:");
                decimal inputPrice = decimal.Parse(Console.ReadLine() ?? "");

                payment +=  inputQuantity * inputPrice;

                // update inventory and basket
                var addBasket = UpdateInventory(inputName, inputQuantity, inputPrice, EntryType.Sell);
                basket.Add(addBasket);
            }

            Console.WriteLine("Do you want to buy something else? (y/n)");
            buyMore = Console.ReadLine() == "y";
        } while (buyMore);
        
        Console.WriteLine("Your purchase are as follow:");
        foreach (var item in basket)
        {
            Console.WriteLine($"Item: {item.Entry.Item_name}, Quantity: {-item.Entry.Item_quantity}, Unit Price: {item.Entry.Item_price}.");
        }
        Console.WriteLine($"The total payment is {payment}.");
    }

}

class ItemEntry // goods
{
    public required string Item_name { get; set; }
    // public required int Item_id { get; set; } 
    public required int Item_quantity { get; set; }
    public required decimal Item_price { get; set; }
}

class InventoryEntry
{
    public required DateTime Entry_time { get; set; }
    public required EntryType Entry_type { get; set; }
    public required ItemEntry Entry { get; set; }

}

enum EntryType
{
    Order,
    Sell,
}
// class ItemOrder: IItem // goods inflow (combine with shipped in for now)
// {
//     public required string Item_name { get; set; }
//     // public required int Item_id { get; set; } 
//     public required int Item_quantity { get; set; }
//     public required decimal Item_price { get; set; }

//     //update inventory
// }

// class ItemSell: IItem // good outflow (combine with bascket for now)
// {
//     public required string Item_name { get; set; }
//     // public required int Item_id { get; set; } 
//     public required int Item_quantity { get; set; }
//     public required decimal Item_price { get; set; }

//     //update inventory (add)
// }



/*use Dictionary*/
// Dictionary<string, int> inventory = [];

// reportStock();
// makeOrder();

// // 1. Know how many of each item we have in stock​
// void reportStock()
// {
//     Console.WriteLine("Current inventory:");
//     printStock(inventory);
// }


// // 2. Allow the manager to order new stock​
// void makeOrder()
// {
//     bool orderMore;
//     do
//     {
//         Console.WriteLine("Please enter the name of product to order:");
//         string orderName = Console.ReadLine();
//         Console.WriteLine("Please enter the quantity to order:");
//         int orderQuant = int.Parse(Console.ReadLine() ?? "");
//         newStock(orderName, orderQuant);

//         Console.WriteLine("Do you want to order more? (y/n)");
//         orderMore = Console.ReadLine() == "y";

//     } while (orderMore);

//     Console.WriteLine("The inventory after your order would be:");
//     printStock(inventory);
// }


// // 3. Allow a customer to add items to a basket (and then purchase all the items)​


// void newStock(string product, int quantity)
// {
//     if (inventory.ContainsKey(product))
//     {
//         inventory[product] += quantity;
//     }
//     else
//     {
//         inventory.Add(product, quantity);
//     }
// }


// // print the stock
// void printStock(Dictionary<string, int> inventory)
// {
//     foreach (var item in inventory)
//     {
//         Console.WriteLine($"{item.Key}: {item.Value}");
//     }
// }