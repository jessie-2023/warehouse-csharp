// See https://aka.ms/new-console-template for more information


Dictionary<string, int> inventory = [];

reportStock();
makeOrder();

// 1. Know how many of each item we have in stock​
void reportStock()
{
    Console.WriteLine("Current inventory:");
    printStock(inventory);
}


// 2. Allow the manager to order new stock​
void makeOrder()
{
    bool orderMore;
    do
    {
        Console.WriteLine("Please enter the name of product to order:");
        string orderName = Console.ReadLine();
        Console.WriteLine("Please enter the quantity to order:");
        int orderQuant = int.Parse(Console.ReadLine() ?? "");
        newStock(orderName, orderQuant);

        Console.WriteLine("Do you want to order more? (y/n)");
        orderMore = Console.ReadLine() == "y";

    } while (orderMore);

    Console.WriteLine("The inventory after your order would be:");
    printStock(inventory);
}


// 3. Allow a customer to add items to a basket (and then purchase all the items)​


void newStock(string product, int quantity)
{
    if (inventory.ContainsKey(product))
    {
        inventory[product] += quantity;
    }
    else
    {
        inventory.Add(product, quantity);
    }
}


// print the stock
void printStock(Dictionary<string, int> inventory)
{
    foreach (var item in inventory)
    {
        Console.WriteLine($"{item.Key}: {item.Value}");
    }
}
