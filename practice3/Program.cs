using System;
using System.Collections.Generic;

interface Commands
{
    void Add();
    void removeCommand();
}

public class ProductList
{
    public string name { get; set; }
    public string manufactor { get; set; }
    public double price { get; set; }

    public ProductList(string name, string manufactor, double price)
    {
        this.name = name;
        this.manufactor = manufactor;
        this.price = price;
    }

    public override string ToString()
    {
        return $"{name} (Manufacturer: {manufactor}, Price: {price})";
    }
}

class ComandsRealization
{
    protected List<ProductList> products = new List<ProductList>();
    protected Stack<Commands> commandHistory = new Stack<Commands>();

    public void addProduct(ProductList prod)
    {
        products.Add(prod);
    }

    public void ShowProducts()
    {
        if (products.Count == 0)
        {
            Console.WriteLine("The basket is empty");
        }
        else
        {
            foreach (var product in products)
            {
                Console.WriteLine(product);
            }
        }
    }

    public void removeProduct(ProductList prod)
    {
        if (products.Contains(prod))
        {
            products.Remove(prod);
        }
    }

    public void addCommand(Commands command)
    {
        commandHistory.Push(command);
    }

    public void removeCommand()
    {
        if (commandHistory.Count > 0)
        {
            var commandToRemove = commandHistory.Pop();
            commandToRemove.removeCommand();
        }
    }


    public List<ProductList> GetProducts()
    {
        return products;
    }
}

class CommandAdd : Commands
{
    protected ComandsRealization command;
    protected ProductList product;

    public CommandAdd(ComandsRealization command, ProductList product)
    {
        this.command = command;
        this.product = product;
    }

    public void Add()
    {
        command.addProduct(product);
        command.addCommand(this);
    }

    public void removeCommand()
    {
        command.removeProduct(product);
    }
}

class CommandDel : Commands
{
    protected ComandsRealization command;
    protected ProductList product;

    public CommandDel(ComandsRealization command, ProductList product)
    {
        this.command = command;
        this.product = product;
    }

    public void Add()
    {
        command.removeProduct(product);
        command.addCommand(this);
    }

    public void removeCommand()
    {
        command.addProduct(product);
    }
}

namespace practiceA.css
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ComandsRealization prodInf = new ComandsRealization();
            string userInput;

            void CustomerInterface()
            {
                do
                {
                    Console.WriteLine("Enter '1' to add product, '0' to delete product,\n" +
                                      "'-' to cancel last action, 'end' to finish the program:");
                    userInput = Console.ReadLine();

                    switch (userInput)
                    {
                        case "1":
                            Console.Write("Product name: ");
                            string name = Console.ReadLine();
                            Console.Write("Manufacturer: ");
                            string manufacturer = Console.ReadLine();
                            Console.Write("Product price: ");
                            double price = Convert.ToDouble(Console.ReadLine());

                            var product = new ProductList(name, manufacturer, price);
                            var addCommand = new CommandAdd(prodInf, product);
                            addCommand.Add();
                            break;

                        case "0":
                            Console.Write("Enter product name to delete: ");
                            string removeName = Console.ReadLine();
                            ProductList productToRemove = null;


                            foreach (var prod in prodInf.GetProducts())
                            {
                                if (prod.name.Equals(removeName))
                                {
                                    productToRemove = prod;
                                    break;
                                }
                            }
                            if (productToRemove == null)
                            {
                                Console.WriteLine("Product not found.");

                            }
                            var removeCommand = new CommandDel(prodInf, productToRemove);
                            removeCommand.Add();
                            break;

                        case "-":
                            prodInf.removeCommand();
                            break;

                        case "end":
                            Console.WriteLine("Task end");
                            break;

                        default:
                            Console.WriteLine("Incorrect input");
                            break;
                    }

                    prodInf.ShowProducts();
                }
                while (userInput != "end");
            }

            CustomerInterface();
        }
    }
}

