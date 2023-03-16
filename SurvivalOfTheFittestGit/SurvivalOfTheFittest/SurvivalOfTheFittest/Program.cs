using ScottPlot;

namespace SurvivalOfTheFittest
{

    class Program
    {
        static class Environment
        {
            public static int plants = 0;
            public static int day = 0;
            public static List<Creature> Carnivores = CreateCreatures(50, "Carnivore");
            public static List<Creature> Herbivores = CreateCreatures(500, "Herbivore");
        }
        public class Stats
        {
            public int HowManyAte { get; set; }
            public int HowManyStarved { get; set; }
            public int HowManyDidntEat { get; set; }
        }
        public class Creature
        {
            public int hunger { get; set; } // How hungry a creature is
            public string? hungerType { get; set; } // Carnivore or Herbivore
            public bool gender { get; set; } // true = male, false = female
            public int gestation { get; set; } // How long it takes to create the child
            public List<Creature>? children { get; set; } // The females child
            public int litter { get; set; } // How many children the creature will create
            public int? dueDate { get; set; } // When the child is born
            public int age { get; set; } // how old the creature is
            public int maxAge { get; set; } // How old it gets before it dies
            public int desirability { get; set; } // Chances a male gets a mate
        }

        static void Main(string[] args)
        {
            // Create a list of creatures
            List<Creature> Carnivores = Environment.Carnivores;
            List<Creature> Herbivores = Environment.Herbivores;

            // Create list for day data
            List<double> day = new List<double>();

            // Create lists for herbivore data
            List<double> herbivoresAmount = new List<double>();
            List<double> herbivoreGestation = new List<double>();
            List<double> herbivoreLitter = new List<double>();
            List<double> herbivoreDesirability = new List<double>();

            // Create lists for carnivore data
            List<double> carnivoresAmount = new List<double>();
            List<double> carnivoreGestation = new List<double>();
            List<double> carnivoreLitter = new List<double>();
            List<double> carnivoreDesirability = new List<double>();

            // Run the simulations
            while (Carnivores.Count != 0 && Herbivores.Count != 0)
            {
                // Set Environment variables
                Environment.plants = 500;
                Environment.day++;


                // Print day
                Console.WriteLine("Day: " + Environment.day);

                // Creatures eat, breed and birth
                if (Carnivores.Count > 0)
                {
                    int CarnivoresDead = Fight(Carnivores);
                    int CarnivoresDecayed = Age(Carnivores);
                    Stats CarnivoreStats = Eat(Carnivores);
                    Breed(Carnivores);
                    Carnivores.AddRange(Birth(Carnivores));

                    // Carnivore Stats
                    Console.WriteLine("Carnivores " + "(" + Carnivores.Count + ")" + ": ");
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine(CarnivoreStats.HowManyAte + " ate");
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.WriteLine(CarnivoreStats.HowManyDidntEat + " didn't eat");
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine(CarnivoreStats.HowManyStarved + " starved");
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(CarnivoresDead + " died in a fight");
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.WriteLine(CarnivoresDecayed + " died of old age");
                    Console.ForegroundColor = ConsoleColor.White;
                }

                if (Herbivores.Count > 0)
                {
                    Stats HerbivoreStats = Eat(Herbivores);
                    int HerbivoresDecayed = Age(Herbivores);
                    Breed(Herbivores);
                    Herbivores.AddRange(Birth(Herbivores));

                    // Herbivores Stats
                    Console.WriteLine("Herbivores " + "(" + Herbivores.Count + ")" + ": ");
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine(HerbivoreStats.HowManyAte + " ate");
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.WriteLine(HerbivoreStats.HowManyDidntEat + " didn't eat");
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(HerbivoreStats.HowManyStarved + " starved");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.WriteLine(HerbivoresDecayed + " died of old age");
                    Console.ForegroundColor = ConsoleColor.White;
                }

                // Set data at the end of the day.
                day.Add(Environment.day);

                // Set Herbivore data
                if (Herbivores.Count > 0)
                {
                    herbivoresAmount.Add(Herbivores.Count);
                    herbivoreGestation.Add(Herbivores.Sum(c => c.gestation) / Herbivores.Count);
                    herbivoreLitter.Add(Herbivores.Sum(c => c.litter) / Herbivores.Count);
                    herbivoreDesirability.Add(Herbivores.Sum(c => c.desirability) / Herbivores.Count);
                }
                else
                {
                    herbivoresAmount.Add(0);
                    herbivoreGestation.Add(0);
                    herbivoreLitter.Add(0);
                    herbivoreDesirability.Add(0);
                }

                // Set carnivore data
                if (Carnivores.Count > 0)
                {
                    carnivoresAmount.Add(Carnivores.Count);
                    carnivoreGestation.Add(Carnivores.Sum(c => c.gestation) / Carnivores.Count);
                    carnivoreLitter.Add(Carnivores.Sum(c => c.litter) / Carnivores.Count);
                    carnivoreDesirability.Add(Carnivores.Sum(c => c.desirability) / Carnivores.Count);
                }
                else
                {
                    carnivoresAmount.Add(0);
                    carnivoreGestation.Add(0);
                    carnivoreLitter.Add(0);
                    carnivoreDesirability.Add(0);
                }

                ConsoleKeyInfo cki;
                cki = Console.ReadKey();
                if (cki.Key == ConsoleKey.Escape)
                    break;
            }

            // Create herbivore graphs
            CreateScottPlot(day, herbivoresAmount, "Day", "Herbivores", "Herbivores");
            CreateScottPlot(day, herbivoreGestation, "Day", "Herbivore Gestation", "HerbivoreGestation");
            CreateScottPlot(day, herbivoreLitter, "Day", "Herbivore Litter", "HerbivoreLitter");
            CreateScottPlot(day, herbivoreDesirability, "Day", "Herbivore Desirability", "HerbivoreDesirability");

            // Create carnivore graphs
            CreateScottPlot(day, carnivoresAmount, "Day", "Carnivores", "Carnivores");
            CreateScottPlot(day, carnivoreGestation, "Day", "Carnivore Gestation", "CarnivoreGestation");
            CreateScottPlot(day, carnivoreLitter, "Day", "Carnivore Litter", "CarnivoreLitter");
            CreateScottPlot(day, carnivoreDesirability, "Day", "Carnivore Desirability", "CarnivoreDesirability");

            Console.ReadLine();
        }

        // Create a scott plot graph
        static void CreateScottPlot(List<double> x, List<double> y, string xLabel, string yLabel, string graphName)
        {
            var plt = new ScottPlot.Plot(400, 300);
            plt.Style(Style.Black);
            plt.AddScatter(x.ToArray(), y.ToArray());
            plt.XAxis.Label(xLabel);
            plt.YAxis.Label(yLabel);
            plt.SaveFig(@"C:\Users\joenh\OneDrive\Skrivebord\Education\School\Hovedforløb 4\Machine Learning\SurvivalOfTheFittest\Graphs\" + graphName + ".png");

        }

        // Creatures fight
        static int Fight(List<Creature> creatures)
        {
            // Create empty list of dead creatures
            int deadCreatures = 0;

            // Create a random
            Random rnd = new Random();

            // While there are more carnivores than half the herbivores
            while (creatures.Count() > (int)(Environment.Herbivores.Count() / 2))
            {
                // Kill a carnivores
                int creatureIndex = rnd.Next(0, creatures.Count());
                creatures.Remove(creatures[creatureIndex]);
                deadCreatures++;
            }

            return deadCreatures;
        }

        // Female give birth
        static List<Creature> Birth(List<Creature> creatures)
        {
            // Create a list of females from the list of creatures
            List<Creature> females = creatures.FindAll(c => c.gender == false).ToList();
            // Create an empty list of children
            List<Creature> children = new List<Creature>();

            // Foreach female
            foreach (Creature female in females)
            {
                // If the female is carrying a child and the due date is the currnt day
                if (female.children != null && female.dueDate == Environment.day)
                {
                    // Add the child to the children list
                    children.AddRange(female.children);

                    // Remove the child from the mother
                    female.children = null;
                }
            }

            // Return the list of children
            return children;
        }

        // Breed list of creatures
        static void Breed(List<Creature> creatures)
        {
            // Create a list of females from list of creatures
            List<Creature> females = creatures.Where(c => c.gender == false).ToList();

            // Create a list of males from list of creatures
            List<Creature> males = creatures.Where(c => c.gender == true).ToList();

            // Create an empty list of children
            List<Creature> children = new List<Creature>();

            // Create a new random
            Random rnd = new Random();

            foreach (Creature male in males.ToList())
            {
                // Foreach female
                foreach (Creature female in females.ToList())
                {
                    // If the female is not carrying a child
                    if (female.children == null && female.hunger < 50 && male.hunger < 50 && rnd.Next(0, 100) < male.desirability)
                    {
                        // Create child with male using the child function
                        female.children = Child(female, male);
                        // Save due date based on female gestation
                        female.dueDate = Environment.day + female.gestation;
                        // Stop searching for a mate
                        break;
                    }
                }
            }
        }

        // Randomly determine childs genetics from parents
        static List<Creature> Child(Creature mother, Creature father)
        {
            // Create a new random
            Random rnd = new Random();
            // Create a list of children
            List<Creature> children = new List<Creature>();

            int litter = rnd.Next(Math.Max(1, (int)(mother.litter / 2)), mother.litter + 1);

            for (int i = 0; i < litter; i++)
            {
                // Create a child
                Creature child = new Creature();

                // Set the childs attributes using the InherentGene function
                child.hunger = 70;
                child.hungerType = mother.hungerType; // Hardcoded to the mothers hunger Type
                child.gender = rnd.Next(0, 2) == 1 ? mother.gender : father.gender; // The gender can't mutate
                child.gestation = InherentGene(mother.gestation, father.gestation);
                child.litter = child.hungerType == "Herbivore" ? InherentGene(mother.litter, father.litter) : 1;
                child.age = 0;
                child.maxAge = rnd.Next(10, 30);
                child.desirability = InherentGene(mother.desirability, father.desirability);

                children.Add(child);
            }
            // Return the children
            return children;
        }

        // Inherent Gene from parents
        static int InherentGene(int mother, int father)
        {
            // Create new gene
            Random rnd = new Random();

            // Get gene from mother or father
            int oldGene = rnd.Next(0, 2) == 1 ? mother : father;
            int newGene;

            // If random is less than 30
            if (rnd.Next(1, 100) < 30)
            {
                // Determine mutation amount
                int mutationAmount = 1 > (int)(oldGene * 0.1) ? 1 : (int)(oldGene * 0.1);

                // Randomly substract or add mutation amount
                newGene = rnd.Next(0, 2) == 1 ? oldGene + mutationAmount : oldGene - mutationAmount;

                // Return the new gene, it can't be below 1 or above 100 
                return Math.Min(100, Math.Max(1, newGene));

            }

            // If no new gene is made return the old gene
            return oldGene;
        }

        // Each creature ina list ages
        static int Age(List<Creature> creatures)
        {
            int CreaturesWhoDied = 0;
            foreach (Creature creature in creatures.ToList())
            {
                creature.age++;
                if (creature.age == creature.maxAge)
                {
                    creatures.Remove(creature);
                    CreaturesWhoDied++;
                }
            }
            return CreaturesWhoDied;
        }

        // Each creature in a list eats
        static Stats Eat(List<Creature> creatures)
        {

            // Statistics
            Stats stats = new Stats();

            switch (creatures[0].hungerType)
            {
                case "Herbivore":
                    foreach (Creature creature in creatures.OrderBy(c => c.age).ToList())
                    {
                        // If creatures hunger is above 0 and there is food
                        if (creature.hunger > 0 && Environment.plants > 0)
                        {
                            Environment.plants--;
                            creature.hunger -= 10;
                            stats.HowManyAte++;
                        }
                        // else don't eat and gain hunger
                        else
                        {
                            creature.hunger += 10;
                            stats.HowManyDidntEat++;
                            // If hunger ever reaches 100 the creature starves to death
                            if (creature.hunger >= 100)
                            {
                                stats.HowManyStarved++;
                                creatures.Remove(creature);
                            }
                        }
                    }
                    break;
                case "Carnivore":
                    foreach (Creature creature in creatures.OrderBy(c => c.age).ToList())
                    {
                        // If creatures hunger is above 0
                        if (creature.hunger > 50 && Environment.Herbivores.Count() > 0)
                        {
                            /*
                            Random rnd = new Random();
                            int victimIndex = rnd.Next(0, Environment.Herbivores.Count());
                            Creature victim = Environment.Herbivores[victimIndex];
                            */
                            Creature victim = Environment.Herbivores.OrderByDescending(c => c.age).ToList()[0];
                            Environment.Herbivores.Remove(victim);
                            creature.hunger -= 50;
                            stats.HowManyAte++;

                        }
                        else
                        {
                            creature.hunger += 10;
                            stats.HowManyDidntEat++;
                            // If hunger ever reaches 100 the creature starves to death
                            if (creature.hunger >= 100)
                            {
                                stats.HowManyStarved++;
                                creatures.Remove(creature);
                            }
                        }
                    }
                    break;
            }

            return stats;
        }

        // Create a list of certain amount of creatures with a certain hunger type
        static List<Creature> CreateCreatures(int amount, string hungerType)
        {
            // Create list of creatures
            List<Creature> creatures = new List<Creature>();

            Random rnd = new Random();

            // Make a loop that runs for a number of times as defined by amount
            for (int i = 0; i < amount; i++)
            {
                if (i % 2 == 0)
                {
                    creatures.Add(new Creature()
                    {
                        hunger = rnd.Next(1, 10) * 10,
                        hungerType = hungerType,
                        gender = true,
                        gestation = rnd.Next(5, 10),
                        litter = hungerType == "Herbivore" ? rnd.Next(1, 3) : 1,
                        age = 0,
                        maxAge = rnd.Next(10, 30),
                        desirability = rnd.Next(10, 30)
                    });
                }
                else
                {
                    creatures.Add(new Creature()
                    {
                        hunger = rnd.Next(1, 10) * 10,
                        hungerType = hungerType,
                        gender = false,
                        gestation = rnd.Next(5, 10),
                        litter = hungerType == "Herbivore" ? rnd.Next(1, 3) : 1,
                        age = 0,
                        maxAge = rnd.Next(10, 30),
                        desirability = rnd.Next(10, 30)
                    });
                }
            }

            return creatures;
        }

    }

}
