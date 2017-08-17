using System;
using System.Linq;
using System.Collections.Generic;

/**
* Auto-generated code below aims at helping you parse
* the standard input according to the problem statement.
**/

public class Link
{
    public int M;
    public int N;
}

public class Node
{
    public int ID;

    public int Distance;
    public int ParentID;

    public List<Node> ConNodes(List<Node> nodes, List<Link> links)
    {
        var result = new List<Node>();
        foreach (var l in links)
        {
            if (l.M == this.ID)
            {
                result.Add(nodes[l.N]);
            }
            else if (l.N == this.ID)
            {
                result.Add(nodes[l.M]);
            }
        }
        return result;
    }
}

class Player
{
    static void Main(string[] args)
    {
        string[] inputs;
        inputs = Console.ReadLine().Split(' ');
        int N = int.Parse(inputs[0]); // the total number of nodes in the level, including the gateways
        int L = int.Parse(inputs[1]); // the number of links
        int E = int.Parse(inputs[2]); // the number of exit gateways

        List<Node> nodes = new List<Node>();
        for (int i = 0; i < N; i++)
        {
            nodes.Add(new Node { ID = i });
        }


        List<Link> links = new List<Link>();
        for (int i = 0; i < L; i++)
        {
            inputs = Console.ReadLine().Split(' ');
            int N1 = int.Parse(inputs[0]); // N1 and N2 defines a link between these nodes
            int N2 = int.Parse(inputs[1]);
            links.Add(new Link { M = N1, N = N2 });
        }

        int[] EI = new int[E];
        var gates = new Node[E];
        for (int i = 0; i < E; i++)
        {
            EI[i] = int.Parse(Console.ReadLine()); // the index of a gateway node
            gates[i] = nodes[EI[i]];
        }

        // game loop
        while (true)
        {
            int SI = int.Parse(Console.ReadLine()); // The index of the node on which the Skynet agent is positioned this turn

            // Write an action using Console.WriteLine()
            // To debug: Console.Error.WriteLine("Debug messages...");

            // Label every node with its distance to Skynet Agent in current run.
            BFS(nodes[SI], nodes, links);

            // Find nearest gate to SI
            Node closeGate = gates.First(ga => ga.Distance == gates.Min(g => g.Distance));

            // Cut to link between nearest gate node and its parent node
            var linktc = from l in links
                         where (l.M == closeGate.ID && l.N == closeGate.ParentID) || (l.N == closeGate.ID && l.M == closeGate.ParentID)
                         select l;

            Link linkToCut = linktc.ToList()[0];

            // Output Example: 0 1 are the indices of the nodes you wish to sever the link between
            Console.WriteLine($"{linkToCut.M} {linkToCut.N}");

            links.Remove(linkToCut);
        }
    }

    /**
     * Breadth-first search (BFS) is an algorithm for traversing or searching tree 
     * or graph data structures. It starts at the tree root and explores the neighbor 
     * nodes first, before moving to the next level neighbors. Thus closer nodes get 
     * visited first.
     **/
    static void BFS(Node root, List<Node> nodes, List<Link> links)
    {
        for (int i = 0; i < nodes.Count; i++)
        {
            nodes[i].Distance = Int32.MaxValue;
        }

        var visit = new Queue<Node>();

        root.Distance = 0;
        visit.Enqueue(root);

        while (visit.Count != 0)
        {
            var current = visit.Dequeue();

            foreach (var node in current.ConNodes(nodes, links))
            {
                if (node.Distance == Int32.MaxValue)
                {
                    node.Distance = current.Distance + 1;
                    node.ParentID = current.ID;
                    visit.Enqueue(node);
                }
            }
        }
    }
}


